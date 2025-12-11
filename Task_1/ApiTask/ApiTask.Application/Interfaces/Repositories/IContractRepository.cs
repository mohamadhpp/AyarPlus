using ApiTask.Application.Interfaces.Repositories.Common;
using ApiTask.Domain.Entities;

namespace ApiTask.Application.Interfaces.Repositories
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        public void Add(ref Contract contract);
    }
}