using WorkBalanceHub.Domain.Entities;
using WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Infrastructure.Data;

namespace WorkBalanceHub.Infrastructure.Repositories;

public class EquipeRepository : Repository<Equipe>, IRepository<Equipe>
{
    public EquipeRepository(WorkBalanceHubDbContext context) : base(context)
    {
    }
}

