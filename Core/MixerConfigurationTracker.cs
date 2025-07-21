using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Tracks mixer configuration values in a thread-safe manner.
/// âš ï¸ THREAD SAFETY: All operations are thread-safe using ConcurrentDictionary and lock objects.
/// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types and proper error handling.
/// </summary>
internal static class MixerConfigurationTracker
{
    private static readonly object _lock = new object();
    private static readonly ConcurrentDictionary<int, float> MixerConfigMap = new ConcurrentDictionary<int, float>();

    /// <summary>
    /// Sets the configuration value for a mixer.
    /// </summary>
    public static void SetConfig(int mixerId, float configValue)
    {
        lock (_lock)
        {
            MixerConfigMap[mixerId] = configValue;
            Main.logger?.Msg(2, string.Format("{0} Set config for mixer {1} to {2:F3}", MIXER_CONFIG_PREFIX, mixerId, configValue));
        }
    }

    /// <summary>
    /// Gets the configuration value for a mixer, or returns 0 if not found.
    /// </summary>
    public static float GetConfig(int mixerId)
    {
        lock (_lock)
        {
            return MixerConfigMap.TryGetValue(mixerId, out float value) ? value : 0f;
        }
    }

    /// <summary>
    /// Removes a mixer configuration.
    /// </summary>
    public static void RemoveConfig(int mixerId)
    {
        lock (_lock)
        {
            MixerConfigMap.TryRemove(mixerId, out _);
            Main.logger?.Msg(2, string.Format("{0} Removed config for mixer {1}", MIXER_CONFIG_PREFIX, mixerId));
        }
    }

    /// <summary>
    /// Gets all mixer configuration IDs.
    /// </summary>
    public static IReadOnlyCollection<int> GetAllConfigIds()
    {
        lock (_lock)
        {
            return new List<int>(MixerConfigMap.Keys).AsReadOnly();
        }
    }
}