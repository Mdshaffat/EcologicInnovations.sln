using EcologicInnovations.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcologicInnovations.Web.Data.Configurations;

/// <summary>
/// Fluent configuration for Product.
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(220);

        builder.Property(x => x.ShortDescription)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.MainImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.ContactFormTitle)
            .HasMaxLength(200);

        builder.Property(x => x.MenuSortOrder)
            .HasDefaultValue(0);

        builder.Property(x => x.ListSortOrder)
            .HasDefaultValue(0);

        builder.Property(x => x.ContactFormEnabled)
            .HasDefaultValue(true);

        builder.Property(x => x.ShowInProductMenu)
            .HasDefaultValue(false);

        builder.Property(x => x.IsFeatured)
            .HasDefaultValue(false);

        builder.Property(x => x.IsPublished)
            .HasDefaultValue(false);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

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

        builder.HasOne(x => x.ProductCategory)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.ProductCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Slug)
            .IsUnique()
            .HasDatabaseName("UX_Products_Slug");

        builder.HasIndex(x => new { x.ProductCategoryId, x.IsPublished, x.IsActive, x.ListSortOrder, x.CreatedAt })
            .HasDatabaseName("IX_Products_Category_Publish_Active_ListSort_CreatedAt");

        builder.HasIndex(x => new { x.ShowInProductMenu, x.IsPublished, x.IsActive, x.MenuSortOrder, x.Title })
            .HasDatabaseName("IX_Products_MenuDropdown");

        builder.HasIndex(x => new { x.IsFeatured, x.IsPublished, x.IsActive, x.CreatedAt })
            .HasDatabaseName("IX_Products_Featured_Publish_Active_CreatedAt");

        builder.HasIndex(x => new { x.IsPublished, x.IsActive, x.CreatedAt })
            .HasDatabaseName("IX_Products_Publish_Active_CreatedAt");

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_Products_CreatedAt");
    }
}
