using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PointlessWaymarks.FeedReader;
using ThreeFeeds.Helpers;

namespace ThreeFeeds.Pages;

public partial class FeedItemListContext : ObservableObject
{
    public FeedItemListContext(IDialogService dialogs)
    {
        Items = [];

        Dialogs = dialogs;

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

    [ObservableProperty] public partial IDialogService Dialogs { get; set; }
    [ObservableProperty] public partial bool IsRunning { get; set; }
    [ObservableProperty] public partial ObservableCollection<FeedItemListItemWrapper> Items { get; set; }
    [ObservableProperty] public partial bool Loading { get; set; } = true;

    private void LoadFeedItemsFinished(Task obj)
    {
        IsRunning = false;
        Loading = false;
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

        var existingIds = Items.Select(x => x.Item.Id).Distinct().ToList();
        var currentIds = currentItems.Select(x => x.Id).Distinct().ToList();

        var toAdd = currentItems.Where(x => !existingIds.Contains(x.Id)).ToList();
        var toRemove = Items.Where(x => !currentIds.Contains(x.Item.Id)).ToList();

        await ThreadSwitcher.ResumeForegroundAsync();

        toRemove.ForEach(x => Items.Remove(x));
        toAdd.ForEach(x => Items.Add(new FeedItemListItemWrapper { Item = x, Dialogs = Dialogs }));

        Items.SortByDescending(x => x.Item.PublishingDate ?? DateTime.Now);

        IsRunning = false;
    }

    public void OnAppearing(object? sender, EventArgs e)
    {
        Task.Run(LoadFeedItems).ContinueWith(LoadFeedItemsFinished);
    }
}