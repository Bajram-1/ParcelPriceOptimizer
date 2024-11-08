using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.DAL.IRepositories
{
    public interface ICourierPricingRuleRepository
    {
        Task<IEnumerable<CourierPricingRule>> GetAllAsync();
        Task AddAsync(CourierPricingRule rule);
        Task<CourierPricingRule> GetByIdAsync(int id);
    }
}