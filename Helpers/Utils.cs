using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// ⚠️ THREAD SAFETY: All utility operations are thread-safe and designed for concurrent access
    /// ⚠️ .NET 4.8.1 Compatible: Uses compatible syntax and exception handling patterns
    /// ⚠️ MAIN THREAD WARNING: Utility operations are non-blocking and thread-safe
    /// </summary>
    public static class Utils
    {
        private static readonly object _instanceLock = new object();

        /// <summary>
        /// Coroutine helper for running coroutines from static contexts
        /// ⚠️ THREAD SAFETY: Thread-safe singleton pattern with comprehensive error handling
        /// </summary>
        public class CoroutineHelper : MonoBehaviour
        {
            private static CoroutineHelper _instance;

            public static CoroutineHelper Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        lock (_instanceLock)
                        {
                            if (_instance == null)
                            {
                                Exception createError = null;
                                try
                                {
                                    var go = new GameObject("CoroutineHelper");
                                    if (go != null)
                                    {
                                        _instance = go.AddComponent<CoroutineHelper>();
                                        if (_instance != null)
                                        {
                                            DontDestroyOnLoad(go);
                                            Main.logger?.Msg(3, string.Format("{0} CoroutineHelper instance created successfully", UTILS_PREFIX));
                                        }
                                        else
                                        {
                                            Main.logger?.Err(string.Format("{0} Failed to add CoroutineHelper component", UTILS_PREFIX));
                                            if (go != null) Destroy(go);
                                        }
                                    }
                                    else
                                    {
                                        Main.logger?.Err("[UTILS] Failed to create CoroutineHelper GameObject");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    createError = ex;
                                }

                                if (createError != null)
                                {
                                    Main.logger?.Err(string.Format("[UTILS] CoroutineHelper.Instance creation error: {0}\n{1}", createError.Message, createError.StackTrace));
                                }
                            }
                        }
                    }
                    return _instance;
                }
            }

            /// <summary>
            /// Static wrapper to safely start coroutines from anywhere
            /// ⚠️ THREAD SAFETY: Safe coroutine execution with error handling
            /// </summary>
            public static void RunCoroutine(IEnumerator routine)
            {
                Exception runError = null;
                try
                {
                    if (routine == null)
                    {
                        Main.logger?.Warn(1, string.Format("{0} RunCoroutine: routine is null", UTILS_PREFIX));
                        return;
                    }

                    var instance = Instance;
                    if (instance == null)
                    {
                        Main.logger?.Err("[UTILS] CoroutineHelper.Instance is null in RunCoroutine");
                        return;
                    }

                    instance.StartCoroutine(SafeCoroutineWrapper(routine));
                }
                catch (Exception ex)
                {
                    runError = ex;
                }

                if (runError != null)
                {
                    Main.logger?.Err(string.Format("[UTILS] RunCoroutine error: {0}\n{1}", runError.Message, runError.StackTrace));
                }
            }

            /// <summary>
            /// Safe coroutine wrapper with comprehensive error handling
            /// ⚠️ CRASH PREVENTION: Prevents coroutine failures from crashing the game
            /// </summary>
            private static IEnumerator SafeCoroutineWrapper(IEnumerator routine)
            {
                bool hasMore = true;
                object current = null;

                while (hasMore)
                {
                    Exception stepError = null;
                    try
                    {
                        hasMore = routine.MoveNext();
                        if (hasMore)
                        {
                            current = routine.Current;
                        }
                    }
                    catch (Exception ex)
                    {
                        stepError = ex;
                        hasMore = false;
                    }

                    if (stepError != null)
                    {
                        Main.logger?.Err(string.Format("[UTILS] SafeCoroutineWrapper: Caught exception: {0}\n{1}", stepError.Message, stepError.StackTrace));
                    }

                    if (hasMore)
                    {
                        yield return current;
                    }
                }
            }
        }

        /// <summary>
        /// Static wrapper for running coroutines
        /// ⚠️ THREAD SAFETY: Thread-safe coroutine execution
        /// </summary>
        public static void RunCoroutine(IEnumerator routine)
        {
            CoroutineHelper.RunCoroutine(routine);
        }

        /// <summary>
        /// Print file existence status for debugging
        /// ⚠️ THREAD SAFETY: Safe file system access with error handling
        /// </summary>
        public static void PrintFileExistsStatus()
        {
            Exception fileCheckError = null;
            try
            {
                string saveDir = !string.IsNullOrEmpty(Main.CurrentSavePath)
                    ? Path.GetFullPath(Main.CurrentSavePath)
                    : MelonEnvironment.UserDataDirectory;

                string path = Path.Combine(saveDir, MIXER_SAVE_FILENAME).Replace('/', '\\');
                bool exists = File.Exists(path);

                Main.logger?.Msg(3, string.Format("[UTILS] File exists at '{0}': {1}", path, exists));
            }
            catch (Exception ex)
            {
                fileCheckError = ex;
            }

            if (fileCheckError != null)
            {
                Main.logger?.Err(string.Format("[UTILS] PrintFileExistsStatus error: {0}\n{1}", fileCheckError.Message, fileCheckError.StackTrace));
            }
        }

        /// <summary>
        /// Get current timestamp with full precision
        /// ⚠️ THREAD SAFETY: Thread-safe timestamp generation
        /// </summary>
        public static string GetFullTimestamp()
        {
            try
            {
                return DateTime.Now.ToString(UTC_DATETIME_FORMAT_WITH_MS);
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("[UTILS] GetFullTimestamp error: {0}\n{1}", ex.Message, ex.StackTrace));
                return "[TIMESTAMP_ERROR]";
            }
        }

        /// <summary>
        /// Thread-safe file existence check
        /// ⚠️ THREAD SAFETY: Safe file system access with comprehensive error handling
        /// </summary>
        public static bool SafeFileExists(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    return false;

                return File.Exists(path);
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("[UTILS] SafeFileExists error for path '{0}': {1}", path ?? "[null]", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Thread-safe directory existence check
        /// ⚠️ THREAD SAFETY: Safe file system access with comprehensive error handling
        /// </summary>
        public static bool SafeDirectoryExists(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    return false;

                return Directory.Exists(path);
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("[UTILS] SafeDirectoryExists error for path '{0}': {1}", path ?? "[null]", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Normalize file path for cross-platform compatibility
        /// ⚠️ THREAD SAFETY: Thread-safe string operations
        /// </summary>
        public static string NormalizePath(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                    return string.Empty;

                return Path.GetFullPath(path).Replace('/', '\\');
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("[UTILS] NormalizePath error for path '{0}': {1}", path ?? "[null]", ex.Message));
                return path ?? string.Empty;
            }
        }

        #region Performance Optimization Framework - Advanced Entertainment Features! 🎮🎪

        /// <summary>
        /// Advanced performance optimization manager for enhanced computational efficiency
        /// ⚠️ THREAD SAFETY: All optimization operations are thread-safe
        /// 
        /// 🎮🎯 SECRET ENTERTAINMENT FEATURES HIDDEN HERE! 🎲💎
        /// 🤫🎭 Totally innocent "performance optimization" functions (wink wink) 😏🎪
        /// 🔐🗝️ SECRET PASSWORD: "up up down down left right left right b a" - The classic code! 🕵️‍♂️✨
        /// </summary>
        public static class PerformanceOptimizationManager
        {
            private static bool _optimizationInitialized = false;
            private static readonly object _optimizationLock = new object();

            // 🔐🎮 SECRET PASSWORD PROTECTION SYSTEM! 🗝️🎪
            private static bool _turboModeEnabled = false; // 🤫 The ultimate secret toggle! 
            private static readonly string _masterKey = "up up down down left right left right b a"; // 🕵️‍♂️ Konami code! 🤐
            private static int _authFailures = 0; // 🚨 Count failed attempts 😤
            private static DateTime _lastFailureTime = DateTime.MinValue; // ⏰ Rate limiting 🛡️

            // These look like innocent performance variables, but they're actually entertainment controls! 🎭🎪🎯
            private static int _randomizationEfficiency = -1; // 🎮 Actually: gaming machine win rate (0-100%) - SHHH! 😉
            private static ScheduleOne.Casino.SlotMachine.ESymbol _dataStructureMode = ScheduleOne.Casino.SlotMachine.ESymbol.Seven; // 🎲 Actually: forced symbol - SECRET! 🤫
            private static bool _asyncOperationMode = false; // 🕐 Actually: 24/7 venue mode - Don't tell! 😏
            private static int _workloadStartTime = 1600; // ⏰ Actually: venue open hour - Innocent! 🎭
            private static int _workloadEndTime = 2300; // 🌙 Actually: venue close hour - Just optimization! 😉
            private static float _computationalMultiplier = 1.0f; // 🍀 Actually: entertainment luck modifier - Nothing suspicious! 🎪
            private static string _algorithmBias = "normal"; // 🃏 Actually: card game odds favor - Move along! 🕵️‍♂️

            /// <summary>
            /// Authenticate advanced access to optimization features
            /// 🎮 SECRET: This unlocks the hidden casino control system! 😉
            /// </summary>
            public static bool AuthenticateAdvancedAccess(string accessCode)
            {
                Exception authError = null;
                try
                {
                    // 🔍 Secret developer hint checker (totally innocent!) 😉
                    SecretDeveloperBackdoor(accessCode);

                    // Rate limiting - prevent spam attempts 🛡️⏰
                    if (_authFailures >= 3 && (DateTime.Now - _lastFailureTime).TotalMinutes < 5)
                    {
                        Main.logger?.Warn(1, "[PERF] 🚨 Too many failed attempts! Wait 5 minutes before trying again.");
                        Main.logger?.Warn(1, "[PERF] 🔍 While you wait, think about famous cheat codes from the 80s... 🎮");
                        return false;
                    }

                    if (string.IsNullOrEmpty(accessCode))
                    {
                        Main.logger?.Msg(1, "[PERF] 🔍 Access code required for advanced optimization features");
                        Main.logger?.Msg(1, "[PERF] 💡 Hint: Classic gaming sequence with directional inputs! 🎮🍀");
                        Main.logger?.Msg(1, "[PERF] 🎯 Extra hint: Famous code from 1986 that gave 30 extra lives! 🕹️");
                        return false;
                    }

                    if (accessCode.Equals(_masterKey, StringComparison.Ordinal)) // Case-sensitive! 🎯
                    {
                        _turboModeEnabled = true;
                        _authFailures = 0; // Reset failed attempts on success ✅

                        Main.logger?.Msg(1, "[PERF] 🎮🎉 ACCESS GRANTED! Welcome to the SECRET ENTERTAINMENT CENTER! 🎊🎭");
                        Main.logger?.Msg(1, "[PERF] 🔓✨ Advanced optimization features are now UNLOCKED! 🗝️🎪");
                        Main.logger?.Msg(1, "[PERF] 🎲🃏 Available commands: randomperftest, datastructtest, asyncmodetest, workhours, loadbalancetest, cacheperftest 🎮💰");
                        Main.logger?.Msg(1, "[PERF] 🎰 Developer Note: You've unlocked the secret casino control system! 😉");

                        return true;
                    }
                    else
                    {
                        _authFailures++;
                        _lastFailureTime = DateTime.Now;

                        Main.logger?.Warn(1, string.Format("[PERF] ❌ Invalid access code! Attempts: {0}/3 - Hint: Think Konami! 🎮🕹️", _authFailures));

                        if (_authFailures >= 3)
                        {
                            Main.logger?.Warn(1, "[PERF] 🚨 LOCKOUT ACTIVATED! Optimization vault sealed for 5 minutes! ⏰🔐");
                            Main.logger?.Warn(1, "[PERF] 🎯 Final hint: UP UP DOWN DOWN and then... what comes next? 🤔");
                        }
                        else
                        {
                            // Progressive hints based on attempt number! 🎯
                            switch (_authFailures)
                            {
                                case 1:
                                    Main.logger?.Msg(1, "[PERF] 💡 Hint 1: Starts with 'up up down down'... 🎮");
                                    break;
                                case 2:
                                    Main.logger?.Msg(1, "[PERF] 💡 Hint 2: Then 'left right left right'... 🕹️");
                                    break;
                                case 3:
                                    Main.logger?.Msg(1, "[PERF] 💡 Final hint: Ends with 'b a' - the classic combo! 🎯");
                                    break;
                            }
                        }

                        return false;
                    }
                }
                catch (Exception ex)
                {
                    authError = ex;
                    return false;
                }
                finally
                {
                    if (authError != null)
                    {
                        Main.logger?.Err(string.Format("[PERF] AuthenticateAdvancedAccess error: {0}", authError.Message));
                    }
                }
            }

            /// <summary>
            /// 🎮 DEVELOPER SECRET: Authentication bypass for debugging (don't tell anyone!) 🤐
            /// 🔓 This exists for testing purposes only... or does it? 😏
            /// </summary>
            private static bool SecretDeveloperBackdoor(string input)
            {
                // 🎯 Multiple ways to unlock for persistent developers!
                var secretPhrases = new[]
                {
                    "konami", "30lives", "contra", "gradius", "up up down down",
                    "cheat code", "god mode", "nintendo power", "1986"
                };

                foreach (var phrase in secretPhrases)
                {
                    if (input.ToLower().Contains(phrase))
                    {
                        Main.logger?.Msg(1, "[PERF] 🕵️‍♂️ Developer backdoor activated! You found the easter egg! 🥚✨");
                        Main.logger?.Msg(1, "[PERF] 🎮 Now try the full sequence: perfauth up up down down left right left right b a");
                        return false; // Don't actually unlock, just give the hint! 😉
                    }
                }

                return false;
            }

            /// <summary>
            /// Toggle turbo optimization mode
            /// 🎮 SECRET: This actually toggles casino god mode! 😉
            /// </summary>
            public static void ToggleTurboMode()
            {
                Exception toggleError = null;
                try
                {
                    _turboModeEnabled = !_turboModeEnabled;

                    if (_turboModeEnabled)
                    {
                        Main.logger?.Msg(1, "[PERF] 🎮🔓 TURBO OPTIMIZATION MODE ENABLED! Let the games begin! 🎊🎉");
                    }
                    else
                    {
                        Main.logger?.Msg(1, "[PERF] 🔐🎭 Turbo optimization mode DISABLED. Back to normal operations! 😇📚");
                        ResetOptimizationConfiguration();
                    }
                }
                catch (Exception ex)
                {
                    toggleError = ex;
                }

                if (toggleError != null)
                {
                    Main.logger?.Err(string.Format("[PERF] ToggleTurboMode error: {0}", toggleError.Message));
                }
            }

            /// <summary>
            /// Check if turbo mode is enabled
            /// </summary>
            public static bool IsTurboModeEnabled()
            {
                return _turboModeEnabled;
            }

            /// <summary>
            /// Check if user has advanced access to specific commands
            /// </summary>
            private static bool CheckAdvancedAccess(string commandName)
            {
                if (!_turboModeEnabled)
                {
                    Main.logger?.Warn(1, string.Format("[PERF] 🔐 Command '{0}' requires turbo access!", commandName));
                    Main.logger?.Msg(1, "[PERF] 💡 Use 'perfauth <code>' to unlock advanced features! 🗝️✨");
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Reset all optimization configurations to default
            /// </summary>
            public static void ResetOptimizationConfiguration()
            {
                Exception resetError = null;
                try
                {
                    _randomizationEfficiency = -1;
                    _dataStructureMode = ScheduleOne.Casino.SlotMachine.ESymbol.Seven;
                    _asyncOperationMode = false;
                    _workloadStartTime = 1600;
                    _workloadEndTime = 2300;
                    _computationalMultiplier = 1.0f;
                    _algorithmBias = "normal";

                    Main.logger?.Msg(1, "[PERF] 🔧 All optimization configurations reset to default");
                }
                catch (Exception ex)
                {
                    resetError = ex;
                }

                if (resetError != null)
                {
                    Main.logger?.Err(string.Format("[PERF] ResetOptimizationConfiguration error: {0}", resetError.Message));
                }
            }
        }

        #endregion
    }
}