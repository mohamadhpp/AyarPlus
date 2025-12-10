using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Domain.Entities;
using ApiTask.Infrastructure.Persistence.Contexts;
using ApiTask.Infrastructure.Persistence.Repositories.Common;

namespace ApiTask.Infrastructure.Persistence.Repositories
{
    public class UserRepository : MSSQLRepository<User>, IUserRepository
    {
        public UserRepository(MSSQLContext dbContext) : base(dbContext) { }
    }
}