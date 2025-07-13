using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

<<<<<<< HEAD
namespace MixerThreholdMod_0_0_1.Threading
=======
namespace MixerThreholdMod_1_0_0.Threading
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
{
    /// <summary>
    /// Thread-safe list implementation for .NET 4.8.1 compatibility.
    /// Provides safe concurrent access to list operations without blocking Unity's main thread.
    /// 
    /// ⚠️ THREAD SAFETY: This class is fully thread-safe using lock synchronization.
    /// All operations are atomic and safe for concurrent access from multiple threads.
    /// 
    /// ⚠️ MAIN THREAD WARNING: While this class is thread-safe, avoid performing 
    /// large operations in synchronous methods from the main thread to prevent UI freezes.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses traditional lock-based synchronization
    /// - Compatible with framework's threading model
    /// - Proper exception handling for all operations
    /// </summary>
    public class ThreadSafeList<T> : IEnumerable<T>
    {
        private readonly List<T> _list = new List<T>();
        private readonly object _lock = new object();

        /// <summary>
        /// Get count of items in the list (thread-safe)
        /// </summary>
        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _list.Count;
                }
            }
        }

        /// <summary>
        /// Add item to the list (thread-safe)
        /// </summary>
        public void Add(T item)
        {
            lock (_lock)
            {
                _list.Add(item);
            }
        }

        /// <summary>
        /// Clear all items from the list (thread-safe)
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
            }
        }

        /// <summary>
        /// Check if any items match the predicate (thread-safe)
        /// </summary>
        public bool Any(Func<T, bool> predicate)
        {
            lock (_lock)
            {
                return _list.Any(predicate);
            }
        }

        /// <summary>
        /// Remove all items matching the predicate (thread-safe)
        /// </summary>
        public void RemoveAll(Predicate<T> match)
        {
            lock (_lock)
            {
                _list.RemoveAll(match);
            }
        }

        /// <summary>
        /// Create a snapshot copy of the list (thread-safe)
        /// ⚠️ MAIN THREAD WARNING: Large lists may cause momentary blocking
        /// </summary>
        public List<T> ToList()
        {
            lock (_lock)
            {
                return new List<T>(_list);
            }
        }

        /// <summary>
        /// Get thread-safe enumerator (creates snapshot)
        /// ⚠️ MAIN THREAD WARNING: Large lists may cause momentary blocking
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            lock (_lock)
            {
                return new List<T>(_list).GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// Async locker for .NET 4.8.1 compatibility
    /// Provides async-safe locking mechanisms to prevent deadlocks in async/await patterns.
    /// 
    /// ⚠️ THREAD SAFETY: This class prevents deadlocks in async operations.
    /// Use this instead of traditional locks in async methods.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses SemaphoreSlim for async-compatible locking
    /// - Compatible cancellation token patterns
    /// - Proper disposal pattern implementation
    /// </summary>
    public class AsyncLocker
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Acquire async lock with optional cancellation token
        /// ⚠️ MAIN THREAD WARNING: Always use ConfigureAwait(false) when calling from Unity
        /// </summary>
        public async Task<IDisposable> LockAsync(CancellationToken ct = default(CancellationToken)) // .NET 4.8.1 compatible default parameter
        {
            await _semaphore.WaitAsync(ct).ConfigureAwait(false);
            return new Releaser(_semaphore);
        }

        /// <summary>
        /// Disposal helper for async locks
        /// </summary>
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
}