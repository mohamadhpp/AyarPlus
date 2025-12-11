using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Domain.Entities;
using ApiTask.Infrastructure.Persistence.Contexts;
using ApiTask.Infrastructure.Persistence.Repositories.Common;

namespace ApiTask.Infrastructure.Persistence.Repositories
{
    public class ContractRepository : MSSQLRepository<Contract>, IContractRepository
    {
        public ContractRepository(MSSQLContext dbContext) : base(dbContext) { }

        public void Add(ref Contract contract)
        {
            _dbContext.Contracts.Add(contract);
            _dbContext.SaveChanges();
        }
    }
}