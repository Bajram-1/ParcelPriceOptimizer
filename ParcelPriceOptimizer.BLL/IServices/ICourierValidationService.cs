using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface ICourierValidationService
    {
        bool IsValidForCompany1(PackageInputViewModel input);
        bool IsValidForCompany2(PackageInputViewModel input);
        bool IsValidForCompany3(PackageInputViewModel input);
    }
}