using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;

namespace WorkBalanceHub.Application.Services;

public class ColaboradorService : IColaboradorService
{
    private readonly IColaboradorRepository _colaboradorRepository;
    private readonly IRepository<Equipe> _equipeRepository;

    public ColaboradorService(IColaboradorRepository colaboradorRepository, IRepository<Equipe> equipeRepository)
    {
        _colaboradorRepository = colaboradorRepository;
        _equipeRepository = equipeRepository;
    }

    public async Task<ColaboradorDto> CriarAsync(CriarColaboradorDto dto, CancellationToken cancellationToken = default)
    {
        var equipeExiste = await _equipeRepository.ExistsAsync(dto.EquipeId, cancellationToken);
        if (!equipeExiste)
            throw new InvalidOperationException("A equipe especificada não existe.");

        var colaboradorExistente = await _colaboradorRepository.GetByEmailAsync(dto.Email, cancellationToken);
        if (colaboradorExistente != null)
            throw new InvalidOperationException("Já existe um colaborador com este e-mail.");

        var colaborador = new Colaborador(dto.Nome, dto.Email, dto.Cargo, dto.EquipeId);
        var colaboradorCriado = await _colaboradorRepository.AddAsync(colaborador, cancellationToken);

        return MapToDto(colaboradorCriado);
    }

    public async Task<ColaboradorDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var colaborador = await _colaboradorRepository.GetByIdAsync(id, cancellationToken);
        return colaborador != null ? MapToDto(colaborador) : null;
    }

    public async Task<PagedResultDto<ColaboradorDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default)
    {
        // Implementação simplificada - em produção, usar IQueryable com EF Core
        var todos = await _colaboradorRepository.GetAllAsync(cancellationToken);
        
        var query = todos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var termo = request.SearchTerm.ToLower();
            query = query.Where(c => 
                c.Nome.ToLower().Contains(termo) ||
                c.Email.ToLower().Contains(termo) ||
                c.Cargo.ToLower().Contains(termo));
        }

        var totalItems = query.Count();

        // Ordenação
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "nome" => request.SortDirection == "desc" ? query.OrderByDescending(c => c.Nome) : query.OrderBy(c => c.Nome),
                "email" => request.SortDirection == "desc" ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email),
                "cargo" => request.SortDirection == "desc" ? query.OrderByDescending(c => c.Cargo) : query.OrderBy(c => c.Cargo),
                _ => query.OrderBy(c => c.Id)
            };
        }
        else
        {
            query = query.OrderBy(c => c.Id);
        }

        // Paginação
        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResultDto<ColaboradorDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<ColaboradorDto> AtualizarAsync(int id, AtualizarColaboradorDto dto, CancellationToken cancellationToken = default)
    {
        var colaborador = await _colaboradorRepository.GetByIdAsync(id, cancellationToken);
        if (colaborador == null)
            throw new InvalidOperationException("Colaborador não encontrado.");

        var equipeExiste = await _equipeRepository.ExistsAsync(dto.EquipeId, cancellationToken);
        if (!equipeExiste)
            throw new InvalidOperationException("A equipe especificada não existe.");

        var colaboradorComEmail = await _colaboradorRepository.GetByEmailAsync(dto.Email, cancellationToken);
        if (colaboradorComEmail != null && colaboradorComEmail.Id != id)
            throw new InvalidOperationException("Já existe outro colaborador com este e-mail.");

        colaborador.Atualizar(dto.Nome, dto.Email, dto.Cargo, dto.EquipeId);
        await _colaboradorRepository.UpdateAsync(colaborador, cancellationToken);

        return MapToDto(colaborador);
    }

    public async Task DeletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var colaborador = await _colaboradorRepository.GetByIdAsync(id, cancellationToken);
        if (colaborador == null)
            throw new InvalidOperationException("Colaborador não encontrado.");

        colaborador.Desativar();
        await _colaboradorRepository.UpdateAsync(colaborador, cancellationToken);
    }

    public async Task<IEnumerable<ColaboradorDto>> ObterPorEquipeIdAsync(int equipeId, CancellationToken cancellationToken = default)
    {
        var colaboradores = await _colaboradorRepository.GetByEquipeIdAsync(equipeId, cancellationToken);
        return colaboradores.Select(MapToDto);
    }

    private static ColaboradorDto MapToDto(Colaborador colaborador)
    {
        return new ColaboradorDto
        {
            Id = colaborador.Id,
            Nome = colaborador.Nome,
            Email = colaborador.Email,
            Cargo = colaborador.Cargo,
            EquipeId = colaborador.EquipeId,
            EquipeNome = colaborador.Equipe?.Nome,
            DataCadastro = colaborador.DataCadastro,
            Ativo = colaborador.Ativo
        };
    }
}

