

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Centralizes Harmony patch initialization for MixerThreholdMod.
    /// âš ï¸ THREAD SAFETY: All operations are thread-safe and use async patterns.
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses explicit types, string.Format, and proper error handling.
    /// âš ï¸ MAIN THREAD WARNING: Never blocks Unity main thread; all patching is async.
    /// </summary>
    internal class PatchInitializer
    {
        private readonly object _lock = new object();
        private bool _patchesInitialized;

        /// <summary>
        /// Initializes all required Harmony patches for the mod.
        /// </summary>
        /// <returns>True if all patches initialized successfully, false if any failed</returns>
        public async Task<bool> InitializeAllPatchesAsync()
        {
            lock (_lock)
            {
                if (_patchesInitialized)
                {
                    Main.logger?.Msg(2, string.Format("{0} Patches already initialized.", PATCH_INIT_PREFIX));
                    return true;
                }
                _patchesInitialized = true;
            }

            Main.logger?.Msg(1, string.Format("{0} Initializing Harmony patches...", PATCH_INIT_PREFIX));
            var errors = new List<string>();

            try
            {
                await Task.Run(() =>
                {
                try
                {
                    Patches.SaveManager_Save_Patch.Initialize();
                    Main.logger?.Msg(2, string.Format("{0} SaveManager_Save_Patch initialized.", PATCH_INIT_PREFIX));
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("SaveManager_Save_Patch: {0}", ex.Message));
                    Main.logger?.Err(string.Format("{0} Error initializing SaveManager_Save_Patch: {1}\n{2}", PATCH_INIT_PREFIX, ex.Message, ex.StackTrace));
                }

                try
                {
                    Patches.LoadManager_LoadedGameFolderPath_Patch.Initialize();
                    Main.logger?.Msg(2, string.Format("{0} LoadManager_LoadedGameFolderPath_Patch initialized.", PATCH_INIT_PREFIX));
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("LoadManager_LoadedGameFolderPath_Patch: {0}", ex.Message));
                    Main.logger?.Err(string.Format("{0} Error initializing LoadManager_LoadedGameFolderPath_Patch: {1}\n{2}", PATCH_INIT_PREFIX, ex.Message, ex.StackTrace));
                }

                try
                {
                    Patches.EntityConfiguration_Destroy_Patch.Initialize();
                    Main.logger?.Msg(2, string.Format("{0} EntityConfiguration_Destroy_Patch initialized.", PATCH_INIT_PREFIX));
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format("EntityConfiguration_Destroy_Patch: {0}", ex.Message));
                    Main.logger?.Err(string.Format("{0} Error initializing EntityConfiguration_Destroy_Patch: {1}\n{2}", PATCH_INIT_PREFIX, ex.Message, ex.StackTrace));
                }
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} Critical error during patch initialization: {1}\n{2}", PATCH_INIT_PREFIX, ex.Message, ex.StackTrace));
                return false;
            }

            if (errors.Count > 0)
            {
                Main.logger?.Warn(1, string.Format("{0} {1} patch(es) failed to initialize: {2}", PATCH_INIT_PREFIX, errors.Count, string.Join(", ", errors.ToArray())));
                return false;
            }

            Main.logger?.Msg(1, string.Format("{0} All Harmony patches initialized successfully.", PATCH_INIT_PREFIX));
            return true;
        }
    }
} 