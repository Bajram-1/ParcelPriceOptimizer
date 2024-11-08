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
    public class ParcelRepository : IParcelRepository
    {
        private readonly List<Parcel> _parcels = new();
        private readonly ApplicationDbContext _context;

        public ParcelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Parcel>> GetAllAsync()
        {
            return await _context.Parcels.Include(p => p.CustomerInput)
                                         .Include(p => p.ShippingQuotes)
                                         .ToListAsync();
        }
        public async Task<Parcel> GetByIdAsync(int id)
        {
            return await _context.Parcels.Include(p => p.CustomerInput)
                                         .Include(p => p.ShippingQuotes)
                                         .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Parcel parcel)
        {
            parcel.Id = _parcels.Count + 1;
            _parcels.Add(parcel);
            await Task.CompletedTask;
        }
        public async Task UpdateAsync(Parcel parcel)
        {
            var existingParcel = _parcels.FirstOrDefault(p => p.Id == parcel.Id);
            if (existingParcel != null)
            {
                existingParcel.Width = parcel.Width;
                existingParcel.Height = parcel.Height;
                existingParcel.Depth = parcel.Depth;
                existingParcel.Weight = parcel.Weight;
                existingParcel.Price = parcel.Price;
            }
            await Task.CompletedTask;
        }
        public async Task DeleteAsync(int id)
        {
            var parcel = _parcels.FirstOrDefault(p => p.Id == id);
            if (parcel != null)
            {
                _parcels.Remove(parcel);
            }
            await Task.CompletedTask;
        }
    }
}
