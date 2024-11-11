using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.BLL.IServices
{
    public interface ICustomerInputService
    {
        Task SaveCustomerInputAsync(CustomerInputViewModel input, decimal price);
    }
}