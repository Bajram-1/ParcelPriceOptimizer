using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.BLL.DTO.ViewModels
{
    public class PackageInputViewModel
    {
        //public int ParcelId { get; set; }
        //public Parcel Parcel { get; set; }
        //public string UserId { get; set; }
        //public ApplicationUser User { get; set; }

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public decimal Weight { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public decimal Volume => Width * Height * Depth;
    }
}