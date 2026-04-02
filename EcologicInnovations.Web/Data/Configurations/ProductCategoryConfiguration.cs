using EcologicInnovations.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcologicInnovations.Web.Data.Configurations;

/// <summary>
/// Fluent configuration for ProductCategory.
/// </summary>
public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.SortOrder)
            .HasDefaultValue(0);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(x => x.Slug)
            .IsUnique()
            .HasDatabaseName("UX_ProductCategories_Slug");

        builder.HasIndex(x => new { x.IsActive, x.SortOrder, x.Name })
            .HasDatabaseName("IX_ProductCategories_IsActive_SortOrder_Name");

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_ProductCategories_CreatedAt");
    }
}
