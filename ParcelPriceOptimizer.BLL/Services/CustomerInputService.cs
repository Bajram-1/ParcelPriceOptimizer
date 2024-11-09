using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.Entities;
using ParcelPriceOptimizer.DAL.IRepositories;
using ParcelPriceOptimizer.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.Services
{
    public class CustomerInputService : ICustomerInputService
    {
        private readonly ICustomerInputRepository _repository;

        public CustomerInputService(ICustomerInputRepository repository)
        {
            _repository = repository;
        }

        public async Task SaveCustomerInputAsync(PackageInputViewModel input, decimal price)
        {
            var customerInput = new CustomerInput
            {
                //UserId = input.UserId,
                CustomerName = input.CustomerName,
                CustomerEmail = input.CustomerEmail,
                Width = input.Width,
                Height = input.Height,
                Depth = input.Depth,
                Weight = input.Weight,
                Price = price,
                SubmittedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(customerInput);
        }
    }
}