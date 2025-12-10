using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Application.Interfaces.Repositories.LocationRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiTask.WebApi.Controllers
{
    [Route("api/company")]
    public class CompanyController(IMapper mapper,
                                   ILogger<CompanyController> logger,
                                   ICompanyRepository companyRepository) : Controller(mapper)
    {
        private readonly ILogger<CompanyController> _logger = logger;
        private readonly ICompanyRepository _companyRepository = companyRepository;

    }
}
