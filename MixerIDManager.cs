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
    public class MixerIDManager
    {
        private static int _nextStableID = 0;
        public static readonly ConcurrentDictionary<MixingStationConfiguration, int> MixerInstanceMap =
            new ConcurrentDictionary<MixingStationConfiguration, int>();

        public static void ResetStableIDCounter()
        {
            try
            {
                _nextStableID = 0;
                MelonLogger.Msg("Stable ID counter reset.");
            }
            catch (Exception ex)
            {
                Main.logger.Err($"ResetStableIDCounter: Error resetting counter: {ex.Message}");
            }
        }

        public static int GetMixerID(MixingStationConfiguration instance)
        {
            try
            {
                if (instance == null)
                {
                    Main.logger.Err("GetMixerID: Cannot assign ID to null MixingStationConfiguration");
                    throw new ArgumentNullException(nameof(instance), "Cannot assign ID to null MixingStationConfiguration");
                }

                if (MixerInstanceMap == null)
                {
                    Main.logger.Err("GetMixerID: MixerInstanceMap is null");
                    throw new InvalidOperationException("MixerInstanceMap is not initialized");
                }

                if (MixerInstanceMap.TryGetValue(instance, out int id))
                {
                    Main.logger.Warn(2, $"Instance already has ID {id}: {instance}");
                    return id;
                }

                id = Interlocked.Increment(ref _nextStableID);
                bool added = MixerInstanceMap.TryAdd(instance, id);
                
                if (added)
                {
                    Main.logger.Msg(3, $"Assigned new ID {id} to instance: {instance}");
                }
                else
                {
                    Main.logger.Warn(1, $"Failed to add instance {id} to MixerInstanceMap, returning ID anyway");
                }
                
                return id;
            }
            catch (ArgumentNullException)
            {
                throw; // Re-throw argument null exceptions
            }
            catch (Exception ex)
            {
                Main.logger.Err($"GetMixerID: Unexpected error: {ex.Message}\n{ex.StackTrace}");
                throw; // Re-throw to maintain expected behavior
            }
        }
    }
}