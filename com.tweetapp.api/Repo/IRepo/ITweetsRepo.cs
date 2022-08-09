using com.tweetapp.api.Models;
using MassTransit;

namespace com.tweetapp.api.Repo.IRepo
{
    public interface ITweetsRepo:IConsumer<Tweet>
    {
        Task<bool> PostTweet(Tweet Tweet);
        Task<Tweet> UpdateTweet(Tweet Tweet);
        Task<Tweet> GetTweet(int id);
        Task<bool> DeleteTweet(int id);
        Task<List<Tweet>> GetTweets();
        Task<List<Tweet>> GetTweetsForUser(int userId);

    }
}
