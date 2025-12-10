using ApiTask.Common.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ApiTask.Application.Attributes.Validation
{
    public class PersianNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is string input)
            {
                string convertedValue = CharacterHelper.ToEnglishNumbers(input);

                // Update the property with the converted value
                var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(validationContext.ObjectInstance, convertedValue);
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid input format.");
        }
    }
}
