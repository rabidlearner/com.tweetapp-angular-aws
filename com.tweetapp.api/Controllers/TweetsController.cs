using com.tweetapp.api.Log;
using com.tweetapp.api.Models;
using com.tweetapp.api.Services.IServices;
using com.tweetapp.api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        private readonly string message = "Unexpected error occured, please go through log files to know more.";
        public TweetsController(ITweetsServices tweetsServices, IUsersServices usersServices,ILog logger)
        {
            this.tweetsServices = tweetsServices;
            this.usersServices = usersServices;
            this.logger = logger;
        }
        [HttpPost("{username}/add")]        
        public async Task<IActionResult> PostTweet([FromBody] PostTweetViewModel tweet, [FromRoute] string username)
        {
            try
            {
                logger.Information("Posting tweet ..");
                bool result = await tweetsServices.PostTweet(tweet.Message, username);
                if (result)
                {
                    logger.Information("tweet posted successfully");
                    return Ok(new { success = "Successfully Posted" });
                }
                logger.Information("Tweet not posted to database");
                return BadRequest("Something went wrong please try again");
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);            
            }
        }
        [HttpPut("{username}/update/{id}")]
        public async Task<IActionResult> UpdateTweet([FromBody] PostTweetViewModel viewModel, [FromRoute] string username, [FromRoute] int id)
        {
            try
            {
                logger.Information("Updating tweet ..");
                var result = await tweetsServices.UpdateTweet(username, id, viewModel);
                logger.Information("Tweet has been updated");
                return Ok(result);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }
        }
        [HttpDelete("{username}/delete/{id}")]
        public async Task<IActionResult> DeleteTweet([FromRoute]string username, [FromRoute]int id)
        {
            try
            {
                logger.Information("Deleting tweet ..");
                bool result = await tweetsServices.DeleteTweet(username, id);
                if (result)
                {
                    logger.Information("Tweet deleted sucessfully");
                    return Ok(new { result = "Successfully Deleted" });
                }
                logger.Error("Something wrong with database");
                return BadRequest("Something went wrong please try again");
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }
        }
        [HttpGet("{username}")]
        public async Task<IActionResult> GetAllTweetsOfUser([FromRoute] string username)
        {
            try
            {
                logger.Information($"getting all tweets for user {username}");
                var tweets = await tweetsServices.GetAllTweetsofUser(username);
                logger.Information("Fetched all tweets for user");
                return Ok(tweets);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }
        }
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllTweets()
        {
            try
            {
                logger.Information("started getting all tweets");
                var tweets = await tweetsServices.GetAllTweets();
                logger.Information("Fetched all tweets");
                return Ok(tweets);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }
        }
        [HttpPut("{username}/like/{id}")]
        public async Task<IActionResult> Like(string username,int id)
        {
            try
            {
                logger.Information("Adding like to tweet");
                var tweet = await tweetsServices.LikeTweet(username, id);
                logger.Information("Added like to tweet");
                return Ok(tweet);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }             
        }
        [HttpPost("{username}/reply/{id}")]
        public async Task<IActionResult> Reply([FromRoute]string username, [FromRoute]int id, [FromBody]PostTweetViewModel postTweetViewModel)
        {
            try
            {
                logger.Information("Adding Reply to tweet");
                var tweet = await tweetsServices.ReplyTweet(username, id, postTweetViewModel.Message);
                logger.Information("Reply has been added successfully");
                return Ok(tweet);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message ?? message);
            }            
        }
    }
}
