namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents one dismissible inline alert rendered near the top of a page.
/// </summary>
public class UiAlertMessageViewModel
{
    /// <summary>
    /// Bootstrap alert style name: success, danger, warning, info.
    /// </summary>
    public string Type { get; set; } = "info";

    /// <summary>
    /// Main alert text.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Optional short heading for the alert.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Whether the alert should be dismissible.
    /// </summary>
    public bool Dismissible { get; set; } = true;
}
