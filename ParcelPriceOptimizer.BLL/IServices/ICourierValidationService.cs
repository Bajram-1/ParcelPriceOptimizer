using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface ICourierValidationService
    {
        bool IsValidForCompany1(CustomerInputViewModel input);
        bool IsValidForCompany2(CustomerInputViewModel input);
        bool IsValidForCompany3(CustomerInputViewModel input);
        bool IsValidForAnyCompany(CustomerInputViewModel input);
    }
}