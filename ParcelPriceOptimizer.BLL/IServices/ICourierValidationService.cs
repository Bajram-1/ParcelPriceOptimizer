using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface ICourierValidationService
    {
        Task<bool> IsValidForAnyCompanyAsync(CustomerInputViewModel input);
        Task<bool> IsValidForNoCompanyAsync(CustomerInputViewModel input);
    }
}