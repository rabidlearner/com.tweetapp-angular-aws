using AutoMapper;
using com.tweetapp.api.Models;
using com.tweetapp.api.ViewModels;

namespace com.tweetapp.api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Data.Entities.User, User>();
            CreateMap<User, Data.Entities.User>();
            CreateMap<User, UserViewModel>();

            CreateMap<Data.Entities.Tweet, Tweet>();
            CreateMap<Tweet, Data.Entities.Tweet>();
            CreateMap<Tweet, TweetsViewModel>();


            CreateMap<Data.Entities.Reply, Reply>();
            CreateMap<Reply, Data.Entities.Reply>();
            CreateMap<Reply, RepliesViewModel>();
        }        
    }
}
