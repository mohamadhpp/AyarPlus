using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Application.Attributes.Validation
{
    public class GuidValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Resolve IStringLocalizer from the ValidationContext
            var localizer = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer<GuidValidationAttribute>));

            if (value is Guid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(localizer["InvalidGuid"]);
        }
    }
}