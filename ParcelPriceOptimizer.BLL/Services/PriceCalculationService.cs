using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.IRepositories;
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
        private readonly ICourierPriceRulingRepository _repository;

        public PriceCalculationService(
            ICourierValidationService validationService,
            ICourierPriceRulingRepository repository)
        {
            _validationService = validationService;
            _repository = repository;
        }

        public async Task<Dictionary<string, decimal>> CalculatePriceAsync(CustomerInputViewModel input)
        {
            var companyPrices = new Dictionary<string, decimal>();
            var pricingRules = await _repository.GetPricingRulesAsync();

            foreach (var rule in pricingRules)
            {
                decimal dimensionPrice = 0;
                decimal weightPrice = 0;

                if (input.Volume >= rule.MinVolume && input.Volume <= rule.MaxVolume)
                {
                    dimensionPrice = rule.DimensionPrice;
                }
                if (input.Weight >= rule.MinWeight && input.Weight <= rule.MaxWeight)
                {
                    weightPrice = rule.WeightPrice;
                }

                decimal finalPrice = Math.Max(dimensionPrice, weightPrice); 
                
                string key = rule.Courier?.Name ?? "Unknown Courier"; 
                
                if (finalPrice > 0) 
                { 
                   companyPrices[key] = finalPrice; 
                }
            }
            return companyPrices;
        }
    }
}