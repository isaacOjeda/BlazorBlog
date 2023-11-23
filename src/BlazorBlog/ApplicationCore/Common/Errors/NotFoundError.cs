using FluentResults;

namespace BlazorBlog.ApplicationCore.Common.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
    }
}
