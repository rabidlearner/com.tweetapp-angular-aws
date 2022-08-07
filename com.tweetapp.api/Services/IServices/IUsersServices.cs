using com.tweetapp.api.Models;
using com.tweetapp.api.ViewModels;

namespace com.tweetapp.api.Services.IServices
{
    public interface IUsersServices
    {
        Task<bool> Register(User user);
        Task<User> Login(LoginViewModel loginViewModel);
        Task<List<UserViewModel>> GetAllUsers();
        Task<string> GetPassword(ForgotPasswordViewModel viewModel);
        Task<List<UserViewModel>> Search(string username);
    }
}
