using WorkBalanceHub.Application.DTOs;

namespace WorkBalanceHub.Application.Services;

public interface IEstacaoTrabalhoService
{
    Task<EstacaoTrabalhoDto> CriarAsync(CriarEstacaoTrabalhoDto dto, CancellationToken cancellationToken = default);
    Task<EstacaoTrabalhoDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<EstacaoTrabalhoDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default);
    Task<EstacaoTrabalhoDto> AtualizarAsync(int id, AtualizarEstacaoTrabalhoDto dto, CancellationToken cancellationToken = default);
    Task DeletarAsync(int id, CancellationToken cancellationToken = default);
}

