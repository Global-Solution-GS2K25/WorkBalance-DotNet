using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;

namespace WorkBalanceHub.Application.Services;

public class LeituraAmbienteService : ILeituraAmbienteService
{
    private readonly ILeituraAmbienteRepository _leituraAmbienteRepository;
    private readonly IRepository<EstacaoTrabalho> _estacaoTrabalhoRepository;

    public LeituraAmbienteService(ILeituraAmbienteRepository leituraAmbienteRepository, IRepository<EstacaoTrabalho> estacaoTrabalhoRepository)
    {
        _leituraAmbienteRepository = leituraAmbienteRepository;
        _estacaoTrabalhoRepository = estacaoTrabalhoRepository;
    }

    public async Task<LeituraAmbienteDto> CriarAsync(CriarLeituraAmbienteDto dto, CancellationToken cancellationToken = default)
    {
        var estacaoExiste = await _estacaoTrabalhoRepository.ExistsAsync(dto.EstacaoTrabalhoId, cancellationToken);
        if (!estacaoExiste)
            throw new InvalidOperationException("A estação de trabalho especificada não existe.");

        var leitura = new LeituraAmbiente(dto.EstacaoTrabalhoId, dto.Temperatura, dto.NivelRuido, dto.Luminosidade);
        var leituraCriada = await _leituraAmbienteRepository.AddAsync(leitura, cancellationToken);

        return MapToDto(leituraCriada);
    }

    public async Task<LeituraAmbienteDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var leitura = await _leituraAmbienteRepository.GetByIdAsync(id, cancellationToken);
        return leitura != null ? MapToDto(leitura) : null;
    }

    public async Task<PagedResultDto<LeituraAmbienteDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default)
    {
        var todas = await _leituraAmbienteRepository.GetAllAsync(cancellationToken);
        var query = todas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var termo = request.SearchTerm.ToLower();
            query = query.Where(l => 
                l.EstacaoTrabalho.Nome.ToLower().Contains(termo) ||
                (l.EstacaoTrabalho.Localizacao != null && l.EstacaoTrabalho.Localizacao.ToLower().Contains(termo)));
        }

        var totalItems = query.Count();

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "dataleitura" => request.SortDirection == "desc" ? query.OrderByDescending(l => l.DataLeitura) : query.OrderBy(l => l.DataLeitura),
                "temperatura" => request.SortDirection == "desc" ? query.OrderByDescending(l => l.Temperatura) : query.OrderBy(l => l.Temperatura),
                "ruido" => request.SortDirection == "desc" ? query.OrderByDescending(l => l.NivelRuido) : query.OrderBy(l => l.NivelRuido),
                _ => query.OrderByDescending(l => l.DataLeitura)
            };
        }
        else
        {
            query = query.OrderByDescending(l => l.DataLeitura);
        }

        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResultDto<LeituraAmbienteDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<LeituraAmbienteDto> AtualizarAsync(int id, AtualizarLeituraAmbienteDto dto, CancellationToken cancellationToken = default)
    {
        var leitura = await _leituraAmbienteRepository.GetByIdAsync(id, cancellationToken);
        if (leitura == null)
            throw new InvalidOperationException("Leitura de ambiente não encontrada.");

        // Criar nova leitura com dados atualizados (histórico não deve ser alterado)
        // Ou criar método de atualização na entidade se necessário
        throw new NotSupportedException("Leituras de ambiente são imutáveis. Crie uma nova leitura.");
    }

    public async Task DeletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var leitura = await _leituraAmbienteRepository.GetByIdAsync(id, cancellationToken);
        if (leitura == null)
            throw new InvalidOperationException("Leitura de ambiente não encontrada.");

        await _leituraAmbienteRepository.DeleteAsync(leitura, cancellationToken);
    }

    public async Task<IEnumerable<LeituraAmbienteDto>> ObterPorEstacaoTrabalhoIdAsync(int estacaoTrabalhoId, CancellationToken cancellationToken = default)
    {
        var leituras = await _leituraAmbienteRepository.GetByEstacaoTrabalhoIdAsync(estacaoTrabalhoId, cancellationToken);
        return leituras.Select(MapToDto);
    }

    private static LeituraAmbienteDto MapToDto(LeituraAmbiente leitura)
    {
        return new LeituraAmbienteDto
        {
            Id = leitura.Id,
            EstacaoTrabalhoId = leitura.EstacaoTrabalhoId,
            EstacaoTrabalhoNome = leitura.EstacaoTrabalho?.Nome,
            DataLeitura = leitura.DataLeitura,
            Temperatura = leitura.Temperatura,
            NivelRuido = leitura.NivelRuido,
            Luminosidade = leitura.Luminosidade,
            DentroDoConforto = leitura.EstaDentroDoConforto()
        };
    }
}

