using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.Entities;
using System.Security.Claims;

namespace ParcelPriceOptimizer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PaymentController(ILogger<PaymentController> logger, IConfiguration configuration, IPaymentService paymentService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _configuration = configuration;
            _paymentService = paymentService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("payment-success")]
        public IActionResult PaymentSuccess()
        {
            try
            {
                _logger.LogInformation("Payment was successful.");
                string frontendUrl = _configuration["FrontendUrl"];
                return Redirect($"{frontendUrl}/payment-success");
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("payment-cancelled")]
        public IActionResult PaymentCancelled()
        {
            try
            {
                _logger.LogInformation("Payment was cancelled.");
                string frontendUrl = _configuration["FrontendUrl"];
                string userId = _userService.GetCurrentUserId();
                return Redirect($"{frontendUrl}/calculate-parcel-price?userId={userId}");
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("create-session")]
        public async Task<IActionResult> CreateStripeSession([FromBody] ParcelPaymentViewModel input)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value;

                input.UserId = userId;

                if (string.IsNullOrEmpty(input.Courier) || input.Price <= 0)
                {
                    return BadRequest("Courier and price must be provided.");
                }

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