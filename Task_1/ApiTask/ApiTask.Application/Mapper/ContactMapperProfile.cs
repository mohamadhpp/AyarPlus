using ApiTask.Application.Dtos.ContactDtos;
using ApiTask.Domain.Entities;
using AutoMapper;

namespace ApiTask.Application.Mapper
{
    public class ContactMapperProfile : Profile
    {
        public ContactMapperProfile()
        {
            CreateMap<AddContactDto, Contact>()
                .ForMember(dest => dest.Front, opt => opt.Ignore())
                .ForMember(dest => dest.Back, opt => opt.Ignore());

            CreateMap<Contact, EditContactDto>()
                .ForMember(dest => dest.Front, opt => opt.Ignore())
                .ForMember(dest => dest.Back, opt => opt.Ignore());
        }
    }
}