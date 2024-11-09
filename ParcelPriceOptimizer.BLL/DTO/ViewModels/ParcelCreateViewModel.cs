using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.BLL.DTO.ViewModels
{
    public class ParcelCreateViewModel
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume => Width * Height * Depth;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CustomerInputId { get; set; }
        public CustomerInput CustomerInput { get; set; }
        public ICollection<ShippingQuote> ShippingQuotes { get; set; }
    }
}