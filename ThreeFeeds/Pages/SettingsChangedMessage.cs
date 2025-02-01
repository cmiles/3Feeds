using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ThreeFeeds.Pages;

public class SettingsChangedMessage(string newRssUrl) : ValueChangedMessage<string>(newRssUrl);