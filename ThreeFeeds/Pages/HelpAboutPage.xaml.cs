using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ThreeFeeds.Pages;

public partial class HelpAboutPage : IDialogService
{
    public HelpAboutPage()
    {
        InitializeComponent();

        BindingContext = new HelpAboutContext(this);
    }

    public async Task ShowSnackbar(string message, CancellationToken cancelToken, Action? action = null,
        string actionButtonText = "Ok", TimeSpan? duration = null, SnackbarOptions? visualOptions = null,
        IView? anchor = null)
    {
        var snackbar = Snackbar.Make(message, action, actionButtonText, duration, visualOptions, anchor);
        await snackbar.Show(cancelToken);
    }

    public async Task ShowToast(string message, CancellationToken cancelToken,
        ToastDuration duration = ToastDuration.Short, double textSize = 14D)
    {
        var toast = Toast.Make(message, duration, textSize);
        await toast.Show(cancelToken);
    }
}