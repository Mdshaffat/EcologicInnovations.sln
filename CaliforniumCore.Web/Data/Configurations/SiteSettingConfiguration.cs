using CaliforniumCore.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaliforniumCore.Web.Data.Configurations;

/// <summary>
/// Fluent configuration for SiteSetting.
/// </summary>
public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
{
    public void Configure(EntityTypeBuilder<SiteSetting> builder)
    {
        builder.ToTable("SiteSettings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Tagline)
            .HasMaxLength(300);

        builder.Property(x => x.LogoUrl)
            .HasMaxLength(500);

        builder.Property(x => x.FaviconUrl)
            .HasMaxLength(500);

        builder.Property(x => x.SupportEmail)
            .HasMaxLength(256);

        builder.Property(x => x.SalesEmail)
            .HasMaxLength(256);

        builder.Property(x => x.Phone)
            .HasMaxLength(50);

        builder.Property(x => x.Address)
            .HasMaxLength(500);

        builder.Property(x => x.FacebookUrl)
            .HasMaxLength(500);

        builder.Property(x => x.LinkedInUrl)
            .HasMaxLength(500);

        builder.Property(x => x.YouTubeUrl)
            .HasMaxLength(500);

        builder.Property(x => x.MetaTitleDefault)
            .HasMaxLength(200);

        builder.Property(x => x.MetaDescriptionDefault)
            .HasMaxLength(500);

        builder.Property(x => x.GoogleMapEmbedUrl)
            .HasMaxLength(1000);

        builder.Property(x => x.HomeValueKicker)
            .HasMaxLength(100);

        builder.Property(x => x.HomeValueTitle)
            .HasMaxLength(200);

        builder.Property(x => x.HomeValueIntro)
            .HasMaxLength(500);

        builder.Property(x => x.HomeValue1IconCssClass)
            .HasMaxLength(100);

        builder.Property(x => x.HomeValue1Title)
            .HasMaxLength(150);

        builder.Property(x => x.HomeValue1Description)
            .HasMaxLength(500);

        builder.Property(x => x.HomeValue2IconCssClass)
            .HasMaxLength(100);

        builder.Property(x => x.HomeValue2Title)
            .HasMaxLength(150);

        builder.Property(x => x.HomeValue2Description)
            .HasMaxLength(500);

        builder.Property(x => x.HomeValue3IconCssClass)
            .HasMaxLength(100);

        builder.Property(x => x.HomeValue3Title)
            .HasMaxLength(150);

        builder.Property(x => x.HomeValue3Description)
            .HasMaxLength(500);

        builder.Property(x => x.HomeValue4IconCssClass)
            .HasMaxLength(100);

        builder.Property(x => x.HomeValue4Title)
            .HasMaxLength(150);

        builder.Property(x => x.HomeValue4Description)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_SiteSettings_CreatedAt");
    }
}
