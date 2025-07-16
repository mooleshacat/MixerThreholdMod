

﻿using MixerThreholdMod_0_0_1;
using ScheduleOne.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Threading;

namespace MixerThreholdMod_1_0_0.Core
{
    public class TrackedMixer
    {
        public MixingStationConfiguration ConfigInstance { get; set; }
        public int MixerInstanceID { get; set; }
        public bool ListenerAdded { get; set; } = false;
    }

    internal static class TrackedMixers
    {
        private static readonly List<TrackedMixer> _mixers = new List<TrackedMixer>();
        private static readonly AsyncLocker _locker = new AsyncLocker();

        // Read-only async snapshot
        public static async Task<IReadOnlyList<TrackedMixer>> GetAllAsync()
        {
            using (await _locker.LockAsync().ConfigureAwait(false))
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
                Main.logger.Err(string.Format("TrackedMixers.GetAllAsync: Error: {0}", ex.Message));
                return new List<TrackedMixer>().AsReadOnly();
            }
        }

        // Add
        public static async Task AddAsync(TrackedMixer mixer)
        {
            if (mixer == null) return;
            using (await _locker.LockAsync().ConfigureAwait(false))
            {
                if (mixer == null)
                {
                    Main.logger.Warn(1, "TrackedMixers.AddAsync: Mixer is null");
                    return;
                }

                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    _mixers.Add(mixer);
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.AddAsync: Error adding mixer {0}: {1}", mixer?.MixerInstanceID ?? -1, ex.Message));
            }
        }

        // Remove by ConfigInstance
        public static async Task RemoveAsync(MixingStationConfiguration configInstance)
        {
            if (configInstance == null) return;
            using (await _locker.LockAsync().ConfigureAwait(false))
            {
                var mixerData = _mixers.FirstOrDefault(tm => tm?.ConfigInstance == configInstance);
                if (mixerData != null)
                    _mixers.Remove(mixerData);
            }
        }

        // Remove by predicate
        public static async Task RemoveAllAsync(Func<TrackedMixer, bool> predicate)
        {
            if (predicate == null) return;
            using (await _locker.LockAsync().ConfigureAwait(false))
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
                            Main.logger.Err(string.Format("TrackedMixers.RemoveAllAsync: Error in predicate for mixer {0}: {1}", tm?.MixerInstanceID ?? -1, predEx.Message));
                        }
                    }

                    foreach (var tm in toRemove)
                    {
                        try
                        {
                            _mixers.Remove(tm);
                        }
                        catch (Exception removeEx)
                        {
                            Main.logger.Err(string.Format("TrackedMixers.RemoveAllAsync: Error removing mixer {0}: {1}", tm?.MixerInstanceID ?? -1, removeEx.Message));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.RemoveAllAsync: Critical error: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }

        // Clear all
        public static async Task ClearAsync()
        {
            using (await _locker.LockAsync().ConfigureAwait(false))
            {
                _mixers.Clear();
            }
        }

        public static async Task<List<TrackedMixer>> ToListAsync()
        {
            using (await _locker.LockAsync().ConfigureAwait(false))
            {
                return _mixers
                    .Where(tm => tm != null)
                    .ToList();
            }
        }

        // LINQ-style Select
        public static async Task<List<TResult>> SelectAsync<TResult>(Func<TrackedMixer, TResult> selector)
        {
            if (selector == null) return new List<TResult>();
            using (await _locker.LockAsync().ConfigureAwait(false))
            {
                return _mixers
                    .Where(tm => tm != null)
                    .Select(selector)
                    .ToList();
            }
        }

        // Count
        public static async Task<int> CountAsync(Func<TrackedMixer, bool> predicate)
        {
            using (await _locker.LockAsync().ConfigureAwait(false))
            {
                return _mixers.Count(tm => tm != null && predicate(tm));
            }
        }

        // Count - synchronous version for compatibility
        public static int Count(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                // For .NET 4.8.1 compatibility, provide synchronous access when needed
                // But prefer the async version in coroutines
                var task = CountAsync(predicate);
                task.Wait(); // Only use this in non-async contexts
                return task.Result;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.Count: Error: {0}", ex.Message));
                return 0;
            }
        }

        // FirstOrDefault
        public static async Task<TrackedMixer> FirstOrDefaultAsync(Func<TrackedMixer, bool> predicate)
        {
            using (await _locker.LockAsync().ConfigureAwait(false))
            {
                return _mixers.FirstOrDefault(tm => tm != null && predicate(tm));
            }
        }

        // Any
        public static async Task<bool> AnyAsync(Func<TrackedMixer, bool> predicate)
        {
            using (await _locker.LockAsync().ConfigureAwait(false))
            {
                return _mixers.Any(tm => tm != null && predicate(tm));
            }
        }

        // Any - synchronous version for compatibility with existing Main.cs code
        public static bool Any(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                var task = AnyAsync(predicate);
                task.Wait(); // Only use this in non-async contexts
                return task.Result;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.Any: Error: {0}", ex.Message));
                return false;
            }
        }

        // Add synchronous version for immediate use in Main.cs
        public static void Add(TrackedMixer mixer)
        {
            try
            {
                var task = AddAsync(mixer);
                task.Wait();
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.Add: Error: {0}", ex.Message));
            }
        }

        // Add synchronous version for immediate use in Main.cs
        public static void RemoveAll(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                var task = RemoveAllAsync(predicate);
                task.Wait();
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.RemoveAll: Error: {0}", ex.Message));
            }
        }
    }

    // Async locker class - keep this for .NET 4.8.1 compatibility
    public class AsyncLocker
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task<IDisposable> LockAsync(CancellationToken ct = default(CancellationToken)) // .NET 4.8.1 compatible default parameter
        {
            await _semaphore.WaitAsync(ct).ConfigureAwait(false);
            return new Releaser(_semaphore);
        }

        private class Releaser : IDisposable
        {
            private readonly SemaphoreSlim _semaphore;

            public Releaser(SemaphoreSlim semaphore)
            {
                _semaphore = semaphore;
            }

            public void Dispose()
            {
                _semaphore.Release();
            }
        }
    }
}