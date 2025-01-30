namespace ThreeFeeds.Helpers;

public class ThreadSwitcher
{
    public static ThreadPoolThreadSwitcher ResumeBackgroundAsync()
    {
        return new ThreadPoolThreadSwitcher();
    }

    public static DispatcherThreadSwitcher ResumeForegroundAsync()
    {
        return new DispatcherThreadSwitcher(MainThread.GetMainThreadSynchronizationContextAsync().Result);
    }
}