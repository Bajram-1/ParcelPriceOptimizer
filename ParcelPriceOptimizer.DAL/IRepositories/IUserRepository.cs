using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.DAL.IRepositories
{
    public interface IUserRepository
    {
        string GetCurrentUserId();
        Task<ApplicationUser> GetByIdAsync(string userId);
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
    }
}