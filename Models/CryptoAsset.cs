namespace CryptoMonitor.Models
{
    public class CryptoAsset
    {
        public int AssetId { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<PriceHistory> PriceHistories { get; set; }

    }
}
