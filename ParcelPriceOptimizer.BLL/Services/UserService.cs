using Microsoft.AspNetCore.Http;
using ParcelPriceOptimizer.BLL.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParcelPriceOptimizer.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            try
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims;

                foreach (var claim in claims)
                {
                    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                }

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User ID claim not found.");
                }

                return userId;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}