using EcologicInnovations.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcologicInnovations.Web.Data.Configurations;

/// <summary>
/// Fluent configuration for MediaFile.
/// </summary>
public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
{
    public void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        builder.ToTable("MediaFiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(260);

        builder.Property(x => x.OriginalFileName)
            .IsRequired()
            .HasMaxLength(260);

        builder.Property(x => x.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.PublicUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ContentType)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.AltText)
            .HasMaxLength(300);

        builder.Property(x => x.Title)
            .HasMaxLength(200);

        builder.Property(x => x.MediaGroup)
            .HasMaxLength(100);

        builder.Property(x => x.UploadedByUserId)
            .HasMaxLength(450);

        builder.Property(x => x.FileSize)
            .HasDefaultValue(0L);

        builder.Property(x => x.UploadedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(x => x.FilePath)
            .IsUnique()
            .HasDatabaseName("UX_MediaFiles_FilePath");

        builder.HasIndex(x => x.PublicUrl)
            .IsUnique()
            .HasDatabaseName("UX_MediaFiles_PublicUrl");

        builder.HasIndex(x => new { x.MediaGroup, x.UploadedAt })
            .HasDatabaseName("IX_MediaFiles_MediaGroup_UploadedAt");

        builder.HasIndex(x => new { x.IsActive, x.UploadedAt })
            .HasDatabaseName("IX_MediaFiles_IsActive_UploadedAt");

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_MediaFiles_CreatedAt");
    }
}
