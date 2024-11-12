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
        /// 1. Simulon nje perdorues te autentikuar me nje claim te UserId. 
        /// 2. Kontrollon qe metoda GetCurrentUserId kthen UserId te pritur. 
        /// </summary> 
        [Fact]
        public void GetCurrentUserId_ShouldReturnUserId()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "04e6bcd6-f077-44fe-b0af-72d33de08c3d")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Setup(_ => _.HttpContext.User).Returns(claimsPrincipal);
            var result = _repository.GetCurrentUserId();
            Assert.Equal("04e6bcd6-f077-44fe-b0af-72d33de08c3d", result);
        }

        /// <summary> 
        /// 1. Simulon nje perdorues te autentikuar pa nje claim te UserId. 
        /// 2. Kontrollon qe metoda GetCurrentUserId hedh nje perjashtim. 
        /// </summary> 
        [Fact]
        public void GetCurrentUserId_ShouldThrowExceptionIfUserIdNotFound()
        {
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.Setup(_ => _.HttpContext.User).Returns(claimsPrincipal);
            var exception = Assert.Throws<Exception>(() => _repository.GetCurrentUserId());
            Assert.Equal("User ID not found in claims.", exception.Message);
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

        /// <summary> 
        /// 1. Shton nje liste perdoruesish ne kontekstin e aplikacionit. 
        /// 2. Kontrollon qe metoda GetAllAsync kthen te gjithe perdoruesit. 
        /// </summary> 
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "user1",
                    UserName = "user1",
                    Email = "user1@example.com",
                    City = "Test City",
                    LastName = "LastName1",
                    Name = "Name1",
                    PostalCode = "12345",
                    Role = "User",
                    State = "Test State",
                    StreetAddress = "123 Test St"
                },
                new ApplicationUser
                {
                    Id = "user2",
                    UserName = "user2",
                    Email = "user2@example.com",
                    City = "Test City",
                    LastName = "LastName2",
                    Name = "Name2",
                    PostalCode = "12345",
                    Role = "User",
                    State = "Test State",
                    StreetAddress = "123 Test St"
                }
            };

            _context.ApplicationUsers.AddRange(users);
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();
            Assert.Equal(2, result.Count());
            Assert.Contains(result, u => u.Id == "user1");
            Assert.Contains(result, u => u.Id == "user2");
        }
    }
}