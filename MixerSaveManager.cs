using MelonLoader;
using MelonLoader.TinyJSON;
using MelonLoader.Utils;
using Newtonsoft.Json;
using ScheduleOne.Management;
using ScheduleOne.Persistence.Datas;
using ScheduleOne.Persistence.Loaders;
using ScheduleOne.Properties;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static HarmonyLib.Code;
using static MelonLoader.Modules.MelonModule;
using static MixerThreholdMod_0_0_1.Main;
using static ScheduleOne.Console;

namespace MixerThreholdMod_0_0_1
{
    public static class MixerSaveManager
    {
        private static CancellationTokenSource saveCts = new CancellationTokenSource();
        public static ConcurrentDictionary<int, float> SavedMixerValues = new ConcurrentDictionary<int, float>();
        public static IEnumerator AttachListenerWhenReady(MixingStationConfiguration instance, int uniqueID)
        {
            // Safety check: bail if instance is null
            if (instance == null)
            {
                Main.logger.Warn(1, $"Instance is null — cannot attach listener for Mixer {uniqueID}");
                yield break;
            }

            // Wait for StartThrehold to become available, but only while instance exists
            while (instance != null && instance.StartThrehold == null)
            {
                yield return null;
            }

            // Double-check instance still exists
            if (instance == null)
            {
                Main.logger.Warn(1, $"Instance destroyed before threshold became available — Mixer {uniqueID}");
                yield break;
            }

            // Restore saved value if exists
            if (Main.savedMixerValues.TryGetValue(uniqueID, out float savedValue))
            {
                instance.StartThrehold.SetValue(savedValue, true);
                Main.logger.Msg(3, $"Restored Mixer {uniqueID} to saved value: {savedValue}");
            }

            // Attach listener
            instance.StartThrehold.onItemChanged.AddListener((float val) =>
            {
                MixerSaveManager.SaveMixerValue(uniqueID, val);
                Main.logger.Msg(3, $"Mixer {uniqueID} changed to {val}");
            });
        }
        public static IEnumerator WriteMixerValuesAsync(string saveFolderPath)
        {
            yield return Task.Run(() => SaveMixerThresholds(saveFolderPath));
        }
        private static void SaveMixerValue(int id, float value)
        {
            // Cancel any previous save operation
            saveCts?.Cancel();
            saveCts = new CancellationTokenSource();

            // Run file save on background thread
            Task.Run(() => SaveMixerValueAsync(id, value, saveCts.Token));
        }

        private static async Task SaveMixerValueAsync(int id, float value, CancellationToken ct)
        {
            string path = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave.json").Replace('/', '\\');

            var saveData = new MixerThresholdSaveData
            {
                MixerValues = new Dictionary<int, float> { { id, value } }
            };

            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

            bool success = await CancellableIoRunner.Run(
                token => FileOperations.SafeWriteAllTextWithCancellationAsync(path, json, token, Main.logger.Warn),
                ct,
                Main.logger.Warn
            );

            if (!success)
            {
                Main.logger.Warn(1, "Mixer value save was canceled or failed.");
            }
        }
        public static async Task SafeWriteAllTextWithCancellationAsync(
            string path,
            string output,
            CancellationToken ct,
            Action<int, string> logger = null)
        {
            string normalizedPath = path.Replace('/', '\\');
            for (int attempt = 1; attempt <= FileOperations.MaxRetries; attempt++)
            {
                if (ct.IsCancellationRequested)
                {
                    logger?.Invoke(1, $"Operation canceled during SafeWriteAllTextWithCancellation for [{normalizedPath}]");
                    ct.ThrowIfCancellationRequested();
                }

                try
                {
                    using (var locker = new FileLockHelper(FileOperations.LockFilePath))
                    {
                        if (!locker.AcquireExclusiveLock())
                        {
                            if (attempt < FileOperations.MaxRetries)
                            {
                                logger?.Invoke(1, $"Could not acquire lock for [{normalizedPath}], retrying...");
                                await Task.Delay(FileOperations.RetryDelayMs * attempt, ct);
                                continue;
                            }
                            else
                            {
                                logger?.Invoke(1, $"Failed to acquire lock after {FileOperations.MaxRetries} attempts for [{normalizedPath}]");
                                return;
                            }
                        }

                        using (var writer = new StreamWriter(path, false, Encoding.UTF8))
                        {
                            await writer.WriteAsync(output).ConfigureAwait(false);
                        }

                        return;
                    }
                }
                catch (Exception ex)
                {
                    logger?.Invoke(1, $"Error writing to file [{path}]: {ex}");
                    if (attempt < FileOperations.MaxRetries)
                    {
                        await Task.Delay(FileOperations.RetryDelayMs * attempt, ct).ConfigureAwait(false);
                    }
                }
            }
        }
        public static void SaveMixerThresholds(string saveFolderPath)
        {
            Utils.CoroutineHelper.RunCoroutine(SaveMixerThresholdsAsync(saveFolderPath));
        }
        public static bool _hasLoggedZeroMixers = false;
        private static IEnumerator SaveMixerThresholdsAsync(string saveFolderPath)
        {

            yield return Task.Run(async () =>
            {
                string filePath = Path.Combine(saveFolderPath, "MixerThresholdSave.json").Replace('/', '\\');
                try
                {
                    var _mixerCount = await TrackedMixers.CountAsync(tm => true);
                    Main.logger.Msg(3, $"Currently tracking {_mixerCount} mixers: {string.Join(", ", await TrackedMixers.SelectAsync(tm => tm.MixerInstanceID))}");
                    if (_mixerCount == 0)
                    {
                        if (!_hasLoggedZeroMixers)
                        {
                            Main.logger.Msg(2, "No mixers tracked (maybe you have none?) — skipping mixer threshold prefs save/create.");
                            Main.logger.Warn(1, "No mixers tracked (maybe you have none?) — suppressing future logs until reload save.");
                            _hasLoggedZeroMixers = true;
                        }
                        return;
                    }
                    var mixerDataSnapshot = new Dictionary<int, float>();
                    foreach (var tm in await TrackedMixers.ToListAsync())
                    {
                        if (tm == null || tm.ConfigInstance == null || tm.ConfigInstance.StartThrehold == null)
                        {
                            Main.logger.Warn(1, $"Removing invalid mixer from tracked list: {tm?.MixerInstanceID ?? -1}");
                            await TrackedMixers.RemoveAsync(tm.ConfigInstance);
                            continue;
                        }
                        try
                        {
                            mixerDataSnapshot[tm.MixerInstanceID] = tm.ConfigInstance.StartThrehold.Value;
                        }
                        catch (Exception ex)
                        {
                            Main.logger.Err($"Error capturing mixer {tm.MixerInstanceID}: {ex}");
                        }
                    }
                    string json = JsonConvert.SerializeObject(new MixerThresholdSaveData
                    {
                        MixerValues = mixerDataSnapshot
                    }, Formatting.Indented);
                    bool success = false;
                    int attempts = 0;
                    while (!success && attempts < 5)
                    {
                        try
                        {
                            // Using SafeWriteAllText locks the file first
                            // Moved into own class, figured I could wrap the whole thing
                            // Same block of code that was here is now there :)
                            FileOperations.SafeWriteAllText(filePath, json);
                            Main.logger.Msg(1, $"Saved {mixerDataSnapshot.Count} mixer thresholds.");
                            success = true;
                        }
                        catch (IOException ex)
                        {
                            Main.logger.Warn(1, $"File write failed [{filePath}] (attempt {attempts + 1}), retrying... {ex.Message}");
                            attempts++;
                            Thread.Sleep(500);
                        }
                        catch (Exception ex)
                        {
                            Main.logger.Err($"Unexpected error saving mixer file [{filePath}]: {ex}");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // catchall at patch level, where my DLL interacts with the game and it's engine
                    // hopefully should catch errors in entire project?
                    Main.logger.Err($"SaveMixerThresholdsAsync: Critical error during saving [{filePath}]");
                    Main.logger.Err($"SaveMixerThresholdsAsync: Caught exception: {ex.Message}\n{ex.StackTrace}");
                }
                finally
                {
                    Main.logger.Msg(3, "Mixer save operation completed.");
                    // CRASHING HERE AFTER SAVING GAME! WHY? CATCHALL ABOVE DOES NOT TRIGGER AND SHOW ANY EXCEPTION?
                    // ANSWER: this function is called, completed successfully (somewhere) and then crashes after it was called
                    //         within that function. Find where it was. Related log below:
                    //
                    // [17:13:44.473] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Menu
                    // [17:13:56.917] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Main
                    // [17:13:56.918] [MixerThreholdMod] Stable ID counter reset.
                    // [17:13:56.918] [MixerThreholdMod] [Info][3]>=[3] Current Save Path at scene load: C:/Users/shawn/AppData/LocalLow/TVGS/Schedule I\Saves\76561197961254567\SaveGame_2
                    // [17:14:22.920] [MixerThreholdMod] [Info][3]>=[3] SaveManager.Save(string) called (Postfix)
                    // [17:14:22.921] [MixerThreholdMod] [Info][3]>=[2] Captured Save Folder Path: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                    // [17:14:22.921] [MixerThreholdMod] [Info][3]>=[2] Saving preferences file BEFORE backing up!
                    // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: written mixer pref file in SaveManager.Save(string) inside Postfix
                    // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: returned execution to SaveManager.Save(string) inside Postfix
                    // [17:14:22.922] [MixerThreholdMod] [Info][3]>=[2] Attempting backup of savegame directory!
                    // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] BackupSaveFolder: returned execution to SaveManager.Save(string) inside Postfix
                    // [17:14:23.910] [MixerThreholdMod] [Info][3]>=[1] Currently tracking 0 mixers: 
                    // [17:14:23.910] [MixerThreholdMod] [Info][3]>=[3] Mixer save operation completed.
                    // [17:14:24.410] [MixerThreholdMod] [Info][3]>=[2] BACKUP ROOT: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\
                    // [17:14:24.410] [MixerThreholdMod] [Info][3]>=[2] SAVE ROOT: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                    // [17:14:24.411] [MixerThreholdMod] [Info][3]>=[2] SAVE ROOT PREFIX: SaveGame_2
                    // [17:14:24.433] [MixerThreholdMod] [Info][3]>=[2] Saved backup to: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\SaveGame_2_backup_2025-07-10_17-14-24
                    // [17:14:24.434] [MixerThreholdMod] [Info][3]>=[2] Filtering to keep latest 5 backups from: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\SaveGame_2_backup_2025-07-10_17-14-24
                    // [17:20:02.907] [MixerThreholdMod] [Info][3]>=[3] SaveManager.Save(string) called (Postfix)
                    // [17:20:02.908] [MixerThreholdMod] [Info][3]>=[2] Captured Save Folder Path: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                    // [17:20:02.909] [MixerThreholdMod] [Info][3]>=[2] Saving preferences file BEFORE backing up!
                    // [17:20:02.910] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: written mixer pref file in SaveManager.Save(string) inside Postfix
                    // [17:20:02.911] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: returned execution to SaveManager.Save(string) inside Postfix
                    // [17:20:02.911] [MixerThreholdMod] [Info][3]>=[2] Attempting backup of savegame directory!
                    // [17:20:02.911] [MixerThreholdMod] [WARN][2]>=[2] BackupSaveFolder: returned execution to SaveManager.Save(string) inside Postfix
                    // [17:20:03.894] [MixerThreholdMod] [Info][3]>=[1] Currently tracking 0 mixers: 
                    // [17:20:03.894] [MixerThreholdMod] [Info][3]>=[3] Mixer save operation completed.[17:13:44.473] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Menu
                    // [17:13:56.917] [MixerThreholdMod] [Info][3]>=[2] Scene loaded: Main
                    // [17:13:56.918] [MixerThreholdMod] Stable ID counter reset.
                    // [17:13:56.918] [MixerThreholdMod] [Info][3]>=[3] Current Save Path at scene load: C:/Users/shawn/AppData/LocalLow/TVGS/Schedule I\Saves\76561197961254567\SaveGame_2
                    // [17:14:22.920] [MixerThreholdMod] [Info][3]>=[3] SaveManager.Save(string) called (Postfix)
                    // [17:14:22.921] [MixerThreholdMod] [Info][3]>=[2] Captured Save Folder Path: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                    // [17:14:22.921] [MixerThreholdMod] [Info][3]>=[2] Saving preferences file BEFORE backing up!
                    // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: written mixer pref file in SaveManager.Save(string) inside Postfix
                    // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: returned execution to SaveManager.Save(string) inside Postfix
                    // [17:14:22.922] [MixerThreholdMod] [Info][3]>=[2] Attempting backup of savegame directory!
                    // [17:14:22.922] [MixerThreholdMod] [WARN][2]>=[2] BackupSaveFolder: returned execution to SaveManager.Save(string) inside Postfix
                    // [17:14:23.910] [MixerThreholdMod] [Info][3]>=[1] Currently tracking 0 mixers: 
                    // [17:14:23.910] [MixerThreholdMod] [Info][3]>=[3] Mixer save operation completed.
                    // [17:14:24.410] [MixerThreholdMod] [Info][3]>=[2] BACKUP ROOT: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\
                    // [17:14:24.410] [MixerThreholdMod] [Info][3]>=[2] SAVE ROOT: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                    // [17:14:24.411] [MixerThreholdMod] [Info][3]>=[2] SAVE ROOT PREFIX: SaveGame_2
                    // [17:14:24.433] [MixerThreholdMod] [Info][3]>=[2] Saved backup to: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\SaveGame_2_backup_2025-07-10_17-14-24
                    // [17:14:24.434] [MixerThreholdMod] [Info][3]>=[2] Filtering to keep latest 5 backups from: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\MixerThreholdMod_backup\SaveGame_2_backup_2025-07-10_17-14-24
                    // [17:20:02.907] [MixerThreholdMod] [Info][3]>=[3] SaveManager.Save(string) called (Postfix)
                    // [17:20:02.908] [MixerThreholdMod] [Info][3]>=[2] Captured Save Folder Path: C:\Users\shawn\AppData\LocalLow\TVGS\Schedule I\Saves\76561197961254567\SaveGame_2
                    // [17:20:02.909] [MixerThreholdMod] [Info][3]>=[2] Saving preferences file BEFORE backing up!
                    // [17:20:02.910] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: written mixer pref file in SaveManager.Save(string) inside Postfix
                    // [17:20:02.911] [MixerThreholdMod] [WARN][2]>=[2] WriteDelayed: returned execution to SaveManager.Save(string) inside Postfix
                    // [17:20:02.911] [MixerThreholdMod] [Info][3]>=[2] Attempting backup of savegame directory!
                    // [17:20:02.911] [MixerThreholdMod] [WARN][2]>=[2] BackupSaveFolder: returned execution to SaveManager.Save(string) inside Postfix
                    // [17:20:03.894] [MixerThreholdMod] [Info][3]>=[1] Currently tracking 0 mixers: 
                    // [17:20:03.894] [MixerThreholdMod] [Info][3]>=[3] Mixer save operation completed.
                    // *CRASH*
                    // RESULT: MIXER SAVE COMPLETES, BUT CALLING METHOD CRASHES - INVESTIGATE CALLER!
                }
            });
        }
        private static bool _hasLoaded = false;
        private static object _loadLock = new object(); // Optional: for thread safety

        public static IEnumerator LoadMixerValuesWhenReady()
        {
            // 👇 Check the flag at the very start
            lock (_loadLock)
            {
                if (_hasLoaded)
                    yield break;

                // First one in, mark as loading
                _hasLoaded = true;
            }

            int attempts = 0;
            while (string.IsNullOrEmpty(Main.CurrentSavePath) && attempts < 50)
            {
                yield return new WaitForSeconds(0.1f);
                attempts++;
            }

            if (!string.IsNullOrEmpty(Main.CurrentSavePath))
            {
                LoadAndApplyMixerThresholds(Main.CurrentSavePath);
                logger.Msg(3, $"Loaded mixer values after save path became available.");
            }
            else
            {
                logger.Warn(1, "Save path never became available. Using empty/default mixer values.");
            }
        }

        public static Dictionary<int, float> LoadMixerValues()
        {
            Main.logger.Msg(1, "Loading mixer values from file...");
            string saveDir = !string.IsNullOrEmpty(Main.CurrentSavePath)
                ? Path.GetFullPath(Main.CurrentSavePath)
                : MelonEnvironment.UserDataDirectory;
            string path = Path.Combine(saveDir, "MixerThresholdSave.json").Replace('/', '\\');
            if (!File.Exists(path))
            {
                Main.logger.Warn(1, "No mixer save file found. Returning empty dictionary.");
                return new Dictionary<int, float>();
            }
            try
            {
                string json = FileOperations.SafeReadAllText(path);
                var saveData = JsonConvert.DeserializeObject<MixerThresholdSaveData>(json);
                Main.logger.Msg(1, $"Loaded {saveData.MixerValues.Count} mixer values from: {path}");
                return saveData.MixerValues;
            }
            catch (Exception ex)
            {
                Main.logger.Err($"Failed to load mixer values: {ex}");
                return new Dictionary<int, float>();
            }
        }
        public async static void LoadAndApplyMixerThresholds(string saveFolderPath)
        {
            string[] pathsToTry;
            if (!string.IsNullOrEmpty(saveFolderPath))
            {
                string savePath = Path.Combine(saveFolderPath, "MixerThresholdSave.json").Replace('/', '\\');
                string persistentPath = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave.json").Replace('/', '\\');
                pathsToTry = new[] { savePath, persistentPath };
            }
            else
            {
                string persistentPath = Path.Combine(MelonEnvironment.UserDataDirectory, "MixerThresholdSave.json").Replace('/', '\\');
                pathsToTry = new[] { persistentPath };
            }

            foreach (var path in pathsToTry)
            {
                if (File.Exists(path))
                {
                    try
                    {
                        string json = FileOperations.SafeReadAllText(path);
                        MixerThresholdSaveData data = JsonConvert.DeserializeObject<MixerThresholdSaveData>(json);
                        foreach (var kvp in data.MixerValues)
                        {
                            SavedMixerValues[kvp.Key] = kvp.Value;
                        }
                        foreach (var tm in await TrackedMixers.GetAllAsync())
                        {
                            if (tm == null) continue;
                            if (tm.ConfigInstance?.StartThrehold != null &&
                                SavedMixerValues.TryGetValue(tm.MixerInstanceID, out float savedValue))
                            {
                                tm.ConfigInstance.StartThrehold.SetValue(savedValue, true);
                                Main.logger.Msg(1, $"Applied saved value {savedValue} to Mixer {tm.MixerInstanceID}");
                            }
                        }
                        Main.logger.Msg(1, $"Loaded {data.MixerValues.Count} mixer thresholds from: {path}");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Main.logger.Err($"Error loading MixerThresholdSave.json from {path}: {ex}");
                    }
                }
            }

            // Only warn if we're past initial load (e.g., save path was set and we still couldn't find the file)
            if (!string.IsNullOrEmpty(saveFolderPath))
            {
                Main.logger.Warn(1, "No MixerThresholdSave.json found in any location. Do you have any mixers?");
            }
        }
        [Serializable]
        public class MixerThresholdSaveData
        {
            public Dictionary<int, float> MixerValues = new Dictionary<int, float>();
        }
    }
}