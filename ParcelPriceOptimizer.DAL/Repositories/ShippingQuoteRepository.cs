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
    public class ShippingQuoteRepository : IShippingQuoteRepository
    {
        private readonly ApplicationDbContext _context;

        public ShippingQuoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShippingQuote>> GetAllAsync()
        {
            return await _context.ShippingQuotes.Include(sq => sq.Parcel)
                                                .Include(sq => sq.Courier)
                                                .ToListAsync();
        }
        public async Task<ShippingQuote> GetByIdAsync(int id)
        {
            return await _context.ShippingQuotes.Include(sq => sq.Parcel)
                                                .Include(sq => sq.Courier)
                                                .FirstOrDefaultAsync(sq => sq.Id == id);
        }
        public async Task AddAsync(ShippingQuote shippingQuote)
        {
            await _context.ShippingQuotes.AddAsync(shippingQuote);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ShippingQuote shippingQuote)
        {
            _context.ShippingQuotes.Update(shippingQuote);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var shippingQuote = await _context.ShippingQuotes.FindAsync(id);
            if (shippingQuote != null)
            {
                _context.ShippingQuotes.Remove(shippingQuote); 
                await _context.SaveChangesAsync();
            }
        }
    }
}