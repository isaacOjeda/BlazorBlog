using BlazorBlog.ApplicationCore.Common.Errors;
using BlazorBlog.Infrastructure.Persistence;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.ApplicationCore.Features.Posts.Queries.GetPostDetails;

public class GetPostDetailsQuery(string slug) : IRequest<Result<GetPostDetailsResponse>>
{
    public string Slug { get; set; } = slug;
}

public class GetPostDetailsHandler(AppDbContext context) : IRequestHandler<GetPostDetailsQuery, Result<GetPostDetailsResponse>>
{

    public async Task<Result<GetPostDetailsResponse>> Handle(GetPostDetailsQuery request, CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .Include(p => p.Author)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Slug == request.Slug, cancellationToken);

        if (post == null)
        {
            return Result.Fail(new NotFoundError($"Post with slug {request.Slug} not found"));
        }

        return Result.Ok(new GetPostDetailsResponse
        {
            PostId = post.PostId,
            Title = post.Title,
            Subtitle = post.Subtitle,
            Slug = post.Slug,
            Author = post.Author.UserName!,
            AuthorId = post.Author.Id,
            Content = post.Content,
            Created = post.Created,
            Updated = post.Updated,
            // Comments = post.Comments.Select(c => new CommentDto
            // {
            //     CommentId = c.CommentId,
            //     Author = c.Author.DisplayName,
            //     AuthorId = c.Author.Id,
            //     Content = c.Content,
            //     Created = c.Created,
            //     Updated = c.Updated
            // }).ToList()
        });
    }
}


public class GetPostDetailsResponse
{
    public int PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    // public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
}