// IL2CPP COMPATIBILITY: Remove direct type references that cause TypeLoadException in IL2CPP builds

// ✅ IL2CPP SAFE: Dynamic type resolution at runtime
// No direct type references - use strings and reflection

//using ScheduleOne.Management; // REMOVED: Use IL2CPPTypeResolver for safe type loading


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

        // Thread-safe dictionary for .NET 4.8.1 - IL2CPP COMPATIBLE: Use object instead of specific type
        public static readonly ConcurrentDictionary<object, int> MixerInstanceMap =
            new ConcurrentDictionary<object, int>();

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
        /// IL2CPP COMPATIBLE: Uses object type to avoid TypeLoadException
        /// </summary>
        public static int GetMixerID(object instance)
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
        /// IL2CPP COMPATIBLE: Uses object type to avoid TypeLoadException
        /// </summary>
        public static bool TryGetMixerID(object instance, out int id)
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
        /// Remove mixer ID mapping using object type for IL2CPP compatibility.
        /// ⚠️ THREAD SAFETY: This method is thread-safe using ConcurrentDictionary.
        /// ⚠️ IL2CPP COMPATIBLE: Uses object type to avoid TypeLoadException
        /// ⚠️ REFLECTION REFERENCE: Called via mixerIdManagerType.GetMethod("RemoveMixerID") in
        /// ⚠️ EntityConfiguration_Destroy_Patch - DO NOT DELETE
        /// </summary>
        public static bool RemoveMixerID(object instance)
        {
            Exception removeError = null;
            try
            {
                // Step 2.1: Null check first (defensive programming)
                if (instance == null)
                {
                    Main.logger?.Warn(1, "RemoveMixerID: Cannot remove null instance");
                    return false;
                }

                // Step 2.2: IL2CPP-SAFE dynamic type verification
                var expectedType = Core.IL2CPPTypeResolver.GetMixingStationConfigurationType();

                // Step 2.3: MONO compatibility check
                if (expectedType != null)
                {
                    // IL2CPP build - verify type matches
                    if (instance.GetType() != expectedType)
                    {
                        Main.logger?.Warn(2, string.Format("RemoveMixerID: Type mismatch. Expected: {0}, Got: {1}",
                            expectedType.Name, instance.GetType().Name));
                        return false;
                    }
                }
                else
                {
                    // MONO build or IL2CPP graceful degradation
                    Main.logger?.Msg(3, "RemoveMixerID: Type resolver unavailable, proceeding with object type");
                }

                // Step 2.4: Proceed with actual removal (thread-safe operation)
                int removedId;
                bool removed = MixerInstanceMap.TryRemove(instance, out removedId);

                // Step 2.5: Comprehensive logging for debugging
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
                removeError = ex;
                return false;
            }
            finally
            {
                // Step 2.6: Exception handling like a hawk (.NET 4.8.1 pattern)
                if (removeError != null)
                {
                    Main.logger?.Err(string.Format("RemoveMixerID: Exception during removal: {0}\n{1}",
                        removeError.Message, removeError.StackTrace));
                }
            }
        }

        /// <summary>
        /// Remove mixer ID mapping using object type for IL2CPP compatibility.
        /// ⚠️ THREAD SAFETY: This method is thread-safe using ConcurrentDictionary.
        /// ⚠️ IL2CPP COMPATIBLE: Uses object type to avoid TypeLoadException
        /// </summary>
        public static bool RemoveMixerID(object instance)
        {
            try
            {
                if (instance == null)
                {
                    Main.logger?.Warn(1, "RemoveMixerID: Cannot remove null instance");
                    return false;
                }

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