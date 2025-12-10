using ApiTask.Application.Dtos.CompanyDtos;
using ApiTask.Domain.Entities;
using AutoMapper;

namespace ApiTask.Application.Mapper
{
    public class CompanyMapperProfile : Profile
    {
        public CompanyMapperProfile()
        {
            CreateMap<AddCompanyDto, Company>();
            CreateMap<EditCompanyDto, Company>();
        }
    }
}
