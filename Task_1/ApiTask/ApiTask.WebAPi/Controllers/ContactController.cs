using ApiTask.Application.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiTask.WebApi.Controllers
{
    [Route("api/contact")]
    public class ContactController(IMapper mapper,
                                   ILogger<ContactController> logger,
                                   IContactRepository contactRepository) : Controller(mapper)
    {
        private readonly ILogger<ContactController> _logger = logger;
        private readonly IContactRepository _contactRepository = contactRepository;

    }
}