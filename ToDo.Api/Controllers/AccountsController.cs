using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDo.Api.Models;
using ToDo.Core.Models;

namespace ToDo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signinManager,
            IConfiguration config,
            ILogger<AccountsController> logger)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _config = config;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
        {
            if (!ModelState.IsValid || userLogin == null)
            {
                _logger.LogError($"Invalid login object provided.");
                return BadRequest();
            }
            try
            {
                ApplicationUser signedUser =await _userManager.FindByEmailAsync(userLogin.Email);
               // var signInResult = await _signinManager.PasswordSignInAsync(signedUser.UserName, userLogin.Password, false, false);
              //  var signInResult = await _signinManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, false);
                if (signedUser==null)
                {
                    _logger.LogError($"Unable to sign-in user {userLogin.Email}.");
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
          

            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("Authentication:JWT:SecurityKey").Value));
            var signInCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _config.GetSection("Authentication:JWT:Issuer").Value,
                audience: _config.GetSection("Authentication:JWT:Audience").Value,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signInCreds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            _logger.LogInformation($"User {userLogin.Email} logged in successfully.");
            return Ok(new { Token = tokenString });
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();

            _logger.LogInformation("User signed out.");
            return NoContent();
        }
    }
}
