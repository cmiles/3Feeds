using System.ComponentModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PointlessWaymarks.FeedReader;
using ThreeFeeds.Helpers;

namespace ThreeFeeds.Pages;

public partial class SettingsItemFeedUrl : ObservableObject
{
    public SettingsItemFeedUrl()
    {
        PropertyChanged += OnPropertyChanged;
    }

    [ObservableProperty] public partial string FeedMessage { get; set; } = string.Empty;
    [ObservableProperty] public partial string FeedState { get; set; } = "Loading";
    [ObservableProperty] public partial string RssUrl { get; set; } = string.Empty;
    [ObservableProperty] public required partial Action<string> SaveRssUrlAction { get; set; }

    [RelayCommand]
    public async Task SaveRssUrl()
    {
        await ThreadSwitcher.ResumeBackgroundAsync();

        SaveRssUrlAction(RssUrl);
        await CheckRssUrl();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.PropertyName)) return;
        if (e.PropertyName.Equals(nameof(RssUrl)))
            try
            {
                Task.Run(CheckRssUrl);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
    }

    public async Task CheckRssUrl()
    {
        try
        {
            var results = await Reader.ReadAsync(RssUrl);

            if (!results.Items.Any())
            {
                FeedState = "Warning";
                FeedMessage = "RSS Url didn't return an error, but no Items were found?";
                return;
            }

            var resultBuilder = new StringBuilder();

            resultBuilder.AppendLine(
                $"{results.Items.Count} Items, Title: {results.Title}, Last Update: {results.LastUpdatedDateString}");
            resultBuilder.AppendLine($"Description: {results.Description}");
            resultBuilder.AppendLine($"Copyright: {results.Copyright}");
            resultBuilder.AppendLine($"FeedType: {results.Type.ToString()}");
            resultBuilder.AppendLine($"Items ({results.Items.Count}):");

            foreach (var loopItem in results.Items)
            {
                resultBuilder.AppendLine(
                    $"  {loopItem.PublishingDateString} - {loopItem.Title} - {loopItem.Author}");
                resultBuilder.AppendLine($"       {loopItem.Link}");
            }

            FeedState = "Success";
            FeedMessage = resultBuilder.ToString();
        }
        catch (Exception e)
        {
            FeedState = "Error";
            FeedMessage = e.Message;
        }
    }
}