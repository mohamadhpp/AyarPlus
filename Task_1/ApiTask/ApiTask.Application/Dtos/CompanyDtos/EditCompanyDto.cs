using ApiTask.Application.Attributes.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Application.Dtos.CompanyDtos
{
    public class EditCompanyDto
    {
        [DisplayName("شناسه")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        public long Id { get; set; }

        [DisplayName("نام")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        public string Title { get; set; }

        [DisplayName("آدرس")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        public string Address { get; set; }

        [DisplayName("توضیحات")]
        public string? Description { get; set; }
    }
}
