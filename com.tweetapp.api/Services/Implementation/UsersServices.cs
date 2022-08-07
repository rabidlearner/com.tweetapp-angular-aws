using AutoMapper;
using com.tweetapp.api.Models;
using com.tweetapp.api.Repo.IRepo;
using com.tweetapp.api.Services.IServices;
using com.tweetapp.api.ViewModels;

namespace com.tweetapp.api.Services.Implementation
{
    public class UsersServices : IUsersServices
    {
        private readonly IUsersRepo usersRepo;
        private readonly IMapper mapper;

        public UsersServices(IUsersRepo usersRepo, IMapper mapper)
        {
            this.usersRepo = usersRepo;
            this.mapper = mapper;
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            var result = await usersRepo.GetUsers();
            return mapper.Map<List<UserViewModel>>(result);
        }

        public async Task<string> GetPassword(ForgotPasswordViewModel viewModel)
        {
            var result = await usersRepo.GetUser(viewModel.UserName);
            if (result != null)
            {
                if (result.PhoneNumber == viewModel.ContactNumber)
                {
                    return result.Password;
                }
            }
            return "";
        }

        public async Task<User> Login(LoginViewModel loginViewModel)
        {
            var result = await usersRepo.GetUser(loginViewModel.Username);
            if(result != null)
            {
                if(result.Password==loginViewModel.Password)
                {
                    return result;
                }
            }
            return null;
        }

        public async Task<bool> Register(User user)
        {
            return await usersRepo.PostUser(user);
        }

        public async Task<List<UserViewModel>> Search(string username)
        {
            var result = await usersRepo.GetUsersByPartial(username);
            return mapper.Map<List<UserViewModel>>(result);
        }
    }
}
