using com.tweetapp.api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace com.tweetapp.api.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}
