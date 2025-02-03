using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PointlessWaymarks.FeedReader;
using ThreeFeeds.Helpers;

namespace ThreeFeeds.Pages;

public partial class FeedItemListItemWrapper : ObservableObject
{
    [ObservableProperty] public required partial IDialogService Dialogs { get; set; }
    [ObservableProperty] public required partial FeedItem Item { get; set; }

    [RelayCommand]
    public async Task OpenLinkInSystemBrowser()
    {
        await ThreadSwitcher.ResumeForegroundAsync();

        try
        {
            var launchedOk = await Browser.Default!.OpenAsync(Item.Link, BrowserLaunchMode.SystemPreferred);

            if (!launchedOk) await Dialogs.DisplayAlert("Error Opening Link", "The link did not open?", "Ok");
        }
        catch (Exception e)
        {
            await Dialogs.DisplayAlert("Error Opening Link", e.Message, "Ok");
        }
    }
}