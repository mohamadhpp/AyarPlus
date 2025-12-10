using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Application.Dtos.IdDtos
{
    public class GuidIdsDto
    {
        [DisplayName("شناسه")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        public Guid[] Ids { get; set; }
    }
}