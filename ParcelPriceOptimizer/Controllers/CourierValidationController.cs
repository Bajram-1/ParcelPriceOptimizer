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

        [HttpPost("validforcompany1")]
        public IActionResult ValidateForCompany1([FromBody] PackageInputViewModel input)
        {
            if (input == null)
            {
                return BadRequest("Invalid package input.");
            }

            bool isValid = _courierValidationService.IsValidForCompany1(input);
            return Ok(isValid);
        }

        [HttpPost("validforcompany2")]
        public IActionResult ValidateForCompany2([FromBody] PackageInputViewModel input)
        {
            if (input == null)
            {
                return BadRequest("Invalid package input.");
            }

            bool isValid = _courierValidationService.IsValidForCompany2(input);
            return Ok(isValid);
        }

        [HttpPost("validforcompany3")]
        public IActionResult ValidateForCompany3([FromBody] PackageInputViewModel input)
        {
            if (input == null)
            {
                return BadRequest("Invalid package input.");
            }

            bool isValid = _courierValidationService.IsValidForCompany3(input);
            return Ok(isValid);
        }
    }
}