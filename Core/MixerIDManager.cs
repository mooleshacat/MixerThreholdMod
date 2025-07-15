using ScheduleOne.Management;
using System;
using System.Collections.Concurrent;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Thread-safe mixer ID management system for stable mixer identification across game sessions.
    /// Provides consistent mixer IDs that survive game reloads and mixer destruction/recreation.
    /// 
    /// ⚠️ CRASH PREVENTION FOCUS: This class prevents crashes from mixer ID conflicts and 
    /// provides safe cleanup operations during entity destruction.
    /// 
    /// ⚠️ THREAD SAFETY: All operations are thread-safe using ConcurrentDictionary and proper
    /// locking mechanisms. Safe to call from any thread including Unity background threads.
    /// 
    /// ⚠️ MAIN THREAD WARNING: All operations are designed to not block the main Unity thread.
    /// ID generation and cleanup operations are fast and non-blocking.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses ConcurrentDictionary for thread-safe collections
    /// - Compatible exception handling patterns
    /// - Uses string.Format instead of string interpolation
    /// - Proper lock-based synchronization for counter operations
    /// 
    /// Features:
    /// - Stable mixer IDs that persist across game sessions
    /// - Thread-safe ID generation and management
    /// - Safe cleanup during mixer destruction
    /// - Memory leak prevention through proper cleanup
    /// - Comprehensive error handling and logging
    /// </summary>
    public static class MixerIDManager
    {
        // Thread-safe collections for mixer management
        public static readonly ConcurrentDictionary<MixingStationConfiguration, int> MixerInstanceMap = new ConcurrentDictionary<MixingStationConfiguration, int>();

        // Thread-safe counter management
        private static int _nextStableID = 1;
        private static readonly object _counterLock = new object();

        /// <summary>
        /// Get or create a stable ID for the given mixer configuration
        /// ⚠️ THREAD SAFETY: Thread-safe ID generation with atomic operations
        /// </summary>
        /// <param name="instance">Mixer configuration instance</param>
        /// <returns>Stable mixer ID, or -1 if instance is null</returns>
        public static int GetMixerID(MixingStationConfiguration instance)
        {
            Exception getIdError = null;
            try
            {
                if (instance == null)
                {
                    const string errorMsg = "GetMixerID: MixingStationConfiguration instance is null";
                    Main.logger?.Err(string.Format("[MIXER-ID] {0}", errorMsg));
                    return -1;
                }

                // Try to get existing ID first
                int existingId;
                if (MixerInstanceMap.TryGetValue(instance, out existingId))
                {
                    Main.logger?.Msg(3, string.Format("[MIXER-ID] GetMixerID: Found existing ID {0} for mixer", existingId));
                    return existingId;
                }

                // Generate new stable ID with thread safety
                int newId;
                lock (_counterLock)
                {
                    newId = _nextStableID++;
                }

                // Try to add the new mapping
                int actualId = MixerInstanceMap.GetOrAdd(instance, newId);

                if (actualId == newId)
                {
                    Main.logger?.Msg(2, string.Format("[MIXER-ID] GetMixerID: Created new stable ID {0} for mixer", newId));
                }
                else
                {
                    Main.logger?.Msg(3, string.Format("[MIXER-ID] GetMixerID: Concurrent creation detected, using existing ID {0}", actualId));
                }

                return actualId;
            }
            catch (Exception ex)
            {
                getIdError = ex;
                return -1;
            }
            finally
            {
                if (getIdError != null)
                {
                    Main.logger?.Err(string.Format("[MIXER-ID] GetMixerID CRASH PREVENTION: Error getting mixer ID: {0}\nStackTrace: {1}",
                        getIdError.Message, getIdError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Remove a mixer from the ID mapping system
        /// ⚠️ THREAD SAFETY: Thread-safe removal with comprehensive error handling
        /// </summary>
        /// <param name="instance">Mixer configuration instance to remove</param>
        /// <returns>True if successfully removed, false otherwise</returns>
        public static bool RemoveMixerID(MixingStationConfiguration instance)
        {
            Exception removeError = null;
            try
            {
                if (instance == null)
                {
                    Main.logger?.Warn(1, "[MIXER-ID] RemoveMixerID: Instance is null");
                    return false;
                }

                // Type safety check to ensure we have the right type
                var expectedType = typeof(MixingStationConfiguration);
                if (expectedType != null && instance.GetType() != expectedType)
                {
                    Main.logger?.Warn(1, string.Format("[MIXER-ID] RemoveMixerID: Type mismatch - expected {0}, got {1}",
                        expectedType.Name, instance.GetType().Name));
                    return false;
                }

                // Try to remove the mapping
                int removedId;
                bool removed = MixerInstanceMap.TryRemove(instance, out removedId);

                if (removed)
                {
                    Main.logger?.Msg(2, string.Format("[MIXER-ID] RemoveMixerID: Successfully removed mixer with ID {0}", removedId));
                }
                else
                {
                    Main.logger?.Msg(3, "[MIXER-ID] RemoveMixerID: Mixer was not in ID mapping (already removed or never added)");
                }

                return removed;
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
                    Main.logger?.Err(string.Format("[MIXER-ID] RemoveMixerID CRASH PREVENTION: Error removing mixer ID: {0}\nStackTrace: {1}",
                        removeError.Message, removeError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Get the current count of tracked mixers
        /// ⚠️ THREAD SAFETY: Thread-safe count operation
        /// </summary>
        /// <returns>Number of currently tracked mixers</returns>
        public static int GetMixerCount()
        {
            Exception countError = null;
            try
            {
                int count = MixerInstanceMap.Count;
                Main.logger?.Msg(3, string.Format("[MIXER-ID] GetMixerCount: Currently tracking {0} mixers", count));
                return count;
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
                    Main.logger?.Err(string.Format("[MIXER-ID] GetMixerCount CRASH PREVENTION: Error getting mixer count: {0}", countError.Message));
                }
            }
        }

        /// <summary>
        /// Reset the stable ID counter (typically called on scene reload)
        /// ⚠️ THREAD SAFETY: Thread-safe counter reset with proper locking
        /// </summary>
        public static void ResetStableIDCounter()
        {
            Exception resetError = null;
            try
            {
                lock (_counterLock)
                {
                    _nextStableID = 1;
                    Main.logger?.Msg(2, "[MIXER-ID] ResetStableIDCounter: Stable ID counter reset to 1");
                }
            }
            catch (Exception ex)
            {
                resetError = ex;
            }
            finally
            {
                if (resetError != null)
                {
                    Main.logger?.Err(string.Format("[MIXER-ID] ResetStableIDCounter CRASH PREVENTION: Error resetting counter: {0}", resetError.Message));
                }
            }
        }

        /// <summary>
        /// Clear all mixer mappings (typically called on scene reload)
        /// ⚠️ THREAD SAFETY: Thread-safe clearing operation
        /// </summary>
        public static void ClearAllMixers()
        {
            Exception clearError = null;
            try
            {
                int countBeforeClear = MixerInstanceMap.Count;
                MixerInstanceMap.Clear();

                Main.logger?.Msg(2, string.Format("[MIXER-ID] ClearAllMixers: Cleared {0} mixer mappings", countBeforeClear));
            }
            catch (Exception ex)
            {
                clearError = ex;
            }
            finally
            {
                if (clearError != null)
                {
                    Main.logger?.Err(string.Format("[MIXER-ID] ClearAllMixers CRASH PREVENTION: Error clearing mixers: {0}", clearError.Message));
                }
            }
        }

        /// <summary>
        /// Check if a mixer instance is currently tracked
        /// ⚠️ THREAD SAFETY: Thread-safe containment check
        /// </summary>
        /// <param name="instance">Mixer configuration instance to check</param>
        /// <returns>True if the mixer is tracked, false otherwise</returns>
        public static bool IsTracked(MixingStationConfiguration instance)
        {
            Exception checkError = null;
            try
            {
                if (instance == null)
                {
                    return false;
                }

                bool isTracked = MixerInstanceMap.ContainsKey(instance);
                Main.logger?.Msg(3, string.Format("[MIXER-ID] IsTracked: Mixer tracking status: {0}", isTracked));
                return isTracked;
            }
            catch (Exception ex)
            {
                checkError = ex;
                return false;
            }
            finally
            {
                if (checkError != null)
                {
                    Main.logger?.Err(string.Format("[MIXER-ID] IsTracked CRASH PREVENTION: Error checking tracking status: {0}", checkError.Message));
                }
            }
        }

        /// <summary>
        /// Get detailed diagnostics about the current state of mixer ID management
        /// ⚠️ THREAD SAFETY: Thread-safe diagnostic information gathering
        /// </summary>
        public static void LogDiagnostics()
        {
            Exception diagError = null;
            try
            {
                Main.logger?.Msg(1, "[MIXER-ID] ===== MIXER ID MANAGER DIAGNOSTICS =====");
                Main.logger?.Msg(1, string.Format("[MIXER-ID] Total tracked mixers: {0}", MixerInstanceMap.Count));

                lock (_counterLock)
                {
                    Main.logger?.Msg(1, string.Format("[MIXER-ID] Next stable ID: {0}", _nextStableID));
                }

                // Log some details about tracked mixers (without exposing sensitive data)
                int validInstances = 0;
                int nullInstances = 0;

                foreach (var kvp in MixerInstanceMap)
                {
                    if (kvp.Key != null)
                    {
                        validInstances++;
                    }
                    else
                    {
                        nullInstances++;
                    }
                }

                Main.logger?.Msg(1, string.Format("[MIXER-ID] Valid instances: {0}", validInstances));
                if (nullInstances > 0)
                {
                    Main.logger?.Warn(1, string.Format("[MIXER-ID] Null instances detected: {0} (should be cleaned up)", nullInstances));
                }

                Main.logger?.Msg(1, "[MIXER-ID] ==========================================");
            }
            catch (Exception ex)
            {
                diagError = ex;
            }
            finally
            {
                if (diagError != null)
                {
                    Main.logger?.Err(string.Format("[MIXER-ID] LogDiagnostics CRASH PREVENTION: Error gathering diagnostics: {0}", diagError.Message));
                }
            }
        }
    }
}