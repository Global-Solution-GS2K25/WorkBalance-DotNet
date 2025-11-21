using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Infrastructure.Data;

namespace WorkBalanceHub.Infrastructure.Repositories;

public class EstacaoTrabalhoRepository : Repository<EstacaoTrabalho>, IRepository<EstacaoTrabalho>
{
    public EstacaoTrabalhoRepository(WorkBalanceHubDbContext context) : base(context)
    {
    }
}

