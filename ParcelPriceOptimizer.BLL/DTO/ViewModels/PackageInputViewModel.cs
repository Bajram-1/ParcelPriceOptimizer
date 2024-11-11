using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.BLL.DTO.ViewModels
{
    public class PackageInputViewModel
    {
        public string UserId { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public decimal Weight { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public decimal Volume => Width * Height * Depth;
    }
}