using com.tweetapp.api.Models;

namespace com.tweetapp.api.Repo.IRepo
{
    public interface IUsersRepo
    {
        Task<bool> PostUser(User user);
        Task<User> UpdateUser(User user);
        Task<User> GetUser(int id);
        Task<User> GetUser(string username);
        Task<List<User>> GetUsers();
        Task<List<User>> GetUsersByPartial(string username);
    }
}
