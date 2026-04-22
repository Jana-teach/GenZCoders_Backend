using System;
using System.Threading.Tasks;

namespace GenZCoders.Services.Zoom
{
    public record ZoomMeetingInfo(string Id, string JoinUrl, string? Password);

    public interface IZoomService
    {
        Task<ZoomMeetingInfo> CreateMeetingAsync(string topic, DateTime startTimeUtc, int durationMinutes);

        string GenerateSignature(string meetingNumber, int role);

        bool IsSdkConfigured();
    }
}

