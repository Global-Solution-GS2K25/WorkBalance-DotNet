using WorkBalanceHub.Application.DTOs;

namespace WorkBalanceHub.Application.Services;

public interface IEquipeService
{
    Task<EquipeDto> CriarAsync(CriarEquipeDto dto, CancellationToken cancellationToken = default);
    Task<EquipeDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<EquipeDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default);
    Task<EquipeDto> AtualizarAsync(int id, AtualizarEquipeDto dto, CancellationToken cancellationToken = default);
    Task DeletarAsync(int id, CancellationToken cancellationToken = default);
}

