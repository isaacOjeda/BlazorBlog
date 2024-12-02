using FluentResults;

namespace BlazorBlog.ApplicationCore.Common.Errors;

public class NotFoundError(string message) : Error(message);
