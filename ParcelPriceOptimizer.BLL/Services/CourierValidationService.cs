using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.IRepositories;

namespace ParcelPriceOptimizer.BLL.Services
{
    public class CourierValidationService : ICourierValidationService
    {
        public bool IsValidForCompany1(CustomerInputViewModel input)
        {
            return input.Weight <= 20 && input.Weight > 0 && input.Volume <= 2000;
        }

        public bool IsValidForCompany2(CustomerInputViewModel input)
        {
            return input.Weight > 10 && input.Weight <= 30 && input.Volume <= 1700;
        }

        public bool IsValidForCompany3(CustomerInputViewModel input)
        {
            return input.Weight >= 10 && input.Volume >= 500;
        }

        public bool IsValidForAnyCompany(CustomerInputViewModel input) 
        { 
            return IsValidForCompany1(input) || IsValidForCompany2(input) || IsValidForCompany3(input); 
        }
    }
}