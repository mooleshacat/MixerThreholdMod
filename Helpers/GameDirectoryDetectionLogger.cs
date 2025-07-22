

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Logs and summarizes game directory detection results for MixerThreholdMod.
///  THREAD SAFETY: All operations are thread-safe.
///  .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class GameDirectoryDetectionLogger
{
    /// <summary>
    /// Logs a summary of detected directories.
    /// </summary>
    public static void LogDetectionSummary(string gameInstallDir, string userDataDir, string saveDir, string melonLoaderDir, string melonLoaderLog)
    {
        try
        {
            Main.logger?.Msg(1, string.Format("{0} === DIRECTORY DETECTION SUMMARY ===", DIRECTORY_RESOLVER_PREFIX));
            Main.logger?.Msg(1, string.Format("{0} Game Install Directory: {1}", DIRECTORY_RESOLVER_PREFIX, gameInstallDir));
            Main.logger?.Msg(1, string.Format("{0} User Data Directory: {1}", DIRECTORY_RESOLVER_PREFIX, userDataDir));
            Main.logger?.Msg(1, string.Format("{0} Save Directory: {1}", DIRECTORY_RESOLVER_PREFIX, saveDir));
            Main.logger?.Msg(1, string.Format("{0} MelonLoader Directory: {1}", DIRECTORY_RESOLVER_PREFIX, melonLoaderDir));
            Main.logger?.Msg(1, string.Format("{0} MelonLoader Log File: {1}", DIRECTORY_RESOLVER_PREFIX, melonLoaderLog));
            Main.logger?.Msg(1, string.Format("{0} ================================", DIRECTORY_RESOLVER_PREFIX));
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error logging directory detection summary: {1}", DIRECTORY_RESOLVER_PREFIX, ex.Message));
        }
    }
}