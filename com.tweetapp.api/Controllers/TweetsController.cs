using com.tweetapp.api.Log;
using com.tweetapp.api.Models;
using com.tweetapp.api.Services.IServices;
using com.tweetapp.api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace com.tweetapp.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/tweets")]
    [ApiVersion("1.0")]   
    
    public class TweetsController : Controller
    {
        private readonly ITweetsServices tweetsServices;
        private readonly IUsersServices usersServices;
        private readonly ILog logger;

        public TweetsController(ITweetsServices tweetsServices, IUsersServices usersServices,ILog logger)
        {
            this.tweetsServices = tweetsServices;
            this.usersServices = usersServices;
            this.logger = logger;
        }
        [HttpPost("{username}/add")]        
        public async Task<string> PostTweet([FromBody] PostTweetViewModel tweet, [FromRoute] string username)
        {
            bool result = await tweetsServices.PostTweet(tweet.Message,username);
            if (result)
            {
                return "Successfully Posted";
            }
            return "Something went wrong please try again";
        }
        [HttpPut("{username}/update/{id}")]
        public async Task<TweetsViewModel> UpdateTweet([FromBody] TweetsViewModel viewModel, [FromRoute] string username, [FromRoute] int id)
        {
            TweetsViewModel result = await tweetsServices.UpdateTweet(username,id,viewModel);
            if (result!=null)
            {
                return result;
            }
            return null;
        }
        [HttpDelete("{username}/delete/{id}")]
        public async Task<string> DeleteTweet([FromRoute]string username, [FromRoute]int id)
        {
            bool result = await tweetsServices.DeleteTweet(username,id);
            if (result)
            {
                return "Successfully Deleted";
            }
            return "Something went wrong please try again";
        }
        [HttpGet("{username}")]
        public async Task<List<TweetsViewModel>> GetAllTweetsOfUser([FromRoute] string username)
        {
            return await tweetsServices.GetAllTweetsofUser(username);            
        }
        [HttpGet]
        [Route("all")]
        public async Task<List<TweetsViewModel>> GetAllTweets()
        {
            logger.Information("started getting all tweets");
            return await tweetsServices.GetAllTweets();
        }
        [HttpPut("{username}/like/{id}")]
        public async Task<TweetsViewModel> Like(string username,int id)
        {
            return await tweetsServices.LikeTweet(username,id);
        }
        [HttpPost("{username}/reply/{id}")]
        public async Task<TweetsViewModel> Reply([FromRoute]string username, [FromRoute]int id, [FromBody]PostTweetViewModel postTweetViewModel)
        {
            return await tweetsServices.ReplyTweet(username, id,postTweetViewModel.Message);
        }
    }
}
