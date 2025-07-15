using System;
using System.IO;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Handles emergency save operations for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe and use async patterns.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class EmergencySaveManager
{
    /// <summary>
    /// Performs an emergency save of mixer data to a dedicated file.
    /// </summary>
    public static async Task<bool> PerformEmergencySaveAsync(string mixerDataJson, string emergencySavePath)
    {
        try
        {
            await Task.Run(() =>
            {
                File.WriteAllText(emergencySavePath, mixerDataJson);
            }).ConfigureAwait(false);

            Main.logger?.Msg(1, string.Format("{0} Emergency save completed: {1}", EMERGENCY_SAVE_PREFIX, emergencySavePath));
            return true;
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error during emergency save: {1}\n{2}", EMERGENCY_SAVE_PREFIX, ex.Message, ex.StackTrace));
            return false;
        }
    }

    /// <summary>
    /// Checks if an emergency save file exists.
    /// </summary>
    public static bool EmergencySaveExists(string emergencySavePath)
    {
        try
        {
            return File.Exists(emergencySavePath);
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error checking emergency save existence: {1}", EMERGENCY_SAVE_PREFIX, ex.Message));
            return false;
        }
    }
}