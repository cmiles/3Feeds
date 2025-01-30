using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ThreeFeeds.Pages;

public partial class SettingsContext : ObservableObject
{
    public SettingsContext()
    {
        var settingOne = new SettingsItemFeedUrl
            { RssUrl = PreferencesHelper.GetRssUrlOne, SaveRssUrlAction = PreferencesHelper.SetRssUrlOne };
        var settingTwo = new SettingsItemFeedUrl
            { RssUrl = PreferencesHelper.GetRssUrlTwo, SaveRssUrlAction = PreferencesHelper.SetRssUrlTwo };
        var settingThree = new SettingsItemFeedUrl
            { RssUrl = PreferencesHelper.GetRssUrlThree, SaveRssUrlAction = PreferencesHelper.SetRssUrlThree };

        UrlSettings = [settingOne, settingTwo, settingThree];
    }

    [ObservableProperty] public partial ObservableCollection<SettingsItemFeedUrl> UrlSettings { get; set; }
}