using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.DAL.IRepositories
{
    public interface ICustomerInputRepository
    {
        Task UpdateAsync(CustomerInput customerInput);
        Task AddAsync(CustomerInput customerInput);
        Task<CustomerInput> GetByIdAsync(int id);
    }
}