using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface IShippingCostService
    {
        Task<decimal> GetShippingCostAsync(PackageInputViewModel input);
        Task<IEnumerable<decimal>> GetAllShippingCostsAsync(IEnumerable<PackageInputViewModel> inputs);
    }
}