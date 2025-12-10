using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTask.Domain.Entities.Location
{
    public class City
    {
        public int Id { get; set; }
        public int ProvinceId { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(ProvinceId))]
        public Province? Province { get; set; }
    }
}