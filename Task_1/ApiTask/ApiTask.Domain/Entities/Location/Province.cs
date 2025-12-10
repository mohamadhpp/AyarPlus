using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTask.Domain.Entities.Location
{
    public class Province
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country? Country { get; set; }
        public virtual List<City>? Cities { get; set; } = new();
    }
}
