using ApiTask.Application.Attributes.Filter;
using ApiTask.Application.Dtos.IdDtos;
using ApiTask.Application.Dtos.UserDtos;
using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Common.Helpers;
using ApiTask.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ApiTask.Application.Dtos.Common;

namespace ApiTask.WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Produces("application/json")]
    [SwaggerTag("مدیریت کاربران")]
    public class UserController(IMapper mapper,
                                ILogger<UserController> logger,
                                IUserRepository userRepository) : Controller(mapper)
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet]
        [Route("get")]
        [ValidateModel]
        [SwaggerOperation(
            Summary = "دریافت لیست کاربران",
            Description = "دریافت لیست تمام کاربران موجود در سیستم",
            OperationId = "GetUsers",
            Tags = new[] { "User" }
        )]
        [SwaggerResponse(200, "لیست کاربران با موفقیت دریافت شد", typeof(IEnumerable<User>))]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Get([FromQuery] ReadDto? dto)
        {
            try
            {
                var query = _userRepository.Query();

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(dto?.Search))
                {
                    query = query.Where(u => u.FirstName.Contains(dto.Search) ||
                                           u.LastName.Contains(dto.Search) ||
                                           u.Email.Contains(dto.Search) ||
                                           u.PhoneNumber.Contains(dto.Search));
                }

                // Apply pagination
                if (dto != null)
                {
                    query = query
                        .Skip((dto.Page - 1) * dto.Size)
                        .Take(dto.Size);
                }

                var users = await _userRepository.ToListAsync(query);

                // Remove password
                foreach (var item in users)
                {
                    item.Password = "";
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return InternalServerErrorJson();
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        [ValidateModel]
        [SwaggerOperation(
            Summary = "دریافت کاربر با شناسه",
            Description = "دریافت اطلاعات یک کاربر براساس شناسه",
            OperationId = "GetUserById",
            Tags = new[] { "User" }
        )]
        [SwaggerResponse(200, "کاربر با موفقیت دریافت شد", typeof(User))]
        [SwaggerResponse(404, "کاربر یافت نشد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                // Remove password
                user.Password = "";

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return InternalServerErrorJson();
            }
        }

        [HttpPost]
        [Route("create")]
        [ValidateModel]
        [SwaggerOperation(
            Summary = "ایجاد کاربر جدید",
            Description = "ایجاد یک کاربر جدید در سیستم با اطلاعات وارد شده",
            OperationId = "CreateUser",
            Tags = new[] { "User" }
        )]
        [SwaggerResponse(200, "کاربر با موفقیت ایجاد شد")]
        [SwaggerResponse(400, "داده‌های ورودی نامعتبر است")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Create([FromBody] AddUserDto dto)
        {
            try
            {
                // Check if email already exists
                var existingEmail = await _userRepository.FirstOrDefaultAsync(u => u.Email == dto.Email);
                if (existingEmail != null)
                {
                    return BadRequest("ایمیل قبلاً ثبت شده است");
                }

                // Check if phone number already exists
                var existingPhone = await _userRepository.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);
                if (existingPhone != null)
                {
                    return BadRequest("شماره همراه قبلاً ثبت شده است");
                }

                var user = _mapper.Map<User>(dto);
                user.Id = Guid.NewGuid();

                await _userRepository.AddAsync(user);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return InternalServerErrorJson();
            }
        }

        [HttpPut]
        [Route("update")]
        [ValidateModel]
        [SwaggerOperation(
            Summary = "بروزرسانی کاربر",
            Description = "بروزرسانی اطلاعات کاربر موجود",
            OperationId = "UpdateUser",
            Tags = new[] { "User" }
        )]
        [SwaggerResponse(200, "کاربر با موفقیت بروزرسانی شد")]
        [SwaggerResponse(400, "داده‌های ورودی نامعتبر است")]
        [SwaggerResponse(404, "کاربر یافت نشد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Update([FromBody] EditUserDto dto)
        {
            try
            {
                var user = await _userRepository.FirstOrDefaultAsync(u => u.Id == dto.Id);
                if (user == null)
                {
                    return NotFound();
                }

                // Check if email already exists
                var existingEmail = await _userRepository.FirstOrDefaultAsync(u => u.Email == dto.Email &&
                                                                                   u.Id != dto.Id);
                if (existingEmail != null)
                {
                    return BadRequest("ایمیل قبلاً ثبت شده است");
                }

                // Check if phone number already exists
                var existingPhone = await _userRepository.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber && 
                                                                                   u.Id != dto.Id);
                if (existingPhone != null)
                {
                    return BadRequest("شماره همراه قبلاً ثبت شده است");
                }

                _mapper.Map(dto, user);

                // If password is not empty or not null, update password
                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    user.Password = SecurityHelper.PBKDF2Hash(dto.Password);
                }

                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return InternalServerErrorJson();
            }
        }

        [HttpDelete]
        [Route("delete")]
        [ValidateModel]
        [SwaggerOperation(
            Summary = "حذف کاربران",
            Description = "حذف یک یا چند کاربر براساس شناسه‌ها",
            OperationId = "DeleteUsers",
            Tags = new[] { "User" }
        )]
        [SwaggerResponse(200, "کاربران با موفقیت حذف شدند")]
        [SwaggerResponse(400, "داده‌های ورودی نامعتبر است")]
        [SwaggerResponse(404, "هیچ کاربری یافت نشد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Delete([FromBody] GuidIdsDto dto)
        {
            try
            {
                if (dto.Ids.Length == 0)
                {
                    return BadRequest();
                }

                var deletedIds = new List<Guid>();
                foreach (var id in dto.Ids)
                {
                    User? user = await _userRepository.FirstOrDefaultAsync(u => u.Id == id);
                    if (user == null)
                    {
                        continue;
                    }

                    // Soft delete
                    user.DeletedAt = DateTime.UtcNow;
                    await _userRepository.UpdateAsync(user);

                    deletedIds.Add(id);
                }

                if (deletedIds.Count == 0)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return InternalServerErrorJson();
            }
        }
    }
}