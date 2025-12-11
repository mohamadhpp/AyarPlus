using ApiTask.Application.Interfaces.Repositories.LocationRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTask.WebApi.Controllers
{
    [ApiController]
    [Route("api/location")]
    [Produces("application/json")]
    [SwaggerTag("دسترسی به لیست کشورها، استان‌ها و شهرها")]
    public class LocationController(IMapper mapper,
                                    ILogger<LocationController> logger,
                                    ICountryRepository countryRepository) : Controller(mapper)
    {
        private readonly ILogger<LocationController> _logger = logger;
        private readonly ICountryRepository _countryRepository = countryRepository;

        [HttpGet]
        [Route("get")]
        [SwaggerOperation(
            Summary = "دریافت لیست کشورها با استان‌ها و شهرها",
            Description = "دریافت لیست تمامی کشورها به همراه استان‌ها و شهرهای مربوط",
            OperationId = "GetLocations",
            Tags = new[] { "Location" }
        )]
        [SwaggerResponse(200, "لیست موقعیت‌های جغرافیایی با موفقیت دریافت شد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var countries = await _countryRepository.Query()
                    .Include(c => c.Provinces)
                    .ThenInclude(c => c.Cities)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.Code,

                        Provinces = c.Provinces.Select(province => new
                        {
                            province.Id,
                            province.Code,
                            province.Name,

                            Cities = province.Cities.Select(city => new
                            {
                                city.Id,
                                city.Name
                            })
                        })
                    })
                    .ToListAsync();

                return Ok(countries);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
                return StatusCode(500, exception.Message);
            }
        }
    }
}