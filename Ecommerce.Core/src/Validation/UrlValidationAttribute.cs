using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Ecommerce.Core.src.Validation
{
    public class UrlValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string url = value as string;
            if (url == null)
            {
                return ValidationResult.Success;
            }

            var urlRegex = new Regex(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$");
            if (!urlRegex.IsMatch(url))
            {
                return new ValidationResult("Invalid URL format");
            }

            return ValidationResult.Success;
        }
    }
}