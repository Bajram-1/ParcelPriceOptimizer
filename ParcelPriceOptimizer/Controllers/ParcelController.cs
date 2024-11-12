using Microsoft.AspNetCore.Mvc;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;

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

            try
            {
                decimal price = _priceCalculationService.CalculatePrice(input);
                await _customerInputService.SaveCustomerInputAsync(input, price);
                _logger.LogInformation("Price calculated successfully: {Price}", price);
                return Ok(new { price });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating price.");
                return StatusCode(500, "An error occurred while calculating the price.");
            }
        }
    }
}