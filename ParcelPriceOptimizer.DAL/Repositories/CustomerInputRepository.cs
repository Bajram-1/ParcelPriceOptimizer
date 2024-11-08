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
    public class CustomerInputRepository : ICustomerInputRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerInputRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerInput>> GetAllAsync()
        {
            return await _context.CustomerInputs.Include(ci => ci.Parcel)
                                                .Include(ci => ci.User)
                                                .ToListAsync();
        }
        public async Task<CustomerInput> GetByIdAsync(int id)
        {
            return await _context.CustomerInputs.Include(ci => ci.Parcel)
                                                .Include(ci => ci.User)
                                                .FirstOrDefaultAsync(ci => ci.Id == id);
        }
        public async Task AddAsync(CustomerInput customerInput)
        {
            await _context.CustomerInputs.AddAsync(customerInput);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(CustomerInput customerInput)
        {
            _context.CustomerInputs.Update(customerInput);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var customerInput = await _context.CustomerInputs.FindAsync(id);
            if (customerInput != null)
            {
                _context.CustomerInputs.Remove(customerInput); 
                await _context.SaveChangesAsync();
            }
        }
    }
}