using CommunityToolkit.Maui.Core;

namespace ThreeFeeds.Pages;

public interface IDialogService
{
    Task<bool> DisplayAlert(string title, string message, string accept, string cancel, FlowDirection flowDirection);
    Task DisplayAlert(string title, string message, string cancel);
    public Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);

    Task ShowSnackbar(string message, CancellationToken cancelToken, Action? action = null,
        string actionButtonText = "Ok", TimeSpan? duration = null, SnackbarOptions? visualOptions = null,
        IView? anchor = null);

    Task ShowToast(string message, CancellationToken cancelToken, ToastDuration duration = ToastDuration.Short,
        double textSize = 14D);
}