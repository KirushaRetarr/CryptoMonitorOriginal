namespace CryptoMonitor.Models
{
    public class CryptoCardModel
    {
        public int AssetId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal? LatestPrice { get; set; }
    }
}
