
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApplication1.ValidationAttributes
{
    public class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            if (string.IsNullOrEmpty(email))
            {
                return new ValidationResult("Email is required.");
            }

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!regex.IsMatch(email))
            {
                return new ValidationResult("Invalid email format.");
            }

            return ValidationResult.Success;
        }
    }
}
