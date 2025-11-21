namespace WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Domain.Entities;

public interface ILeituraAmbienteRepository : IRepository<LeituraAmbiente>
{
    Task<IEnumerable<LeituraAmbiente>> GetByEstacaoTrabalhoIdAsync(int estacaoTrabalhoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LeituraAmbiente>> GetByEstacaoTrabalhoIdAndPeriodoAsync(int estacaoTrabalhoId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);
    Task<IEnumerable<LeituraAmbiente>> GetUltimasLeiturasAsync(int estacaoTrabalhoId, int quantidade, CancellationToken cancellationToken = default);
}

