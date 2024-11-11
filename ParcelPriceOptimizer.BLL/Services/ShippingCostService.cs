using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.Services
{
    public class ShippingCostService : IShippingCostService
    {
        private readonly IPriceCalculationService _priceCalculationService;

        public ShippingCostService(IPriceCalculationService priceCalculationService)
        {
            _priceCalculationService = priceCalculationService;
        }

        public async Task<decimal> GetShippingCostAsync(PackageInputViewModel input) 
        { 
            return await _priceCalculationService.CalculatePriceAsync(input); 
        }

        public async Task<IEnumerable<decimal>> GetAllShippingCostsAsync(IEnumerable<PackageInputViewModel> inputs)
        {
            var costs = new List<decimal>();
            foreach (var input in inputs)
            {
                costs.Add(await GetShippingCostAsync(input));
            }
            return costs;
        }
    }
}