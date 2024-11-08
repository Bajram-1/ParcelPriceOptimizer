using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.Entities
{
    public class CustomerInput : BaseEntityWithKey
    {
        public int ParcelId { get; set; }
        public Parcel Parcel { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    }
}
