using Microsoft.Extensions.DependencyInjection;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL;
using ParcelPriceOptimizer.DAL.IRepositories;
using ParcelPriceOptimizer.DAL.Repositories;

namespace ParcelPriceOptimizer.BLL.Services
{
    public class CourierPricingService : ICourierPricingService
    {
        private readonly IServiceProvider _serviceProvider;

        public CourierPricingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<decimal> GetPriceRulingAsync(int courierId, PackageInputViewModel input)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var pricingRuleRepository = new CourierPricingRuleRepository(context);
                var pricingRules = await pricingRuleRepository.GetByCourierIdAsync(courierId);

                if (pricingRules == null || !pricingRules.Any())
                {
                    throw new Exception("No pricing rules found for the specified courier.");
                }

                decimal packageVolume = input.Width * input.Height * input.Depth;
                decimal maxDimensionPrice = 0; decimal maxWeightPrice = 0;

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
        }
        public async Task<decimal> CalculateOptimalShippingPriceAsync(PackageInputViewModel input)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var courierRepository = new CourierRepository(context);
                var couriers = await courierRepository.GetAllAsync();
                var priceTasks = new List<Task<decimal>>();

                foreach (var courier in couriers)
                {
                    priceTasks.Add(GetPriceRulingAsync(courier.Id, input));
                }

                decimal[] prices;

                try
                {
                    prices = await Task.WhenAll(priceTasks);
                }
                catch (Exception)
                {
                    prices = new decimal[] { decimal.MaxValue };
                }

                var bestPrice = prices.Min();

                if (bestPrice == decimal.MaxValue)
                {
                    throw new Exception("No suitable pricing found for the input package.");
                }
                return bestPrice;
            }
        }
    }
}