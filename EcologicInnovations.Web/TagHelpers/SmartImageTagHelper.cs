using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EcologicInnovations.Web.TagHelpers;

/// <summary>
/// Adds performance-oriented defaults to image tags.
/// Non-hero images become lazy-loaded by default, while hero images can be marked eager.
/// </summary>
[HtmlTargetElement("img", Attributes = "asp-smart-image")]
public class SmartImageTagHelper : TagHelper
{
    /// <summary>
    /// Enables the smart image behavior on this img tag.
    /// Usage: <img asp-smart-image="true" ... />
    /// </summary>
    [HtmlAttributeName("asp-smart-image")]
    public bool SmartImage { get; set; } = true;

    /// <summary>
    /// Marks the image as a hero/above-the-fold asset.
    /// Hero images remain eager and get high fetch priority.
    /// </summary>
    [HtmlAttributeName("asp-is-hero")]
    public bool IsHero { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!SmartImage)
        {
            return;
        }

        output.Attributes.SetAttribute("decoding", "async");

        if (IsHero)
        {
            output.Attributes.SetAttribute("loading", "eager");
            output.Attributes.SetAttribute("fetchpriority", "high");
        }
        else
        {
            output.Attributes.SetAttribute("loading", "lazy");
            output.Attributes.SetAttribute("fetchpriority", "low");
        }

        if (!output.Attributes.ContainsName("alt"))
        {
            output.Attributes.SetAttribute("alt", string.Empty);
        }
    }
}
