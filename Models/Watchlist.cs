namespace CryptoMonitor.Models
{
    public class Watchlist
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int AssetId { get; set; }
        public CryptoAsset Asset { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
