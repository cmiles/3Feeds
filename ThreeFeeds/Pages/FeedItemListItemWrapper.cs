using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PointlessWaymarks.FeedReader;

namespace ThreeFeeds.Pages;

public partial class FeedItemListItemWrapper : ObservableObject
{
    [ObservableProperty] public required partial FeedItem Item { get; set; }
    [ObservableProperty] public partial bool ShowDescription { get; set; }

    [RelayCommand]
    public void ToggleShowDescription()
    {
        ShowDescription = !ShowDescription;
    }

    [RelayCommand]
    public void OpenLinkInSystemBrowser()
    {
        Browser.Default.OpenAsync(Item.Link, BrowserLaunchMode.SystemPreferred);
    }
}