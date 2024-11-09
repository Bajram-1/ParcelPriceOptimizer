using ParcelPriceOptimizer.BLL.DTO.ViewModels;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface IPackageInputService
    {
        Task<PackageInputViewModel> CreatePackageInputAsync(PackageInputViewModel input);
        Task<IEnumerable<PackageInputViewModel>> GetAllCustomerInputsAsync();
        Task<PackageInputViewModel> GetCustomerInputByIdAsync(int id);
    }
}