using ScheduleOne.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0.Threading;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Simplified mixer tracking system with thread-safe operations.
    /// Tracks mixer configurations and their associated unique IDs.
    /// 
    /// ⚠️ THREAD SAFETY: All operations are thread-safe using AsyncLocker and ThreadSafeList.
    /// Methods provide both async and sync versions for compatibility.
    /// 
    /// ⚠️ MAIN THREAD WARNING: Synchronous methods may block briefly - prefer async versions
    /// in Unity coroutines to prevent frame drops.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Compatible async/await patterns
    /// - Traditional exception handling
    /// - Explicit type declarations
    /// </summary>
    public class TrackedMixer
    {
        public MixingStationConfiguration ConfigInstance { get; set; }
        public int MixerInstanceID { get; set; }
        public bool ListenerAdded { get; set; }

        public TrackedMixer()
        {
            ListenerAdded = false;
        }
    }

    /// <summary>
    /// Thread-safe collection manager for tracked mixers
    /// </summary>
    public static class TrackedMixers
    {
        private static readonly ThreadSafeList<TrackedMixer> _mixers = new ThreadSafeList<TrackedMixer>();
        private static readonly AsyncLocker _locker = new AsyncLocker();

        /// <summary>
        /// Get all tracked mixers (async, thread-safe)
        /// </summary>
        public static async Task<IReadOnlyList<TrackedMixer>> GetAllAsync()
        {
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    return _mixers.ToList().AsReadOnly();
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.GetAllAsync: Error: {0}", ex.Message));
                return new List<TrackedMixer>().AsReadOnly();
            }
        }

        /// <summary>
        /// Add mixer to tracking (async, thread-safe)
        /// </summary>
        public static async Task AddAsync(TrackedMixer mixer)
        {
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
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.AddAsync: Error adding mixer {0}: {1}", mixer?.MixerInstanceID ?? -1, ex.Message));
            }
        }

        /// <summary>
        /// Remove mixers matching predicate (async, thread-safe)
        /// </summary>
        public static async Task RemoveAllAsync(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                if (predicate == null)
                {
                    Main.logger.Warn(1, "TrackedMixers.RemoveAllAsync: Predicate is null");
                    return;
                }

                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    var toRemove = new List<TrackedMixer>();

                    foreach (var tm in _mixers)
                    {
                        try
                        {
                            if (predicate(tm))
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
                            _mixers.RemoveAll(m => m == tm);
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

        /// <summary>
        /// Find first mixer matching predicate (async, thread-safe)
        /// </summary>
        public static async Task<TrackedMixer> FirstOrDefaultAsync(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    return _mixers.ToList().FirstOrDefault(predicate);
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.FirstOrDefaultAsync: Error: {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Count mixers matching predicate (async, thread-safe)
        /// </summary>
        public static async Task<int> CountAsync(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    return _mixers.ToList().Count(predicate);
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.CountAsync: Error: {0}", ex.Message));
                return 0;
            }
        }

        /// <summary>
        /// Check if any mixers match predicate (async, thread-safe)
        /// </summary>
        public static async Task<bool> AnyAsync(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                using (await _locker.LockAsync().ConfigureAwait(false))
                {
                    return _mixers.ToList().Any(predicate);
                }
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.AnyAsync: Error: {0}", ex.Message));
                return false;
            }
        }

        // Synchronous versions for compatibility with existing Main.cs code
        // ⚠️ MAIN THREAD WARNING: These may cause brief blocking

        /// <summary>
        /// Add mixer (synchronous version)
        /// ⚠️ MAIN THREAD WARNING: May cause brief blocking
        /// </summary>
        public static void Add(TrackedMixer mixer)
        {
            try
            {
                var task = AddAsync(mixer);
                task.Wait(5000); // 5 second timeout
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.Add: Error: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Remove all matching mixers (synchronous version)
        /// ⚠️ MAIN THREAD WARNING: May cause brief blocking
        /// </summary>
        public static void RemoveAll(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                var task = RemoveAllAsync(predicate);
                task.Wait(5000); // 5 second timeout
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.RemoveAll: Error: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Check if any mixers match (synchronous version)
        /// ⚠️ MAIN THREAD WARNING: May cause brief blocking
        /// </summary>
        public static bool Any(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                var task = AnyAsync(predicate);
                task.Wait(5000); // 5 second timeout
                return task.Result;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.Any: Error: {0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Count mixers (synchronous version)
        /// ⚠️ MAIN THREAD WARNING: May cause brief blocking
        /// </summary>
        public static int Count(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                var task = CountAsync(predicate);
                task.Wait(5000); // 5 second timeout
                return task.Result;
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("TrackedMixers.Count: Error: {0}", ex.Message));
                return 0;
            }
        }
    }
}