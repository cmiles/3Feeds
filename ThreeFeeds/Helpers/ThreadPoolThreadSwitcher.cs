using System.Runtime.CompilerServices;

namespace ThreeFeeds.Helpers;

public struct ThreadPoolThreadSwitcher : INotifyCompletion
{
    public bool IsCompleted =>
        SynchronizationContext.Current == null;

    public ThreadPoolThreadSwitcher GetAwaiter()
    {
        return this;
    }

    public void GetResult()
    {
    }

    public void OnCompleted(Action continuation)
    {
        ThreadPool.QueueUserWorkItem(_ => continuation());
    }
}