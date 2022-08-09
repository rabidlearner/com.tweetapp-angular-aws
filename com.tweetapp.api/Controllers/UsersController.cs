using com.tweetapp.api.Models;
using com.tweetapp.api.Services.IServices;
using com.tweetapp.api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.tweetapp.api.Controllers
{
    [Route("api/v{version:apiVersion}/tweets")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices usersServices;
        private readonly IConfiguration _configuration;

        public UsersController(IUsersServices usersServices, IConfiguration _configuration)
        {
            this.usersServices = usersServices;
            this._configuration = _configuration;
        }
        [HttpPost("register")]
        public async Task<string> Register(User user)
        {
            var result = await usersServices.Register(user);
            if(result)
            {
                return "Successfully Registered";
            }
            return "Something went wrong please try again";
        }
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] LoginViewModel loginViewModel)
        {
            User result = await usersServices.Login(loginViewModel);
            if (result == null)
            {
                return NotFound();
            }
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", result.Id.ToString()),
                        new Claim("DisplayName", result.FirstName),
                        new Claim("UserName", result.UserName)
                        
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            
        }
        [HttpGet("users/all")]
        [Authorize]
        public async Task<List<UserViewModel>> GetAllUsers()
        {
            return await usersServices.GetAllUsers();            
        }
        [HttpGet("{username}/Forgot")]
        [Authorize]
        public async Task<string> ForgotPassword([FromQuery] ForgotPasswordViewModel viewModel)
        {
            return await usersServices.GetPassword(viewModel);
        }
        [HttpGet("user/search/{username}")]
        [Authorize]
        public async Task<List<UserViewModel>> Search(string username)
        {
            return await usersServices.Search(username);
        }
    }
}
