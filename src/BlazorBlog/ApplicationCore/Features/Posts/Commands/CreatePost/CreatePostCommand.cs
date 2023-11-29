using BlazorBlog.ApplicationCore.Common.Contracts;
using BlazorBlog.ApplicationCore.Entities;
using BlazorBlog.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.ApplicationCore.Features.Posts.Commands.CreatePost;

public class CreatePostCommand(string title, string content, string subtitle) : IRequest
{
    [Required]
    public string Title { get; set; } = title;
    [Required]
    public string Content { get; set; } = content;
    [Required]
    public string Subtitle { get; set; } = subtitle;
}

public class CreatePostCommandHandler(DbContextFactory<AppDbContext> contextFactory, ICurrentUserService currentUser) : IRequestHandler<CreatePostCommand>
{
    public async Task Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var newPost = new Post
        {
            Title = request.Title,
            Subtitle = request.Subtitle,
            Content = request.Content,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            AuthorId = currentUser.UserId,
            Slug = GenerateSlugFromTitle(request.Title),

        };

        context.Posts.Add(newPost);

        await context.SaveChangesAsync(cancellationToken);
    }


    private static string GenerateSlugFromTitle(string title)
    {
        var slug = title.ToLowerInvariant().Replace(" ", "-");
        return slug;
    }
}