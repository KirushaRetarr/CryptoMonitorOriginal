using System.ComponentModel.DataAnnotations;

namespace CryptoMonitor.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
} 