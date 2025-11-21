using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;

namespace WorkBalanceHub.Application.Services;

public class EstacaoTrabalhoService : IEstacaoTrabalhoService
{
    private readonly IRepository<EstacaoTrabalho> _estacaoTrabalhoRepository;

    public EstacaoTrabalhoService(IRepository<EstacaoTrabalho> estacaoTrabalhoRepository)
    {
        _estacaoTrabalhoRepository = estacaoTrabalhoRepository;
    }

    public async Task<EstacaoTrabalhoDto> CriarAsync(CriarEstacaoTrabalhoDto dto, CancellationToken cancellationToken = default)
    {
        var estacao = new EstacaoTrabalho(dto.Nome, dto.Localizacao, dto.Descricao);
        var estacaoCriada = await _estacaoTrabalhoRepository.AddAsync(estacao, cancellationToken);
        return MapToDto(estacaoCriada);
    }

    public async Task<EstacaoTrabalhoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var estacao = await _estacaoTrabalhoRepository.GetByIdAsync(id, cancellationToken);
        return estacao != null ? MapToDto(estacao) : null;
    }

    public async Task<PagedResultDto<EstacaoTrabalhoDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default)
    {
        var todas = await _estacaoTrabalhoRepository.GetAllAsync(cancellationToken);
        var query = todas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var termo = request.SearchTerm.ToLower();
            query = query.Where(e => 
                e.Nome.ToLower().Contains(termo) ||
                (e.Localizacao != null && e.Localizacao.ToLower().Contains(termo)) ||
                (e.Descricao != null && e.Descricao.ToLower().Contains(termo)));
        }

        var totalItems = query.Count();

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "nome" => request.SortDirection == "desc" ? query.OrderByDescending(e => e.Nome) : query.OrderBy(e => e.Nome),
                "datacadastro" => request.SortDirection == "desc" ? query.OrderByDescending(e => e.DataCadastro) : query.OrderBy(e => e.DataCadastro),
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

        return new PagedResultDto<EstacaoTrabalhoDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<EstacaoTrabalhoDto> AtualizarAsync(int id, AtualizarEstacaoTrabalhoDto dto, CancellationToken cancellationToken = default)
    {
        var estacao = await _estacaoTrabalhoRepository.GetByIdAsync(id, cancellationToken);
        if (estacao == null)
            throw new InvalidOperationException("Estação de trabalho não encontrada.");

        estacao.Atualizar(dto.Nome, dto.Localizacao, dto.Descricao);
        await _estacaoTrabalhoRepository.UpdateAsync(estacao, cancellationToken);

        return MapToDto(estacao);
    }

    public async Task DeletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var estacao = await _estacaoTrabalhoRepository.GetByIdAsync(id, cancellationToken);
        if (estacao == null)
            throw new InvalidOperationException("Estação de trabalho não encontrada.");

        estacao.Desativar();
        await _estacaoTrabalhoRepository.UpdateAsync(estacao, cancellationToken);
    }

    private static EstacaoTrabalhoDto MapToDto(EstacaoTrabalho estacao)
    {
        return new EstacaoTrabalhoDto
        {
            Id = estacao.Id,
            Nome = estacao.Nome,
            Localizacao = estacao.Localizacao,
            Descricao = estacao.Descricao,
            DataCadastro = estacao.DataCadastro,
            Ativa = estacao.Ativa
        };
    }
}

