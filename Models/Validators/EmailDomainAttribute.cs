using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CryptoMonitor.Models.Validators
{
    public class EmailDomainAttribute : ValidationAttribute
    {
        private readonly string[] _allowedDomains = new[]
        {
            "gmail.com",
            "mail.ru",
            "ku.edu.kz",
            "yandex.ru",
            "inbox.ru",
            "yandex.com",
            "gmail.ru"
        };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Email is required");

            string email = value.ToString();
            
            // Проверка на наличие @
            if (!email.Contains("@"))
                return new ValidationResult("Email must contain @ symbol");

            // Проверка домена
            string domain = email.Split('@')[1].ToLower();
            if (!_allowedDomains.Contains(domain))
                return new ValidationResult($"Email domain must be one of: {string.Join(", ", _allowedDomains)}");

            return ValidationResult.Success;
        }
    }
} 