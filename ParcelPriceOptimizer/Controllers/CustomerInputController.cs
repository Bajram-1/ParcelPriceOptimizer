using Microsoft.AspNetCore.Mvc;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;

namespace ParcelPriceOptimizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInputController : ControllerBase
    {
        private readonly ICustomerInputService _customerInputService;
        private readonly IPriceCalculationService _priceCalculationService;

        public CustomerInputController(ICustomerInputService customerInputService, IPriceCalculationService priceCalculationService)
        {
            _customerInputService = customerInputService;
            _priceCalculationService = priceCalculationService;
        }

        [HttpPost("save")] 
        public async Task<IActionResult> SaveCustomerInput([FromBody] PackageInputViewModel input) 
        { 
            if (input == null) 
            { 
                return BadRequest("Invalid package input."); 
            } 
            decimal calculatedPrice = await _priceCalculationService.CalculatePriceAsync(input); 
            await _customerInputService.SaveCustomerInputAsync(input, calculatedPrice); 
            return Ok("Customer input saved successfully."); 
        }
    }
}