using Microsoft.AspNetCore.Mvc;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;

namespace ParcelPriceOptimizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly ICourierPricingService _courierPricingService;

        public PricingController(ICourierPricingService courierPricingService)
        {
            _courierPricingService = courierPricingService;
        }

        [HttpPost("calculate-optimal-price")]
        public async Task<IActionResult> CalculateOptimalShippingPrice([FromBody] PackageInputViewModel input)
        {
            try
            {
                if (input == null)
                    return BadRequest("Package input cannot be null.");

                if (input.Width <= 0 || input.Height <= 0 || input.Depth <= 0 || input.Weight <= 0)
                    return BadRequest("Package dimensions and weight must be greater than zero.");

                var optimalPrice = await _courierPricingService.CalculateOptimalShippingPriceAsync(input);
                return Ok(optimalPrice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error calculating optimal price: {ex.Message}");
            }
        }

        [HttpPost("calculate-price/{courierId}")]
        public async Task<IActionResult> GetPriceRuling(int courierId, [FromBody] PackageInputViewModel input)
        {
            try
            {
                if (input == null)
                    return BadRequest("Package input cannot be null.");

                if (input.Width <= 0 || input.Height <= 0 || input.Depth <= 0 || input.Weight <= 0)
                    return BadRequest("Package dimensions and weight must be greater than zero.");

                var price = await _courierPricingService.GetPriceRulingAsync(courierId, input);
                return Ok(price);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error calculating price for courier {courierId}: {ex.Message}");
            }
        }
    }
}