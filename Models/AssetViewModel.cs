namespace CryptoMonitor.Models
{
    public class AssetViewModel
    {
        public int AssetId { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal? LatestPrice { get; set; }
        public decimal? MarketCap { get; set; }
    }
}
