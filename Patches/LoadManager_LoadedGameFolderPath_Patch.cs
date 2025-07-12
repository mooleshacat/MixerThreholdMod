using HarmonyLib;
using MelonLoader;
using ScheduleOne.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MixerThreholdMod_0_0_1
{
    [HarmonyPatch(typeof(LoadManager), "StartGame")]
    public static class LoadManager_LoadedGameFolderPath_Patch
    {
        public static void Postfix(LoadManager __instance, SaveInfo info, bool allowLoadStacking)
        {
            if (info == null || string.IsNullOrEmpty(info.SavePath))
                return;

            string __savePath = info?.SavePath;

            try
            {
                Main.logger.Msg(3, string.Format("LoadManager_LoadedGameFolderPath_Patch: Postfix called with result: {0}", __savePath ?? "null"));

                if (!string.IsNullOrEmpty(__savePath))
                {
                    Main.CurrentSavePath = __savePath;

                    string path = Utils.NormalizePath(Path.Combine(__savePath, "MixerThresholdSave.json"));
                    
                    int _mixerCount = 0;
                    try
                    {
                        _mixerCount = TrackedMixers.Count(tm => tm != null);
                    }
                    catch (Exception countEx)
                    {
                        Main.logger.Err(string.Format("LoadManager_LoadedGameFolderPath_Patch: Error counting mixers: {0}", countEx.Message));
                    }

                    if (_mixerCount == 0)
                    {
                        // Use the same flag, so when one triggers it suppresses the other :)
                        if (!MixerSaveManager._hasLoggedZeroMixers)
                        {
                            Main.logger.Msg(2, "No mixers tracked (maybe you have none?) — skipping mixer threshold prefs save/create.");
                            Main.logger.Warn(1, "No mixers tracked (maybe you have none?) — suppressing future logs until reload save.");
                            MixerSaveManager._hasLoggedZeroMixers = true;
                        }
                        return;
                    }

                    if (!string.IsNullOrEmpty(path) && !File.Exists(path))
                    {
                        try
                        {
                            Main.logger.Warn(1, "MixerThresholdSave.json missing on load — creating it now.");
                            Utils.CoroutineHelper.RunCoroutine(SaveThresholdsCoroutine(__savePath));
                        }
                        catch (Exception coroutineEx)
                        {
                            Main.logger.Err(string.Format("LoadManager_LoadedGameFolderPath_Patch: Error starting save coroutine: {0}", coroutineEx.Message));
                        }
                    }
                }
                else
                {
                    Main.logger.Warn(1, "LoadedGameFolderPath is null or empty — cannot track save path.");
                }
            }
            catch (Exception ex)
            {
                // catchall at patch level, where my DLL interacts with the game and it's engine
                // hopefully should catch errors in entire project?
                Main.logger.Err("LoadManager_LoadedGameFolderPath_Patch: Failed during path handling");
                Main.logger.Err(string.Format("LoadManager_LoadedGameFolderPath_Patch: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private static IEnumerator SaveThresholdsCoroutine(string savePath)
        {
            try
            {
                if (string.IsNullOrEmpty(savePath))
                {
                    Main.logger.Warn(1, "SaveThresholdsCoroutine: savePath is null or empty");
                    yield break;
                }
                // This will run the async save logic safely
                MixerSaveManager.SaveMixerThresholds(savePath);
            }
            catch (Exception ex)
            {
                Main.logger.Err(string.Format("SaveThresholdsCoroutine: Error: {0}\n{1}", ex.Message, ex.StackTrace));
            }
            
            yield return null; // Done
        }
    }
}