using ApiTask.Application.Interfaces.Repositories.LocationRepositories;
using ApiTask.Domain.Entities.Location;
using ApiTask.Infrastructure.Persistence.Contexts;
using ApiTask.Infrastructure.Persistence.Repositories.Common;

namespace ApiTask.Infrastructure.Persistence.Repositories.LocationRepositories
{
    public class CityRepository : MSSQLRepository<City>, ICityRepository
    {
        public CityRepository(MSSQLContext dbContext) : base(dbContext) { }
    }
}