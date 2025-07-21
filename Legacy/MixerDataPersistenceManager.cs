

using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using MelonLoader;
using MelonLoader.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using MixerThreholdMod_1_0_0.Constants;    // âœ… ESSENTIAL - Keep this! Our constants!
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Utils
{
    /// <summary>
    /// Comprehensive mixer save/load management system with crash prevention focus.
    /// Handles mixer value persistence, backup management, and event attachment.
    /// 
    /// âš ï¸ CRASH PREVENTION FOCUS: This system is specifically designed to prevent 
    /// save corruption and data loss during crashes, repeated saves, and extended gameplay.
    /// 
    /// âš ï¸ THREAD SAFETY: All save operations are thread-safe with proper locking mechanisms.
    /// Coroutines are used to prevent blocking Unity's main thread during file operations.
    /// 
    /// âš ï¸ MAIN THREAD WARNING: Emergency save methods are designed for crash scenarios and 
    /// use blocking I/O. Regular save operations use coroutines to avoid main thread blocking.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses string.Format instead of string interpolation
    /// - Compatible async patterns with proper ConfigureAwait usage
    /// - Manual dictionary operations instead of modern LINQ where needed
    /// - Proper exception handling and resource cleanup
    /// 
    /// Key Features:
    /// - Save cooldown system to prevent corruption from rapid saves
    /// - Automatic backup creation and cleanup (maintains 5 most recent)
    /// - Event attachment with multiple fallback strategies
    /// - Emergency save functionality for crash scenarios
    /// - Atomic file operations with temp file + rename strategy
    /// </summary>
    public static class MixerSaveManager
    {
        // Concurrency protection fields
        private static bool isBackupInProgress         private static readonly object backupLock         private static bool isSaveInProgress         private static readonly object saveLock         private static DateTime lastSaveTime 		private static readonly TimeSpan SAVE_COOLDOWN 		private static readonly TimeSpan BACKUP_INTERVAL             bool loadCompleted             Exception loadError             Task.Run(async ()                     loadCompleted                     loadError                     loadCompleted             if (loadError !                string saveFile                     string json                         var data                         if (data !                            var mixerValues                 string saveFile                     string json =>                        var data                         if (data !                            var mixerValues             bool attachCompleted             bool eventAttached             Exception attachError             Task.Run(()                     eventAttached                     attachCompleted                     attachError                     attachCompleted             if (attachError !                var numberFieldType                 var eventNames                     var eventInfo                     if (eventInfo != null && eventInfo.EventHandlerType !                        var handler                         if (handler !                if (eventHandlerType =                    return new Action<float>((float newValue)                 else if (eventHandlerType =                    return new System.EventHandler((object sender, EventArgs e)                 else if (eventHandlerType.IsGenericType && eventHandlerType.GetGenericTypeDefinition() =                    var paramType                     if (paramType =                        return new Action<float>((float newValue)                 bool hasOldValue                 if (sender !                    var senderType                     var valueProperty                     if (valueProperty != null && valueProperty.PropertyType =                        float currentValue             float lastKnownValue             bool hasInitialValue                 Exception pollError                 float? currentValue                     currentValue                     pollError                 if (pollError =                        lastKnownValue                         hasInitialValue                         lastKnownValue                 if (pollError !                if (numberField =                var type                 var propertyNames                     var property                     if (property != null && property.PropertyType =                var methodNames                     var method                     if (method != null && method.ReturnType =            bool canProceed                     isSaveInProgress                     lastSaveTime                     canProceed                     isSaveInProgress             if (Main.savedMixerValues.Count =                    isSaveInProgress             bool needsBackup             bool saveCompleted             Exception saveError             Task.Run(async ()                     saveCompleted                     saveError                     saveCompleted                 isSaveInProgress             if (saveError !                string saveFile                 var mixerValuesDict                     mixerValuesDict[kvp.Key]                 var saveData                     [MIXER_VALUES_KEY]                     [SAVE_TIME_KEY]                     [VERSION_KEY]                 string json >                string persistentPath                     string persistentFile                 string saveFile                 var mixerValuesDict                     mixerValuesDict[kvp.Key]                 var saveData                     [MIXER_VALUES_KEY]                     [SAVE_TIME_KEY]                     [VERSION_KEY]                 string json                 string persistentPath                     string persistentFile                 string saveFile                 var mixerValuesDict                     mixerValuesDict[kvp.Key]                 var saveData                     [MIXER_VALUES_KEY]                     [SAVE_TIME_KEY]                     [VERSION_KEY]                 string json                 string persistentPath                     string persistentFile =>            bool canProceed                     isBackupInProgress                     canProceed                     isBackupInProgress             bool backupCompleted             Exception backupError             Task.Run(()                     backupCompleted                     backupError                     backupCompleted                 isBackupInProgress             if (backupError !            string backupDir             string sourceFile             string timestamp             string backupFile             var allBackupFiles                 var sortedBackups = allBackupFiles.OrderByDescending(f                 var oldBackups                 string backupDir                 var allBackupFiles                 var latestBackup = allBackupFiles.OrderByDescending(f                 if (latestBackup =                if (Main.savedMixerValues.Count =                string persistentPath                 string emergencyFile                 var mixerValuesDict                     mixerValuesDict[kvp.Key]                 var saveData                     [MIXER_VALUES_KEY]                     [SAVE_TIME_KEY]                     ["Reason"]                 string json }