using System.ComponentModel.DataAnnotations;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Input model used when editing media metadata such as title, alt text, media group, and active state.
/// </summary>
public class MediaEditInputModel
{
    /// <summary>
    /// Media file id being edited.
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Editable alt text for accessibility and HTML reuse.
    /// </summary>
    [StringLength(300)]
    [Display(Name = "Alt Text")]
    public string? AltText { get; set; }

    /// <summary>
    /// Editable friendly title.
    /// </summary>
    [StringLength(200)]
    [Display(Name = "Title")]
    public string? Title { get; set; }

    /// <summary>
    /// Editable media group.
    /// </summary>
    [StringLength(100)]
    [Display(Name = "Media Group")]
    public string? MediaGroup { get; set; }

    /// <summary>
    /// Current active state.
    /// </summary>
    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Public URL shown read-only in the editor.
    /// </summary>
    public string? PublicUrl { get; set; }

    /// <summary>
    /// Prebuilt reusable HTML snippet shown read-only in the editor.
    /// </summary>
    public string? CopyHtmlSnippet { get; set; }
}
