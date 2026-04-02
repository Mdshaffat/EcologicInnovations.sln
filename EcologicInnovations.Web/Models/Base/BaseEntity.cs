using System.ComponentModel.DataAnnotations;

namespace EcologicInnovations.Web.Models.Base;

/// <summary>
/// Base entity used by most database tables in the application.
/// It standardizes the primary key and audit timestamps.
/// Additional state fields such as IsActive, IsPublished, or SortOrder
/// are added only where they make business sense.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Primary key for the entity.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// UTC date and time when the record was created.
    /// This should be set automatically during insert.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC date and time when the record was last updated.
    /// This should be refreshed automatically during update.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
