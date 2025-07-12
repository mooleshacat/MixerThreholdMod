using ScheduleOne.Management;
using System;
using System.Collections.Concurrent;

namespace MixerThreholdMod_0_0_1.Core
{
    /// <summary>
    /// Thread-safe mixer ID management system for .NET 4.8.1 compatibility.
    /// Provides stable, unique IDs for mixer configurations across save/load cycles.
    /// 
    /// ⚠️ THREAD SAFETY: This class is fully thread-safe using ConcurrentDictionary.
    /// All operations are atomic and safe for multi-threaded access.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses ConcurrentDictionary for thread-safe operations
    /// - Compatible exception handling patterns
    /// - Proper null checks and defensive programming
    /// 
    /// Purpose:
    /// - Assigns stable IDs to mixer configurations
    /// - Persists across game sessions for save/load functionality
    /// - Prevents ID conflicts and provides collision detection
    /// </summary>
    public static class MixerIDManager
    {
        private static int _nextStableID = 1;
        private static readonly object _counterLock = new object();
        
        // Thread-safe dictionary for .NET 4.8.1
        public static readonly ConcurrentDictionary<MixingStationConfiguration, int> MixerInstanceMap = 
            new ConcurrentDictionary<MixingStationConfiguration, int>();

        /// <summary>
        /// Reset the stable ID counter to 1. Used when starting new game sessions.
        /// ⚠️ THREAD SAFETY: This method is thread-safe using lock synchronization.
        /// </summary>
        public static void ResetStableIDCounter()
        {
            try
            {
                lock (_counterLock)
                {
                    _nextStableID = 1;
                }
                Main.logger?.Msg(3, "MixerIDManager: Reset stable ID counter to 1");
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("ResetStableIDCounter: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Get or assign a unique mixer ID for the given configuration instance.
        /// ⚠️ THREAD SAFETY: This method is thread-safe and handles concurrent access.
        /// </summary>
        public static int GetMixerID(MixingStationConfiguration instance)
        {
            try
            {
                if (instance == null)
                {
                    const string errorMsg = "Cannot assign ID to null MixingStationConfiguration";
                    Main.logger?.Err(errorMsg);
                    throw new ArgumentNullException("instance", errorMsg); // .NET 4.8.1 compatible
                }

                // Try to get existing ID first - .NET 4.8.1 compatible
                int existingId;
                if (MixerInstanceMap.TryGetValue(instance, out existingId))
                {
                    Main.logger?.Warn(2, string.Format("Instance already has ID {0}: {1}", existingId, instance));
                    return existingId;
                }

                // Generate new ID atomically
                int newId;
                lock (_counterLock)
                {
                    newId = _nextStableID++;
                }

                // Attempt to add to map - returns actual ID (new or existing)
                int actualId = MixerInstanceMap.GetOrAdd(instance, newId);

                if (actualId == newId)
                {
                    Main.logger?.Msg(3, string.Format("Assigned new ID {0} to instance: {1}", newId, instance));
                }
                else
                {
                    Main.logger?.Warn(2, string.Format("Another thread assigned ID {0} to instance: {1}", actualId, instance));
                }

                return actualId;
            }
            catch (ArgumentNullException)
            {
                throw; // Re-throw argument exceptions
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("GetMixerID: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Try to get mixer ID without throwing exceptions.
        /// ⚠️ THREAD SAFETY: This method is thread-safe and won't throw exceptions.
        /// </summary>
        public static bool TryGetMixerID(MixingStationConfiguration instance, out int id)
        {
            id = -1;
            try
            {
                if (instance == null) return false;
                return MixerInstanceMap.TryGetValue(instance, out id);
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("TryGetMixerID: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        /// <summary>
        /// Remove mixer ID mapping. Used for cleanup when mixers are destroyed.
        /// ⚠️ THREAD SAFETY: This method is thread-safe using ConcurrentDictionary.
        /// </summary>
        public static bool RemoveMixerID(MixingStationConfiguration instance)
        {
            try
            {
                if (instance == null)
                {
                    Main.logger?.Warn(1, "RemoveMixerID: Cannot remove null instance");
                    return false;
                }

                // .NET 4.8.1 compatible - use explicit out parameter
                int removedId;
                bool removed = MixerInstanceMap.TryRemove(instance, out removedId);
                if (removed)
                {
                    Main.logger?.Msg(3, string.Format("Removed mixer ID {0} for instance: {1}", removedId, instance));
                }
                else
                {
                    Main.logger?.Warn(2, string.Format("Failed to remove mixer ID for instance: {0}", instance));
                }

                return removed;
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("RemoveMixerID: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        /// <summary>
        /// Get current count of tracked mixers.
        /// ⚠️ THREAD SAFETY: This method is thread-safe using ConcurrentDictionary.Count.
        /// </summary>
        public static int GetMixerCount()
        {
            try
            {
                return MixerInstanceMap.Count;
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("GetMixerCount: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                return 0;
            }
        }
    }
}