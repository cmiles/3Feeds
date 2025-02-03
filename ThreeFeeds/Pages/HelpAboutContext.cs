using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThreeFeeds.Helpers;

namespace ThreeFeeds.Pages;

public partial class HelpAboutContext : ObservableObject
{
    public HelpAboutContext(IDialogService dialogs)
    {
        Dialogs = dialogs;
    }

    [ObservableProperty] public partial IDialogService Dialogs { get; set; }

    public string HelpAboutMarkdown =>
        """
        # 3Feeds
        ## Limited Distraction RSS App

        Finding ways to doom scroll, waste time and have your life sucked away by the internet is EASY!

        A difficult challenge these days is finding ways to access news, information, education, and entertainment on the Internet without being tempted by time-wasting clickbait.

        This app displays the Titles from a maximum of 3 RSS Feeds and opens the item in the browser if you press/click it. That's all it does! No organizing, no summaries, no marking items read, no distractions, no ads, no suggested articles... And with a maximum of 3 RSS Feeds you can't add 'just one more feed'.

        This app might seem pointless - there are already plenty of more functional RSS Readers that can do this and much more. I wrote this app so that I had somewhere to plug into a 'news' RSS feeds and have a single, no distractions, no temptations, way to check-in. The app is deliberately simple/minimal and deliberately boring...

        ---
        
        # Privacy
        
        The 3Feeds app does not monitor, record or report on any aspect of your app usage. Your feed links are stored on your local device - there is no cloud storage, sync or backup for your feeds or their contents.
        
        This application uses the internet to retrieve your specified feeds and will transmit information in plain/clear text if the feed url is 'http'.
        
        ---

        # About the Author

        3Feeds is the product of Charles Miles and Pointless Waymarks Software. I am a Software Developer, Outdoor Retail Specialist and Artist living in Southern Arizona. I create software, photographs, audio and text - often focusing on the local landscape. Places you can find me:
        
         - [Pointless Waymarks Software](https://software.pointlesswaymarks.com/)
         - [Pointless Waymarks - Ramblings, Questionable Geographics, Photographic Half-truths](https://software.pointlesswaymarks.com/)
         - [cmiles.info - Life, Tech and Unimportant Minutiae](https://software.pointlesswaymarks.com/)

        ---

        # Tools
        - [Microsoft Visual Studio](https://visualstudio.microsoft.com/)
        - [Jetbrains ReSharper](https://www.jetbrains.com/resharper/)
        - [Fossil](https://fossil-scm.org/)
        - [Chisel - Fossil SCM Hosting](https://chiselapp.com/)

        ---
        
        # Software/Libraries/Packages
        
        This application is MIT licensed and the code is available here: [3Feeds Fossil Repository](https://chiselapp.com/user/cmiles/repository/3feeds/). This app has been tested on Windows and Android - other platforms supported by Maui may work but have not been tested.

        - [.NET 9](https://dotnet.microsoft.com/en-us/)
        - [MAUI](https://dotnet.microsoft.com/en-us/apps/maui)
        - [C# FeedReader - Pointless Waymarks Tools](https://chiselapp.com/user/cmiles/repository/pointless-waymarks-tools/): Originally based on [FeedReader](https://github.com/arminreiter/FeedReader) project from [Armin Reiter](https://arminreiter.com/)
        - [.NET MAUI Community Toolkit](https://github.com/CommunityToolkit/Maui)
        - [.NET Community Toolkit](https://github.com/CommunityToolkit/dotnet)
        - [AngleSharp](https://github.com/AngleSharp/AngleSharp)
        - [0xc3u/Indiko.Maui.Controls.Markdown](https://github.com/0xc3u/Indiko.Maui.Controls.Markdown)
        """;

    [RelayCommand]
    public async Task OpenLinkInSystemBrowser(string link)
    {
        await ThreadSwitcher.ResumeForegroundAsync();

        try
        {
            var launchedOk = await Browser.Default.OpenAsync(link, BrowserLaunchMode.SystemPreferred);

            if (!launchedOk) await Dialogs.DisplayAlert("Error Opening Link", "The link did not open?", "Ok");
        }
        catch (Exception e)
        {
            await Dialogs.DisplayAlert("Error Opening Link", e.Message, "Ok");
        }
    }
}