using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
using ParcelPriceOptimizer.BLL.IServices;
using ParcelPriceOptimizer.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ParcelPriceOptimizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<ApplicationUser> logger, IEmailSender emailSender, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    _logger.LogWarning("User not found: {Email}", model.Email);
                    return Unauthorized("Invalid email or password.");
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Invalid password for user: {Email}", model.Email);
                    return Unauthorized("Invalid email or password.");
                }

                var token = _tokenService.GenerateJwtToken(user);
                _logger.LogInformation("Generated JWT token: {Token}", token);
                return Ok(new { token });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)); var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = encodedCode }, protocol: HttpContext.Request.Scheme); await _emailSender.SendEmailAsync(model.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."); return Ok(new { Message = "Registration successful. Please check your email to confirm your account." });
                }    
                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    return BadRequest("Invalid email confirmation request.");
                }
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return BadRequest("The user ID is invalid.");
                }

                var decodedCodeBytes = WebEncoders.Base64UrlDecode(code);
                var decodedCode = Encoding.UTF8.GetString(decodedCodeBytes);
                var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

                if (result.Succeeded)
                {
                    return Ok("Email confirmed successfully!");
                }

                return BadRequest("Email confirmation failed.");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenViewModel model)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(model.Token);
                
                var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Invalid token.");
                    return BadRequest("Invalid token.");
                }
                
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found for the given token.");
                    return Unauthorized("Invalid token.");
                }

                var newToken = _tokenService.GenerateJwtToken(user);
                return Ok(new { newToken });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}