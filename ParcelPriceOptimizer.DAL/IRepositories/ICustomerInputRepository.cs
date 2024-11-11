using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.DAL.IRepositories
{
    public interface ICustomerInputRepository
    {
        Task AddAsync(CustomerInput customerInput);
    }
}