using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ParcelPriceOptimizer.DAL.Entities;
using ParcelPriceOptimizer.DAL.Repositories;
using ParcelPriceOptimizer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace ParcelPriceOptimizer.Tests
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<ILogger<UserRepository>> _loggerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _context = new ApplicationDbContext(options);
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _loggerMock = new Mock<ILogger<UserRepository>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _repository = new UserRepository(_context, _userManagerMock.Object, _loggerMock.Object, _httpContextAccessorMock.Object);
        }

        /// <summary> 
        /// 1. Simulon kthimin e nje perdoruesi nga UserManager. 
        /// 2. Kontrollon qe metoda GetByIdAsync kthen perdoruesin e pritur. 
        /// </summary> 
        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            var user = new ApplicationUser
            {
                Id = "testUserId"
            };

            _userManagerMock.Setup(um => um.FindByIdAsync("testUserId")).ReturnsAsync(user);
            var result = await _repository.GetByIdAsync("testUserId");
            Assert.NotNull(result);
            Assert.Equal("testUserId", result.Id);
        }
    }
}