using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ThreeFeeds.Pages;

public partial class SettingsContext : ObservableObject
{
    public SettingsContext(IDialogService dialogs)
    {
        Dialogs = dialogs;
        var settingOne = new SettingsItemFeedUrl(Dialogs)
        {
            UserRssUrl = PreferencesHelper.GetRssUrlOne, ReferenceRssUrl = PreferencesHelper.GetRssUrlOne,
            SaveRssUrlAction = PreferencesHelper.SetRssUrlOne
        };
        var settingTwo = new SettingsItemFeedUrl(Dialogs)
        {
            UserRssUrl = PreferencesHelper.GetRssUrlTwo, ReferenceRssUrl = PreferencesHelper.GetRssUrlTwo,
            SaveRssUrlAction = PreferencesHelper.SetRssUrlTwo
        };
        var settingThree = new SettingsItemFeedUrl(Dialogs)
        {
            UserRssUrl = PreferencesHelper.GetRssUrlThree, ReferenceRssUrl = PreferencesHelper.GetRssUrlThree,
            SaveRssUrlAction = PreferencesHelper.SetRssUrlThree
        };

        UrlSettings = [settingOne, settingTwo, settingThree];
    }

    public IDialogService Dialogs { get; }

    [ObservableProperty] public partial ObservableCollection<SettingsItemFeedUrl> UrlSettings { get; set; }
}