using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParcelPriceOptimizer.DAL.Repositories;
using ParcelPriceOptimizer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ParcelPriceOptimizer.DAL.Entities;
using Microsoft.Extensions.Logging.Abstractions;

namespace ParcelPriceOptimizer.Tests
{
    public class CustomerInputRepositoryTests
    {
        private readonly CustomerInputRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly Mock<ILogger<CustomerInputRepository>> _loggerMock;

        public CustomerInputRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogger<CustomerInputRepository>>();
            _repository = new CustomerInputRepository(_context, _loggerMock.Object);
        }

        /// <summary> 
        /// 1. Shton nje entitet te ri CustomerInput ne bazen e te dhenave. 
        /// 2. Ruaj ndryshimet ne menyre asinkrone. 
        /// 3. Regjistron informacion ne rast te suksesit ose regjistron nje gabim nëse ndodh një përjashtim. 
        /// </summary>
        [Fact]
        public async Task AddAsync_ShouldAddCustomerInput()
        {
            var customerInput = new CustomerInput
            {
                UserId = "testUser",
                Width = 10,
                Height = 10,
                Depth = 10,
                Weight = 5,
                Price = 50,
                StripeSessionId = "session123",
                StripePaymentIntentId = "intent123",
                IsPaymentCompleted = true
            };
            await _repository.AddAsync(customerInput);
            var result = await _context.CustomerInputs.FindAsync(customerInput.Id);

            Assert.NotNull(result);
            Assert.Equal(customerInput.UserId, result.UserId);
            Assert.Equal(customerInput.Width, result.Width);
            Assert.Equal(customerInput.Height, result.Height);
            Assert.Equal(customerInput.Depth, result.Depth);
            Assert.Equal(customerInput.Weight, result.Weight);
            Assert.Equal(customerInput.Price, result.Price);
            Assert.Equal(customerInput.StripeSessionId, result.StripeSessionId);
            Assert.Equal(customerInput.StripePaymentIntentId, result.StripePaymentIntentId);
            Assert.Equal(customerInput.IsPaymentCompleted, result.IsPaymentCompleted);
        }

        /// <summary> 
        /// 1. Perditeson nje entitet ekzistues CustomerInput ne bazen e te dhenave. 
        /// 2. Ruaj ndryshimet ne menyre asinkrone. 
        /// </summary>
        [Fact]
        public async Task UpdateAsync_ShouldUpdateCustomerInput()
        {
            var customerInput = new CustomerInput
            {
                UserId = "testUser",
                Width = 10,
                Height = 10,
                Depth = 10,
                Weight = 5,
                Price = 50,
                StripeSessionId = "session123",
                StripePaymentIntentId = "intent123",
                IsPaymentCompleted = true
            };

            await _repository.AddAsync(customerInput);
            customerInput.Weight = 10;
            customerInput.Price = 100;
            await _repository.UpdateAsync(customerInput);
            var result = await _context.CustomerInputs.FindAsync(customerInput.Id);
            Assert.NotNull(result);
            Assert.Equal(10, result.Weight);
            Assert.Equal(100, result.Price);
        }

        /// <summary> 
        /// 1. Merr nje entitet CustomerInput nga baza e te dhenave sipas ID-së. 
        /// 2. Kthen entitetin CustomerInput nese gjendet. 
        /// 3. Kthen null nese entiteti nuk gjendet. 
        /// </summary>
        [Fact]
        public async Task GetByIdAsync_ShouldReturnCustomerInput()
        {
            var customerInput = new CustomerInput
            {
                UserId = "testUser",
                Width = 10,
                Height = 10,
                Depth = 10,
                Weight = 5,
                Price = 50,
                StripeSessionId = "session123",
                StripePaymentIntentId = "intent123",
                IsPaymentCompleted = true
            };

            await _repository.AddAsync(customerInput);
            var result = await _repository.GetByIdAsync(customerInput.Id);
            Assert.NotNull(result);
            Assert.Equal(customerInput.UserId, result.UserId);
        }
    }
}