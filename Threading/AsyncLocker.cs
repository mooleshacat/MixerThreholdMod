

using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Async locker for .NET 4.8.1 compatibility.
/// Provides async-safe locking mechanisms to prevent deadlocks in async/await patterns.
/// âš ï¸ THREAD SAFETY: Prevents deadlocks in async operations.
/// </summary>
public class AsyncLocker
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public async Task<IDisposable> LockAsync(CancellationToken ct = default(CancellationToken))
    {
        await _semaphore.WaitAsync(ct).ConfigureAwait(false);
        return new Releaser(_semaphore);
    }

    private class Releaser : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private bool _disposed = false;

        public Releaser(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _semaphore.Release();
                _disposed = true;
            }
        }
    }
}