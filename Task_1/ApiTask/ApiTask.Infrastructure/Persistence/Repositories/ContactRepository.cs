using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Domain.Entities;
using ApiTask.Infrastructure.Persistence.Contexts;
using ApiTask.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTask.Infrastructure.Persistence.Repositories
{
    public class ContactRepository : MSSQLRepository<Contact>, IContactRepository
    {
        public ContactRepository(MSSQLContext dbContext) : base(dbContext) { }
    }
}