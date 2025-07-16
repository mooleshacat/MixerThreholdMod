using System;
using MixerThreholdMod_1_0_0.Core;

namespace MixerThreholdMod_1_0_0.Patches
{
    /// <summary>
    /// Patch for LoadManager.LoadedGameFolderPath to intercept and enhance folder path loading.
    /// ⚠️ THREAD SAFETY: All operations are thread-safe.
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses explicit types and error handling.
    /// ⚠️ MAIN THREAD WARNING: Never blocks Unity main thread.
    /// </summary>
    public static class LoadManager_LoadedGameFolderPath_Patch
    {
        private static readonly Logger logger = new Logger();

        /// <summary>
        /// Intercepts LoadManager.LoadedGameFolderPath and applies custom logic.
        /// </summary>
        /// <param name="folderPath">The loaded game folder path.</param>
        public static void Prefix(string folderPath)
        {
            try
            {
                logger.Msg(1, "[LoadManager_LoadedGameFolderPath_Patch] Prefix: Intercepted LoadManager.LoadedGameFolderPath call.");

                // Example: Validate folder path before proceeding
                if (string.IsNullOrEmpty(folderPath))
                {
                    logger.Warn(1, "[LoadManager_LoadedGameFolderPath_Patch] Prefix: folderPath is null or empty.");
                    return;
                }

                // Add further patch logic as needed...

            }
            catch (Exception ex)
            {
                logger.Err(string.Format("Prefix failed in LoadManager_LoadedGameFolderPath_Patch: {0}\nStack Trace: {1}", ex.Message, ex.StackTrace));
            }
        }
    }
}