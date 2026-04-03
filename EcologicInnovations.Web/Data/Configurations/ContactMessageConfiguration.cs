using EcologicInnovations.Web.Models.Entities;
using EcologicInnovations.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcologicInnovations.Web.Data.Configurations;

/// <summary>
/// Fluent configuration for ContactMessage.
/// </summary>
public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.ToTable("ContactMessages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Company)
            .HasMaxLength(200);

        builder.Property(x => x.Subject)
            .HasMaxLength(200);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(x => x.SourceType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.SourceTitle)
            .HasMaxLength(250);

        builder.Property(x => x.PageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue(ContactMessageStatus.New);

        builder.Property(x => x.AdminNote)
            .HasMaxLength(2000);

        builder.Property(x => x.SubmitterIpAddress)
            .HasMaxLength(45);

        builder.Property(x => x.SubmitterUserAgent)
            .HasMaxLength(512);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.ContactMessages)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.BlogPost)
            .WithMany(x => x.ContactMessages)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => new { x.Status, x.CreatedAt })
            .HasDatabaseName("IX_ContactMessages_Status_CreatedAt");

        builder.HasIndex(x => new { x.SourceType, x.CreatedAt })
            .HasDatabaseName("IX_ContactMessages_SourceType_CreatedAt");

        builder.HasIndex(x => x.ProductId)
            .HasDatabaseName("IX_ContactMessages_ProductId");

        builder.HasIndex(x => x.BlogPostId)
            .HasDatabaseName("IX_ContactMessages_BlogPostId");

        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_ContactMessages_CreatedAt");
    }
}
