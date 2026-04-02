namespace EcologicInnovations.Web.ViewModels.Shared;

/// <summary>
/// Represents one toast notification shown in a floating stack.
/// </summary>
public class UiToastMessageViewModel
{
    /// <summary>
    /// Toast style name: success, danger, warning, info.
    /// </summary>
    public string Type { get; set; } = "info";

    /// <summary>
    /// Short toast title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Main toast message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Auto-hide time in milliseconds.
    /// </summary>
    public int Delay { get; set; } = 3500;
}
