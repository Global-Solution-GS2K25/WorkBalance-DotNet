using WorkBalanceHub.Application.DTOs;

namespace WorkBalanceHub.Application.Services;

public interface ICheckInService
{
    Task<CheckInDto> CriarAsync(CriarCheckInDto dto, CancellationToken cancellationToken = default);
    Task<CheckInDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<CheckInDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default);
    Task<CheckInDto> AtualizarAsync(int id, AtualizarCheckInDto dto, CancellationToken cancellationToken = default);
    Task DeletarAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CheckInDto>> ObterPorColaboradorIdAsync(int colaboradorId, CancellationToken cancellationToken = default);
    Task<IndicadoresBemEstarDto> ObterIndicadoresPorEquipeAsync(int equipeId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);
}

