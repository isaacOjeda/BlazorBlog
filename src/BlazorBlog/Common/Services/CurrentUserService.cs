using BlazorBlog.ApplicationCore.Common.Contracts;
using System.Security.Claims;

namespace BlazorBlog.Common.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        UserId = userId ?? string.Empty;
        IsAuthenticated = !string.IsNullOrEmpty(UserId);
        UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
    }

    public string UserId { get; }
    public bool IsAuthenticated { get; }
    public string UserName { get; }
}
