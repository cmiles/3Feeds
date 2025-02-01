using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PointlessWaymarks.CommonTools;
using PointlessWaymarks.FeedReader;
using ThreeFeeds.Helpers;

namespace ThreeFeeds.Pages;

public partial class FeedItemListContext : ObservableObject
{
    public FeedItemListContext()
    {
        Items = [];

        WeakReferenceMessenger.Default.Register<SettingsChangedMessage>(this, async void (_, _) =>
        {
            try
            {
                await LoadFeedItems();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        });
    }

    [ObservableProperty] public partial bool IsRunning { get; set; }
    [ObservableProperty] public partial ObservableCollection<FeedItemListItemWrapper> Items { get; set; }

    private void LoadFeedItemsFinished(Task obj)
    {
        IsRunning = false;
    }

    [RelayCommand]
    public async Task LoadFeedItems()
    {
        await ThreadSwitcher.ResumeBackgroundAsync();

        var currentItems = new List<FeedItem>();

        var feedOneUrl = PreferencesHelper.GetRssUrlOne;

        if (!string.IsNullOrWhiteSpace(feedOneUrl))
            try
            {
                var feedOne = await Reader.ReadAsync(feedOneUrl);
                currentItems.AddRange(feedOne.Items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        var feedTwoUrl = PreferencesHelper.GetRssUrlTwo;

        if (!string.IsNullOrWhiteSpace(feedTwoUrl))
            try
            {
                var feedTwo = await Reader.ReadAsync(feedTwoUrl);
                currentItems.AddRange(feedTwo.Items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        var feedThreeUrl = PreferencesHelper.GetRssUrlThree;

        if (!string.IsNullOrWhiteSpace(feedThreeUrl))
            try
            {
                var feedThree = await Reader.ReadAsync(feedThreeUrl);
                currentItems.AddRange(feedThree.Items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        var currentIds = Items.Select(x => x.Item.Id).Distinct().ToList();

        var toAdd = currentItems.Where(x => !currentIds.Contains(x.Id)).ToList();

        await ThreadSwitcher.ResumeForegroundAsync();

        toAdd.ForEach(x => Items.Add(new FeedItemListItemWrapper { Item = x }));

        Items.SortByDescending(x => x.Item.PublishingDate ?? DateTime.Now);

        IsRunning = false;
    }

    public void OnAppearing(object? sender, EventArgs e)
    {
        Task.Run(LoadFeedItems).ContinueWith(LoadFeedItemsFinished);
    }
}