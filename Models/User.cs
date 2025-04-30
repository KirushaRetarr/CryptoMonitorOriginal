using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoMonitor.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public DateTime RegisteredAt { get; set; }
        public ICollection<Watchlist> Watchlists { get; set; }
        public ICollection<ReportLog> ReportLogs { get; set; }
    }
}
