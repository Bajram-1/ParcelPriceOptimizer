using Microsoft.Extensions.DependencyInjection;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.BLL.Services;

namespace ParcelPriceOptimizer.BLL
{
    public static class BLLStartup
    {
        public static void RegisterBLLServices(this IServiceCollection services)
        {
            services.AddScoped<ICourierPricingService, CourierPricingService>();
            services.AddScoped<ICourierValidationService, CourierValidationService>();
            services.AddScoped<ICustomerInputService, CustomerInputService>();
            services.AddScoped<IPriceCalculationService, PriceCalculationService>();
            services.AddScoped<IShippingCostService, ShippingCostService>();
        }
    }
}