using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GenZCoders.Services.Zoom
{
    public class ZoomService : IZoomService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ZoomService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var accountId = _configuration["Zoom:AccountId"] ?? "";
            var clientId = _configuration["Zoom:ClientId"] ?? "";
            var clientSecret = _configuration["Zoom:ClientSecret"] ?? "";

            if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new InvalidOperationException("Zoom credentials are not configured. Please set Zoom:AccountId, Zoom:ClientId, and Zoom:ClientSecret in appsettings.json");
            }

            var url = $"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={accountId}";

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var basicToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicToken);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(
                    $"Zoom OAuth failed ({response.StatusCode}): {body}. Ensure you use a Server-to-Server OAuth app with correct AccountId, ClientId, and ClientSecret.");
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            return doc.RootElement.GetProperty("access_token").GetString()!;
        }

        public async Task<ZoomMeetingInfo> CreateMeetingAsync(string topic, DateTime startTimeUtc, int durationMinutes)
        {
            var accessToken = await GetAccessTokenAsync();

            var payload = new
            {
                topic,
                type = 2, // scheduled meeting
                start_time = startTimeUtc.ToString("o"),
                duration = durationMinutes,
                timezone = "UTC"
            };

            var json = JsonSerializer.Serialize(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.zoom.us/v2/users/me/meetings")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(
                    $"Zoom API create meeting failed ({response.StatusCode}): {body}. Ensure your Server-to-Server OAuth app has meeting:write scope.");
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);
            var root = doc.RootElement;

            var id = root.GetProperty("id").ToString();
            var joinUrl = root.GetProperty("join_url").GetString()!;
            var password = root.TryGetProperty("password", out var pwEl) ? pwEl.GetString() : null;

            return new ZoomMeetingInfo(id, joinUrl, password);
        }

        public string GenerateSignature(string meetingNumber, int role)
        {
            // Zoom SDK Key migration (2026): Use Client ID and Client Secret instead of deprecated SdkKey/SdkSecret
            var clientId = _configuration["Zoom:MeetingSdkClientId"] ?? "";
            var clientSecret = _configuration["Zoom:MeetingSdkClientSecret"] ?? "";

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new InvalidOperationException(
                    "Zoom Meeting SDK credentials are not configured. Set Zoom:MeetingSdkClientId and Zoom:MeetingSdkClientSecret in appsettings.json. See ZOOM_SETUP.md.");
            }

            if (!long.TryParse(meetingNumber, out var meetingNumberNumeric))
            {
                throw new InvalidOperationException("Invalid meeting number. It must be numeric digits only.");
            }

            // Zoom is strict about numeric types and required fields in the signature payload.
            // Also subtract a few seconds from iat to avoid clock-skew issues.
            var iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 30;
            var exp = iat + 60 * 60; // 1 hour

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clientSecret));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var payload = new JwtPayload
            {
                { "sdkKey", clientId }, // JWT claim name stays "sdkKey", value is now Client ID
                { "mn", meetingNumberNumeric },
                { "role", role },
                { "iat", iat },
                { "exp", exp },
                { "tokenExp", exp }
            };

            var header = new JwtHeader(creds);
            var token = new JwtSecurityToken(header, payload);

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }

        public bool IsSdkConfigured()
        {
            var clientId = _configuration["Zoom:MeetingSdkClientId"] ?? "";
            var clientSecret = _configuration["Zoom:MeetingSdkClientSecret"] ?? "";
            return !string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret);
        }
    }
}

