using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Manages mixer instance IDs for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
internal static class MixerIDManager
{
    private static readonly object _lock = new object();
    private static readonly ConcurrentDictionary<int, int> MixerInstanceMap = new ConcurrentDictionary<int, int>();
    private static int _stableIDCounter = 1;

    /// <summary>
    /// Gets a stable ID for a mixer instance.
    /// </summary>
    public static int GetStableID(int instanceID)
    {
        lock (_lock)
        {
            if (!MixerInstanceMap.ContainsKey(instanceID))
            {
                MixerInstanceMap[instanceID] = _stableIDCounter++;
                Main.logger?.Msg(2, string.Format("{0} Assigned stable ID {1} to instance {2}", MIXER_ID_PREFIX, MixerInstanceMap[instanceID], instanceID));
            }
            return MixerInstanceMap[instanceID];
        }
    }

    /// <summary>
    /// Clears all stable ID mappings.
    /// </summary>
    public static void Clear()
    {
        lock (_lock)
        {
            MixerInstanceMap.Clear();
            _stableIDCounter = 1;
            Main.logger?.Msg(1, string.Format("{0} Cleared all stable ID mappings.", MIXER_ID_PREFIX));
        }
    }

    /// <summary>
    /// Resets the stable ID counter.
    /// </summary>
    public static void ResetStableIDCounter()
    {
        lock (_lock)
        {
            _stableIDCounter = 1;
            Main.logger?.Msg(1, string.Format("{0} Stable ID counter reset.", MIXER_ID_PREFIX));
        }
    }
}