namespace ThreeFeeds.Pages;

public static class PreferencesHelper
{
    public static string GetRssUrlOne => Preferences.Get("tfs_rss_url_one", string.Empty);

    public static string GetRssUrlThree => Preferences.Get("tfs_rss_url_three", string.Empty);

    public static string GetRssUrlTwo => Preferences.Get("tfs_rss_url_two", string.Empty);

    public static void SetRssUrlOne(string value)
    {
        Preferences.Set("tfs_rss_url_one", value);
    }

    public static void SetRssUrlTwo(string value)
    {
        Preferences.Set("tfs_rss_url_two", value);
    }

    public static void SetRssUrlThree(string value)
    {
        Preferences.Set("tfs_rss_url_three", value);
    }
}