using Microsoft.EntityFrameworkCore;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Infrastructure.Data;

namespace WorkBalanceHub.Infrastructure.Repositories;

public class CheckInRepository : Repository<CheckIn>, ICheckInRepository
{
    public CheckInRepository(WorkBalanceHubDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CheckIn>> GetByColaboradorIdAsync(int colaboradorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Colaborador)
                .ThenInclude(c => c!.Equipe)
            .Where(c => c.ColaboradorId == colaboradorId)
            .OrderByDescending(c => c.DataCheckIn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CheckIn>> GetByColaboradorIdAndPeriodoAsync(int colaboradorId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Colaborador)
                .ThenInclude(c => c!.Equipe)
            .Where(c => c.ColaboradorId == colaboradorId && 
                       c.DataCheckIn >= dataInicio && 
                       c.DataCheckIn <= dataFim)
            .OrderByDescending(c => c.DataCheckIn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CheckIn>> GetByEquipeIdAndPeriodoAsync(int equipeId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Colaborador)
                .ThenInclude(c => c!.Equipe)
            .Where(c => c.Colaborador!.EquipeId == equipeId && 
                       c.DataCheckIn >= dataInicio && 
                       c.DataCheckIn <= dataFim)
            .OrderByDescending(c => c.DataCheckIn)
            .ToListAsync(cancellationToken);
    }

    public async Task<CheckIn?> GetCheckInHojeAsync(int colaboradorId, CancellationToken cancellationToken = default)
    {
        var hoje = DateTime.UtcNow.Date;
        var amanha = hoje.AddDays(1);
        
        return await _dbSet
            .Include(c => c.Colaborador)
                .ThenInclude(c => c!.Equipe)
            .FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId && 
                                     c.DataCheckIn >= hoje && 
                                     c.DataCheckIn < amanha, cancellationToken);
    }

    public override async Task<CheckIn?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Colaborador)
                .ThenInclude(c => c!.Equipe)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
}

