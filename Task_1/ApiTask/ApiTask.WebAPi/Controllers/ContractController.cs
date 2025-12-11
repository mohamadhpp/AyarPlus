using ApiTask.Application.Attributes.Filter;
using ApiTask.Application.Dtos.ContractDtos;
using ApiTask.Application.Dtos.IdDtos;
using ApiTask.Application.Interfaces.Repositories;
using ApiTask.Domain.Entities;
using ApiTask.WebApi.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTask.WebApi.Controllers
{
    [ApiController]
    [Route("api/contract")]
    [Produces("application/json")]
    [SwaggerTag("مدیریت قراردادها")]
    public class ContractController(IMapper mapper,
                                   ILogger<ContractController> logger,
                                   IContractRepository contractRepository,
                                   IWebHostEnvironment environment,
                                   IConfiguration configuration) : Controller(mapper)
    {
        private readonly ILogger<ContractController> _logger = logger;
        private readonly IContractRepository _contractRepository = contractRepository;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly IConfiguration _configuration = configuration;

        [HttpGet]
        [Route("get")]
        [ValidateModel]
        [SwaggerOperation(
            Summary = "دریافت لیست قراردادها",
            Description = "دریافت لیست تمامی قراردادها موجود در سیستم",
            OperationId = "GetContracts",
            Tags = new[] { "Contract" }
        )]
        [SwaggerResponse(200, "لیست قراردادها با موفقیت دریافت شد", typeof(IEnumerable<Contract>))]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Get([FromQuery] Application.Dtos.Common.ReadDto? dto)
        {
            try
            {
                var query = _contractRepository.Query();

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(dto?.Search))
                {
                    query = query.Where(c => c.Name.Contains(dto.Search) ||
                                             (c.Email != null && c.Email.Contains(dto.Search)) ||
                                             (c.Phone != null && c.Phone.Contains(dto.Search)) ||
                                             (c.Type != null && c.Type.Contains(dto.Search)));
                }

                // Apply pagination
                if (dto != null)
                {
                    query = query
                        .Skip((dto.Page - 1) * dto.Size)
                        .Take(dto.Size);
                }

                var contracts = await _contractRepository.ToListAsync(query);

                return Ok(contracts);
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
            Summary = "دریافت قرارداد با شناسه",
            Description = "دریافت اطلاعات یک قرارداد خاص بر اساس شناسه",
            OperationId = "GetContractById",
            Tags = new[] { "Contract" }
        )]
        [SwaggerResponse(200, "قرارداد با موفقیت دریافت شد", typeof(Contract))]
        [SwaggerResponse(404, "قرارداد یافت نشد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var contract = await _contractRepository.FirstOrDefaultAsync(c => c.Id == id);
                if (contract == null)
                {
                    return NotFound();
                }

                return Ok(contract);
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
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "ایجاد قرارداد جدید",
            Description = "ایجاد یک قرارداد جدید در سیستم با اطلاعات وارد شده و تصاویر",
            OperationId = "CreateContract",
            Tags = new[] { "Contract" }
        )]
        [SwaggerResponse(200, "قرارداد با موفقیت ایجاد شد")]
        [SwaggerResponse(400, "داده‌های ورودی نامعتبر است")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Create([FromForm] AddContractDto dto)
        {
            try
            {
                var contract = _mapper.Map<Contract>(dto);
                contract.Id = Guid.NewGuid();

                // Handle file uploads
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                string folderPath = Path.Combine(Constant.ContractsFolder, contract.Id.ToString());

                if (dto.Front != null)
                {
                    contract.Front = await FileHelper.SaveFileAsync(dto.Front, folderPath, allowedExtensions);
                }

                if (dto.Back != null)
                {
                    contract.Back = await FileHelper.SaveFileAsync(dto.Back, folderPath, allowedExtensions);
                }

                await _contractRepository.AddAsync(contract);

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
        [Consumes("multipart/form-data")]
        [SwaggerOperation(
            Summary = "بروزرسانی قرارداد",
            Description = "بروزرسانی اطلاعات قرارداد موجود",
            OperationId = "UpdateContract",
            Tags = new[] { "Contract" }
        )]
        [SwaggerResponse(200, "قرارداد با موفقیت بروزرسانی شد")]
        [SwaggerResponse(400, "داده‌های ورودی نامعتبر است")]
        [SwaggerResponse(404, "قرارداد یافت نشد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> Update([FromForm] EditContractDto dto)
        {
            try
            {
                var contract = await _contractRepository.FirstOrDefaultAsync(c => c.Id == dto.Id);
                if (contract == null)
                {
                    return NotFound();
                }

                // Store old file paths for deletion
                var oldFront = contract.Front;
                var oldBack = contract.Back;

                _mapper.Map(dto, contract);

                // Handle file uploads
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                string folderPath = Path.Combine(Constant.ContractsFolder, contract.Id.ToString());

                if (dto.Front != null)
                {
                    contract.Front = await FileHelper.SaveFileAsync(dto.Front, folderPath, allowedExtensions);

                    // Delete old file
                    if (!string.IsNullOrEmpty(oldFront))
                    {
                        FileHelper.DeleteFile(Path.Combine(folderPath, oldFront));
                    }
                }

                if (dto.Back != null)
                {
                    contract.Back = await FileHelper.SaveFileAsync(dto.Back, folderPath, allowedExtensions);

                    // Delete old file
                    if (!string.IsNullOrEmpty(oldBack))
                    {
                        FileHelper.DeleteFile(Path.Combine(folderPath, oldBack));
                    }
                }

                contract.UpdatedAt = DateTime.UtcNow;
                await _contractRepository.UpdateAsync(contract);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null);
                return InternalServerErrorJson();
            }
        }

        [HttpGet]
        [Route("get-image/{id}/{side}")]
        [SwaggerOperation(
            Summary = "دریافت تصویر قرارداد",
            Description = "دریافت تصویر جلو یا پشت قرارداد بر اساس شناسه",
            OperationId = "GetContractImage",
            Tags = new[] { "Contract" }
        )]
        [SwaggerResponse(200, "تصویر با موفقیت دریافت شد")]
        [SwaggerResponse(404, "قرارداد یا تصویر یافت نشد")]
        [SwaggerResponse(400, "پارامتر side باید front یا back باشد")]
        [SwaggerResponse(500, "خطای سرور")]
        public async Task<IActionResult> GetImage(Guid id, string side)
        {
            try
            {
                // Validate side parameter
                if (side.ToLower() != "front" && side.ToLower() != "back")
                {
                    return BadRequest(new { message = "پارامتر side باید front یا back باشد" });
                }

                // Get contract
                var contract = await _contractRepository.FirstOrDefaultAsync(c => c.Id == id);
                if (contract == null)
                {
                    return NotFound(new { message = "قرارداد یافت نشد" });
                }

                // Get image path based on side
                string? imageName = side.ToLower() == "front" ? contract.Front : contract.Back;

                if (string.IsNullOrEmpty(imageName))
                {
                    return NotFound(new { message = $"تصویر {(side.ToLower() == "front" ? "جلو" : "پشت")} برای این قرارداد وجود ندارد" });
                }

                // Build full file path
                string folderPath = Path.Combine(Constant.ContractsFolder, contract.Id.ToString());
                var fullPath = Path.Combine(folderPath, imageName);

                if (!System.IO.File.Exists(fullPath))
                {
                    return NotFound(new { message = "فایل تصویر یافت نشد" });
                }

                // Get file extension and determine content type
                var extension = Path.GetExtension(fullPath).ToLower();
                var contentType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".gif" => "image/gif",
                    _ => "application/octet-stream"
                };

                // Read and return file
                var imageBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
                return File(imageBytes, contentType);
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
            Summary = "حذف قراردادها",
            Description = "حذف یک یا چند قرارداد براساس شناسه‌ها",
            OperationId = "DeleteContracts",
            Tags = new[] { "Contract" }
        )]
        [SwaggerResponse(200, "قراردادها با موفقیت حذف شدند")]
        [SwaggerResponse(400, "داده‌های ورودی نامعتبر است")]
        [SwaggerResponse(404, "هیچ قراردادی یافت نشد")]
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
                    Contract? contract = await _contractRepository.FirstOrDefaultAsync(c => c.Id == id);
                    if (contract == null)
                    {
                        continue;
                    }

                    contract.DeletedAt = DateTime.UtcNow;
                    await _contractRepository.UpdateAsync(contract);

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
