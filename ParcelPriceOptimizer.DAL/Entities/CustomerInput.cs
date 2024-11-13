using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.Entities
{
    public class CustomerInput : BaseEntityWithKey
    {
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public decimal Volume => Width * Height * Depth;
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public string? StripeSessionId { get; set; } 
        public string? StripePaymentIntentId { get; set; }
        public bool IsPaymentCompleted { get; set; }
    }
}