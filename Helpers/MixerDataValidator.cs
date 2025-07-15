using System;
using System.Collections.Generic;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Validates mixer data integrity for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class MixerDataValidator
{
    /// <summary>
    /// Validates the provided mixer data dictionary.
    /// Returns true if valid, false otherwise.
    /// </summary>
    public static bool ValidateMixerData(Dictionary<int, float> mixerData, out string error)
    {
        error = null;
        if (mixerData == null)
        {
            error = string.Format("{0} Mixer data is null.", VALIDATOR_PREFIX);
            return false;
        }

        if (mixerData.Count == 0)
        {
            error = string.Format("{0} Mixer data is empty.", VALIDATOR_PREFIX);
            return false;
        }

        foreach (var kvp in mixerData)
        {
            if (kvp.Key < 0)
            {
                error = string.Format("{0} Invalid mixer ID: {1}", VALIDATOR_PREFIX, kvp.Key);
                return false;
            }
            if (float.IsNaN(kvp.Value) || float.IsInfinity(kvp.Value))
            {
                error = string.Format("{0} Invalid mixer value for ID {1}: {2}", VALIDATOR_PREFIX, kvp.Key, kvp.Value);
                return false;
            }
        }

        return true;
    }
}