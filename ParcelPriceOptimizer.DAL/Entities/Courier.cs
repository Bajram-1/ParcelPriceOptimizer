using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.Entities
{
    public class Courier : BaseEntityWithKey
    {
        public string Name { get; set; }
        public decimal MinWeight { get; set; }
        public decimal MaxWeight { get; set; }
        public decimal MinVolume { get; set; }
        public decimal MaxVolume { get; set; }
        public ICollection<CourierPricingRule> CourierPricingRules { get; set; }
    }
}