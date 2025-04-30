using System.ComponentModel.DataAnnotations;
using CryptoMonitor.Models.Validators;

namespace CryptoMonitor.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailDomain(ErrorMessage = "Invalid email domain")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [EnglishOnly(ErrorMessage = "Password must contain only English characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
} 