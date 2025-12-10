using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Domain.Entities;
using ApiTask.Infrastructure.Persistence.Contexts;
using ApiTask.Infrastructure.Persistence.Repositories.Common;

namespace ApiTask.Infrastructure.Persistence.Repositories.UserRepositories
{
    public class CompanyRepository : MSSQLRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(MSSQLContext dbContext) : base(dbContext) { }
    }
}