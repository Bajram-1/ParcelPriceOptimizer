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

        public ParcelController(IPriceCalculationService priceCalculationService, ICustomerInputService customerInputService)
        {
            _priceCalculationService = priceCalculationService;
            _customerInputService = customerInputService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculatePrice([FromBody] PackageInputViewModel input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input data.");
            }

            decimal price = _priceCalculationService.CalculatePrice(input);

            return Ok(new { price });
        }
    }
}
