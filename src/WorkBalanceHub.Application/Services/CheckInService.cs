using WorkBalanceHub.Application.DTOs;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;

namespace WorkBalanceHub.Application.Services;

public class CheckInService : ICheckInService
{
    private readonly ICheckInRepository _checkInRepository;
    private readonly IColaboradorRepository _colaboradorRepository;

    public CheckInService(ICheckInRepository checkInRepository, IColaboradorRepository colaboradorRepository)
    {
        _checkInRepository = checkInRepository;
        _colaboradorRepository = colaboradorRepository;
    }

    public async Task<CheckInDto> CriarAsync(CriarCheckInDto dto, CancellationToken cancellationToken = default)
    {
        var colaborador = await _colaboradorRepository.GetByIdAsync(dto.ColaboradorId, cancellationToken);
        if (colaborador == null)
            throw new InvalidOperationException("Colaborador não encontrado.");

        var checkInHoje = await _checkInRepository.GetCheckInHojeAsync(dto.ColaboradorId, cancellationToken);
        if (checkInHoje != null)
            throw new InvalidOperationException("Já existe um check-in para hoje. Use a atualização para modificar.");

        var checkIn = new CheckIn(dto.ColaboradorId, dto.Humor, dto.NivelEstresse, dto.QualidadeSono, 
                                  dto.SintomasFisicos, dto.Observacoes);
        var checkInCriado = await _checkInRepository.AddAsync(checkIn, cancellationToken);

        return MapToDto(checkInCriado);
    }

    public async Task<CheckInDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var checkIn = await _checkInRepository.GetByIdAsync(id, cancellationToken);
        return checkIn != null ? MapToDto(checkIn) : null;
    }

    public async Task<PagedResultDto<CheckInDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default)
    {
        var todos = await _checkInRepository.GetAllAsync(cancellationToken);
        var query = todos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var termo = request.SearchTerm.ToLower();
            query = query.Where(c => 
                c.Colaborador.Nome.ToLower().Contains(termo) ||
                (c.SintomasFisicos != null && c.SintomasFisicos.ToLower().Contains(termo)) ||
                (c.Observacoes != null && c.Observacoes.ToLower().Contains(termo)));
        }

        var totalItems = query.Count();

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "datacheckin" => request.SortDirection == "desc" ? query.OrderByDescending(c => c.DataCheckIn) : query.OrderBy(c => c.DataCheckIn),
                "humor" => request.SortDirection == "desc" ? query.OrderByDescending(c => c.Humor) : query.OrderBy(c => c.Humor),
                "estresse" => request.SortDirection == "desc" ? query.OrderByDescending(c => c.NivelEstresse) : query.OrderBy(c => c.NivelEstresse),
                _ => query.OrderByDescending(c => c.DataCheckIn)
            };
        }
        else
        {
            query = query.OrderByDescending(c => c.DataCheckIn);
        }

        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResultDto<CheckInDto>
        {
            Items = items,
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<CheckInDto> AtualizarAsync(int id, AtualizarCheckInDto dto, CancellationToken cancellationToken = default)
    {
        var checkIn = await _checkInRepository.GetByIdAsync(id, cancellationToken);
        if (checkIn == null)
            throw new InvalidOperationException("Check-in não encontrado.");

        checkIn.Atualizar(dto.Humor, dto.NivelEstresse, dto.QualidadeSono, dto.SintomasFisicos, dto.Observacoes);
        await _checkInRepository.UpdateAsync(checkIn, cancellationToken);

        return MapToDto(checkIn);
    }

    public async Task DeletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var checkIn = await _checkInRepository.GetByIdAsync(id, cancellationToken);
        if (checkIn == null)
            throw new InvalidOperationException("Check-in não encontrado.");

        await _checkInRepository.DeleteAsync(checkIn, cancellationToken);
    }

    public async Task<IEnumerable<CheckInDto>> ObterPorColaboradorIdAsync(int colaboradorId, CancellationToken cancellationToken = default)
    {
        var checkIns = await _checkInRepository.GetByColaboradorIdAsync(colaboradorId, cancellationToken);
        return checkIns.Select(MapToDto);
    }

    public async Task<IndicadoresBemEstarDto> ObterIndicadoresPorEquipeAsync(int equipeId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
    {
        var checkIns = await _checkInRepository.GetByEquipeIdAndPeriodoAsync(equipeId, dataInicio, dataFim, cancellationToken);
        var checkInsList = checkIns.ToList();

        if (!checkInsList.Any())
        {
            return new IndicadoresBemEstarDto
            {
                EquipeId = equipeId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                MediaHumor = 0,
                MediaEstresse = 0,
                MediaQualidadeSono = 0,
                TotalCheckIns = 0
            };
        }

        return new IndicadoresBemEstarDto
        {
            EquipeId = equipeId,
            EquipeNome = checkInsList.First().Colaborador?.Equipe?.Nome,
            DataInicio = dataInicio,
            DataFim = dataFim,
            MediaHumor = checkInsList.Average(c => c.Humor),
            MediaEstresse = checkInsList.Average(c => c.NivelEstresse),
            MediaQualidadeSono = checkInsList.Average(c => c.QualidadeSono),
            TotalCheckIns = checkInsList.Count
        };
    }

    private static CheckInDto MapToDto(CheckIn checkIn)
    {
        return new CheckInDto
        {
            Id = checkIn.Id,
            ColaboradorId = checkIn.ColaboradorId,
            ColaboradorNome = checkIn.Colaborador?.Nome,
            DataCheckIn = checkIn.DataCheckIn,
            Humor = checkIn.Humor,
            NivelEstresse = checkIn.NivelEstresse,
            QualidadeSono = checkIn.QualidadeSono,
            SintomasFisicos = checkIn.SintomasFisicos,
            Observacoes = checkIn.Observacoes
        };
    }
}

