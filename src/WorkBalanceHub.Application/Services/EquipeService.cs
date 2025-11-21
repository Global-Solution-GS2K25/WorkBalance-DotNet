using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;

namespace WorkBalanceHub.Application.Services;

public class EquipeService : IEquipeService
{
    private readonly IRepository<Equipe> _equipeRepository;

    public EquipeService(IRepository<Equipe> equipeRepository)
    {
        _equipeRepository = equipeRepository;
    }

    public async Task<EquipeDto> CriarAsync(CriarEquipeDto dto, CancellationToken cancellationToken = default)
    {
        var equipe = new Equipe(dto.Nome, dto.Descricao);
        var equipeCriada = await _equipeRepository.AddAsync(equipe, cancellationToken);
        return MapToDto(equipeCriada);
    }

    public async Task<EquipeDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var equipe = await _equipeRepository.GetByIdAsync(id, cancellationToken);
        return equipe != null ? MapToDto(equipe) : null;
    }

    public async Task<PagedResultDto<EquipeDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default)
    {
        var todas = await _equipeRepository.GetAllAsync(cancellationToken);
        var query = todas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var termo = request.SearchTerm.ToLower();
            query = query.Where(e => 
                e.Nome.ToLower().Contains(termo) ||
                (e.Descricao != null && e.Descricao.ToLower().Contains(termo)));
        }

        var totalItems = query.Count();

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "nome" => request.SortDirection == "desc" ? query.OrderByDescending(e => e.Nome) : query.OrderBy(e => e.Nome),
                "datacriacao" => request.SortDirection == "desc" ? query.OrderByDescending(e => e.DataCriacao) : query.OrderBy(e => e.DataCriacao),
                _ => query.OrderBy(e => e.Id)
            };
        }
        else
        {
            query = query.OrderBy(e => e.Id);
        }

        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResultDto<EquipeDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<EquipeDto> AtualizarAsync(int id, AtualizarEquipeDto dto, CancellationToken cancellationToken = default)
    {
        var equipe = await _equipeRepository.GetByIdAsync(id, cancellationToken);
        if (equipe == null)
            throw new InvalidOperationException("Equipe não encontrada.");

        equipe.Atualizar(dto.Nome, dto.Descricao);
        await _equipeRepository.UpdateAsync(equipe, cancellationToken);

        return MapToDto(equipe);
    }

    public async Task DeletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var equipe = await _equipeRepository.GetByIdAsync(id, cancellationToken);
        if (equipe == null)
            throw new InvalidOperationException("Equipe não encontrada.");

        equipe.Desativar();
        await _equipeRepository.UpdateAsync(equipe, cancellationToken);
    }

    private static EquipeDto MapToDto(Equipe equipe)
    {
        return new EquipeDto
        {
            Id = equipe.Id,
            Nome = equipe.Nome,
            Descricao = equipe.Descricao,
            DataCriacao = equipe.DataCriacao,
            Ativa = equipe.Ativa,
            TotalColaboradores = equipe.Colaboradores?.Count(c => c.Ativo) ?? 0
        };
    }
}

