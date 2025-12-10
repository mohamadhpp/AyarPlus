using ApiTask.Application.Interfaces.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiTask.WebApi.Controllers
{
    [Route("api/user")]
    public class UserController(IMapper mapper,
                                ILogger<UserController> logger,
                                IUserRepository userRepository) : Controller(mapper)
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly IUserRepository _userRepository = userRepository;

    }
}