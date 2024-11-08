using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParcelPriceOptimizer.DAL.IRepositories;
using ParcelPriceOptimizer.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL
{
    public static class DALStartup
    {
        public static void RegisterDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ICourierRepository, CourierRepository>();
            services.AddScoped<ICourierPricingRuleRepository, CourierPricingRuleRepository>();
            services.AddScoped<ICustomerInputRepository, CustomerInputRepository>();
            services.AddScoped<IParcelRepository, ParcelRepository>();
            services.AddScoped<IShippingQuoteRepository, ShippingQuoteRepository>();
        }
    }
}