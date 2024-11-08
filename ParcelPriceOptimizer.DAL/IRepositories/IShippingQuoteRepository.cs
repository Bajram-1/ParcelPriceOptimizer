using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.DAL.IRepositories
{
    public interface IShippingQuoteRepository
    {
        Task<IEnumerable<ShippingQuote>> GetAllAsync();
        Task<ShippingQuote> GetByIdAsync(int id);
        Task AddAsync(ShippingQuote shippingQuote);
        Task UpdateAsync(ShippingQuote shippingQuote);
        Task DeleteAsync(int id);
    }
}