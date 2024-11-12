using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.Services
{
    public class PriceCalculationService : IPriceCalculationService
    {
        private readonly ICourierValidationService _validationService;
        private readonly ICustomerInputService _customerInputService;

        public PriceCalculationService(ICourierValidationService validationService, ICustomerInputService customerInputService)
        {
            _validationService = validationService;
            _customerInputService = customerInputService;
        }

        public decimal CalculatePrice(CustomerInputViewModel input)
        {
            if (!_validationService.IsValidForAnyCompany(input))
            {
                return -1;
            }

            decimal dimensionPrice = GetDimensionPrice(input);
            decimal weightPrice = GetWeightPrice(input);
            decimal finalPrice = Math.Max(dimensionPrice, weightPrice);

            return finalPrice;
        }

        private decimal GetDimensionPrice(CustomerInputViewModel input)
        {
            if (_validationService.IsValidForCompany1(input))
            {
                return (input.Volume <= 1000) ? 10 :
                       (input.Volume <= 2000) ? 20 : 0;
            }
            if (_validationService.IsValidForCompany2(input))
            {
                return (input.Volume <= 1000) ? 11.99M :
                       (input.Volume <= 1700) ? 21.99M : 0;
            }
            if (_validationService.IsValidForCompany3(input))
            {
                return (input.Volume <= 1000) ? 9.50M :
                       (input.Volume <= 2000) ? 19.50M :
                       (input.Volume <= 5000) ? 48.50M : 147.50M;
            }

            return 0;
        }

        private decimal GetWeightPrice(CustomerInputViewModel input)
        {
            if (_validationService.IsValidForCompany1(input))
            {
                return (input.Weight <= 2) ? 15 :
                       (input.Weight <= 15) ? 18 :
                       (input.Weight <= 20) ? 35 : 0;
            }
            if (_validationService.IsValidForCompany2(input))
            {
                return (input.Weight <= 15) ? 16.50M :
                       (input.Weight <= 25) ? 36.50M :
                       (input.Weight > 25) ? 40 + (0.417M * (input.Weight - 25)) : 0;
            }
            if (_validationService.IsValidForCompany3(input))
            {
                return (input.Weight <= 20) ? 16.99M :
                       (input.Weight <= 30) ? 33.99M :
                       (input.Weight > 30) ? 43.99M + (0.41M * (input.Weight - 25)) : 0;
            }

            return 0;
        }
    }
}