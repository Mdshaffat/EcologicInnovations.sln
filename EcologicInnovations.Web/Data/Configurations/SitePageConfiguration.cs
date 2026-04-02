using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcologicInnovations.Web.Data.Configurations;

/// <summary>
/// Fluent configuration for SitePage.
/// </summary>
public class SitePageConfiguration : IEntityTypeConfiguration<SitePage>
{
    public void Configure(EntityTypeBuilder<SitePage> builder)
    {
        builder.ToTable("SitePages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.PageKey)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.BannerImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.ShortIntro)
            .HasMaxLength(1000);

        builder.Property(x => x.MetaTitle)
            .HasMaxLength(200);

        builder.Property(x => x.MetaDescription)
            .HasMaxLength(500);

        builder.Property(x => x.IsPublished)
            .HasDefaultValue(false);

        builder.Property(x => x.SortOrder)
            .HasDefaultValue(0);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(x => x.PageKey)
            .IsUnique()
            .HasDatabaseName("UX_SitePages_PageKey");

        builder.HasIndex(x => x.Slug)
            .IsUnique()
            .HasDatabaseName("UX_SitePages_Slug");

        builder.HasIndex(x => new { x.IsPublished, x.SortOrder, x.CreatedAt })
            .HasDatabaseName("IX_SitePages_IsPublished_SortOrder_CreatedAt");
    }
}
