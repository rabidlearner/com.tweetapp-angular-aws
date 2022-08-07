using com.tweetapp.api.Models;
using com.tweetapp.api.Services.IServices;
using com.tweetapp.api.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.tweetapp.api.Controllers
{
    [Route("api/v{version:apiVersion}/tweets")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices usersServices;

        public UsersController(IUsersServices usersServices)
        {
            this.usersServices = usersServices;
        }
        [HttpPost("/register")]
        public async Task<string> Register(User user)
        {
            var result = await usersServices.Register(user);
            if(result)
            {
                return "Successfully Registered";
            }
            return "Something went wrong please try again";
        }
        [HttpGet("/login")]
        public async Task<string> Login([FromQuery] LoginViewModel loginViewModel)
        {
            User result = await usersServices.Login(loginViewModel);
            if (result == null)
            {
                return "User Not Found";
            }
            return result.UserName;
        }
        [HttpGet("/users/all")]
        public async Task<List<UserViewModel>> GetAllUsers()
        {
            return await usersServices.GetAllUsers();            
        }
        [HttpGet("/{username}/Forgot")]
        public async Task<string> ForgotPassword([FromQuery] ForgotPasswordViewModel viewModel)
        {
            return await usersServices.GetPassword(viewModel);
        }
        [HttpGet("/user/search/{username}")]
        public async Task<List<UserViewModel>> Search(string username)
        {
            return await usersServices.Search(username);
        }
    }
}
