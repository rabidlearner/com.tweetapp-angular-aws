using com.tweetapp.api.Models;

namespace com.tweetapp.api.Repo.IRepo
{
    public interface ITweetsRepo
    {
        Task<bool> PostTweet(Tweet Tweet);
        Task<Tweet> UpdateTweet(Tweet Tweet);
        Task<Tweet> GetTweet(int id);
        Task<bool> DeleteTweet(int id);
        Task<List<Tweet>> GetTweets();
        Task<List<Tweet>> GetTweetsForUser(int userId);

    }
}
