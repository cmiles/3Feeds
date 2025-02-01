using System.ComponentModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PointlessWaymarks.FeedReader;
using ThreeFeeds.Helpers;

namespace ThreeFeeds.Pages;

public partial class SettingsItemFeedUrl : ObservableObject
{
    public SettingsItemFeedUrl(IDialogService dialogs)
    {
        PropertyChanged += OnPropertyChanged;
        Dialogs = dialogs;
    }

    public IDialogService Dialogs { get; init; }

    [ObservableProperty] public partial string FeedMessage { get; set; } = string.Empty;
    [ObservableProperty] public partial string FeedState { get; set; } = "Loading";
    [ObservableProperty] public partial bool HasChanges { get; set; }
    [ObservableProperty] public partial string ReferenceRssUrl { get; set; } = string.Empty;
    [ObservableProperty] public required partial Action<string> SaveRssUrlAction { get; set; }
    [ObservableProperty] public partial string UserRssUrl { get; set; } = string.Empty;

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task SaveRssUrl()
    {
        await ThreadSwitcher.ResumeBackgroundAsync();

        try
        {
            UserRssUrl = UserRssUrl.Trim();
            SaveRssUrlAction(UserRssUrl);
            ReferenceRssUrl = UserRssUrl;
            HasChanges = false;
            WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(UserRssUrl));
        }
        catch (Exception e)
        {
            await Dialogs.DisplayAlert("Error Saving URL", e.Message, "OK");
        }

        await CheckRssUrl();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.PropertyName)) return;
        if (e.PropertyName.Equals(nameof(UserRssUrl)))
            try
            {
                HasChanges = !UserRssUrl.Trim().Equals(ReferenceRssUrl);
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
            var results = await Reader.ReadAsync(UserRssUrl);

            if (!results.Items.Any())
            {
                FeedState = "Warning";
                FeedMessage = "RSS Url didn't return an error, but no Items were found?";
                return;
            }

            var resultBuilder = new StringBuilder();

            resultBuilder.AppendLine(
                $"{results.Items.Count} Items, Title: {results.Title}, Last Update: {results.LastUpdatedDateString}");
            //resultBuilder.AppendLine($"Description: {results.Description}");
            //resultBuilder.AppendLine($"Copyright: {results.Copyright}");
            resultBuilder.AppendLine($"FeedType: {results.Type.ToString()}");
            //resultBuilder.AppendLine($"Items ({results.Items.Count})");

            //foreach (var loopItem in results.Items)
            //{
            //    resultBuilder.AppendLine(
            //        $"  {loopItem.PublishingDateString} - {loopItem.Title} - {loopItem.Author}");
            //    resultBuilder.AppendLine($"       {loopItem.Link}");
            //}

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