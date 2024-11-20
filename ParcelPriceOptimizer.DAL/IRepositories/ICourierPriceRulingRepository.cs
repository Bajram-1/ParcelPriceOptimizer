using ParcelPriceOptimizer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.IRepositories
{
    public interface ICourierPriceRulingRepository
    {
        Task<List<CourierPricingRule>> GetPricingRulesAsync();
    }
}
