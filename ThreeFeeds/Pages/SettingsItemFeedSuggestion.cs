using CommunityToolkit.Mvvm.ComponentModel;

namespace ThreeFeeds.Pages;

public partial class SettingsItemFeedSuggestion : ObservableObject
{
    [ObservableProperty] public partial string Description { get; set; } = string.Empty;
    [ObservableProperty] public partial string Url { get; set; } = string.Empty;
}