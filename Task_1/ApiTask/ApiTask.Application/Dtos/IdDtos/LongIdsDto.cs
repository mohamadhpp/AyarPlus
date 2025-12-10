using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Application.Dtos.IdDtos
{
    public class LongIdsDto
    {
        [DisplayName("شناسه")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        public long[] Ids { get; set; }
    }
}
