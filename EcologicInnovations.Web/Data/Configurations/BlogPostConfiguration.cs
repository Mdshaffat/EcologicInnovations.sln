using EcologicInnovations.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcologicInnovations.Web.Data.Configurations;

/// <summary>
/// Fluent configuration for BlogPost.
/// </summary>
public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("BlogPosts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(220);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(220);

        builder.Property(x => x.FeatureImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.Excerpt)
            .IsRequired()
            .HasMaxLength(1200);

        builder.Property(x => x.ContactFormTitle)
            .HasMaxLength(200);

        builder.Property(x => x.IsFeatured)
            .HasDefaultValue(false);

        builder.Property(x => x.IsPublished)
            .HasDefaultValue(false);

        builder.Property(x => x.ShowContactForm)
            .HasDefaultValue(false);

        builder.Property(x => x.MetaTitle)
            .HasMaxLength(200);

        builder.Property(x => x.MetaDescription)
            .HasMaxLength(500);

        builder.Property(x => x.OgImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.HasOne(x => x.BlogCategory)
            .WithMany(x => x.BlogPosts)
            .HasForeignKey(x => x.BlogCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.Slug)
            .IsUnique()
            .HasDatabaseName("UX_BlogPosts_Slug");

        builder.HasIndex(x => new { x.BlogCategoryId, x.IsPublished, x.PublishedAt, x.CreatedAt })
            .HasDatabaseName("IX_BlogPosts_Category_Publish_PublishedAt_CreatedAt");

        builder.HasIndex(x => new { x.IsFeatured, x.IsPublished, x.PublishedAt })
            .HasDatabaseName("IX_BlogPosts_Featured_Publish_PublishedAt");

        builder.HasIndex(x => new { x.IsPublished, x.PublishedAt, x.CreatedAt })
            .HasDatabaseName("IX_BlogPosts_Publish_PublishedAt_CreatedAt");

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_BlogPosts_CreatedAt");
    }
}
