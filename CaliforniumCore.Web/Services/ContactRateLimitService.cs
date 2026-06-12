using CaliforniumCore.Web.Data;
using CaliforniumCore.Web.Services.Interfaces;
using CaliforniumCore.Web.ViewModels.Contact;
using Microsoft.EntityFrameworkCore;

namespace CaliforniumCore.Web.Services;

public class ContactRateLimitService : IContactRateLimitService
{
    private const string DeviceCookieName = "cc_contact_device";
    private const int MaxMessages = 3;
    private static readonly TimeSpan Window = TimeSpan.FromHours(36);

    private readonly ApplicationDbContext _dbContext;

    public ContactRateLimitService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ContactRateLimitResult> CheckAsync(
        HttpContext httpContext,
        ContactFormInputModel input,
        CancellationToken cancellationToken = default)
    {
        var deviceId = EnsureDeviceId(httpContext);
        var ipAddress = GetClientIpAddress(httpContext);
        var email = input.Email.Trim().ToLowerInvariant();
        var windowStartUtc = DateTime.UtcNow.Subtract(Window);

        var recentMessages = await _dbContext.ContactMessages
            .AsNoTracking()
            .Where(x => x.CreatedAt >= windowStartUtc)
            .Where(x =>
                x.Email.ToLower() == email ||
                x.SubmitterDeviceId == deviceId ||
                (!string.IsNullOrWhiteSpace(ipAddress) && x.SubmitterIpAddress == ipAddress))
            .OrderBy(x => x.CreatedAt)
            .Select(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        if (recentMessages.Count < MaxMessages)
        {
            return new ContactRateLimitResult(true, deviceId, recentMessages.Count, null, null);
        }

        var oldestCountedMessage = recentMessages
            .OrderBy(x => x)
            .Take(recentMessages.Count - MaxMessages + 1)
            .Last();
        var retryAfterUtc = oldestCountedMessage.Add(Window);

        return new ContactRateLimitResult(
            false,
            deviceId,
            recentMessages.Count,
            retryAfterUtc,
            "For security, one person or device can send up to 3 messages in 36 hours. Please try again later.");
    }

    private static string EnsureDeviceId(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue(DeviceCookieName, out var existing) &&
            existing.Length is >= 24 and <= 64 &&
            existing.All(char.IsLetterOrDigit))
        {
            return existing;
        }

        var deviceId = Guid.NewGuid().ToString("N");
        httpContext.Response.Cookies.Append(
            DeviceCookieName,
            deviceId,
            new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Secure = httpContext.Request.IsHttps,
                Expires = DateTimeOffset.UtcNow.AddDays(180)
            });

        return deviceId;
    }

    private static string? GetClientIpAddress(HttpContext httpContext)
    {
        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        return httpContext.Connection.RemoteIpAddress?.ToString();
    }

    private static string? GetUserAgent(HttpContext httpContext)
    {
        var userAgent = httpContext.Request.Headers.UserAgent.ToString();
        if (string.IsNullOrWhiteSpace(userAgent))
        {
            return null;
        }

        return userAgent.Length > 512 ? userAgent[..512] : userAgent;
    }
}
