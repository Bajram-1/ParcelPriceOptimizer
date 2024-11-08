using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.DAL.IRepositories
{
    public interface ICustomerInputRepository
    {
        Task<IEnumerable<CustomerInput>> GetAllAsync();
        Task<CustomerInput> GetByIdAsync(int id);
        Task AddAsync(CustomerInput customerInput);
        Task UpdateAsync(CustomerInput customerInput);
        Task DeleteAsync(int id);
    }
}