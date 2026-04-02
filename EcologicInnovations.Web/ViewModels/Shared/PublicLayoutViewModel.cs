namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Optional aggregate model for shared public shell components.
/// This is useful if you later want one service to prepare the entire public layout state.
/// </summary>
public class PublicLayoutViewModel
{
    /// <summary>
    /// Navigation data for the top navbar.
    /// </summary>
    public PublicNavigationViewModel Navigation { get; set; } = new();

    /// <summary>
    /// Footer data for the bottom footer.
    /// </summary>
    public PublicFooterViewModel Footer { get; set; } = new();

    /// <summary>
    /// Final layout-level SEO state.
    /// </summary>
    public LayoutSeoViewModel Seo { get; set; } = new();
}
