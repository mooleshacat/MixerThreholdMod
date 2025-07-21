

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using System.IO;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Detects the user data directory for MixerThreholdMod.
/// âš ï¸ THREAD SAFETY: All operations are thread-safe.
/// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class UserDataDirectoryResolver
{
    /// <summary>
    /// Attempts to detect the user data directory using Unity and MelonLoader APIs.
    /// </summary>
    public static string DetectUserDataDirectory()
    {
        try
        {
            string persistentDataPath = Application.persistentDataPath;
            if (!string.IsNullOrEmpty(persistentDataPath))
            {
                if (!Directory.Exists(persistentDataPath))
                {
                    Directory.CreateDirectory(persistentDataPath);
                }
                Main.logger?.Msg(1, string.Format("{0} User data directory detected via Unity API: {1}", DIRECTORY_RESOLVER_PREFIX, persistentDataPath));
                return persistentDataPath;
            }
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error detecting user data directory via Unity: {1}", DIRECTORY_RESOLVER_PREFIX, ex.Message));
        }

        // Fallback: MelonLoader user data directory
        try
        {
            string melonUserData = MelonLoader.MelonEnvironment.UserDataDirectory;
            if (!string.IsNullOrEmpty(melonUserData) && Directory.Exists(melonUserData))
            {
                Main.logger?.Msg(1, string.Format("{0} User data directory detected via MelonLoader: {1}", DIRECTORY_RESOLVER_PREFIX, melonUserData));
                return melonUserData;
            }
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error detecting user data directory via MelonLoader: {1}", DIRECTORY_RESOLVER_PREFIX, ex.Message));
        }

        Main.logger?.Warn(1, string.Format("{0} User data directory not found.", DIRECTORY_RESOLVER_PREFIX));
        return string.Empty;
    }
}