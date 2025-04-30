namespace CryptoMonitor.Models
{
    public class PriceHistory
    {
        public int HistoryId { get; set; }
        public int AssetId { get; set; }
        public CryptoAsset Asset { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal PriceUsd { get; set; }
        public decimal Volume24h { get; set; }
        public decimal MarketCap { get; set; }
    }
}
