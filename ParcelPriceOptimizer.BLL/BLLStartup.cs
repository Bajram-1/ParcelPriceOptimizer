using Microsoft.Extensions.DependencyInjection;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.BLL.Services;

namespace ParcelPriceOptimizer.BLL
{
    public static class BLLStartup
    {
        public static void RegisterBLLServices(this IServiceCollection services)
        {
            services.AddScoped<ICourierValidationService, CourierValidationService>();
            services.AddScoped<ICustomerInputService, CustomerInputService>();
            services.AddScoped<IPriceCalculationService, PriceCalculationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}