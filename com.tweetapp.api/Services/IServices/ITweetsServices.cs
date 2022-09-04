using com.tweetapp.api.ViewModels;

namespace com.tweetapp.api.Services.IServices
{
    public interface ITweetsServices
    {
        Task<bool> PostTweet(string message, string username);
        Task<TweetsViewModel> UpdateTweet(string username, int id, PostTweetViewModel viewModel);
        Task<bool> DeleteTweet(string username, int id);
        Task<List<TweetsViewModel>> GetAllTweetsofUser(string username);
        Task<List<TweetsViewModel>> GetAllTweets();
        Task<TweetsViewModel> LikeTweet(string username, int id);
        Task<TweetsViewModel> ReplyTweet(string username, int id, string message);
    }
}
