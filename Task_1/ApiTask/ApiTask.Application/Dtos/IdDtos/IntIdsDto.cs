using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Application.Dtos.IdDtos
{
    public class IntIdsDto
    {
        [DisplayName("شناسه")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        public int[] Ids { get; set; }
    }
}
