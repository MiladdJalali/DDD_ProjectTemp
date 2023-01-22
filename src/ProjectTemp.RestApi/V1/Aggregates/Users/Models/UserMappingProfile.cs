using AutoMapper;
using ProjectTemp.Application.Aggregates.Users.Commands.CreateUser;
using ProjectTemp.Application.Aggregates.Users.Commands.UpdateUser;
using ProjectTemp.Application.Aggregates.Users.Queries;
using ProjectTemp.Application.Aggregates.Users.Queries.GetUserCollection;
using ProjectTemp.RestApi.V1.Models;

namespace ProjectTemp.RestApi.V1.Aggregates.Users.Models
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<SearchModel, GetUserCollectionQuery>();
            CreateMap<UserRequest, CreateUserCommand>();
            CreateMap<UserRequest, UpdateUserCommand>();
            CreateMap<UserQueryResult, UserResponse>();
        }
    }
}