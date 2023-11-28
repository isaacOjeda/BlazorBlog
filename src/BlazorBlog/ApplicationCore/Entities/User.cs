using Microsoft.AspNetCore.Identity;

namespace BlazorBlog.ApplicationCore.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
}
