using ApiTask.Domain.Entities.Common;
using ApiTask.Domain.Entities.Location;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTask.Domain.Entities
{
    public class Contact : Base
    {
        public Guid Id { get; set; }
        public long? CompanyId { get; set; }
        public Guid? UserId { get; set; }
        public int? CityId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }

        [MaxLength(200)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(50)]
        [Phone]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? TaxNumber { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }

        [MaxLength(20)]
        public string? ZipCode { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(200)]
        [Url]
        public string? Website { get; set; }

        [MaxLength(100)]
        public string? Reference { get; set; }

        [MaxLength(50)]
        public string? FileNumber { get; set; }

        [MaxLength(500)]
        public string? Front { get; set; }

        [MaxLength(500)]
        public string? Back { get; set; }


        [ForeignKey(nameof(CityId))]
        public City? City { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company? Company { get; set; }
    }
}