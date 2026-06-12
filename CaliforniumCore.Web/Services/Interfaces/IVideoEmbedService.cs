using CaliforniumCore.Web.ViewModels.Shared;

namespace CaliforniumCore.Web.Services.Interfaces;

public interface IVideoEmbedService
{
    string? NormalizeSourceUrl(string? value);

    VideoEmbedViewModel? BuildEmbed(string? value);
}
