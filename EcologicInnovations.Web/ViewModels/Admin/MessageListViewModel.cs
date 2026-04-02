using EcologicInnovations.Web.ViewModels.Shared;

namespace EcologicInnovations.Web.ViewModels.Admin;

/// <summary>
/// Main admin message list page model.
/// </summary>
public class MessageListViewModel
{
    /// <summary>
    /// Current active filters.
    /// </summary>
    public MessageListFilterViewModel Filter { get; set; } = new();

    /// <summary>
    /// Messages matching the filter.
    /// </summary>
    public List<MessageListItemViewModel> Messages { get; set; } = new();

    /// <summary>
    /// Pagination state.
    /// </summary>
    public PaginationViewModel Pagination { get; set; } = new();
}
