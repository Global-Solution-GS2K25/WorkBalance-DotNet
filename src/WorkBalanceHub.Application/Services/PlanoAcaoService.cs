using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;

namespace WorkBalanceHub.Application.Services;

public class PlanoAcaoService : IPlanoAcaoService
{
    private readonly IPlanoAcaoRepository _planoAcaoRepository;
    private readonly IRepository<Equipe> _equipeRepository;

    public PlanoAcaoService(IPlanoAcaoRepository planoAcaoRepository, IRepository<Equipe> equipeRepository)
    {
        _planoAcaoRepository = planoAcaoRepository;
        _equipeRepository = equipeRepository;
    }

    public async Task<PlanoAcaoDto> CriarAsync(CriarPlanoAcaoDto dto, CancellationToken cancellationToken = default)
    {
        var equipeExiste = await _equipeRepository.ExistsAsync(dto.EquipeId, cancellationToken);
        if (!equipeExiste)
            throw new InvalidOperationException("A equipe especificada não existe.");

        var plano = new PlanoAcao(dto.Titulo, dto.Descricao, dto.EquipeId, dto.DataInicio, dto.DataFim);
        var planoCriado = await _planoAcaoRepository.AddAsync(plano, cancellationToken);

        return MapToDto(planoCriado);
    }

    public async Task<PlanoAcaoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var plano = await _planoAcaoRepository.GetByIdAsync(id, cancellationToken);
        return plano != null ? MapToDto(plano) : null;
    }

    public async Task<PagedResultDto<PlanoAcaoDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default)
    {
        var todos = await _planoAcaoRepository.GetAllAsync(cancellationToken);
        var query = todos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var termo = request.SearchTerm.ToLower();
            query = query.Where(p => 
                p.Titulo.ToLower().Contains(termo) ||
                p.Descricao.ToLower().Contains(termo) ||
                (p.Equipe.Nome != null && p.Equipe.Nome.ToLower().Contains(termo)));
        }

        var totalItems = query.Count();

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "titulo" => request.SortDirection == "desc" ? query.OrderByDescending(p => p.Titulo) : query.OrderBy(p => p.Titulo),
                "datainicio" => request.SortDirection == "desc" ? query.OrderByDescending(p => p.DataInicio) : query.OrderBy(p => p.DataInicio),
                "status" => request.SortDirection == "desc" ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
                _ => query.OrderByDescending(p => p.DataCriacao)
            };
        }
        else
        {
            query = query.OrderByDescending(p => p.DataCriacao);
        }

        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResultDto<PlanoAcaoDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<PlanoAcaoDto> AtualizarAsync(int id, AtualizarPlanoAcaoDto dto, CancellationToken cancellationToken = default)
    {
        var plano = await _planoAcaoRepository.GetByIdAsync(id, cancellationToken);
        if (plano == null)
            throw new InvalidOperationException("Plano de ação não encontrado.");

        plano.Atualizar(dto.Titulo, dto.Descricao, dto.DataInicio, dto.DataFim, dto.Observacoes);
        await _planoAcaoRepository.UpdateAsync(plano, cancellationToken);

        return MapToDto(plano);
    }

    public async Task DeletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var plano = await _planoAcaoRepository.GetByIdAsync(id, cancellationToken);
        if (plano == null)
            throw new InvalidOperationException("Plano de ação não encontrado.");

        await _planoAcaoRepository.DeleteAsync(plano, cancellationToken);
    }

    public async Task<PlanoAcaoDto> AlterarStatusAsync(int id, AlterarStatusPlanoAcaoDto dto, CancellationToken cancellationToken = default)
    {
        var plano = await _planoAcaoRepository.GetByIdAsync(id, cancellationToken);
        if (plano == null)
            throw new InvalidOperationException("Plano de ação não encontrado.");

        var novoStatus = Enum.Parse<StatusPlanoAcao>(dto.Status, ignoreCase: true);
        plano.AlterarStatus(novoStatus);
        await _planoAcaoRepository.UpdateAsync(plano, cancellationToken);

        return MapToDto(plano);
    }

    public async Task<IEnumerable<PlanoAcaoDto>> ObterPorEquipeIdAsync(int equipeId, CancellationToken cancellationToken = default)
    {
        var planos = await _planoAcaoRepository.GetByEquipeIdAsync(equipeId, cancellationToken);
        return planos.Select(MapToDto);
    }

    private static PlanoAcaoDto MapToDto(PlanoAcao plano)
    {
        return new PlanoAcaoDto
        {
            Id = plano.Id,
            Titulo = plano.Titulo,
            Descricao = plano.Descricao,
            EquipeId = plano.EquipeId,
            EquipeNome = plano.Equipe?.Nome,
            DataInicio = plano.DataInicio,
            DataFim = plano.DataFim,
            Status = plano.Status.ToString(),
            DataCriacao = plano.DataCriacao,
            Observacoes = plano.Observacoes,
            EstaAtivo = plano.EstaAtivo()
        };
    }
}

