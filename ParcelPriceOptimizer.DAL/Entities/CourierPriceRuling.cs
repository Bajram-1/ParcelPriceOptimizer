using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.Entities
{
    public class CourierPricingRule : BaseEntityWithKey
    {
        public int CourierId { get; set; }
        public Courier Courier { get; set; }
        public decimal MinVolume { get; set; }
        public decimal MaxVolume { get; set; }
        public decimal DimensionPrice { get; set; }
        public decimal MinWeight { get; set; }
        public decimal MaxWeight { get; set; }
        public decimal WeightPrice { get; set; }
    }
}