using WorkBalanceHub.Application.DTOs;

namespace WorkBalanceHub.Application.Services;

public interface ILeituraAmbienteService
{
    Task<LeituraAmbienteDto> CriarAsync(CriarLeituraAmbienteDto dto, CancellationToken cancellationToken = default);
    Task<LeituraAmbienteDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResultDto<LeituraAmbienteDto>> BuscarAsync(SearchRequestDto request, CancellationToken cancellationToken = default);
    Task<LeituraAmbienteDto> AtualizarAsync(int id, AtualizarLeituraAmbienteDto dto, CancellationToken cancellationToken = default);
    Task DeletarAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<LeituraAmbienteDto>> ObterPorEstacaoTrabalhoIdAsync(int estacaoTrabalhoId, CancellationToken cancellationToken = default);
}

