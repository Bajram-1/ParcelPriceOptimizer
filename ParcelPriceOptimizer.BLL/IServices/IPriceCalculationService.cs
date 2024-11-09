using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface IPriceCalculationService
    {
        decimal CalculatePrice(PackageInputViewModel input);
        decimal GetPriceRuling(int courierId, PackageInputViewModel input);
    }
}