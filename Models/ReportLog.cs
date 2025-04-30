namespace CryptoMonitor.Models
{
    public class ReportLog
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string ReportType { get; set; }
        public string Format { get; set; }       // PDF, Excel, CSV, JSON
        public DateTime CreatedAt { get; set; }
        public string FilePath { get; set; }
    }
}
