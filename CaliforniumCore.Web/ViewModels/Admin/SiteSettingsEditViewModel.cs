using System.ComponentModel.DataAnnotations;

namespace CaliforniumCore.Web.ViewModels.Admin;

public class SiteSettingsEditViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; } = "Californium Core";

    [StringLength(300)]
    public string? Tagline { get; set; }

    [StringLength(500)]
    [Display(Name = "Logo URL")]
    public string? LogoUrl { get; set; }

    [StringLength(500)]
    [Display(Name = "Favicon URL")]
    public string? FaviconUrl { get; set; }

    [StringLength(256)]
    [EmailAddress]
    [Display(Name = "Support Email")]
    public string? SupportEmail { get; set; }

    [StringLength(256)]
    [EmailAddress]
    [Display(Name = "Sales Email")]
    public string? SalesEmail { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [Display(Name = "Footer HTML")]
    public string? FooterHtml { get; set; }

    [StringLength(500)]
    [Url]
    [Display(Name = "Facebook URL")]
    public string? FacebookUrl { get; set; }

    [StringLength(500)]
    [Url]
    [Display(Name = "LinkedIn URL")]
    public string? LinkedInUrl { get; set; }

    [StringLength(500)]
    [Url]
    [Display(Name = "YouTube URL")]
    public string? YouTubeUrl { get; set; }

    [StringLength(200)]
    [Display(Name = "Default Meta Title")]
    public string? MetaTitleDefault { get; set; }

    [StringLength(500)]
    [Display(Name = "Default Meta Description")]
    public string? MetaDescriptionDefault { get; set; }

    [StringLength(1000)]
    [Display(Name = "Google Map Embed URL")]
    public string? GoogleMapEmbedUrl { get; set; }

    [StringLength(100)]
    [Display(Name = "Section Kicker")]
    public string? HomeValueKicker { get; set; }

    [StringLength(200)]
    [Display(Name = "Section Title")]
    public string? HomeValueTitle { get; set; }

    [StringLength(500)]
    [Display(Name = "Section Intro")]
    public string? HomeValueIntro { get; set; }

    [StringLength(100)]
    [Display(Name = "Card 1 Icon CSS")]
    public string? HomeValue1IconCssClass { get; set; }

    [StringLength(150)]
    [Display(Name = "Card 1 Title")]
    public string? HomeValue1Title { get; set; }

    [StringLength(500)]
    [Display(Name = "Card 1 Description")]
    public string? HomeValue1Description { get; set; }

    [StringLength(100)]
    [Display(Name = "Card 2 Icon CSS")]
    public string? HomeValue2IconCssClass { get; set; }

    [StringLength(150)]
    [Display(Name = "Card 2 Title")]
    public string? HomeValue2Title { get; set; }

    [StringLength(500)]
    [Display(Name = "Card 2 Description")]
    public string? HomeValue2Description { get; set; }

    [StringLength(100)]
    [Display(Name = "Card 3 Icon CSS")]
    public string? HomeValue3IconCssClass { get; set; }

    [StringLength(150)]
    [Display(Name = "Card 3 Title")]
    public string? HomeValue3Title { get; set; }

    [StringLength(500)]
    [Display(Name = "Card 3 Description")]
    public string? HomeValue3Description { get; set; }

    [StringLength(100)]
    [Display(Name = "Card 4 Icon CSS")]
    public string? HomeValue4IconCssClass { get; set; }

    [StringLength(150)]
    [Display(Name = "Card 4 Title")]
    public string? HomeValue4Title { get; set; }

    [StringLength(500)]
    [Display(Name = "Card 4 Description")]
    public string? HomeValue4Description { get; set; }
}
