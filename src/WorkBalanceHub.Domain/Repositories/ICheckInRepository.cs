namespace WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Domain.Entities;

public interface ICheckInRepository : IRepository<CheckIn>
{
    Task<IEnumerable<CheckIn>> GetByColaboradorIdAsync(int colaboradorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CheckIn>> GetByColaboradorIdAndPeriodoAsync(int colaboradorId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);
    Task<IEnumerable<CheckIn>> GetByEquipeIdAndPeriodoAsync(int equipeId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);
    Task<CheckIn?> GetCheckInHojeAsync(int colaboradorId, CancellationToken cancellationToken = default);
}

