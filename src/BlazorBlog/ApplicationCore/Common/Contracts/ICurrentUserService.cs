namespace BlazorBlog.ApplicationCore.Common.Contracts;

public interface ICurrentUserService
{
    string UserId { get; }
    bool IsAuthenticated { get; }
    string UserName { get; }
}
