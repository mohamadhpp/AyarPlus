using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Application.Attributes.Validation
{
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSizeBytes;

        // Constructor to accept the max size in bytes
        public FileSizeAttribute(int maxSizeBytes)
        {
            _maxSizeBytes = maxSizeBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxSizeBytes)
                {
                    return new ValidationResult(ErrorMessage ?? $"حجم فایل بیشتر از حد مجاز است.");
                }
            }

            // If there's no file or the size is okay, validation passes
            return ValidationResult.Success;
        }
    }
}
