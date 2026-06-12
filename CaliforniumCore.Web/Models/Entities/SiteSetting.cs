using System.ComponentModel.DataAnnotations;
using CaliforniumCore.Web.Models.Base;

namespace CaliforniumCore.Web.Models.Entities;

/// <summary>
/// Stores global site-wide settings used across the public website and admin CMS.
/// This typically behaves like a singleton record and contains business identity,
/// contact channels, branding assets, and default SEO values.
/// </summary>
public class SiteSetting : BaseEntity
{
    /// <summary>
    /// Public business name shown in the header, footer, and SEO metadata.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Short business tagline used in hero sections, footers, or metadata.
    /// </summary>
    [StringLength(300)]
    public string? Tagline { get; set; }

    /// <summary>
    /// Public URL of the company logo.
    /// Usually points to a file in the Media library.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Public URL of the site favicon.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? FaviconUrl { get; set; }

    /// <summary>
    /// Primary support email address displayed publicly.
    /// </summary>
    [StringLength(256)]
    [EmailAddress]
    public string? SupportEmail { get; set; }

    /// <summary>
    /// Sales or business inquiry email used on contact areas.
    /// </summary>
    [StringLength(256)]
    [EmailAddress]
    public string? SalesEmail { get; set; }

    /// <summary>
    /// Public phone number displayed on the website.
    /// </summary>
    [StringLength(50)]
    public string? Phone { get; set; }

    /// <summary>
    /// Business address displayed in footer and contact page.
    /// </summary>
    [StringLength(500)]
    public string? Address { get; set; }

    /// <summary>
    /// Footer HTML content for copyright, links, or small business notes.
    /// This is admin-managed HTML and should be sanitized before render.
    /// </summary>
    public string? FooterHtml { get; set; }

    /// <summary>
    /// Public Facebook page URL.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? FacebookUrl { get; set; }

    /// <summary>
    /// Public LinkedIn page URL.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? LinkedInUrl { get; set; }

    /// <summary>
    /// Public YouTube channel URL.
    /// </summary>
    [StringLength(500)]
    [Url]
    public string? YouTubeUrl { get; set; }

    /// <summary>
    /// Default SEO title used when a page-specific meta title is not provided.
    /// </summary>
    [StringLength(200)]
    public string? MetaTitleDefault { get; set; }

    /// <summary>
    /// Default SEO description used when a page-specific meta description is not provided.
    /// </summary>
    [StringLength(500)]
    public string? MetaDescriptionDefault { get; set; }

    /// <summary>
    /// Optional Google Maps embed URL shown on the public About Us page.
    /// Store only the iframe src URL, not the full iframe HTML.
    /// </summary>
    [StringLength(1000)]
    public string? GoogleMapEmbedUrl { get; set; }

    [StringLength(100)]
    public string? HomeValueKicker { get; set; }

    [StringLength(200)]
    public string? HomeValueTitle { get; set; }

    [StringLength(500)]
    public string? HomeValueIntro { get; set; }

    [StringLength(100)]
    public string? HomeValue1IconCssClass { get; set; }

    [StringLength(150)]
    public string? HomeValue1Title { get; set; }

    [StringLength(500)]
    public string? HomeValue1Description { get; set; }

    [StringLength(100)]
    public string? HomeValue2IconCssClass { get; set; }

    [StringLength(150)]
    public string? HomeValue2Title { get; set; }

    [StringLength(500)]
    public string? HomeValue2Description { get; set; }

    [StringLength(100)]
    public string? HomeValue3IconCssClass { get; set; }

    [StringLength(150)]
    public string? HomeValue3Title { get; set; }

    [StringLength(500)]
    public string? HomeValue3Description { get; set; }

    [StringLength(100)]
    public string? HomeValue4IconCssClass { get; set; }

    [StringLength(150)]
    public string? HomeValue4Title { get; set; }

    [StringLength(500)]
    public string? HomeValue4Description { get; set; }
}
