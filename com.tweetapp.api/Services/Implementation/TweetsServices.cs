using AutoMapper;
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
        

        public TweetsServices(IUsersRepo usersRepo, ITweetsRepo tweetsRepo, IRepliesRepo repliesRepo, IMapper mapper,IPublishEndpoint publishEndpoint)
        {
            this.usersRepo = usersRepo;
            this.tweetsRepo = tweetsRepo;
            this.repliesRepo = repliesRepo;
            this.mapper = mapper;
            this.publishEndpoint = publishEndpoint;
            
        }

        public async Task<bool> DeleteTweet(string username, int id)
        {
            var user = await usersRepo.GetUser(username);
            if (user == null)
            {
                return false;
            }
            await repliesRepo.DeleteReplies(id);
            return await tweetsRepo.DeleteTweet(id);
        }

        public async Task<List<TweetsViewModel>> GetAllTweets()
        {
            var tweets = await tweetsRepo.GetTweets();
            List<TweetsViewModel> result = new List<TweetsViewModel>();
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
            return result;
        }

        public async Task<List<TweetsViewModel>> GetAllTweetsofUser(string username)
        {
            var user = await usersRepo.GetUser(username);
            var tweets = await tweetsRepo.GetTweetsForUser(user.Id);
            List<TweetsViewModel> result = new List<TweetsViewModel>();
            foreach (var tweet in tweets)
            {
                var replies = await repliesRepo.GetReplies(tweet.Id);
                var repliesVm = mapper.Map<List<RepliesViewModel>>(replies);
                var tweetVm = mapper.Map<TweetsViewModel>(tweet);
                tweetVm.UserName=username;
                tweetVm.RepliesViewModels = repliesVm;
                result.Add(tweetVm);
            }
            return result;
        }

        public async Task<TweetsViewModel> LikeTweet(string username, int id)
        {
            var user = await usersRepo.GetUser(username);
            if (user == null)
            {
                return null;
            }
            var tweet = await tweetsRepo.GetTweet(id);
            tweet.Likes += 1;
            var updatedtweet = await tweetsRepo.UpdateTweet(tweet);
            if (updatedtweet == null)
            {
                return null;
            }
            var replies = await repliesRepo.GetReplies(tweet.Id);
            var repliesVm = mapper.Map<List<RepliesViewModel>>(replies);
            var tweetVm = mapper.Map<TweetsViewModel>(tweet);
            var usertweet = await usersRepo.GetUser(tweet.UserId);
            tweetVm.UserName = usertweet.UserName;
            tweetVm.RepliesViewModels = repliesVm;
            return tweetVm;
        }

        public async Task<bool> PostTweet(string message, string username)
        {
            var user = await usersRepo.GetUser(username);
            if(user == null)
            {
                return false;
            }
            Tweet tweet = new Tweet()
            {
                UserId = user.Id,
                Message = message,
                Likes = 0
            };
            await publishEndpoint.Publish<Tweet>(tweet);
            return true;
            //return await tweetsRepo.PostTweet(tweet);
        }

        public async Task<TweetsViewModel> ReplyTweet(string username, int id, string message)
        {            
            Reply reply = new Reply()
            {
                Message = message,
                TweetId = id,
                Username = username
            };
            var result = await repliesRepo.PostReply(reply);
            if(result)
            {
                var tweet = await tweetsRepo.GetTweet(id);
                var replies = await repliesRepo.GetReplies(tweet.Id);
                var repliesVm = mapper.Map<List<RepliesViewModel>>(replies);
                var tweetVm = mapper.Map<TweetsViewModel>(tweet);
                var user = await usersRepo.GetUser(tweet.UserId);
                tweetVm.UserName = user.UserName;
                tweetVm.RepliesViewModels = repliesVm;
                return tweetVm;
            }
            return null;
        }

        public async Task<TweetsViewModel> UpdateTweet(string username, int id, TweetsViewModel viewModel)
        {
            var user = await usersRepo.GetUser(username);
            if (user == null)
            {
                return null;
            }
            var tweet = new Tweet()
            {
                Id = id,
                Message = viewModel.Message,
                Likes = viewModel.Likes,
                UserId = user.Id
            };
            var updatedtweet = await tweetsRepo.UpdateTweet(tweet);
            if(updatedtweet ==null)
            {
                return null;
            }
            return viewModel;
        }
    }
}
