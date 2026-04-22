using GenZCoders.DTOs.MediaDto;
using GenZCoders.Services.MediaService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers
{
    [ApiController]
    [Route("api/medias")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private const string UploadFolder = "uploads";
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const int MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        public MediaController(IMediaService mediaService, IWebHostEnvironment env, IConfiguration config)
        {
            _mediaService = mediaService;
            _env = env;
            _config = config;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(MaxFileSizeBytes)]
        public async Task<IActionResult> Upload(IFormFile? file, [FromQuery] string folder = "payment_proofs")
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file provided." });

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
                return BadRequest(new { message = "Invalid file type. Allowed: jpg, jpeg, png, gif, webp." });

            if (file.Length > MaxFileSizeBytes)
                return BadRequest(new { message = "File too large. Max 10 MB." });

            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var uploadsDir = Path.Combine(webRoot, UploadFolder, folder);
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var baseUrl = _config["App:BaseUrl"]?.TrimEnd('/')
                ?? $"{Request.Scheme}://{Request.Host}";
            var relativePath = $"/{UploadFolder}/{folder}/{fileName}";
            var fullUrl = $"{baseUrl}{relativePath}";

            return Ok(new { url = fullUrl, filePath = relativePath });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MediaCreateDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Request body is required." });

            if (string.IsNullOrWhiteSpace(dto.TableName))
                return BadRequest(new { message = "TableName is required." });

            if (dto.TableId <= 0)
                return BadRequest(new { message = "TableId must be greater than 0." });

            if (string.IsNullOrWhiteSpace(dto.FilePath))
                return BadRequest(new { message = "FilePath is required." });

            try
            {
                await _mediaService.AddMediaAsync(dto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message;
                return StatusCode(500, new
                {
                    message = $"Failed to save media: {ex.Message}",
                    detail = innerMessage
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string tableName,
            [FromQuery] long tableId)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return BadRequest(new { message = "tableName is required." });

            if (tableId <= 0)
                return BadRequest(new { message = "tableId must be greater than 0." });

            try
            {
                var result = await _mediaService.GetMediasAsync(tableName, tableId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (id <= 0)
                return BadRequest(new { message = "id must be greater than 0." });

            try
            {
                await _mediaService.DeleteMediaAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}
