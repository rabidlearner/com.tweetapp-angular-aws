using com.tweetapp.api.Log;
using com.tweetapp.api.Repo.Implementation;
using com.tweetapp.api.Repo.IRepo;
using com.tweetapp.api.Services.Implementation;
using com.tweetapp.api.Services.IServices;

namespace com.tweetapp.api.Helpers
{
    public static class DependencyInjectionHelper
    {        
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepo, UsersRepo>();
            services.AddScoped<ITweetsRepo, TweetsRepo>();
            services.AddScoped<IRepliesRepo, RepliesRepo>();

            services.AddScoped<IUsersServices, UsersServices>();
            services.AddScoped<ITweetsServices, TweetsServices>();

            services.AddScoped<ILog, LogNLog>();
        }
    }
}
