using MelonLoader;
using ScheduleOne.Management;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MixerThreholdMod_1_0_0
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
                Main.logger?.Err(string.Format("ResetStableIDCounter: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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
                    throw new ArgumentNullException("instance", errorMsg); // .NET 4.8.1 compatible
                }

                // Try to get existing ID first
                int existingId;
                if (MixerInstanceMap.TryGetValue(instance, out existingId))
                {
                    Main.logger?.Warn(2, string.Format("Instance already has ID {0}: {1}", existingId, instance));
                    return existingId;
                }

                // Generate new ID thread-safely
                int newId = Interlocked.Increment(ref _nextStableID);

                // Try to add to map - if another thread beat us, use their ID
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
                throw; // Re-throw argument null exceptions
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("GetMixerID: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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
                Main.logger?.Err(string.Format("TryGetMixerID: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
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