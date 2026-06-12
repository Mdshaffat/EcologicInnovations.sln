using CaliforniumCore.Web.ViewModels.Contact;

namespace CaliforniumCore.Web.Services.Interfaces;

public interface IContactRateLimitService
{
    Task<ContactRateLimitResult> CheckAsync(
        HttpContext httpContext,
        ContactFormInputModel input,
        CancellationToken cancellationToken = default);
}

public sealed record ContactRateLimitResult(
    bool IsAllowed,
    string DeviceId,
    int ExistingMessageCount,
    DateTime? RetryAfterUtc,
    string? ErrorMessage);
