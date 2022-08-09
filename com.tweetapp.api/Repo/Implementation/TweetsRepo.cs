using AutoMapper;
using com.tweetapp.api.Data;
using com.tweetapp.api.Models;
using com.tweetapp.api.Repo.IRepo;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace com.tweetapp.api.Repo.Implementation
{
    public class TweetsRepo : ITweetsRepo
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;

        public TweetsRepo(AppDbContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<Tweet> context)
        {
            await PostTweet(context.Message);
        }

        public async Task<bool> DeleteTweet(int id)
        {
            var tweet = await db.Tweets.FirstOrDefaultAsync(m => m.Id == id);
            if(tweet == null)
            {
                return false;
            }
            db.Tweets.Remove(tweet);
            return await db.SaveChangesAsync() > 0;

        }

        public async Task<Tweet> GetTweet(int id)
        {
            var tweet = await db.Tweets.FirstOrDefaultAsync(m => m.Id == id);
            return mapper.Map<Tweet>(tweet);
        }

        public async Task<List<Tweet>> GetTweets()
        {
            var tweet = await db.Tweets.ToListAsync();
            return mapper.Map<List<Tweet>>(tweet);
        }

        public async Task<List<Tweet>> GetTweetsForUser(int userId)
        {
            var tweet = await db.Tweets.Where(m=>m.UserId==userId).ToListAsync();
            return mapper.Map<List<Tweet>>(tweet);
        }

        public async Task<bool> PostTweet(Tweet Tweet)
        {
            await db.Tweets.AddAsync(mapper.Map<Data.Entities.Tweet>(Tweet));
            return await db.SaveChangesAsync()>0;

        }

        public async Task<Tweet> UpdateTweet(Tweet Tweet)
        {
            db.Entry(await db.Tweets.FirstOrDefaultAsync(x => x.Id == Tweet.Id)).CurrentValues.SetValues(Tweet);
            await db.SaveChangesAsync();
            return Tweet;
        }
    }
}
