using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.IRepositories;
using ParcelPriceOptimizer.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.Services
{
    public class CourierPricingService : ICourierPricingService
    {
        private readonly ICourierPricingRuleRepository _courierPricingRuleRepository;
        private readonly ICourierRepository _courierRepository;
        private readonly IParcelRepository _parcelRepository;

        public CourierPricingService(ICourierPricingRuleRepository courierPricingRuleRepository, ICourierRepository courierRepository, IParcelRepository parcelRepository)
        {
            _courierPricingRuleRepository = courierPricingRuleRepository;
            _courierRepository = courierRepository;
            _parcelRepository = parcelRepository;
        }

        public async Task<decimal> GetPriceRulingAsync(int courierId, PackageInputViewModel input)
        {
            var pricingRules = await _courierPricingRuleRepository.GetByCourierIdAsync(courierId);

            if (pricingRules == null || !pricingRules.Any())
            {
                throw new Exception("No pricing rules found for the specified courier.");
            }

            decimal packageVolume = input.Width * input.Height * input.Depth;

            decimal maxDimensionPrice = 0;
            decimal maxWeightPrice = 0;

            foreach (var rule in pricingRules)
            {
                if (packageVolume >= rule.MinVolume && packageVolume <= rule.MaxVolume)
                {
                    maxDimensionPrice = Math.Max(maxDimensionPrice, rule.DimensionPrice);
                }

                if (input.Weight >= rule.MinWeight && input.Weight <= rule.MaxWeight)
                {
                    maxWeightPrice = Math.Max(maxWeightPrice, rule.WeightPrice);
                }
            }

            decimal finalPrice = Math.Max(maxDimensionPrice, maxWeightPrice);

            if (finalPrice == 0)
            {
                throw new Exception("No valid pricing rule found for the given package dimensions and weight.");
            }
            return finalPrice;
        }
        public async Task<decimal> CalculateOptimalShippingPriceAsync(PackageInputViewModel input)
        {
            var couriers = await _courierRepository.GetAllAsync();

            var priceTasks = couriers.Select(async courier =>
            {
                try
                {
                    return await GetPriceRulingAsync(courier.Id, input);
                }
                catch (Exception ex)
                {
                    return decimal.MaxValue;
                }
            });

            var prices = await Task.WhenAll(priceTasks);

            var bestPrice = prices.Min();

            if (bestPrice == decimal.MaxValue)
            {
                throw new Exception("No suitable pricing found for the input package.");
            }

            return bestPrice;
        }
    }
}