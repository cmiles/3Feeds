namespace ThreeFeeds.Pages;

public partial class FeedItemListPage : ContentPage
{
    public FeedItemListPage()
    {
        InitializeComponent();

        var feedItemListContext = new FeedItemListContext();
        BindingContext = feedItemListContext;

        Appearing += feedItemListContext.OnAppearing;
    }
}