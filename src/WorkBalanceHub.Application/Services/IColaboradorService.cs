using WorkBalanceHub.Application.DTOs;

namespace WorkBalanceHub.Application.Services;

public interface IColaboradorService
{
    Task<ColaboradorDto> CriarAsync(CriarColaboradorDto dto, CancellationToken cancellationToken = default);
    Task<ColaboradorDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<ColaboradorDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default);
    Task<ColaboradorDto> AtualizarAsync(int id, AtualizarColaboradorDto dto, CancellationToken cancellationToken = default);
    Task DeletarAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ColaboradorDto>> ObterPorEquipeIdAsync(int equipeId, CancellationToken cancellationToken = default);
}

