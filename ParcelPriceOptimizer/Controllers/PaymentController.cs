using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.Entities;

namespace ParcelPriceOptimizer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaymentService _paymentService;


        public PaymentController(ILogger<PaymentController> logger, IConfiguration configuration, UserManager<ApplicationUser> userManager, IPaymentService paymentService)
        {
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _paymentService = paymentService;
        }

        [HttpGet("payment-success")]
        public IActionResult PaymentSuccess()
        {
            _logger.LogInformation("Payment was successful.");
            string frontendUrl = _configuration["FrontendUrl"];
            return Redirect($"{frontendUrl}/payment-success");
        }
        [HttpGet("payment-cancelled")]
        public IActionResult PaymentCancelled()
        {
            _logger.LogInformation("Payment was cancelled.");
            string frontendUrl = _configuration["FrontendUrl"];
            return Redirect($"{frontendUrl}/calculate-parcel-price");
        }
        [HttpPost("create-session")]
        public async Task<IActionResult> CreateStripeSession([FromBody] ParcelPaymentViewModel input)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    _logger.LogWarning("User not logged in or could not be found.");
                    return Unauthorized("User is not logged in.");
                }

                var userId = user.Id;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    _logger.LogWarning("User ID is null or empty.");
                    return BadRequest("User ID cannot be null or empty.");
                }

                input.UserId = userId;

                var domain = $"{Request.Scheme}://{Request.Host.Value}/";
                var paymentUrl = await _paymentService.CreateStripeSessionAsync(input, domain, userId);
                return Ok(new { url = paymentUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Stripe session.");
                return StatusCode(500, "An error occurred while creating the payment session.");
            }
        }
    }
}