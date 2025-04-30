namespace CryptoMonitor.Models
{
    public class ApiSource
    {
        public int SourceId { get; set; }
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<DataUpdateLog> DataUpdateLogs { get; set; }
    }
}
