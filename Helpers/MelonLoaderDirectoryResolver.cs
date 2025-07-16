using System;
using System.IO;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

/// <summary>
/// Detects the MelonLoader directory and log files for MixerThreholdMod.
/// ⚠️ THREAD SAFETY: All operations are thread-safe.
/// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
/// </summary>
public static class MelonLoaderDirectoryResolver
{
    /// <summary>
    /// Attempts to detect the MelonLoader directory and log file.
    /// </summary>
    public static (string melonLoaderDirectory, string logFile) DetectMelonLoaderDirectory()
    {
        string melonLoaderDirectory = string.Empty;
        string logFile = string.Empty;

        try
        {
            string gameRoot = MelonLoader.MelonEnvironment.GameRootDirectory;
            if (!string.IsNullOrEmpty(gameRoot))
            {
                melonLoaderDirectory = Path.Combine(gameRoot, "MelonLoader");
                if (Directory.Exists(melonLoaderDirectory))
                {
                    string[] logFiles = { "Latest.log", "Console.log", "MelonLoader.log" };
                    foreach (var fileName in logFiles)
                    {
                        string candidate = Path.Combine(melonLoaderDirectory, fileName);
                        if (File.Exists(candidate))
                        {
                            logFile = candidate;
                            break;
                        }
                    }
                    Main.logger?.Msg(1, string.Format("{0} MelonLoader directory detected: {1}", DIRECTORY_RESOLVER_PREFIX, melonLoaderDirectory));
                    if (!string.IsNullOrEmpty(logFile))
                    {
                        Main.logger?.Msg(1, string.Format("{0} MelonLoader log file detected: {1}", DIRECTORY_RESOLVER_PREFIX, logFile));
                    }
                    else
                    {
                        Main.logger?.Warn(1, string.Format("{0} No MelonLoader log file found in: {1}", DIRECTORY_RESOLVER_PREFIX, melonLoaderDirectory));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Main.logger?.Err(string.Format("{0} Error detecting MelonLoader directory: {1}", DIRECTORY_RESOLVER_PREFIX, ex.Message));
        }

        return (melonLoaderDirectory, logFile);
    }
}