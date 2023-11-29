using BlazorBlog.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.ApplicationCore.Features.Posts.Queries.GetPosts;

public class GetPostsQuery : IRequest<GetPostsResponse>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}


public class GetPostsHandler(IDbContextFactory<AppDbContext> dbFactory) : IRequestHandler<GetPostsQuery, GetPostsResponse>
{

    public async Task<GetPostsResponse> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        await using var context = await dbFactory.CreateDbContextAsync(cancellationToken);

        var response = new GetPostsResponse
        {
            Count = await context.Posts.CountAsync(cancellationToken),
            Posts = await context.Posts
                .OrderByDescending(x => x.Created)
                .Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .Select(x => new GetPostsResponse.GetPostsItems
                {
                    PostId = x.PostId,
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    Slug = x.Slug,
                    Created = x.Created,
                    Updated = x.Updated,
                    Author = x.Author.UserName
                })
                .ToListAsync(cancellationToken)
        };

        return response;
    }
}


public class GetPostsResponse
{
    public List<GetPostsItems> Posts { get; set; } = [];
    public int Count { get; set; }

    public class GetPostsItems
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Author { get; set; } = string.Empty;
    }
}
