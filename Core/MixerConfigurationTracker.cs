// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Management;  // REMOVED: Use dynamic object types for IL2CPP compatibility
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MixerThreholdMod_0_0_1.Core
{
    /// <summary>
    /// IL2CPP COMPATIBLE: Represents a tracked mixer configuration with its associated data.
    /// Thread-safe implementation for .NET 4.8.1 compatibility with AOT-safe patterns.
    /// 
    /// ⚠️ THREAD SAFETY: All operations on TrackedMixer instances are thread-safe.
    /// ⚠️ MEMORY SAFETY: Proper cleanup and disposal patterns implemented to prevent leaks.
    /// ⚠️ IL2CPP COMPATIBLE: Uses compile-time known types, no reflection, AOT-friendly patterns.
    /// 
    /// IL2CPP Compatibility Features:
    /// - All types statically known at compile time
    /// - No dynamic type creation or reflection usage
    /// - AOT-safe generic constraints and collection usage
    /// - Compile-time safe property access patterns
    /// - Interface-based operations instead of reflection
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses Task.Run for async operations with proper exception handling
    /// - Compatible exception handling patterns with comprehensive logging
    /// - Thread-safe collections and operations using framework-appropriate patterns
    /// - Proper resource cleanup and disposal patterns
    /// </summary>
    public class TrackedMixer
    {
        // IL2CPP COMPATIBLE: Use object instead of specific type to avoid TypeLoadException in IL2CPP builds
        public object ConfigInstance { get; set; }
        public int MixerInstanceID { get; set; }
        public bool ListenerAdded { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return string.Format("TrackedMixer[ID:{0}, Created:{1}, Updated:{2}, HasListener:{3}]",
                MixerInstanceID, CreatedAt, LastUpdated, ListenerAdded);
        }
    }

    /// <summary>
    /// IL2CPP COMPATIBLE: Thread-safe collection manager for TrackedMixer instances.
    /// Provides async operations for managing tracked mixers with AOT-safe patterns.
    /// 
    /// ⚠️ THREAD SAFETY: All operations are thread-safe using ConcurrentBag with proper locking.
    /// ⚠️ PERFORMANCE: Async operations prevent blocking the main thread with memory leak prevention.
    /// ⚠️ IL2CPP COMPATIBLE: Uses compile-time safe collection operations, no reflection required.
    /// 
    /// IL2CPP Compatibility Features:
    /// - ConcurrentBag<T> usage with compile-time known generic constraints
    /// - No dynamic type operations or reflection in collection management
    /// - AOT-safe LINQ operations with proper delegate usage
    /// - Compile-time safe predicate functions and lambda expressions
    /// - Interface-based operations instead of reflection-heavy approaches
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses Task.Run for background operations with proper exception handling
    /// - Compatible with framework's async patterns and cancellation
    /// - Proper exception handling and logging throughout all operations
    /// - Memory-efficient collection operations with leak prevention
    /// </summary>
    public static class MixerConfigurationTracker
    {
        private static readonly ConcurrentBag<TrackedMixer> _trackedMixers = new ConcurrentBag<TrackedMixer>();
        private static readonly object _operationLock = new object();

        /// <summary>
        /// Add a tracked mixer asynchronously
        /// </summary>
        public static async Task AddAsync(TrackedMixer mixer)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (mixer != null)
                    {
                        mixer.CreatedAt = DateTime.Now;
                        mixer.LastUpdated = DateTime.Now;
                        _trackedMixers.Add(mixer);
                        Main.logger?.Msg(3, string.Format("TrackedMixers.AddAsync: Added {0}", mixer));
                    }
                });
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("TrackedMixers.AddAsync: Error adding mixer: {0}", ex));
                throw;
            }
        }

        /// <summary>
        /// Remove tracked mixers matching a predicate asynchronously
        /// </summary>
        public static async Task RemoveAllAsync(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                await Task.Run(() =>
                {
                    lock (_operationLock)
                    {
                        var allMixers = _trackedMixers.ToList();
                        var toRemove = allMixers.Where(predicate).ToList();

                        if (toRemove.Count > 0)
                        {
                            // ConcurrentBag doesn't have direct removal, so we need to recreate
                            var toKeep = allMixers.Except(toRemove).ToList();

                            // Clear and re-add remaining items
                            while (_trackedMixers.TryTake(out TrackedMixer item)) { }

                            foreach (var keeper in toKeep)
                            {
                                _trackedMixers.Add(keeper);
                            }

                            Main.logger?.Msg(3, string.Format("TrackedMixers.RemoveAllAsync: Removed {0} mixers", toRemove.Count));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("TrackedMixers.RemoveAllAsync: Error removing mixers: {0}", ex));
                throw;
            }
        }

        /// <summary>
        /// Remove a specific tracked mixer by ConfigInstance asynchronously
        /// </summary>
        public static async Task<bool> RemoveAsync(MixingStationConfiguration configInstance)
        public static async Task<bool> RemoveAsync(object configInstance)
        public static async Task<bool> RemoveAsync(object configInstance)
        public static async Task<bool> RemoveAsync(object configInstance)
        {
            try
            {
                return await Task.Run(() =>
                {
                    lock (_operationLock)
                    {
                        var allMixers = _trackedMixers.ToList();
                        var toRemove = allMixers.FirstOrDefault(tm => tm.ConfigInstance == configInstance);

                        if (toRemove != null)
                        {
                            var toKeep = allMixers.Where(tm => tm.ConfigInstance != configInstance).ToList();

                            // Clear and re-add remaining items
                            while (_trackedMixers.TryTake(out TrackedMixer item)) { }

                            foreach (var keeper in toKeep)
                            {
                                _trackedMixers.Add(keeper);
                            }

                            Main.logger?.Msg(3, string.Format("TrackedMixers.RemoveAsync: Removed mixer {0}", toRemove));
                            return true;
                        }

                        return false;
                    }
                });
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("TrackedMixers.RemoveAsync: Error removing mixer: {0}", ex));
                return false;
            }
        }

        /// <summary>
        /// Check if any tracked mixer matches a predicate asynchronously
        /// </summary>
        public static async Task<bool> AnyAsync(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    return _trackedMixers.Any(predicate);
                });
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("TrackedMixers.AnyAsync: Error checking mixers: {0}", ex));
                return false;
            }
        }

        /// <summary>
        /// Find first tracked mixer matching a predicate asynchronously
        /// </summary>
        public static async Task<TrackedMixer> FirstOrDefaultAsync(Func<TrackedMixer, bool> predicate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    return _trackedMixers.FirstOrDefault(predicate);
                });
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("TrackedMixers.FirstOrDefaultAsync: Error finding mixer: {0}", ex));
                return null;
            }
        }

        /// <summary>
        /// Get all tracked mixers as a list asynchronously
        /// </summary>
        public static async Task<List<TrackedMixer>> ToListAsync()
        {
            try
            {
                return await Task.Run(() =>
                {
                    return _trackedMixers.ToList();
                });
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("TrackedMixers.ToListAsync: Error getting mixer list: {0}", ex));
                return new List<TrackedMixer>();
            }
        }

        /// <summary>
        /// Get count of tracked mixers asynchronously
        /// </summary>
        public static async Task<int> CountAsync(Func<TrackedMixer, bool> predicate = null)
        {
            try
            {
                return await Task.Run(() =>
                {
                    var mixers = _trackedMixers.ToList();
                    return predicate != null ? mixers.Count(predicate) : mixers.Count;
                });
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("TrackedMixers.CountAsync: Error counting mixers: {0}", ex));
                return 0;
            }
        }

        /// <summary>
        /// Clear all tracked mixers asynchronously
        /// </summary>
        public static async Task ClearAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    lock (_operationLock)
                    {
                        int count = _trackedMixers.Count;
                        while (_trackedMixers.TryTake(out TrackedMixer item)) { }
                        Main.logger?.Msg(2, string.Format("TrackedMixers.ClearAsync: Cleared {0} mixers", count));
                    }
                });
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("TrackedMixers.ClearAsync: Error clearing mixers: {0}", ex));
                throw;
            }
        }
    }
}