using ApiTask.Application.Attributes.Filter;
using ApiTask.Application.Dtos.CompanyDtos;
using ApiTask.Application.Dtos.IdDtos;
using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ApiTask.Application.Dtos.Common;

namespace ApiTask.WebApi.Controllers
{
    [ApiController]
    [Route("api/company")]
    [Produces("application/json")]
    [SwaggerTag("مدیریت شرکت‌ها")]
    public class CompanyController(IMapper mapper,
                                   ILogger<CompanyController> logger,
                                   ICompanyRepository companyRepository) : Controller(mapper)
    {
        private readonly ILogger<CompanyController> _logger = logger;
        private readonly ICompanyRepository _companyRepository = companyRepository;

        [HttpGet]
        [Route("get")]
        [ValidateModel]
        [SwaggerOperation(
            Summary = "دریافت لیست شرکت‌ها",
            Description = "دریافت لیست تمام شرکت‌های موجود در سیستم",
            OperationId = "GetCompanies",
            Tags = new[] { "Company" }
        )]
        [SwaggerResponse(200, "لیست شرکت‌ها با موفقیت دریافت شد", typeof(IEnumerable<Company>))]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Get([FromQuery] ReadDto? dto)
        {
            try
            {
                var query = _companyRepository.Query();

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(dto?.Search))
                {
                    query = query.Where(c => c.Title.Contains(dto.Search) ||
                                             (c.Description != null && c.Description.Contains(dto.Search)) ||
                                             c.Address.Contains(dto.Search));
                }

                // Apply pagination
                if (dto != null)
                {
                    query = query
                        .Skip((dto.Page - 1) * dto.Size)
                        .Take(dto.Size);
                }

                var companies = await _companyRepository.ToListAsync(query);

                return Ok(companies);
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
            Summary = "دریافت شرکت با شناسه",
            Description = "دریافت اطلاعات یک شرکت براساس شناسه",
            OperationId = "GetCompanyById",
            Tags = new[] { "Company" }
        )]
        [SwaggerResponse(200, "شرکت با موفقیت دریافت شد", typeof(Company))]
        [SwaggerResponse(404, "شرکت یافت نشد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var company = await _companyRepository.FirstOrDefaultAsync(c => c.Id == id);
                if (company == null)
                {
                    return NotFound();
                }

                return Ok(company);
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
            Summary = "ایجاد شرکت جدید",
            Description = "ایجاد یک شرکت جدید در سیستم با اطلاعات وارد شده",
            OperationId = "CreateCompany",
            Tags = new[] { "Company" }
        )]
        [SwaggerResponse(200, "شرکت با موفقیت ایجاد شد")]
        [SwaggerResponse(400, "داده‌های ورودی نامعتبر است")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Create([FromBody] AddCompanyDto dto)
        {
            try
            {
                var company = _mapper.Map<Company>(dto);
                await _companyRepository.AddAsync(company);

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
            Summary = "بروزرسانی شرکت",
            Description = "بروزرسانی اطلاعات شرکت موجود",
            OperationId = "UpdateCompany",
            Tags = new[] { "Company" }
        )]
        [SwaggerResponse(200, "شرکت با موفقیت بروزرسانی شد")]
        [SwaggerResponse(400, "داده‌های ورودی نامعتبر است")]
        [SwaggerResponse(404, "شرکت یافت نشد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Update([FromBody] EditCompanyDto dto)
        {
            try
            {
                var company = await _companyRepository.FirstOrDefaultAsync(c => c.Id == dto.Id);
                if (company == null)
                {
                    return NotFound();
                }

                _mapper.Map(dto, company);
                company.UpdatedAt = DateTime.UtcNow;

                await _companyRepository.UpdateAsync(company);

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
            Summary = "حذف شرکت‌ها",
            Description = "حذف یک یا چند شرکت براساس شناسه‌ها",
            OperationId = "DeleteCompanies",
            Tags = new[] { "Company" }
        )]
        [SwaggerResponse(200, "شرکت‌ها با موفقیت حذف شدند")]
        [SwaggerResponse(400, "داده‌های ورودی نامعتبر است")]
        [SwaggerResponse(404, "هیچ شرکتی یافت نشد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Delete([FromBody] LongIdsDto dto)
        {
            try
            {
                if (dto.Ids.Length == 0)
                {
                    return BadRequest();
                }

                var deletedIds = new List<long>();
                foreach (var id in dto.Ids)
                {
                    Company? company = await _companyRepository.FirstOrDefaultAsync(c => c.Id == id);
                    if (company == null)
                    {
                        continue;
                    }

                    company.DeletedAt = DateTime.UtcNow;
                    await _companyRepository.UpdateAsync(company);

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