

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using System.IO;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Detects the game installation directory for MixerThreholdMod.
/// âš ï¸ THREAD SAFETY: All operations are thread-safe.
/// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class GameInstallDirectoryResolver
{
    /// <summary>
    /// Attempts to detect the game installation directory using Unity and MelonLoader APIs.
    /// </summary>
    public static string DetectGameInstallDirectory()
    {
        try
        {
            // Unity Application.dataPath points to "Schedule I_Data", parent is install dir
            string dataPath = Application.dataPath;
            if (!string.IsNullOrEmpty(dataPath))
            {
                var gameDirectory = new DirectoryInfo(dataPath).Parent;
                if (gameDirectory != null && gameDirectory.Exists)
                {
                    Main.logger?.Msg(1, string.Format("{0} Game install directory detected via Unity API: {1}", DIRECTORY_RESOLVER_PREFIX, gameDirectory.FullName));
                    return gameDirectory.FullName;
                }
            }
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error detecting game install directory via Unity: {1}", DIRECTORY_RESOLVER_PREFIX, ex.Message));
        }

        // Fallback: MelonLoader's game root
        try
        {
            string melonGameRoot = MelonLoader.MelonEnvironment.GameRootDirectory;
            if (!string.IsNullOrEmpty(melonGameRoot) && Directory.Exists(melonGameRoot))
            {
                Main.logger?.Msg(1, string.Format("{0} Game install directory detected via MelonLoader: {1}", DIRECTORY_RESOLVER_PREFIX, melonGameRoot));
                return melonGameRoot;
            }
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error detecting game install directory via MelonLoader: {1}", DIRECTORY_RESOLVER_PREFIX, ex.Message));
        }

        Main.logger?.Warn(1, string.Format("{0} Game install directory not found.", DIRECTORY_RESOLVER_PREFIX));
        return string.Empty;
    }
}