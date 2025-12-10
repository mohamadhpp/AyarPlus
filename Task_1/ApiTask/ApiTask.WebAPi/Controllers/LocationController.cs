using ApiTask.Application.Interfaces.Repositories.LocationRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiTask.WebApi.Controllers
{
    [Route("api/location")]
    public class LocationController(IMapper mapper,
                                    ILogger<LocationController> logger,
                                    ICountryRepository countryRepository) : Controller(mapper)
    {
        private readonly ILogger<LocationController> _logger = logger;
        private readonly ICountryRepository _countryRepository = countryRepository;

        [Route("get")]
        public async Task<ActionResult> Get()
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