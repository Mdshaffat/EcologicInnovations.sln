namespace EcologicInnovations.Web.ViewModels.Home;

/// <summary>
/// Represents a short value proposition card on the home page.
/// </summary>
public class HomeValuePointViewModel
{
    /// <summary>
    /// Small icon class or key used in the UI.
    /// </summary>
    public string? IconCssClass { get; set; }

    /// <summary>
    /// Card title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Card description.
    /// </summary>
    public string? Description { get; set; }
}
