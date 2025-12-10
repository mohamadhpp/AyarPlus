using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiTask.WebApi.Controllers
{
    [ApiController]
    public class Controller : ControllerBase
    {
        protected readonly IMapper _mapper;

        public Controller(IMapper mapper)
        {
            _mapper = mapper;
        }


        #region Error

        protected IActionResult ErrorPage(int statusCode)
        {
            return RedirectToAction("Index", "Error", new
            {
                code = statusCode
            });
        }

        protected IActionResult InternalServerErrorJson()
        {
            return StatusCode(500, "مشکلی در سیستم رخ داده است!");
        }

        #endregion
    }
}
