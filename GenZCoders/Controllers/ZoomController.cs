using GenZCoders.Services.Zoom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GenZCoders.Controllers
{
    public class ZoomSignatureRequest
    {
        public string MeetingNumber { get; set; } = string.Empty;
        public int Role { get; set; } // 1 = host, 0 = attendee
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ZoomController : ControllerBase
    {
        private readonly IZoomService _zoomService;
        private readonly IConfiguration _configuration;

        public ZoomController(IZoomService zoomService, IConfiguration configuration)
        {
            _zoomService = zoomService;
            _configuration = configuration;
        }

        [HttpGet("sdk-available")]
        public IActionResult GetSdkAvailable()
        {
            var available = _zoomService.IsSdkConfigured();
            return Ok(new { sdkAvailable = available });
        }

        [HttpPost("signature")]
        public IActionResult GetSignature([FromBody] ZoomSignatureRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.MeetingNumber))
                {
                    return BadRequest(new { message = "MeetingNumber is required" });
                }

                var signature = _zoomService.GenerateSignature(request.MeetingNumber, request.Role);
                // For @zoom/meetingsdk@5.x embedded join, sdkKey is required.
                // We return the Meeting SDK Client ID here.
                var sdkKey = _configuration["Zoom:MeetingSdkClientId"];
                return Ok(new { signature, sdkKey });
            }
            catch (InvalidOperationException ex)
            {
                var msg = ex.Message;
                if (msg.Contains("Meeting SDK") || msg.Contains("ClientId") || msg.Contains("ClientSecret"))
                {
                    msg += " Use Client ID and Client Secret from your Meeting SDK app. Until then, use 'Join in browser'.";
                }
                return BadRequest(new { message = msg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message ?? "Failed to generate signature" });
            }
        }
    }
}

