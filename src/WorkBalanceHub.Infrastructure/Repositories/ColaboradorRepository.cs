using Microsoft.EntityFrameworkCore;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Infrastructure.Data;

namespace WorkBalanceHub.Infrastructure.Repositories;

public class ColaboradorRepository : Repository<Colaborador>, IColaboradorRepository
{
    public ColaboradorRepository(WorkBalanceHubDbContext context) : base(context)
    {
    }

    public async Task<Colaborador?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Equipe)
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<Colaborador>> GetByEquipeIdAsync(int equipeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Equipe)
            .Where(c => c.EquipeId == equipeId && c.Ativo)
            .ToListAsync(cancellationToken);
    }

    public override async Task<Colaborador?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Equipe)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}

