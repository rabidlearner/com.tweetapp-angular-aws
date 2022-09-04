using com.tweetapp.api.Log;
using com.tweetapp.api.Models;
using com.tweetapp.api.Services.IServices;
using com.tweetapp.api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices usersServices;
        private readonly IConfiguration _configuration;
        private readonly ILog logger;
        private readonly string message = "Unexpected error occured, please go through log files to know more.";

        public UsersController(IUsersServices usersServices, IConfiguration _configuration,ILog logger)
        {
            this.usersServices = usersServices;
            this._configuration = _configuration;
            this.logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                logger.Information("Started Registering New User");
                var result = await usersServices.Register(user);
                if (result)
                {
                    logger.Information("Registered Successfully");
                    return Ok(new {result = "Successfully Registered"});
                }
                logger.Error("User detailes not stored in Database");
                return BadRequest("Something went wrong please try again");
            }
            catch(Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }
            
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                logger.Information("Login Method was called");
                User result = await usersServices.Login(loginViewModel);
                if (result == null)
                {
                    logger.Warning("User deatails not found");
                    return NotFound();
                }
                logger.Information("Adding Claims to User");
                var claims = new[] 
                {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", result.Id.ToString()),
                        new Claim("DisplayName", result.FirstName),
                        new Claim("UserName", result.UserName)
                };
                logger.Information("cliams added successfully");
                logger.Information("Creating JWT Token");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);
                logger.Information("JWT Token Created Successfully");
                return Ok(new {JwtToken = new JwtSecurityTokenHandler().WriteToken(token), name = result.FirstName , userName = result.UserName });
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }

        }
        [HttpGet("users/all")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                logger.Information("Fetching All User Details");
                var users = await usersServices.GetAllUsers();
                logger.Information("User details feched successfully");
                return Ok(users);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }
                     
        }
        [HttpGet("{username}/Forgot")]
        
        public async Task<IActionResult> ForgotPassword([FromQuery] ForgotPasswordViewModel viewModel)
        {
            try
            {
                logger.Information("Fetching Password for user");
                var password = await usersServices.GetPassword(viewModel);
                logger.Information("Password fetched Successfully");
                return Ok(new { password = password });
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }
        }
        [HttpGet("user/search/{username}")]
        [Authorize]
        public async Task<IActionResult> Search(string username)
        {
            try
            {
                logger.Information("Search for users started");
                var users = await usersServices.Search(username);
                logger.Information("User details found successfully");
                return Ok(users);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }
            
        }
    }
}
