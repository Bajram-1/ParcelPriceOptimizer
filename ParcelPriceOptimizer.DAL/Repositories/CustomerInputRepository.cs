using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParcelPriceOptimizer.DAL.Entities;
using ParcelPriceOptimizer.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.DAL.Repositories
{
    public class CustomerInputRepository : ICustomerInputRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerInputRepository> _logger;

        public CustomerInputRepository(ApplicationDbContext context, ILogger<CustomerInputRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(CustomerInput customerInput) 
        {  
            await _context.CustomerInputs.AddAsync(customerInput); 
            
            try 
            { 
                await _context.SaveChangesAsync(); 
                _logger.LogInformation("CustomerInput saved successfully."); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error saving CustomerInput. UserId={UserId}", customerInput.UserId); 
                throw; 
            } 
        }
    }
}