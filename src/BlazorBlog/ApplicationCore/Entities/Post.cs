namespace BlazorBlog.ApplicationCore.Entities;

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? Image { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public bool IsDeleted { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string AuthorId { get; set; } = default!;

    public User Author { get; set; } = null!;
    public HashSet<Comment> Comments { get; set; } = new HashSet<Comment>();
}
