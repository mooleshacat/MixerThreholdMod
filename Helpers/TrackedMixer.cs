using ScheduleOne.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Threading;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Represents a tracked mixer configuration with metadata.
    /// 
    /// ⚠️ THREAD SAFETY: This class is designed to be used with thread-safe collections.
    /// Individual property access is not synchronized - use appropriate locking when accessing.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses standard property syntax
    /// - Compatible with framework's serialization
    /// </summary>
    public class TrackedMixer
    {
        /// <summary>
        /// The actual mixer configuration instance from the game
        /// ⚠️ THREAD SAFETY: Access to this property should be synchronized with the containing collection
        /// </summary>
        public MixingStationConfiguration ConfigInstance { get; set; }

        /// <summary>
        /// Unique stable ID for this mixer instance
        /// ⚠️ THREAD SAFETY: This ID should remain constant once assigned
        /// </summary>
        public int MixerInstanceID { get; set; }

        /// <summary>
        /// Tracks whether event listeners have been attached to this mixer
        /// ⚠️ THREAD SAFETY: This flag should be set atomically to prevent double-attachment
        /// </summary>
        public bool ListenerAdded { get; set; } = false;
    }

    /// <summary>
    /// Thread-safe collection manager for tracked mixer instances.
    /// Provides async and sync access patterns for .NET 4.8.1 compatibility.
    /// 
    /// ⚠️ THREAD SAFETY: All operations are thread-safe using AsyncLocker.
    /// Safe to call from any thread including Unity background threads.
    /// 
    /// ⚠️ MAIN THREAD WARNING: Synchronous methods can block the calling thread.
    /// Prefer async methods when called from Unity's main thread.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses compatible async/await patterns with ConfigureAwait(false)
    /// - Provides both sync and async versions for compatibility
    /// - Uses SemaphoreSlim for cross-platform locking
    /// </summary>
    public static class TrackedMixers
    {
        private static readonly List<TrackedMixer> _mixers = new List<TrackedMixer>();
        private static readonly AsyncLocker _locker = new AsyncLocker();

        /// <summary>
        /// Get a read-only snapshot of all tracked mixers
        /// ⚠️ THREAD SAFETY: Returns a snapshot copy, safe for iteration
        /// </summary>
        /// <returns>Read-only list of tracked mixers</returns>
        public static async Task<IReadOnlyList<TrackedMixer>> GetAllAsync()
        {
            Exception getAllError = null;
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    if (_mixers == null)
                    {
                        Main.logger.Warn(1, "TrackedMixers.GetAllAsync: _mixers is null, returning empty list");
                        return new List<TrackedMixer>().AsReadOnly();
                    }

                    return _mixers
                        .Where(tm => tm != null)
                        .ToList()
                        .AsReadOnly();
                }
            }
            catch (Exception ex)
            {
                getAllError = ex;
                return new List<TrackedMixer>().AsReadOnly();
            }
            finally
            {
                if (getAllError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.GetAllAsync CRASH PREVENTION: Error: {0}\nStackTrace: {1}",
                        getAllError.Message, getAllError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Add a new mixer to tracking
        /// ⚠️ THREAD SAFETY: Thread-safe addition with null checking
        /// </summary>
        /// <param name="mixer">Mixer to add</param>
        public static async Task AddAsync(TrackedMixer mixer)
        {
            Exception addError = null;
            try
            {
                if (mixer == null)
                {
                    Main.logger.Warn(1, "TrackedMixers.AddAsync: Mixer is null");
                    return;
                }

                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    _mixers.Add(mixer);
                    Main.logger.Msg(3, string.Format("TrackedMixers.AddAsync: Added mixer with ID {0}", mixer.MixerInstanceID));
                }
            }
            catch (Exception ex)
            {
                addError = ex;
            }
            finally
            {
                if (addError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.AddAsync CRASH PREVENTION: Error adding mixer {0}: {1}\nStackTrace: {2}",
                        mixer?.MixerInstanceID ?? -1, addError.Message, addError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Remove a mixer by its configuration instance
        /// ⚠️ THREAD SAFETY: Thread-safe removal with comprehensive error handling
        /// </summary>
        /// <param name="configInstance">Configuration instance to remove</param>
        /// <returns>True if mixer was found and removed</returns>
        public static async Task<bool> RemoveAsync(MixingStationConfiguration configInstance)
        {
            Exception removeError = null;
            try
            {
                if (configInstance == null)
                {
                    Main.logger.Warn(1, "TrackedMixers.RemoveAsync: ConfigInstance is null");
                    return false;
                }

                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    var mixerData = _mixers.FirstOrDefault(tm => tm?.ConfigInstance == configInstance);
                    if (mixerData != null)
                    {
                        _mixers.Remove(mixerData);
                        Main.logger.Msg(3, string.Format("TrackedMixers.RemoveAsync: Removed mixer with ID {0}", mixerData.MixerInstanceID));
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                removeError = ex;
                return false;
            }
            finally
            {
                if (removeError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.RemoveAsync CRASH PREVENTION: Error removing mixer: {0}\nStackTrace: {1}",
                        removeError.Message, removeError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Remove all mixers matching the predicate
        /// ⚠️ THREAD SAFETY: Thread-safe bulk removal with individual error handling
        /// </summary>
        /// <param name="predicate">Predicate to match mixers for removal</param>
        public static async Task RemoveAllAsync(Func<TrackedMixer, bool> predicate)
        {
            Exception removeAllError = null;
            try
            {
                if (predicate == null)
                {
                    Main.logger.Warn(1, "TrackedMixers.RemoveAllAsync: Predicate is null");
                    return;
                }

                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    if (_mixers == null)
                    {
                        Main.logger.Warn(1, "TrackedMixers.RemoveAllAsync: _mixers is null");
                        return;
                    }

                    var toRemove = new List<TrackedMixer>();

                    // Find mixers to remove
                    foreach (var tm in _mixers)
                    {
                        try
                        {
                            if (tm != null && predicate(tm))
                            {
                                toRemove.Add(tm);
                            }
                        }
                        catch (Exception predEx)
                        {
                            Main.logger.Err(string.Format("TrackedMixers.RemoveAllAsync: Error in predicate for mixer {0}: {1}",
                                tm?.MixerInstanceID ?? -1, predEx.Message));
                        }
                    }

                    // Remove identified mixers
                    foreach (var tm in toRemove)
                    {
                        try
                        {
                            _mixers.Remove(tm);
                        }
                        catch (Exception removeEx)
                        {
                            Main.logger.Err(string.Format("TrackedMixers.RemoveAllAsync: Error removing mixer {0}: {1}",
                                tm?.MixerInstanceID ?? -1, removeEx.Message));
                        }
                    }

                    if (toRemove.Count > 0)
                    {
                        Main.logger.Msg(2, string.Format("TrackedMixers.RemoveAllAsync: Removed {0} mixers", toRemove.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                removeAllError = ex;
            }
            finally
            {
                if (removeAllError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.RemoveAllAsync CRASH PREVENTION: Critical error: {0}\nStackTrace: {1}",
                        removeAllError.Message, removeAllError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Clear all tracked mixers
        /// ⚠️ THREAD SAFETY: Thread-safe clearing operation
        /// </summary>
        public static async Task ClearAsync()
        {
            Exception clearError = null;
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    int count = _mixers.Count;
                    _mixers.Clear();
                    Main.logger.Msg(2, string.Format("TrackedMixers.ClearAsync: Cleared {0} mixers", count));
                }
            }
            catch (Exception ex)
            {
                clearError = ex;
            }
            finally
            {
                if (clearError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.ClearAsync CRASH PREVENTION: Error: {0}\nStackTrace: {1}",
                        clearError.Message, clearError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Get a list copy of all tracked mixers
        /// ⚠️ THREAD SAFETY: Returns a snapshot copy, safe for iteration
        /// </summary>
        /// <returns>List copy of tracked mixers</returns>
        public static async Task<List<TrackedMixer>> ToListAsync()
        {
            Exception toListError = null;
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    return _mixers
                        .Where(tm => tm != null)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                toListError = ex;
                return new List<TrackedMixer>();
            }
            finally
            {
                if (toListError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.ToListAsync CRASH PREVENTION: Error: {0}\nStackTrace: {1}",
                        toListError.Message, toListError.StackTrace));
                }
            }
        }

        /// <summary>
        /// LINQ-style Select operation
        /// ⚠️ THREAD SAFETY: Thread-safe projection with error handling
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="selector">Selection function</param>
        /// <returns>List of projected results</returns>
        public static async Task<List<TResult>> SelectAsync<TResult>(Func<TrackedMixer, TResult> selector)
        {
            Exception selectError = null;
            try
            {
                if (selector == null)
                {
                    return new List<TResult>();
                }

                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    return _mixers
                        .Where(tm => tm != null)
                        .Select(selector)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                selectError = ex;
                return new List<TResult>();
            }
            finally
            {
                if (selectError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.SelectAsync CRASH PREVENTION: Error: {0}\nStackTrace: {1}",
                        selectError.Message, selectError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Count mixers matching predicate
        /// ⚠️ THREAD SAFETY: Thread-safe counting with error handling
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <returns>Count of matching mixers</returns>
        public static async Task<int> CountAsync(Func<TrackedMixer, bool> predicate)
        {
            Exception countError = null;
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    return _mixers.Count(tm => tm != null && predicate(tm));
                }
            }
            catch (Exception ex)
            {
                countError = ex;
                return 0;
            }
            finally
            {
                if (countError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.CountAsync CRASH PREVENTION: Error: {0}\nStackTrace: {1}",
                        countError.Message, countError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Find first mixer matching predicate
        /// ⚠️ THREAD SAFETY: Thread-safe search with error handling
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <returns>First matching mixer or null</returns>
        public static async Task<TrackedMixer> FirstOrDefaultAsync(Func<TrackedMixer, bool> predicate)
        {
            Exception firstError = null;
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    return _mixers.FirstOrDefault(tm => tm != null && predicate(tm));
                }
            }
            catch (Exception ex)
            {
                firstError = ex;
                return null;
            }
            finally
            {
                if (firstError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.FirstOrDefaultAsync CRASH PREVENTION: Error: {0}\nStackTrace: {1}",
                        firstError.Message, firstError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Check if any mixer matches predicate
        /// ⚠️ THREAD SAFETY: Thread-safe existence check with error handling
        /// </summary>
        /// <param name="predicate">Predicate to match</param>
        /// <returns>True if any mixer matches</returns>
        public static async Task<bool> AnyAsync(Func<TrackedMixer, bool> predicate)
        {
            Exception anyError = null;
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    return _mixers.Any(tm => tm != null && predicate(tm));
                }
            }
            catch (Exception ex)
            {
                anyError = ex;
                return false;
            }
            finally
            {
                if (anyError != null)
                {
                    Main.logger.Err(string.Format("TrackedMixers.AnyAsync CRASH PREVENTION: Error: {0}\nStackTrace: {1}",
                        anyError.Message, anyError.StackTrace));
                }
            }
        }

        // Synchronous versions for backward compatibility with existing Main.cs code

        /// <summary>
        /// Synchronous count operation for compatibility
        /// ⚠️ MAIN THREAD WARNING: Can block calling thread - prefer async version
        /// </summary>
        public static int Count(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                var task = CountAsync(predicate);
                task.Wait();
                return task.Result;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.Count CRASH PREVENTION: Error: {0}", ex.Message));
                return 0;
            }
        }

        /// <summary>
        /// Synchronous existence check for compatibility
        /// ⚠️ MAIN THREAD WARNING: Can block calling thread - prefer async version
        /// </summary>
        public static bool Any(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                var task = AnyAsync(predicate);
                task.Wait();
                return task.Result;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.Any CRASH PREVENTION: Error: {0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Synchronous add operation for compatibility
        /// ⚠️ MAIN THREAD WARNING: Can block calling thread - prefer async version
        /// </summary>
        public static void Add(TrackedMixer mixer)
        {
            try
            {
                var task = AddAsync(mixer);
                task.Wait();
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.Add CRASH PREVENTION: Error: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Synchronous bulk removal for compatibility
        /// ⚠️ MAIN THREAD WARNING: Can block calling thread - prefer async version
        /// </summary>
        public static void RemoveAll(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                var task = RemoveAllAsync(predicate);
                task.Wait();
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.RemoveAll CRASH PREVENTION: Error: {0}", ex.Message));
            }
        }
    }

    /// <summary>
    /// Async locking mechanism for .NET 4.8.1 compatibility
    /// 
    /// ⚠️ THREAD SAFETY: This class provides async-compatible locking using SemaphoreSlim.
    /// Safe to use across different thread contexts including Unity background threads.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses SemaphoreSlim which is available in .NET 4.8.1
    /// - Compatible async/await patterns with ConfigureAwait(false)
    /// - Proper IDisposable implementation for resource cleanup
    /// </summary>
    public class AsyncLocker
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Acquire an async lock
        /// ⚠️ THREAD SAFETY: Thread-safe lock acquisition with cancellation support
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Disposable lock releaser</returns>
        public async Task<IDisposable> LockAsync(CancellationToken ct = default(CancellationToken))
        {
            await _semaphore.WaitAsync(ct).ConfigureAwait(false);
            return new Releaser(_semaphore);
        }

        /// <summary>
        /// Lock releaser for proper resource cleanup
        /// ⚠️ THREAD SAFETY: Thread-safe release mechanism
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