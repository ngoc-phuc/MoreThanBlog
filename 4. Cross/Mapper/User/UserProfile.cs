using Abstraction.Repository.Model;
using AutoMapper;
using Core.Utils;

namespace Mapper.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, LoggedInUser>().IgnoreAllNonExisting();
        }
    }
}