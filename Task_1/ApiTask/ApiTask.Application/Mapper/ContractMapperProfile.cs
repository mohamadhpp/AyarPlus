using ApiTask.Application.Dtos.ContractDtos;
using ApiTask.Domain.Entities;
using AutoMapper;

namespace ApiTask.Application.Mapper
{
    public class ContractMapperProfile : Profile
    {
        public ContractMapperProfile()
        {
            CreateMap<AddContractDto, Contract>()
                .ForMember(dest => dest.Front, opt => opt.Ignore())
                .ForMember(dest => dest.Back, opt => opt.Ignore());

            CreateMap<EditContractDto, Contract>()
                .ForMember(dest => dest.Front, opt => opt.Ignore())
                .ForMember(dest => dest.Back, opt => opt.Ignore());
        }
    }
}