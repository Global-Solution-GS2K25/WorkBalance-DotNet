using WorkBalanceHub.Application.DTOs;

namespace WorkBalanceHub.Application.Services;

public interface IPlanoAcaoService
{
    Task<PlanoAcaoDto> CriarAsync(CriarPlanoAcaoDto dto, CancellationToken cancellationToken = default);
    Task<PlanoAcaoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<PlanoAcaoDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default);
    Task<PlanoAcaoDto> AtualizarAsync(int id, AtualizarPlanoAcaoDto dto, CancellationToken cancellationToken = default);
    Task DeletarAsync(int id, CancellationToken cancellationToken = default);
    Task<PlanoAcaoDto> AlterarStatusAsync(int id, AlterarStatusPlanoAcaoDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<PlanoAcaoDto>> ObterPorEquipeIdAsync(int equipeId, CancellationToken cancellationToken = default);
}

