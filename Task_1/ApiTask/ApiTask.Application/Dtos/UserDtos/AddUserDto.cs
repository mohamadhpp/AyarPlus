using ApiTask.Application.Attributes.Validation;
using ApiTask.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Application.Dtos.UserDtos
{
    public class AddUserDto
    {
        [DisplayName("نام")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        public string FirstName { get; set; }

        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        public string LastName { get; set; }

        [DisplayName("ایمیل")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        [PersianNumber]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "{0} وارد شده صحیح نیست!")]
        public string Email { get; set; }

        [DisplayName("شماره همراه")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        [PersianNumber]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "{0} وارد شده صحیح نیست!")]
        public string PhoneNumber { get; set; }

        [DisplayName("رمز عبور")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        [PersianNumber]
        [Length(minimumLength: 8, maximumLength: 25, ErrorMessage = "{0} باید حداقل 8 و حداکثر 25 کاراکتر باشد!")]
        public string Password { get; set; }

        [DisplayName("تکرار رمز عبور")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        [PersianNumber]
        [Compare("Password", ErrorMessage = "رمز عبور و تکرار رمز عبور یکسان نیست!")]
        public string ConfirmPassword { get; set; }

        [DisplayName("وضعیت")]
        [Required(ErrorMessage = "{0} ضروری است!")]
        public UserStatus Status { get; set; }
    }
}