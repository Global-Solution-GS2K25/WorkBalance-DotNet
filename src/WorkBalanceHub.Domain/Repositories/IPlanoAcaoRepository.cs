namespace WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Domain.Entities;

public interface IPlanoAcaoRepository : IRepository<PlanoAcao>
{
    Task<IEnumerable<PlanoAcao>> GetByEquipeIdAsync(int equipeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PlanoAcao>> GetByEquipeIdAndPeriodoAsync(int equipeId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);
    Task<IEnumerable<PlanoAcao>> GetAtivosAsync(CancellationToken cancellationToken = default);
}

