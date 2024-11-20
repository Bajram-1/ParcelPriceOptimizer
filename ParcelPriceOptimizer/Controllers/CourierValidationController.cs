using Microsoft.AspNetCore.Mvc;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;

namespace ParcelPriceOptimizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourierValidationController : ControllerBase
    {
        private readonly ICourierValidationService _courierValidationService;

        public CourierValidationController(ICourierValidationService courierValidationService)
        {
            _courierValidationService = courierValidationService;
        }

        [HttpPost("validforanycompany")]
        public async Task<IActionResult> ValidateForAnyCompanyAsync([FromBody] CustomerInputViewModel input)
        {
            try
            {
                if (input == null)
                {
                    return BadRequest("Invalid package input.");
                }

                bool isValid = await _courierValidationService.IsValidForAnyCompanyAsync(input);
                return Ok(isValid);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("invalidforallcompanies")]
        public async Task<IActionResult> ValidateForNoCompanyAsync([FromBody] CustomerInputViewModel input)
        {
            try
            {
                if (input == null)
                {
                    return BadRequest("Invalid package input.");
                }

                bool isInvalid = await _courierValidationService.IsValidForNoCompanyAsync(input);
                return Ok(isInvalid);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}