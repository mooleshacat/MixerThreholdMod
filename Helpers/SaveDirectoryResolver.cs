

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using System.IO;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Detects the save directory for MixerThreholdMod.
///  THREAD SAFETY: All operations are thread-safe.
///  .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class SaveDirectoryResolver
{
    /// <summary>
    /// Attempts to detect the save directory using user data directory and known patterns.
    /// </summary>
    public static string DetectSaveDirectory(string userDataDirectory)
    {
        try
        {
            if (!string.IsNullOrEmpty(userDataDirectory))
            {
                var savesPath = Path.Combine(userDataDirectory, "Saves");
                if (Directory.Exists(savesPath))
                {
                    Main.logger?.Msg(1, string.Format("{0} Save directory detected: {1}", DIRECTORY_RESOLVER_PREFIX, savesPath));
                    return savesPath;
                }
            }
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error detecting save directory: {1}", DIRECTORY_RESOLVER_PREFIX, ex.Message));
        }

        Main.logger?.Warn(1, string.Format("{0} Save directory not found.", DIRECTORY_RESOLVER_PREFIX));
        return string.Empty;
    }
}