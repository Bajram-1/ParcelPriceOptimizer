using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParcelPriceOptimizer.DAL.Entities;
using ParcelPriceOptimizer.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.Repositories
{
    public class CourierPriceRulingRepository : ICourierPriceRulingRepository
    {
        private readonly ApplicationDbContext _context;

        public CourierPriceRulingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourierPricingRule>> GetPricingRulesAsync()
        {
            return await _context.CourierPricingRules
                                 .Include(rule => rule.Courier)
                                 .ToListAsync();
        }
    }
}
