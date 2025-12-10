using ApiTask.Infrastructure.Persistence.Contexts;

namespace ApiTask.Infrastructure.Persistence.Repositories.Common
{
    public class MSSQLRepository<T>(MSSQLContext dbContext) : MSSQLGenericRepository<T>(dbContext) where T : class
    {
        protected readonly MSSQLContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
}