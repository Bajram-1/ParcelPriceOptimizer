using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.DAL.IRepositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByIdAsync(string userId);
    }
}