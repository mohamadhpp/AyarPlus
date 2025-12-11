using ApiTask.Application.Dtos.UserDtos;
using ApiTask.Common.Helpers;
using ApiTask.Domain.Entities;
using AutoMapper;

namespace ApiTask.Application.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<AddUserDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => SecurityHelper.PBKDF2Hash(src.Password)));

            CreateMap<EditUserDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
