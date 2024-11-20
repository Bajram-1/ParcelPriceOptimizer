using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParcelPriceOptimizer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<CourierPricingRule> CourierPricingRules { get; set; }
        public DbSet<CustomerInput> CustomerInputs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Courier>()
                .HasMany(c => c.CourierPricingRules)
                .WithOne(r => r.Courier)
                .HasForeignKey(r => r.CourierId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerInput>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CustomerInputs)
                .HasForeignKey(ci => ci.UserId) 
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}