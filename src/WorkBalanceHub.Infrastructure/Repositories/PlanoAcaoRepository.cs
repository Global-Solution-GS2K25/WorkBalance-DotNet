using Microsoft.EntityFrameworkCore;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Infrastructure.Data;

namespace WorkBalanceHub.Infrastructure.Repositories;

public class PlanoAcaoRepository : Repository<PlanoAcao>, IPlanoAcaoRepository
{
    public PlanoAcaoRepository(WorkBalanceHubDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PlanoAcao>> GetByEquipeIdAsync(int equipeId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Equipe)
            .Where(p => p.EquipeId == equipeId)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PlanoAcao>> GetByEquipeIdAndPeriodoAsync(int equipeId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Equipe)
            .Where(p => p.EquipeId == equipeId && 
                       p.DataInicio >= dataInicio && 
                       p.DataFim <= dataFim)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PlanoAcao>> GetAtivosAsync(CancellationToken cancellationToken = default)
    {
        var hoje = DateTime.UtcNow.Date;
        return await _dbSet
            .Include(p => p.Equipe)
            .Where(p => p.Status != StatusPlanoAcao.Cancelado && 
                       p.Status != StatusPlanoAcao.Concluido &&
                       hoje >= p.DataInicio.Date && 
                       hoje <= p.DataFim.Date)
            .OrderByDescending(p => p.DataCriacao)
            .ToListAsync(cancellationToken);
    }

    public override async Task<PlanoAcao?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Equipe)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}

