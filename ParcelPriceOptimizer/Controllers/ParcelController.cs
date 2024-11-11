using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.BLL.Services;
using System.Security.Claims;

namespace ParcelPriceOptimizer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcelController : ControllerBase
    {
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ICustomerInputService _customerInputService;
        private readonly IUserService _userService;
        private readonly ILogger<ParcelController> _logger;

        public ParcelController(IPriceCalculationService priceCalculationService, ICustomerInputService customerInputService, IUserService userService, ILogger<ParcelController> logger)
        {
            _priceCalculationService = priceCalculationService;
            _customerInputService = customerInputService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("calculate")] 
        public async Task<IActionResult> CalculatePrice([FromBody] PackageInputViewModel input) 
        { 
            if (!ModelState.IsValid) 
            { 
                _logger.LogError("Invalid input data."); 
                return BadRequest("Invalid input data."); 
            }

            var userId = _userService.GetCurrentUserId();

            if (string.IsNullOrEmpty(userId)) 
            { 
                _logger.LogError("User not logged in."); 
                return Unauthorized("User not logged in."); 
            } 
            
            _logger.LogInformation("Fetched User ID: {UserId}", userId); 
            
            input.UserId = userId; 
            _logger.LogInformation("Calculating price for user ID: {UserId}", userId); 
            
            try 
            { 
                decimal price = await _priceCalculationService.CalculatePriceAsync(input); 
                _logger.LogInformation("Price calculated successfully: {Price}", price); 
                return Ok(new { price }); 
            } 
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "Error calculating price."); 
            } 
        }
    }  
}