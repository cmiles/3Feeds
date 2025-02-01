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
        SaveRssUrlCommand.NotifyCanExecuteChanged();
    }

    public IDialogService Dialogs { get; init; }

    [ObservableProperty] public partial string FeedMessage { get; set; } = string.Empty;
    [ObservableProperty] public partial string FeedState { get; set; } = "Loading";
    [ObservableProperty] public partial bool HasChanges { get; set; }
    [ObservableProperty] public partial string ReferenceRssUrl { get; set; } = string.Empty;
    [ObservableProperty] public required partial Action<string> SaveRssUrlAction { get; set; }
    [ObservableProperty] public partial SettingsItemFeedSuggestion? SelectedSuggestion { get; set; }

    [ObservableProperty]
    public partial List<SettingsItemFeedSuggestion> Suggestions { get; set; } =
    [
        new() { Description = "NPR", Url = "https://feeds.npr.org/1001/rss.xml" },
        new() { Description = "BBC World News", Url = "http://feeds.bbci.co.uk/news/world/rss.xml" },
        new() { Description = "France 24", Url = "https://www.france24.com/en/rss" },
        new()
        {
            Description = "Reuters", Url = "https://www.reutersagency.com/feed/?taxonomy=best-topics&post_type=best"
        },
        new() { Description = "TechCrunch", Url = "https://techcrunch.com/feed/" },
        new() { Description = "Hacker News Front Page", Url = "https://techcrunch.com/feed/" }
    ];

    [ObservableProperty] public partial string UserRssUrl { get; set; } = string.Empty;

    [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(HasChanges))]
    public async Task SaveRssUrl()
    {
        await ThreadSwitcher.ResumeBackgroundAsync();

        try
        {
            UserRssUrl = UserRssUrl.Trim();
            SaveRssUrlAction(UserRssUrl);
            ReferenceRssUrl = UserRssUrl;
            WeakReferenceMessenger.Default.Send(new SettingsChangedMessage(UserRssUrl));
        }
        catch (Exception e)
        {
            await Dialogs.DisplayAlert("Error Saving URL", e.Message, "OK");
        }

        await CheckRssUrl();
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task UseAndSaveSuggestion()
    {
        await ThreadSwitcher.ResumeBackgroundAsync();
        if (SelectedSuggestion is null)
        {
            await ThreadSwitcher.ResumeForegroundAsync();
            await Dialogs.DisplayAlert("No Suggestion Selected", "Please select a suggestion to use", "OK");
            return;
        }

        UserRssUrl = SelectedSuggestion.Url;
        await SaveRssUrl();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.PropertyName)) return;
        if (e.PropertyName.Equals(nameof(UserRssUrl)) || e.PropertyName.Equals(nameof(ReferenceRssUrl)))
            try
            {
                HasChanges = !UserRssUrl.Trim().Equals(ReferenceRssUrl);
                SaveRssUrlCommand.NotifyCanExecuteChanged();
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
                FeedMessage = "Warning: RSS Url didn't return an error, but no Items were found?";
                return;
            }

            var resultBuilder = new StringBuilder();

            resultBuilder.AppendLine(
                $"Success: {results.Items.Count} Items, Title: {results.Title}, Last Update: {results.LastUpdatedDateString}");
            resultBuilder.AppendLine($"FeedType: {results.Type.ToString()}");

            FeedState = "Success";
            FeedMessage = resultBuilder.ToString();
        }
        catch (Exception e)
        {
            FeedState = "Error";
            FeedMessage = $"ERROR: {e.Message}";
        }
    }
}