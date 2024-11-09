using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface ICourierPricingService
    {
        Task<decimal> GetPriceRulingAsync(int courierId, PackageInputViewModel input);
        Task<decimal> CalculateOptimalShippingPriceAsync(PackageInputViewModel input);
    }
}