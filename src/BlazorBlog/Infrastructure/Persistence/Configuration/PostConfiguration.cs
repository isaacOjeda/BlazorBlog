using BlazorBlog.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorBlog.Infrastructure.Persistence.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.PostId);

        builder.HasOne(builder => builder.Author)
            .WithMany(user => user.Posts)
            .HasForeignKey(post => post.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Property(p => p.Slug)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(p => p.Slug)
            .IsUnique();
    }
}
