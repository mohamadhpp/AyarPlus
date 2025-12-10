using ApiTask.Domain.Entities.Common;
using ApiTask.Domain.Enums;

namespace ApiTask.Domain.Entities
{
    public class User : Base
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Disable;
    }
}