namespace BlazorBlog.ApplicationCore.Entities;

public class Comment
{
    public int CommentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public bool IsDeleted { get; set; }
    public int PostId { get; set; }
    public string UserId { get; set; } = default!;

    public Post Post { get; set; } = null!;
    public User Author { get; set; } = null!;
}
