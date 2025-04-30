using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CryptoMonitor.Models.Validators
{
    public class EnglishOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Password is required");

            string password = value.ToString();
            
            // Проверка на наличие только английских букв, цифр и специальных символов
            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]*$"))
                return new ValidationResult("Password must contain only English letters, numbers and special characters");

            return ValidationResult.Success;
        }
    }
} 