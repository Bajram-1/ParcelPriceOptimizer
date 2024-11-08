using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.Entities
{
    public class ShippingQuote : BaseEntityWithKey
    {
        public int ParcelId { get; set; }
        public Parcel Parcel { get; set; }
        public int CourierId { get; set; }
        public Courier Courier { get; set; }
        public decimal CalculatedPrice { get; set; }
        public DateTime QuotedAt { get; set; } = DateTime.UtcNow;
    }
}