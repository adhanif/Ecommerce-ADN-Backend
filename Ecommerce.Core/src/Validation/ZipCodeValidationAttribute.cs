using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Ecommerce.Core.src.Validation
{
    public class ZipCodeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string zipCode = value as string;
            var digitCheck = new Regex(@"^\d$");
            if (zipCode.Length != 5)
            {
                return new ValidationResult("Zip code must be 5 characters long");
            }
            if (zipCode.StartsWith("0"))
            {
                return new ValidationResult("Zip code cannot start with 0");
            }
            if (zipCode.Contains(" "))
            {
                return new ValidationResult("Zip code cannot contain spaces");
            }
            if (digitCheck.IsMatch(zipCode))
            {
                return new ValidationResult("Zip code contains only digits");
            }
            return ValidationResult.Success;
        }
    }
}