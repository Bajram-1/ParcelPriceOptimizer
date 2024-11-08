using Microsoft.EntityFrameworkCore;
using ParcelPriceOptimizer.DAL.Entities;
using ParcelPriceOptimizer.DAL.IRepositories;

namespace ParcelPriceOptimizer.DAL.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly ApplicationDbContext _context;

        public CourierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Courier>> GetAllAsync()
        {
            return await _context.Couriers.Include(c => c.CourierPricingRules)
                                          .Include(c => c.ShippingQuotes)
                                          .ToListAsync();
        }

        public async Task<Courier> GetByIdAsync(int id)
        {
            return await _context.Couriers.Include(c => c.CourierPricingRules)
                                          .Include(c => c.ShippingQuotes)
                                          .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task AddAsync(Courier courier)
        {
            await _context.Couriers.AddAsync(courier);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Courier courier)
        {
            _context.Couriers.Update(courier);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var courier = await _context.Couriers.FindAsync(id);
            if (courier != null)
            {
                _context.Couriers.Remove(courier);
                await _context.SaveChangesAsync();
            }
        }
    }
}