using ApiTask.Application.Interfaces.Repositories.LocationRepositories;
using ApiTask.Domain.Entities.Location;
using ApiTask.Infrastructure.Persistence.Contexts;
using ApiTask.Infrastructure.Persistence.Repositories.Common;

namespace ApiTask.Infrastructure.Persistence.Repositories.LocationRepositories
{
    public class CountryRepository : MSSQLRepository<Country>, ICountryRepository
    {
        public CountryRepository(MSSQLContext dbContext) : base(dbContext) { }

        public void AddCountry(ref Country country)
        {
            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();
        }
    }
}