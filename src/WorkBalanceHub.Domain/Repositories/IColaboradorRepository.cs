namespace WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Domain.Entities;

public interface IColaboradorRepository : IRepository<Colaborador>
{
    Task<Colaborador?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Colaborador>> GetByEquipeIdAsync(int equipeId, CancellationToken cancellationToken = default);
}

