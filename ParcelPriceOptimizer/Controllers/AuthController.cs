using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using ParcelPriceOptimizer.BLL.DTO.ViewModels;
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
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly IEmailSender _emailSender;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<ApplicationUser> logger, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
            _emailSender = emailSender;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
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
            var token = GenerateJwtToken(user);
            _logger.LogInformation("Generated JWT token: {Token}", token);
            return Ok(new { token });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
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

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
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

            // Decode the token
            var decodedCodeBytes = WebEncoders.Base64UrlDecode(code);
            var decodedCode = Encoding.UTF8.GetString(decodedCodeBytes);
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (result.Succeeded)
            {
                return Ok("Email confirmed successfully!");
            }

            return BadRequest("Email confirmation failed.");
        }
    }
}