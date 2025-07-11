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
            _nextStableID = 0;
            MelonLogger.Msg("Stable ID counter reset.");
        }

        public static int GetMixerID(MixingStationConfiguration instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance), "Cannot assign ID to null MixingStationConfiguration");

            if (MixerInstanceMap.TryGetValue(instance, out int id))
            {
                Main.logger.Warn(2, $"Instance already has ID {id}: {instance}");
                return id;
            }

            id = Interlocked.Increment(ref _nextStableID);
            MixerInstanceMap.TryAdd(instance, id);
            Main.logger.Msg(3, $"Assigned new ID {id} to instance: {instance}");
            return id;
        }
    }
}