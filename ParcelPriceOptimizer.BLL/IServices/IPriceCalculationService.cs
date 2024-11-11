using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface IPriceCalculationService
    {
        Task<decimal> CalculatePriceAsync(PackageInputViewModel input);
        Task<decimal> GetPriceRulingAsync(int courierId, PackageInputViewModel input);
    }
}