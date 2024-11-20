using Microsoft.AspNetCore.Mvc;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using System.Globalization;

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
        public async Task<IActionResult> CalculatePrice([FromBody] CustomerInputViewModel input)
        {
            try
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

                input.UserId = userId;
                var companyPrices = await _priceCalculationService.CalculatePriceAsync(input);
                var result = companyPrices.Select(c => new
                {
                    Courier = c.Key,
                    Price = c.Value.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"))
                }).ToList();

                if (result.Any())
                {
                    return Ok(result);
                }

                _logger.LogError("No pricing data found.");
                return NotFound("No pricing data available.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while calculating the price: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}