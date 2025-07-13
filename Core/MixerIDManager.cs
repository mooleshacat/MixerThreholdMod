// IL2CPP COMPATIBLE: Remove direct type references that cause TypeLoadException in IL2CPP builds
// using ScheduleOne.Management;  // REMOVED: Use dynamic object types for IL2CPP compatibility
using System;
using System.Collections.Concurrent;

<<<<<<< HEAD
namespace MixerThreholdMod_0_0_1.Core
=======
namespace MixerThreholdMod_1_0_0.Core
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        // Thread-safe dictionary for .NET 4.8.1
        public static readonly ConcurrentDictionary<MixingStationConfiguration, int> MixerInstanceMap =
            new ConcurrentDictionary<MixingStationConfiguration, int>();
=======
        // Thread-safe dictionary for .NET 4.8.1 - IL2CPP COMPATIBLE: Use object instead of specific type
        public static readonly ConcurrentDictionary<object, int> MixerInstanceMap =
            new ConcurrentDictionary<object, int>();
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
        // Thread-safe dictionary for .NET 4.8.1 - IL2CPP COMPATIBLE: Use object instead of specific type
        public static readonly ConcurrentDictionary<object, int> MixerInstanceMap =
            new ConcurrentDictionary<object, int>();
>>>>>>> aa94715 (performance optimizations, cache manager)
=======
        // Thread-safe dictionary for .NET 4.8.1 - IL2CPP COMPATIBLE: Use object instead of specific type
        public static readonly ConcurrentDictionary<object, int> MixerInstanceMap =
            new ConcurrentDictionary<object, int>();
>>>>>>> 2bf7ffe (performance optimizations, cache manager)

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
<<<<<<< HEAD
                Main.logger?.Err(string.Format("ResetStableIDCounter: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
=======
                Main.logger?.Err($"ResetStableIDCounter: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
                    throw new ArgumentNullException("instance", errorMsg);
                }

                // Try to get existing ID first
                int existingId;
                if (MixerInstanceMap.TryGetValue(instance, out existingId))
                {
                    Main.logger?.Warn(2, string.Format("Instance already has ID {0}: {1}", existingId, instance));
=======
<<<<<<<< HEAD:Helpers/MixerIDManager.cs
                    throw new ArgumentNullException(nameof(instance), errorMsg);
========
                    throw new ArgumentNullException("instance", errorMsg);
>>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Core/MixerIDManager.cs
                }

                // Try to get existing ID first
                if (MixerInstanceMap.TryGetValue(instance, out int existingId))
                {
                    Main.logger?.Warn(2, $"Instance already has ID {existingId}: {instance}");
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
                    Main.logger?.Msg(3, string.Format("Assigned new ID {0} to instance: {1}", newId, instance));
                }
                else
                {
                    Main.logger?.Warn(2, string.Format("Another thread assigned ID {0} to instance: {1}", actualId, instance));
=======
                    Main.logger?.Msg(3, $"Assigned new ID {newId} to instance: {instance}");
                }
                else
                {
                    Main.logger?.Warn(2, $"Another thread assigned ID {actualId} to instance: {instance}");
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }

                return actualId;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                Main.logger?.Err(string.Format("GetMixerID: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
=======
                Main.logger?.Err($"GetMixerID: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
                Main.logger?.Err(string.Format("TryGetMixerID: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
=======
                Main.logger?.Err($"TryGetMixerID: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                return false;
            }
        }

        /// <summary>
<<<<<<< HEAD
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
=======
        /// Remove mixer ID mapping. Used for cleanup when mixers are destroyed.
        /// ⚠️ THREAD SAFETY: This method is thread-safe using ConcurrentDictionary.
        /// </summary>
        public static bool RemoveMixerID(MixingStationConfiguration instance)
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        {
            try
            {
                if (instance == null)
                {
                    Main.logger?.Warn(1, "RemoveMixerID: Cannot remove null instance");
                    return false;
                }

<<<<<<< HEAD
                int removedId;
                bool removed = MixerInstanceMap.TryRemove(instance, out removedId);
                if (removed)
                {
                    Main.logger?.Msg(3, string.Format("Removed mixer ID {0} for instance: {1}", removedId, instance));
                }
                else
                {
                    Main.logger?.Warn(2, string.Format("Failed to remove mixer ID for instance: {0}", instance));
=======
                bool removed = MixerInstanceMap.TryRemove(instance, out int removedId);
                if (removed)
                {
                    Main.logger?.Msg(3, $"Removed mixer ID {removedId} for instance: {instance}");
                }
                else
                {
                    Main.logger?.Warn(2, $"Failed to remove mixer ID for instance: {instance}");
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }

                return removed;
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                Main.logger?.Err(string.Format("RemoveMixerID: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
=======
                Main.logger?.Err($"RemoveMixerID: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
                Main.logger?.Err(string.Format("GetMixerCount: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
=======
                Main.logger?.Err($"GetMixerCount: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                return 0;
            }
        }
    }
}