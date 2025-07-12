using MelonLoader;
using ScheduleOne.Management;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MixerThreholdMod_0_0_1
{
    public static class MixerIDManager
    {
        private static int _nextStableID = 0;
        private static readonly object _resetLock = new object();

        public static readonly ConcurrentDictionary<MixingStationConfiguration, int> MixerInstanceMap =
            new ConcurrentDictionary<MixingStationConfiguration, int>();

        public static void ResetStableIDCounter()
        {
            try
            {
                lock (_resetLock)
                {
                    _nextStableID = 0;
                    MixerInstanceMap.Clear();
                    MelonLogger.Msg("Stable ID counter reset and instance map cleared.");
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"ResetStableIDCounter: Caught exception: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        public static int GetMixerID(MixingStationConfiguration instance)
        {
            try
            {
                if (instance == null)
                {
                    const string errorMsg = "Cannot assign ID to null MixingStationConfiguration";
                    Main.logger?.Err(errorMsg);
                    throw new ArgumentNullException(nameof(instance), errorMsg);
                }

                // Try to get existing ID first
                if (MixerInstanceMap.TryGetValue(instance, out int existingId))
                {
                    Main.logger?.Warn(2, $"Instance already has ID {existingId}: {instance}");
                    return existingId;
                }

                // Generate new ID thread-safely
                int newId = Interlocked.Increment(ref _nextStableID);

                // Try to add to map - if another thread beat us, use their ID
                int actualId = MixerInstanceMap.GetOrAdd(instance, newId);

                if (actualId == newId)
                {
                    Main.logger?.Msg(3, $"Assigned new ID {newId} to instance: {instance}");
                }
                else
                {
                    Main.logger?.Warn(2, $"Another thread assigned ID {actualId} to instance: {instance}");
                }

                return actualId;
            }
            catch (ArgumentNullException)
            {
                throw; // Re-throw argument null exceptions
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"GetMixerID: Caught exception: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        public static bool TryGetMixerID(MixingStationConfiguration instance, out int id)
        {
            id = 0;
            try
            {
                if (instance == null)
                {
                    Main.logger?.Warn(2, "TryGetMixerID: instance is null");
                    return false;
                }

                return MixerInstanceMap.TryGetValue(instance, out id);
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"TryGetMixerID: Caught exception: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        public static bool RemoveMixerID(MixingStationConfiguration instance)
        {
            try
            {
                if (instance == null)
                {
                    Main.logger?.Warn(2, "RemoveMixerID: instance is null");
                    return false;
                }

                bool removed = MixerInstanceMap.TryRemove(instance, out int removedId);
                if (removed)
                {
                    Main.logger?.Msg(3, $"Removed mixer ID {removedId} for instance: {instance}");
                }
                else
                {
                    Main.logger?.Warn(2, $"Failed to remove mixer ID for instance: {instance}");
                }

                return removed;
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"RemoveMixerID: Caught exception: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        public static int GetMixerCount()
        {
            try
            {
                return MixerInstanceMap.Count;
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"GetMixerCount: Caught exception: {ex.Message}\n{ex.StackTrace}");
                return 0;
            }
        }
    }
}