using System.Runtime.CompilerServices;

namespace ThreeFeeds.Helpers;

public struct DispatcherThreadSwitcher : INotifyCompletion
{
    private readonly SynchronizationContext _context;

    internal DispatcherThreadSwitcher(SynchronizationContext uiContext)
    {
        _context = uiContext;
    }

    public bool IsCompleted => _context == SynchronizationContext.Current;

    public DispatcherThreadSwitcher GetAwaiter()
    {
        return this;
    }

    public void GetResult()
    {
    }

    public void OnCompleted(Action continuation)
    {
        MainThread.BeginInvokeOnMainThread(continuation);
    }
}