using Microsoft.EntityFrameworkCore;
using ParcelPriceOptimizer.DAL.Entities;
using ParcelPriceOptimizer.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.Repositories
{
    public class CourierPricingRuleRepository : ICourierPricingRuleRepository
    {
        private readonly ApplicationDbContext _context;

        public CourierPricingRuleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourierPricingRule>> GetAllAsync()
        {
            return await _context.CourierPricingRules.ToListAsync();
        }

        public async Task AddAsync(CourierPricingRule rule)
        {
            await _context.AddAsync(rule);
            await _context.SaveChangesAsync();
        }

        public async Task<CourierPricingRule> GetByIdAsync(int id)
        {
            return await _context.CourierPricingRules.FindAsync(id);
        }

        public async Task<IEnumerable<CourierPricingRule>> GetByCourierIdAsync(int courierId)
        {
            return await _context.CourierPricingRules
                .Where(r => r.CourierId == courierId)
                .ToListAsync();
        }
    }
}