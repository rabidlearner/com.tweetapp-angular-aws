using AutoMapper;
using com.tweetapp.api.Log;
using com.tweetapp.api.Models;
using com.tweetapp.api.Repo.IRepo;
using com.tweetapp.api.Services.IServices;
using com.tweetapp.api.ViewModels;
using MassTransit;

namespace com.tweetapp.api.Services.Implementation
{
    public class TweetsServices : ITweetsServices
    {
        private readonly IUsersRepo usersRepo;
        private readonly ITweetsRepo tweetsRepo;
        private readonly IRepliesRepo repliesRepo;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILog logger;

        public TweetsServices(IUsersRepo usersRepo, ITweetsRepo tweetsRepo, IRepliesRepo repliesRepo, IMapper mapper,IPublishEndpoint publishEndpoint,ILog logger)
        {
            this.usersRepo = usersRepo;
            this.tweetsRepo = tweetsRepo;
            this.repliesRepo = repliesRepo;
            this.mapper = mapper;
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
        }

        public async Task<bool> DeleteTweet(string username, int id)
        {
            try
            {
                logger.Information("caled services");
                logger.Information("Fetching User details");
                var user = await usersRepo.GetUser(username);
                if (user == null)
                {
                    logger.Warning("User details not found");
                    throw new Exception("User details not found");                    
                }
                var tweet = await tweetsRepo.GetTweet(id);
                if (tweet == null)
                {
                    logger.Warning("tweet details doesn't match");
                    throw new Exception("tweet details doesn't match");
                }
                if(tweet.UserId != user.Id)
                {
                    logger.Warning("No Permissions to delete");
                    throw new Exception("No Permissions to delete");
                }
                logger.Information("User details matched");
                logger.Information("Performing delete operations");
                logger.Information("Deleting Replies");
                await repliesRepo.DeleteReplies(id);
                logger.Information("Replies Deleted");
                logger.Information("Deleting Tweet");
                return await tweetsRepo.DeleteTweet(id);                
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<List<TweetsViewModel>> GetAllTweets()
        {
            try
            {
                logger.Information("Called Services");
                logger.Information("getting tweet details from repo");
                var tweets = await tweetsRepo.GetTweets();
                logger.Information("fetched tweet details from repo");
                List<TweetsViewModel> result = new List<TweetsViewModel>();
                logger.Information("creating tweets view model");
                foreach (var tweet in tweets)
                {                    
                    var replies = await repliesRepo.GetReplies(tweet.Id);
                    var repliesVm = mapper.Map<List<RepliesViewModel>>(replies);
                    var tweetVm = mapper.Map<TweetsViewModel>(tweet);
                    var user = await usersRepo.GetUser(tweet.UserId);
                    tweetVm.UserName = user.UserName;
                    tweetVm.RepliesViewModels = repliesVm;
                    result.Add(tweetVm);                    
                }
                logger.Information("tweets view model created");
                return result;
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<List<TweetsViewModel>> GetAllTweetsofUser(string username)
        {
            try
            {
                logger.Information("Called Services");
                logger.Information("Fetching User");
                var user = await usersRepo.GetUser(username);
                if(user == null)
                {
                    logger.Information("user details not found");
                    throw new Exception("User Details Not Found");
                }
                logger.Information("User Details found");
                logger.Information("calling tweets repo");
                var tweets = await tweetsRepo.GetTweetsForUser(user.Id);
                logger.Information("fetched details from repo");
                List<TweetsViewModel> result = new List<TweetsViewModel>();
                logger.Information("creating tweets view model");
                foreach (var tweet in tweets)
                {
                    var replies = await repliesRepo.GetReplies(tweet.Id);
                    var repliesVm = mapper.Map<List<RepliesViewModel>>(replies);
                    var tweetVm = mapper.Map<TweetsViewModel>(tweet);
                    tweetVm.UserName = username;
                    tweetVm.RepliesViewModels = repliesVm;
                    result.Add(tweetVm);
                }
                logger.Information("tweetsview model created");
                return result;
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<TweetsViewModel> LikeTweet(string username, int id)
        {
            try
            {
                logger.Information("Called services");
                logger.Information("fetching user details");
                var user = await usersRepo.GetUser(username);
                if (user == null)
                {
                    logger.Information("user details not found");
                    throw new Exception("User Details Not Found");
                }
                logger.Information("user details found");
                logger.Information("Fetching tweet");
                var tweet = await tweetsRepo.GetTweet(id);
                if(tweet == null)
                {
                    logger.Information("tweet details not found");
                    throw new Exception("Tweet Details Not Found");
                }
                logger.Information("Updating tweet details");
                tweet.Likes += 1;
                var updatedtweet = await tweetsRepo.UpdateTweet(tweet);                
                if (updatedtweet == null)
                {
                    logger.Information("Tweet details not updated to database");
                    throw new Exception("Database operation failed");
                }
                logger.Information("tweet details updated");
                logger.Information("creating tweet view model");
                var replies = await repliesRepo.GetReplies(tweet.Id);
                var repliesVm = mapper.Map<List<RepliesViewModel>>(replies);
                var tweetVm = mapper.Map<TweetsViewModel>(tweet);
                var usertweet = await usersRepo.GetUser(tweet.UserId);
                tweetVm.UserName = usertweet.UserName;
                tweetVm.RepliesViewModels = repliesVm;
                logger.Information("tweet view model created");
                return tweetVm;
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<bool> PostTweet(string message, string username)
        {
            try
            {
                logger.Information("called service");
                logger.Information("getting user from repo");
                var user = await usersRepo.GetUser(username);
                if (user == null)
                {
                    logger.Information("User not found");
                    throw new Exception("User not found");                    
                }
                logger.Information("User found");
                Tweet tweet = new Tweet()
                {
                    UserId = user.Id,
                    Message = message,
                    Likes = 0
                };
                logger.Information("Published tweet to rabbitmq");
                await publishEndpoint.Publish<Tweet>(tweet);
                return true;
                //return await tweetsRepo.PostTweet(tweet);
            }
            catch (Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<TweetsViewModel> ReplyTweet(string username, int id, string message)
        {
            try
            {
                logger.Information("called service");
                Reply reply = new Reply()
                {
                    Message = message,
                    TweetId = id,
                    Username = username
                };
                logger.Information("posting reply to repo");
                var result = await repliesRepo.PostReply(reply);
                logger.Information("posted reply to repo");
                if (result)
                {
                    logger.Information("creating tweet view model");
                    var tweet = await tweetsRepo.GetTweet(id);
                    var replies = await repliesRepo.GetReplies(tweet.Id);
                    var repliesVm = mapper.Map<List<RepliesViewModel>>(replies);
                    var tweetVm = mapper.Map<TweetsViewModel>(tweet);
                    var user = await usersRepo.GetUser(tweet.UserId);
                    tweetVm.UserName = user.UserName;
                    tweetVm.RepliesViewModels = repliesVm;
                    logger.Information("created tweet view model");
                    return tweetVm;
                }
                throw new Exception("Database operation failed");
            }
            catch(Exception ex)
            {
                logger.Error($"Exception : {ex.Message}");
                logger.Error($"Stacktrace : {ex.StackTrace}");
                throw;
            }
        }

        public async Task<TweetsViewModel> UpdateTweet(string username, int id, PostTweetViewModel viewModel)
        {
            try
            {
                logger.Information("called service");
                logger.Information("Fetching user details");
                var user = await usersRepo.GetUser(username);
                if (user == null)
                {
                    logger.Information("user not found");
                    throw new Exception("User Details Not Found");
                }
                logger.Information("user found");
                logger.Information("updating tweet");
                logger.Information("Fetching current tweet details");
                var tweetfromdb = await tweetsRepo.GetTweet(id);
                if(tweetfromdb.UserId != user.Id)
                {
                    logger.Information("No permissions to change tweet");
                    throw new Exception("No permissions to change tweet");
                }
                logger.Information("tweet details fetched sucessfully");
                var tweet = new Tweet()
                {
                    Id = id,
                    Message = viewModel.Message,
                    Likes = tweetfromdb.Likes,
                    UserId = tweetfromdb.UserId
                };
                var updatedtweet = await tweetsRepo.UpdateTweet(tweet);
                if (updatedtweet == null)
                {
                    logger.Information("tweet details not updated to repo");
                    throw new Exception("Database operation failed");
                }
                logger.Information("creating tweet view model");
                var replies = await repliesRepo.GetReplies(tweet.Id);
                var repliesVm = mapper.Map<List<RepliesViewModel>>(replies);
                var tweetVm = mapper.Map<TweetsViewModel>(tweet);
                tweetVm.UserName = user.UserName;
                tweetVm.RepliesViewModels = repliesVm;
                logger.Information("created tweet view model");
                return tweetVm;
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
