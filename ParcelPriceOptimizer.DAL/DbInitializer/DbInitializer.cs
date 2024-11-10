using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParcelPriceOptimizer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db; private readonly ILogger<DbInitializer> _logger; private readonly UserManager<ApplicationUser> _userManager; private readonly RoleManager<IdentityRole> _roleManager; public DbInitializer(ApplicationDbContext db, ILogger<DbInitializer> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) { _db = db; _logger = logger; _userManager = userManager; _roleManager = roleManager; }
        public async Task InitializeAsync()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    await _db.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying migrations.");
            }
            if (!_db.Couriers.Any())
            {
                var couriers = new List<Courier> {
                    new Courier { Name = "Company 1" },
                    new Courier { Name = "Company 2" },
                    new Courier { Name = "Company 3" }
                };

                _db.Couriers.AddRange(couriers);
                await _db.SaveChangesAsync();
                var company1 = await _db.Couriers.FirstOrDefaultAsync(c => c.Name == "Company 1");
                var company2 = await _db.Couriers.FirstOrDefaultAsync(c => c.Name == "Company 2");
                var company3 = await _db.Couriers.FirstOrDefaultAsync(c => c.Name == "Company 3");
                var pricingRules = new List<CourierPricingRule> {
                    //Company 1 Ruling
                    new CourierPricingRule 
                    { 
                        CourierId = company1.Id, 
                        MinVolume = 0, 
                        MaxVolume = 1000, 
                        DimensionPrice = 10, 
                        MinWeight = 0, 
                        MaxWeight = 2, 
                        WeightPrice = 15 
                    }, 
                    new CourierPricingRule 
                    { 
                        CourierId = company1.Id, 
                        MinVolume = 1001, 
                        MaxVolume = 2000, 
                        DimensionPrice = 20, 
                        MinWeight = 2.1m, 
                        MaxWeight = 15, 
                        WeightPrice = 18 
                    }, 
                    new CourierPricingRule 
                    { 
                        CourierId = company1.Id, 
                        MinVolume = 0, 
                        MaxVolume = 2000, 
                        DimensionPrice = 20, 
                        MinWeight = 15.1m, 
                        MaxWeight = 20, 
                        WeightPrice = 35 
                    }, 
                    //Company 2 ruling
                    new CourierPricingRule 
                    { 
                        CourierId = company2.Id, 
                        MinVolume = 0, 
                        MaxVolume = 1000, 
                        DimensionPrice = 11.99m, 
                        MinWeight = 10.1m, 
                        MaxWeight = 15, 
                        WeightPrice = 16.5m 
                    }, 
                    new CourierPricingRule 
                    { 
                        CourierId = company2.Id, 
                        MinVolume = 1001, 
                        MaxVolume = 1700, 
                        DimensionPrice = 21.99m, 
                        MinWeight = 15.1m, 
                        MaxWeight = 25, 
                        WeightPrice = 36.5m 
                    }, 
                    new CourierPricingRule 
                    { 
                        CourierId = company2.Id, 
                        MinVolume = 0, 
                        MaxVolume = 1700, 
                        DimensionPrice = 21.99m, 
                        MinWeight = 25.1m, 
                        MaxWeight = 30, 
                        WeightPrice = 40 
                    }, 
                    //Company 3 ruling
                    new CourierPricingRule 
                    { 
                        CourierId = company3.Id, 
                        MinVolume = 0, 
                        MaxVolume = 1000, 
                        DimensionPrice = 9.5m, 
                        MinWeight = 10, 
                        MaxWeight = 20, 
                        WeightPrice = 16.99m 
                    }, 
                    new CourierPricingRule 
                    { 
                        CourierId = company3.Id, 
                        MinVolume = 1001, 
                        MaxVolume = 2000, 
                        DimensionPrice = 19.5m, 
                        MinWeight = 20.1m, 
                        MaxWeight = 30, 
                        WeightPrice = 33.99m 
                    }, 
                    new CourierPricingRule 
                    { 
                        CourierId = company3.Id, 
                        MinVolume = 2001, 
                        MaxVolume = 5000, 
                        DimensionPrice = 48.5m, 
                        MinWeight = 30.1m, 
                        MaxWeight = 40, 
                        WeightPrice = 43.99m 
                    } 
                }; 
                
                _db.CourierPricingRules.AddRange(pricingRules); 
                await _db.SaveChangesAsync();
            } 
            // Role and Admin User Initialization
            await SeedRolesAndAdminUserAsync(); 
        } 
        
        private async Task SeedRolesAndAdminUserAsync() 
        { 
            // Create roles if they don't exist
            if (!await _roleManager.RoleExistsAsync("Admin")) 
            { 
                await _roleManager.CreateAsync(new IdentityRole("Admin")); 
            } 
            if (!await _roleManager.RoleExistsAsync("Customer")) 
            { 
                await _roleManager.CreateAsync(new IdentityRole("Customer")); 
            } 
            
            // Create Admin user if it doesn't exist
            if (await _userManager.FindByEmailAsync("admin@gmail.com") == null) 
            { 
                var adminUser = new ApplicationUser 
                { 
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    Role = "Admin",
                    Name = "Bani",
                    LastName = "Shehi",
                    StreetAddress = "Rruga Arben Lami",
                    City = "Tirane",
                    State = "Shqiperi",
                    PostalCode = "1001",
                    CreatedAt = DateTime.Now,
                    PhoneNumber = "1234567890" 
                }; 
                
                var result = await _userManager.CreateAsync(adminUser, "Admin@123"); 
                
                if (result.Succeeded) 
                { 
                    await _userManager.AddToRoleAsync(adminUser, "Admin"); 
                } 
                else 
                { 
                    _logger.LogError("Failed to create default admin user. Errors: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description))); 
                } 
            } 
        } 
    }
}