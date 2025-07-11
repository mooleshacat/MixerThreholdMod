using MixerThreholdMod_0_0_1;
using ScheduleOne.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.PlayerLoop;

namespace MixerThreholdMod_0_0_1
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
                return _mixers
                    .Where(tm => tm != null)
                    .ToList()
                    .AsReadOnly();
            }
        }

        // Add
        public static async Task AddAsync(TrackedMixer mixer)
        {
            if (mixer == null) return;
            using (await _locker.LockAsync().ConfigureAwait(false))
            {
                _mixers.Add(mixer);
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
                _mixers.RemoveAll(tm => tm != null && predicate(tm));
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

        // Count
        public static int Count(Func<TrackedMixer, bool> predicate)
        {
            return _mixers.Count(tm => tm != null && predicate(tm));
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
    }

    // Async locker class
    public class AsyncLocker
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task<IDisposable> LockAsync(CancellationToken ct = default)
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