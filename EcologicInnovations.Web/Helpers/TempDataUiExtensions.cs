using System.Text.Json;
using EcologicInnovations.Web.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EcologicInnovations.Web.Helpers;

/// <summary>
/// Standardizes how alerts and toast messages are stored in TempData across
/// public and admin controllers.
/// </summary>
public static class TempDataUiExtensions
{
    private const string AlertsKey = "__UiAlerts";
    private const string ToastsKey = "__UiToasts";

    public static void AddSuccessAlert(this ITempDataDictionary tempData, string message, string? title = null)
        => AddAlert(tempData, "success", message, title);

    public static void AddDangerAlert(this ITempDataDictionary tempData, string message, string? title = null)
        => AddAlert(tempData, "danger", message, title);

    public static void AddWarningAlert(this ITempDataDictionary tempData, string message, string? title = null)
        => AddAlert(tempData, "warning", message, title);

    public static void AddInfoAlert(this ITempDataDictionary tempData, string message, string? title = null)
        => AddAlert(tempData, "info", message, title);

    public static void AddSuccessToast(this ITempDataDictionary tempData, string message, string? title = "Success", int delay = 3500)
        => AddToast(tempData, "success", message, title, delay);

    public static void AddDangerToast(this ITempDataDictionary tempData, string message, string? title = "Error", int delay = 4500)
        => AddToast(tempData, "danger", message, title, delay);

    public static void AddWarningToast(this ITempDataDictionary tempData, string message, string? title = "Warning", int delay = 4000)
        => AddToast(tempData, "warning", message, title, delay);

    public static void AddInfoToast(this ITempDataDictionary tempData, string message, string? title = "Notice", int delay = 3500)
        => AddToast(tempData, "info", message, title, delay);

    public static List<UiAlertMessageViewModel> GetUiAlerts(this ITempDataDictionary tempData)
        => Deserialize<List<UiAlertMessageViewModel>>(tempData, AlertsKey) ?? new();

    public static List<UiToastMessageViewModel> GetUiToasts(this ITempDataDictionary tempData)
        => Deserialize<List<UiToastMessageViewModel>>(tempData, ToastsKey) ?? new();

    private static void AddAlert(ITempDataDictionary tempData, string type, string message, string? title)
    {
        var items = tempData.GetUiAlerts();
        items.Add(new UiAlertMessageViewModel
        {
            Type = type,
            Message = message,
            Title = title
        });

        tempData[AlertsKey] = JsonSerializer.Serialize(items);
    }

    private static void AddToast(ITempDataDictionary tempData, string type, string message, string? title, int delay)
    {
        var items = tempData.GetUiToasts();
        items.Add(new UiToastMessageViewModel
        {
            Type = type,
            Title = title,
            Message = message,
            Delay = delay
        });

        tempData[ToastsKey] = JsonSerializer.Serialize(items);
    }

    private static T? Deserialize<T>(ITempDataDictionary tempData, string key)
    {
        if (!tempData.TryGetValue(key, out var raw) || raw is null)
        {
            return default;
        }

        var json = raw.ToString();
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json);
    }
}
