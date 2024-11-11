using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL;
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
        private readonly ILogger<CustomerInputService> _logger;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;

        public CustomerInputService(ICustomerInputRepository repository, ILogger<CustomerInputService> logger, IUserService userService, IUserRepository userRepository)
        {
            _repository = repository;
            _logger = logger;
            _userService = userService;
            _userRepository = userRepository;
        }

        public async Task SaveCustomerInputAsync(PackageInputViewModel input, decimal price)
        {
            var userId = _userService.GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var customerInput = new CustomerInput
            {
                UserId = user.Id,
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