using Microsoft.EntityFrameworkCore;
using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Infrastructure.Data;

namespace WorkBalanceHub.Infrastructure.Repositories;

public class LeituraAmbienteRepository : Repository<LeituraAmbiente>, ILeituraAmbienteRepository
{
    public LeituraAmbienteRepository(WorkBalanceHubDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LeituraAmbiente>> GetByEstacaoTrabalhoIdAsync(int estacaoTrabalhoId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.EstacaoTrabalho)
            .Where(l => l.EstacaoTrabalhoId == estacaoTrabalhoId)
            .OrderByDescending(l => l.DataLeitura)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LeituraAmbiente>> GetByEstacaoTrabalhoIdAndPeriodoAsync(int estacaoTrabalhoId, DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.EstacaoTrabalho)
            .Where(l => l.EstacaoTrabalhoId == estacaoTrabalhoId && 
                       l.DataLeitura >= dataInicio && 
                       l.DataLeitura <= dataFim)
            .OrderByDescending(l => l.DataLeitura)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LeituraAmbiente>> GetUltimasLeiturasAsync(int estacaoTrabalhoId, int quantidade, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.EstacaoTrabalho)
            .Where(l => l.EstacaoTrabalhoId == estacaoTrabalhoId)
            .OrderByDescending(l => l.DataLeitura)
            .Take(quantidade)
            .ToListAsync(cancellationToken);
    }

    public override async Task<LeituraAmbiente?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(l => l.EstacaoTrabalho)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }
}

