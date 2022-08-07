using AutoMapper;
using com.tweetapp.api.Data;
using com.tweetapp.api.Models;
using com.tweetapp.api.Repo.IRepo;
using Microsoft.EntityFrameworkCore;

namespace com.tweetapp.api.Repo.Implementation
{
    public class UsersRepo : IUsersRepo
    {
        AppDbContext db;
        private readonly IMapper mapper;

        public UsersRepo(AppDbContext db,IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<User> GetUser(int id)
        {
            var user = await db.Users.FirstOrDefaultAsync(m => m.Id == id);
            return mapper.Map<User>(user);
        }

        public async Task<User> GetUser(string username)
        {
            var user = await db.Users.FirstOrDefaultAsync(m => m.UserName == username);
            return mapper.Map<User>(user);
        }

        public async Task<List<User>> GetUsers()
        {
            var user = await db.Users.ToListAsync();
            return mapper.Map<List<User>>(user);
        }

        public async Task<bool> PostUser(User user)
        {
            await db.Users.AddAsync(mapper.Map<Data.Entities.User>(user));
            return (await db.SaveChangesAsync())>0;
        }

        public async Task<User> UpdateUser(User user)
        {
            db.Entry(await db.Users.FirstOrDefaultAsync(x => x.Id == user.Id)).CurrentValues.SetValues(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetUsersByPartial(string username)
        {
            var user = await db.Users.Where(m=>m.UserName.ToLower().Contains(username)).ToListAsync();
            return mapper.Map<List<User>>(user);
        }
    }
}
