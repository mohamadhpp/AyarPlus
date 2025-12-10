using ApiTask.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ApiTask.Domain.Entities
{
    public class Company : Base
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
    }
}
