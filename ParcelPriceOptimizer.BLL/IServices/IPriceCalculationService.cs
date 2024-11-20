using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface IPriceCalculationService
    {
        Task<Dictionary<string, decimal>> CalculatePriceAsync(CustomerInputViewModel input);
    }
}