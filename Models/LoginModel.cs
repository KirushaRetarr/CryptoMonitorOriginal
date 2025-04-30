using System.ComponentModel.DataAnnotations;
using CryptoMonitor.Models.Validators;

namespace CryptoMonitor.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailDomain(ErrorMessage = "Invalid email domain")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [EnglishOnly(ErrorMessage = "Password must contain only English characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
} 