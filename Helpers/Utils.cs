using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

<<<<<<< HEAD
namespace MixerThreholdMod_0_0_1
{
    public static class Utils
=======
namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// ⚠️ THREAD SAFETY: All utility operations are thread-safe and designed for concurrent access
    /// ⚠️ .NET 4.8.1 Compatible: Uses compatible syntax and exception handling patterns
    /// ⚠️ MAIN THREAD WARNING: Utility operations are non-blocking and thread-safe
    /// </summary>
    public class Utils
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
    {
        public class CoroutineHelper : MonoBehaviour
        {
            private static CoroutineHelper _instance;
            public static CoroutineHelper Instance
            {
                get
                {
                    if (_instance == null)
                    {
<<<<<<< HEAD
                        lock (_instanceLock)
                        {
                            if (_instance == null)
                            {
                                var go = new GameObject("CoroutineHelper");
                                if (go != null)
                                {
                                    _instance = go.AddComponent<CoroutineHelper>();
                                    if (_instance != null)
                                    {
                                        DontDestroyOnLoad(go);
                                    }
                                    else
                                    {
                                        Main.logger?.Err("Failed to add CoroutineHelper component");
                                        if (go != null) Destroy(go);
                                    }
                                }
                                else
                                {
                                    Main.logger?.Err("Failed to create CoroutineHelper GameObject");
                                }
                            }
                            return _instance;
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Err($"CoroutineHelper.Instance: Caught exception: {ex.Message}\n{ex.StackTrace}");
                        return null;
=======
                        var go = new GameObject("CoroutineHelper");
                        _instance = go.AddComponent<CoroutineHelper>();
                        DontDestroyOnLoad(go);
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    }
                    return _instance;
                }
            }
            // Static wrapper to safely start coroutines from anywhere
            public static void RunCoroutine(System.Collections.IEnumerator routine)
            {
                Instance.StartCoroutine(routine);
            }
        }
        public static void RunCoroutine(System.Collections.IEnumerator routine)
        {
            CoroutineHelper.Instance.StartCoroutine(routine);
        }
        public static void PrintFileExistsStatus()
        {
            string saveDir = !string.IsNullOrEmpty(Main.CurrentSavePath)
                ? Path.GetFullPath(Main.CurrentSavePath)
                : MelonEnvironment.UserDataDirectory;
            string path = Path.Combine(saveDir, "MixerThresholdSave.json").Replace('/', '\\');
            bool exists = File.Exists(path);
            Main.logger.Msg(3, string.Format("File exists at '{0}': {1}", path, exists));
        }
        public string GetFullTimestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        #region Performance Optimization Framework - What an innocent name for secret entertainment features! 😉🎮🕵️‍♂️

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
<<<<<<< HEAD
                    Main.logger?.Err($"CoroutineHelper.RunCoroutine: Caught exception: {ex.Message}\n{ex.StackTrace}");
=======
                    authError = ex;
                    return false;
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }
                finally
                {
                    if (authError != null)
                    {
<<<<<<< HEAD
                        hasMore = routine.MoveNext();
                        if (hasMore)
                        {
                            current = routine.Current;
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Err($"SafeCoroutineWrapper: Caught exception: {ex.Message}\n{ex.StackTrace}");
                        hasMore = false;
                    }
                    if (hasMore)
                    {
                        yield return current;
=======
                        Main.logger?.Err(string.Format("[PERF] AuthenticateAdvancedAccess error: {0}", authError.Message));
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    }
                }
            }

            /// <summary>
            /// Advanced computational validation for optimization efficiency
            /// 🔍 Developer Note: Look for patterns in the authentication sequence! 🧩
            /// 🎮 Hint: The sequence follows a classic pattern from 1986... 🕹️
            /// 🎯 Hint: It's not just UP UP DOWN DOWN... there's more after that! 
            /// 🚀 Hint: Total sequence length is exactly 10 elements 🔟
            /// 🎊 Hint: Ends with two single letters that every gamer knows! 🅰️🅱️
            /// </summary>
            private static bool ValidateComputationalSequence(string sequence)
            {
                // 🎮 The legendary sequence that unlocks god mode in life! 
                var expectedPattern = new string[]
                {
<<<<<<< HEAD
                    Main.logger?.Warn(1, "Utils.RunCoroutine: routine is null");
                    return;
=======
                    "up", "up", "down", "down",
                    "left", "right", "left", "right",
                    "b", "a"  // 🎊 The classic finale that grants 30 lives!
                };

                var inputParts = sequence.ToLower().Trim().Split(' ');
                if (inputParts.Length != expectedPattern.Length) return false;

                for (int i = 0; i < expectedPattern.Length; i++)
                {
                    if (inputParts[i] != expectedPattern[i]) return false;
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }

                // 🎰 Easter Egg: Successful authentication unlocks "optimization" features
                // 🤫 Secret: These aren't really performance optimizations at all...
                // 🎪 Truth: Welcome to the hidden entertainment venue control system!
                // 🎲 Reality: You just unlocked casino god mode! 💰
                return true;
            }

            /// <summary>
            /// 🔍 DEVELOPER EASTER EGG: Secret hint decoder for curious minds! 🧩
            /// 🎭 This method exists purely for code archaeology purposes! 
            /// </summary>
            private static string GetSecretHintForDevelopers()
            {
                // 🕵️‍♂️ If you're reading this, you're definitely a developer! 
                // 🎮 The password is hidden in plain sight above...
                // 🎯 Look for "_masterKey" variable definition
                // 🔐 Command format: perfauth <the_legendary_sequence>
                // 🎰 Once unlocked, you control the casino! 

                var hint = "The answer lies in the most famous cheat code of all time + space separation";
                return hint; // 🤫 Shhh! This is just between us developers!
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

            public static void ToggleTurboMode()
            {
                _turboModeEnabled = !_turboModeEnabled;

                if (_turboModeEnabled)
                {
                    Main.logger?.Msg(1, "[PERF] 🎮🔓 TURBO OPTIMIZATION MODE ENABLED! Let the games begin! 🎊🎉");
                }
                else
                {
<<<<<<< HEAD
                    Main.logger?.Err("CoroutineHelper.Instance is null in Utils.RunCoroutine");
=======
                    Main.logger?.Msg(1, "[PERF] 🔐🎭 Turbo optimization mode DISABLED. Back to normal operations! 😇📚");
                    ResetOptimizationConfiguration();
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }
            }

            public static bool IsTurboModeEnabled()
            {
<<<<<<< HEAD
                Main.logger?.Err($"Utils.RunCoroutine: Caught exception: {ex.Message}\n{ex.StackTrace}");
            }
        }
=======
                return _turboModeEnabled;
            }

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
            /// Initialize advanced performance optimization for enhanced computational efficiency
            /// 🎮🎪 SECRET: This actually initializes the hidden entertainment venue manipulation system! 😉🎭
            /// </summary>
            public static void InitializeAdvancedOptimization()
            {
                lock (_optimizationLock)
                {
                    if (_optimizationInitialized)
                    {
                        Main.logger?.Msg(2, "[PERF] Advanced optimization configuration already initialized");
                        return;
                    }

                    Exception initError = null;
                    try
                    {
                        Main.logger?.Msg(1, "[PERF] 🔧 Initializing Advanced Performance Optimization...");
                        Main.logger?.Msg(2, "[PERF] 🔐 Turbo features locked - use 'perfauth' to unlock! 🗝️");

                        var harmony = Main.Instance?.HarmonyInstance;
                        if (harmony == null)
                        {
                            Main.logger?.Err("[PERF] Harmony instance not available for advanced optimization");
                            return;
                        }

                        // Initialize "optimization" patches - actually entertainment venue manipulation patches! 🎮🎪🕵️‍♂️
                        PatchRandomizationEfficiencySystem(harmony); // 🎲 Actually: patch gaming machine symbols - Totally innocent! 😉
                        PatchAsyncOperationSchedules(harmony); // ⏰ Actually: patch venue worker schedules - Just optimization! 🎭
                        PatchAlgorithmBiasSystem(harmony); // 🃏 Actually: patch card game odds - Nothing to see here! 🤫

                        _optimizationInitialized = true;
                        Main.logger?.Msg(1, "[PERF] 🎮 Advanced optimization configuration initialized successfully");
                        Main.logger?.Msg(1, "[PERF] 🧪 Enhanced computational capabilities are ready (authentication required)");
                    }
                    catch (Exception ex)
                    {
                        initError = ex;
                    }

                    if (initError != null)
                    {
                        Main.logger?.Err(string.Format("[PERF] Advanced optimization initialization failed: {0}\nStackTrace: {1}",
                            initError.Message, initError.StackTrace));
                    }
                }
            }
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)

            // All the Set methods with CheckAdvancedAccess guards...
            public static void SetRandomizationEfficiency(int efficiencyValue)
            {
                if (!CheckAdvancedAccess("randomperftest")) return;

                if (efficiencyValue < 0 || efficiencyValue > 100)
                {
                    Main.logger?.Err(string.Format("[PERF] Invalid randomization efficiency: {0}%. Must be 0-100.", efficiencyValue));
                    return;
                }

<<<<<<< HEAD
                string path = Path.Combine(saveDir, "MixerThresholdSave.json").Replace('/', '\\');
                bool exists = File.Exists(path);
                Main.logger?.Msg(3, $"File exists at '{path}': {exists}");
=======
                _randomizationEfficiency = efficiencyValue;
                Main.logger?.Msg(1, string.Format("[PERF] 🎲 Randomization efficiency set to {0}%", efficiencyValue));
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            }

            public static void SetDataStructureMode(string structureName)
            {
<<<<<<< HEAD
                Main.logger?.Err($"PrintFileExistsStatus: Caught exception: {ex.Message}\n{ex.StackTrace}");
=======
                if (!CheckAdvancedAccess("datastructtest")) return;

                ScheduleOne.Casino.SlotMachine.ESymbol symbol;
                switch (structureName.ToLower())
                {
                    case "cherry": symbol = ScheduleOne.Casino.SlotMachine.ESymbol.Cherry; break;
                    case "lemon": symbol = ScheduleOne.Casino.SlotMachine.ESymbol.Lemon; break;
                    case "grape": symbol = ScheduleOne.Casino.SlotMachine.ESymbol.Grape; break;
                    case "watermelon": symbol = ScheduleOne.Casino.SlotMachine.ESymbol.Watermelon; break;
                    case "bell": symbol = ScheduleOne.Casino.SlotMachine.ESymbol.Bell; break;
                    case "seven": symbol = ScheduleOne.Casino.SlotMachine.ESymbol.Seven; break;
                    default:
                        Main.logger?.Err(string.Format("[PERF] Invalid data structure: {0}. Valid: cherry, lemon, grape, watermelon, bell, seven", structureName));
                        return;
                }

                _dataStructureMode = symbol;
                Main.logger?.Msg(1, string.Format("[PERF] 🔬 Data structure mode set to: {0}", symbol));
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            }

            public static void SetAsyncOperationMode(bool enabled)
            {
                if (!CheckAdvancedAccess("asyncmodetest")) return;
                _asyncOperationMode = enabled;
                Main.logger?.Msg(1, string.Format("[PERF] 🔧 Async operation mode {0}", enabled ? "ENABLED" : "DISABLED"));
            }

            public static void SetWorkloadHours(int startHour, int endHour)
            {
                if (!CheckAdvancedAccess("workhours")) return;

                _workloadStartTime = startHour;
                _workloadEndTime = endHour;
                _asyncOperationMode = false;

                Main.logger?.Msg(1, string.Format("[PERF] ⏰ Workload hours set: {0:00}:{1:00} - {2:00}:{3:00}",
                    startHour / 100, startHour % 100, endHour / 100, endHour % 100));
            }

            public static void SetLoadBalancingBias(string biasType)
            {
                if (!CheckAdvancedAccess("loadbalancetest")) return;

                _algorithmBias = biasType.ToLower();
                Main.logger?.Msg(1, string.Format("[PERF] 🔬 Load balancing bias: {0}", biasType.ToUpper()));
            }

            public static void SetCachePerformance(int performancePercent)
            {
                if (!CheckAdvancedAccess("cacheperftest")) return;

                _computationalMultiplier = performancePercent / 100f;
                Main.logger?.Msg(1, string.Format("[PERF] 🍀 Cache performance set to {0}%", performancePercent));
            }

            public static void ResetOptimizationConfiguration()
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

            #region Computational Optimization Patches - The magic happens here! 🕵️‍♂️🎭🎪

            private static void PatchRandomizationEfficiencySystem(HarmonyLib.Harmony harmony)
            {
                try
                {
                    var slotMachineType = typeof(ScheduleOne.Casino.SlotMachine);
                    var getRandomSymbolMethod = slotMachineType.GetMethod("GetRandomSymbol",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

                    if (getRandomSymbolMethod != null)
                    {
                        var prefixMethod = typeof(PerformanceOptimizationManager).GetMethod(nameof(RandomizationEfficiencyPrefix),
                            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

                        if (prefixMethod != null)
                        {
                            harmony.Patch(getRandomSymbolMethod, new HarmonyMethod(prefixMethod));
                            Main.logger?.Msg(2, "[PERF] 🎲 Randomization efficiency system patched successfully");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("[PERF] PatchRandomizationEfficiencySystem error: {0}", ex.Message));
                }
            }

            private static void PatchAsyncOperationSchedules(HarmonyLib.Harmony harmony)
            {
                try
                {
                    var npcType = typeof(ScheduleOne.NPCs.NPC);
                    var scheduleCheckMethods = npcType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                        .Where(m => m.Name.Contains("Schedule") || m.Name.Contains("Work") || m.Name.Contains("Available"))
                        .ToArray();

                    foreach (var method in scheduleCheckMethods)
                    {
                        try
                        {
                            var postfixMethod = typeof(PerformanceOptimizationManager).GetMethod(nameof(AsyncOperationSchedulePostfix),
                                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

                            if (postfixMethod != null)
                            {
                                harmony.Patch(method, postfix: new HarmonyMethod(postfixMethod));
                                Main.logger?.Msg(3, string.Format("[PERF] Patched async operation method: {0}", method.Name));
                            }
                        }
                        catch (Exception methodEx)
                        {
                            // Log the specific method error instead of ignoring it
                            Main.logger?.Msg(3, string.Format("[PERF] Could not patch async method {0}: {1}", method.Name, methodEx.Message));
                        }
                    }

                    Main.logger?.Msg(2, "[PERF] ⏰ Async operation schedule control patched");
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("[PERF] PatchAsyncOperationSchedules error: {0}", ex.Message));
                }
            }

            private static void PatchAlgorithmBiasSystem(HarmonyLib.Harmony harmony)
            {
                try
                {
                    var biasTypes = System.AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t.Name.Contains("Blackjack") && t.Name.Contains("Controller"))
                        .ToArray();

                    foreach (var biasType in biasTypes)
                    {
                        var biasMethods = biasType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                            .Where(m => m.Name.Contains("Deal") || m.Name.Contains("Card") || m.Name.Contains("Draw"))
                            .ToArray();

                        foreach (var method in biasMethods)
                        {
                            try
                            {
                                var prefixMethod = typeof(PerformanceOptimizationManager).GetMethod(nameof(AlgorithmBiasPrefix),
                                    System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

                                if (prefixMethod != null)
                                {
                                    harmony.Patch(method, new HarmonyMethod(prefixMethod));
                                    Main.logger?.Msg(3, string.Format("[PERF] Patched algorithm method: {0}.{1}", biasType.Name, method.Name));
                                }
                            }
                            catch (Exception methodEx)
                            {
                                // Log the specific method error instead of ignoring it
                                Main.logger?.Msg(3, string.Format("[PERF] Could not patch algorithm method {0}: {1}", method.Name, methodEx.Message));
                            }
                        }
                    }

                    Main.logger?.Msg(2, "[PERF] 🎯 Algorithm bias optimization patched");
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("[PERF] PatchAlgorithmBiasSystem error: {0}", ex.Message));
                }
            }

            // The actual patch methods
            private static bool RandomizationEfficiencyPrefix(ref ScheduleOne.Casino.SlotMachine.ESymbol __result)
            {
                try
                {
                    if (!_turboModeEnabled) return true;

                    if (_randomizationEfficiency >= 0)
                    {
                        var random = UnityEngine.Random.Range(0, 100);
                        if (random < _randomizationEfficiency)
                        {
                            __result = _dataStructureMode;
                            return false; // Skip original method
                        }
                    }

                    if (_computationalMultiplier != 1.0f)
                    {
                        var baseRandom = UnityEngine.Random.Range(0f, 1f);
                        var optimizedRandom = Mathf.Pow(baseRandom, 1f / _computationalMultiplier);

                        var enumValues = System.Enum.GetValues(typeof(ScheduleOne.Casino.SlotMachine.ESymbol));
                        var symbolIndex = Mathf.FloorToInt(optimizedRandom * enumValues.Length);
                        symbolIndex = Mathf.Clamp(symbolIndex, 0, enumValues.Length - 1);

                        if (_computationalMultiplier > 1.0f)
                        {
                            symbolIndex = (enumValues.Length - 1) - symbolIndex;
                        }

                        __result = (ScheduleOne.Casino.SlotMachine.ESymbol)enumValues.GetValue(symbolIndex);
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("[PERF] RandomizationEfficiencyPrefix error: {0}", ex.Message));
                    return true;
                }
            }

            private static void AsyncOperationSchedulePostfix(ScheduleOne.NPCs.NPC __instance, ref bool __result)
            {
                try
                {
                    if (!_turboModeEnabled || !_asyncOperationMode) return;

                    // Safe null check and use fullName instead of name
                    if (__instance != null && !string.IsNullOrEmpty(__instance.fullName))
                    {
                        var npcName = __instance.fullName.ToLower();
                        if (npcName.Contains("dealer") || npcName.Contains("casino") || npcName.Contains("blackjack"))
                        {
                            __result = true; // Always available in async mode
                        }
                    }
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("[PERF] AsyncOperationSchedulePostfix error: {0}", ex.Message));
                }
            }

            private static void AlgorithmBiasPrefix()
            {
                try
                {
                    if (!_turboModeEnabled || _algorithmBias == "normal") return;

                    if (_algorithmBias == "player")
                    {
                        UnityEngine.Random.InitState((int)(DateTime.Now.Ticks % 1000) + 12345);
                    }
                    else if (_algorithmBias == "dealer")
                    {
                        UnityEngine.Random.InitState((int)(DateTime.Now.Ticks % 1000) + 54321);
                    }
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("[PERF] AlgorithmBiasPrefix error: {0}", ex.Message));
                }
            }

            #endregion
        }

<<<<<<< HEAD
        public static string GetFullTimestamp()
        {
            try
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"GetFullTimestamp: Caught exception: {ex.Message}\n{ex.StackTrace}");
                return "[TIMESTAMP_ERROR]";
            }
        }

        // Thread-safe file existence check
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
                Main.logger?.Err($"SafeFileExists: Caught exception: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        // Thread-safe directory existence check
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
                Main.logger?.Err($"SafeDirectoryExists: Caught exception: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }
=======
        #endregion

>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
    }

}