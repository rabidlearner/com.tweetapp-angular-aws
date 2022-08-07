using AutoMapper;
using com.tweetapp.api.Data;
using com.tweetapp.api.Models;
using com.tweetapp.api.Repo.IRepo;
using Microsoft.EntityFrameworkCore;

namespace com.tweetapp.api.Repo.Implementation
{
    public class RepliesRepo : IRepliesRepo
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;

        public RepliesRepo(AppDbContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task DeleteReplies(int id)
        {
            var replies = await db.Replies.Where(m => m.TweetId == id).ToListAsync();
            foreach(var repliesItem in replies)
            {
                db.Replies.Remove(repliesItem);
            }
            await db.SaveChangesAsync();            
        }

        public async Task<List<Reply>> GetReplies(int tweetId)
        {
            var replies = await db.Replies.Where(m => m.TweetId == tweetId).ToListAsync();
            return mapper.Map<List<Reply>>(replies);
        }

        public async Task<bool> PostReply(Reply reply)
        {
            await db.Replies.AddAsync(mapper.Map<Data.Entities.Reply>(reply));
            return await db.SaveChangesAsync()>0;
        }
    }
}
