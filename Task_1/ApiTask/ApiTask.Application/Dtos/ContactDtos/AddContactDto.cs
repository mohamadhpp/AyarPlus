using ApiTask.Application.Attributes.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Application.Dtos.ContactDtos
{
    public class AddContactDto
    {
        [DisplayName("شناسه شرکت")]
        public long? CompanyId { get; set; }

        [DisplayName("شناسه کاربر")]
        public Guid? UserId { get; set; }

        [DisplayName("شناسه شهر")]
        public int? CityId { get; set; }

        [DisplayName("نام")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        public string Name { get; set; }

        [DisplayName("نوع")]
        [MaxLength(50, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        public string? Type { get; set; }

        [DisplayName("ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        [EmailAddress(ErrorMessage = "فرمت {0} صحیح نیست!")]
        public string? Email { get; set; }

        [DisplayName("تلفن")]
        [MaxLength(50, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        [Phone(ErrorMessage = "فرمت {0} صحیح نیست!")]
        public string? Phone { get; set; }

        [DisplayName("شماره مالیاتی")]
        [MaxLength(100, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        public string? TaxNumber { get; set; }

        [DisplayName("آدرس")]
        [MaxLength(500, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        public string? Address { get; set; }

        [DisplayName("کد پستی")]
        [MaxLength(20, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        public string? ZipCode { get; set; }

        [DisplayName("استان")]
        [MaxLength(100, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        public string? State { get; set; }

        [DisplayName("وب‌سایت")]
        [MaxLength(200, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        [Url(ErrorMessage = "فرمت {0} صحیح نیست!")]
        public string? Website { get; set; }

        [DisplayName("مرجع")]
        [MaxLength(100, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        public string? Reference { get; set; }

        [DisplayName("شماره پرونده")]
        [MaxLength(50, ErrorMessage = "{0} نمی‌تواند بیشتر از {1} کاراکتر باشد!")]
        public string? FileNumber { get; set; }

        [DisplayName("تصویر صفحه")]
        [FileSize(5 * 1024 * 1024, ErrorMessage = "حجم {0} نمی‌تواند بیشتر از 5 مگابایت باشد!")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "فرمت {0} مجاز نیست!")]
        public IFormFile? Front { get; set; }

        [DisplayName("تصویر پشت صفحه")]
        [FileSize(5 * 1024 * 1024, ErrorMessage = "حجم {0} نمی‌تواند بیشتر از 5 مگابایت باشد!")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" }, ErrorMessage = "فرمت {0} مجاز نیست!")]
        public IFormFile? Back { get; set; }
    }
}