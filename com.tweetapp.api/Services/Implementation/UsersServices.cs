using AutoMapper;
using com.tweetapp.api.Log;
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
        private readonly ILog logger;

        public UsersServices(IUsersRepo usersRepo, IMapper mapper,ILog logger)
        {
            this.usersRepo = usersRepo;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            try
            {
                logger.Information("called service");
                logger.Information("fetching Users details");
                var result = await usersRepo.GetUsers();
                logger.Information("Fetched User details");
                return mapper.Map<List<UserViewModel>>(result);
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<string> GetPassword(ForgotPasswordViewModel viewModel)
        {
            try
            {
                logger.Information("Called service");
                logger.Information("Fetching user details");
                var result = await usersRepo.GetUser(viewModel.UserName);
                if (result != null)
                {
                    logger.Information("User details found");
                    if (result.PhoneNumber == viewModel.ContactNumber)
                    {
                        logger.Information("Phone number matches");
                        return result.Password;
                    }
                    logger.Information("Phone number doesn't match");
                    throw new Exception("Phone number doesn't match");
                }
                logger.Information("User details not found");
                throw new Exception("User details not found");                
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<User> Login(LoginViewModel loginViewModel)
        {
            try
            {
                logger.Information("called service");
                logger.Information("Fetching user details");
                var result = await usersRepo.GetUser(loginViewModel.Username);
                if (result != null)
                {
                    logger.Information("User details found");
                    if (result.Password == loginViewModel.Password)
                    {
                        logger.Information("Password matches");
                        return result;
                    }
                    logger.Information("Password Doesn't match");
                    throw new Exception("Password Doesn't match");                    
                }
                logger.Information("User details not found");
                throw new Exception("User details not found");               
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> Register(User user)
        {
            try
            {
                logger.Information("Called service");
                logger.Information("Posting user details");
                return await usersRepo.PostUser(user);                
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<List<UserViewModel>> Search(string username)
        {
            try
            {
                logger.Information("called service");
                logger.Information("Fetching users details");
                var result = await usersRepo.GetUsersByPartial(username);
                logger.Information("Fetched users details");
                return mapper.Map<List<UserViewModel>>(result);
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }
    }
}
