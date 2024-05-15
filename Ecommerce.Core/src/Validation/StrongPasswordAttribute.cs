using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Ecommerce.Core.src.Validation
{
    public class StrongPasswordAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || !(value is string password))
            {
                return new ValidationResult("Password must not be null or empty.");
            }

            // Regex pattern for a strong password:
            // At least one uppercase letter, one lowercase letter, one digit, one special character, and a minimum length of 6 characters
            var regexPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{6,}$";

            if (!Regex.IsMatch(password, regexPattern))
            {
                return new ValidationResult("Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 6 characters long.");
            }

            return ValidationResult.Success;
        }
    }
}