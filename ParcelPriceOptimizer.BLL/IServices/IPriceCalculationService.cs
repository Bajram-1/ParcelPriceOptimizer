using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface IPriceCalculationService
    {
        decimal CalculatePrice(CustomerInputViewModel input);
    }
}