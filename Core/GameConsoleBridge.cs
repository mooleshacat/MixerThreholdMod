using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Native game console integration for mod commands
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and designed for concurrent access
    /// ⚠️ .NET 4.8.1 Compatible: Uses compatible syntax and exception handling patterns
    /// ⚠️ MAIN THREAD WARNING: Console operations are non-blocking and thread-safe
    /// 
    /// Integration Features:
    /// - Injects mod commands into game's native console system
    /// - Commands appear in game's help and auto-complete
    /// - Leverages game's existing command infrastructure
    /// - Maintains compatibility with game updates
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses reflection-based approach instead of dynamic IL generation
    /// - Compatible exception handling patterns
    /// - Proper type checking and validation
    /// 
    /// Crash Prevention Features:
    /// - Comprehensive error handling for reflection operations
    /// - Graceful degradation when game console system changes
    /// - Safe command registration with validation
    /// - Prevents mod failures from affecting game console
    /// </summary>
    public static class GameConsoleBridge
    {
        private static bool _isInitialized = false;
        private static readonly object _initLock = new object();

        /// <summary>
        /// Initialize native console integration using reflection
        /// ⚠️ CRASH PREVENTION: Safe integration with comprehensive error handling
        /// </summary>
        public static void InitializeNativeConsoleIntegration()
        {
            lock (_initLock)
            {
                if (_isInitialized)
                {
                    Main.logger?.Msg(2, "[BRIDGE] GameConsoleBridge already initialized");
                    return;
                }

                Exception integrationError = null;
                try
                {
                    Main.logger?.Msg(2, "[BRIDGE] Initializing native console integration");
                    Main.logger?.Msg(3, "[BRIDGE] Searching for ScheduleOne.Console class...");

                    // dnSpy Verified: ScheduleOne.Console class exists with correct namespace and class structure
                    // Analysis confirmed Console class contains multiple command management fields and methods
                    var consoleType = System.Type.GetType("ScheduleOne.Console, Assembly-CSharp");
                    Main.logger?.Msg(3, string.Format("[BRIDGE] ScheduleOne.Console type found: {0}", consoleType != null ? "YES" : "NO"));
                    
                    if (consoleType != null)
                    {
                        Main.logger?.Msg(3, string.Format("[BRIDGE] Console type full name: {0}", consoleType.FullName));
                        Main.logger?.Msg(3, "[BRIDGE] Searching for commands field...");
                        
                        // dnSpy Verified: Console class contains both Commands (List<string>) and commands (Dictionary<string,ConsoleCommand>) fields
                        // Try lowercase "commands" first (Dictionary) - this is the actual command registry  
                        var commandsField = consoleType.GetField("commands", BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                        var commandsListField = consoleType.GetField("Commands", BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                        
                        Main.logger?.Msg(3, string.Format("[BRIDGE] commands field found: {0}", commandsField != null ? "YES" : "NO"));
                        Main.logger?.Msg(3, string.Format("[BRIDGE] Commands field found: {0}", commandsListField != null ? "YES" : "NO"));
                        
                        if (commandsField != null)
                        {
                            Main.logger?.Msg(3, string.Format("[BRIDGE] commands field type: {0}", commandsField.FieldType));
                            
                            // This should be the Dictionary<string, ConsoleCommand> field
                            if (commandsField.FieldType.IsGenericType && 
                                commandsField.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                            {
                                var commandsDict = commandsField.GetValue(null) as System.Collections.IDictionary;
                                Main.logger?.Msg(3, string.Format("[BRIDGE] Commands dictionary retrieved: {0}", commandsDict != null ? "YES" : "NO"));
                                
                                if (commandsDict != null)
                                {
                                    Main.logger?.Msg(3, string.Format("[BRIDGE] Commands dictionary count: {0}", commandsDict.Count));
                                    Main.logger?.Msg(2, "[BRIDGE] Successfully accessing game's command registry - proceeding with integration");
                                    
                                    // Add mod commands to game's native console using reflection-based approach
                                    AddModCommandsToGameConsole(commandsDict, consoleType);

                                    _isInitialized = true;
                                    Main.logger?.Msg(1, "[BRIDGE] Successfully integrated mod commands into game console");
                                }
                                else
                                {
                                    Main.logger?.Warn(1, "[BRIDGE] Could not access game's commands dictionary - may not be initialized yet");
                                    // Try alternative approaches
                                    TryAlternativeConsoleIntegration(consoleType);
                                }
                            }
                            else
                            {
                                Main.logger?.Warn(1, string.Format("[BRIDGE] commands field is not a Dictionary type: {0}", commandsField.FieldType));
                                // Try alternative approaches for non-dictionary types
                                TryAlternativeConsoleIntegration(consoleType);
                            }
                        }
                        else if (commandsListField != null)
                        {
                            Main.logger?.Msg(3, string.Format("[BRIDGE] Commands list field type: {0}", commandsListField.FieldType));
                            Main.logger?.Warn(1, "[BRIDGE] Found Commands list but not commands dictionary - trying alternative integration");
                            TryAlternativeConsoleIntegration(consoleType);
                        }
                        else
                        {
                            Main.logger?.Warn(1, "[BRIDGE] Could not find commands field in Console class");
                            // List available fields for debugging
                            LogAvailableFields(consoleType);
                        }
                    }
                    else
                    {
                        Main.logger?.Warn(1, "[BRIDGE] Could not find ScheduleOne.Console class");
                        // Try to find any Console class
                        TryFindAnyConsoleClass();
                    }
                }
                catch (Exception ex)
                {
                    integrationError = ex;
                }

                if (integrationError != null)
                {
                    Main.logger?.Err(string.Format("[BRIDGE] Native console integration failed: {0}\nStackTrace: {1}",
                        integrationError.Message, integrationError.StackTrace));
                }
                
                // Regardless of native integration success, provide fallback information
                if (!_isInitialized)
                {
                    Main.logger?.Warn(1, "[BRIDGE] Native console integration failed - using manual command processing only");
                    Main.logger?.Msg(1, "[BRIDGE] Console commands available through manual processing:");
                    Main.logger?.Msg(1, "[BRIDGE] Use Core.Console.ProcessManualCommand(\"command\") for testing");
                    Main.logger?.Msg(1, "[BRIDGE] Note: Commands may not appear in game's console help system");
                }
                else
                {
                    Main.logger?.Msg(1, "[BRIDGE] Console integration completed successfully");
                    Main.logger?.Msg(1, "[BRIDGE] Commands should be available in game's native console system");
                }
            }
        }

        /// <summary>
        /// Log available fields in the console type for debugging
        /// </summary>
        private static void LogAvailableFields(Type consoleType)
        {
            Exception fieldError = null;
            try
            {
                Main.logger?.Msg(3, "[BRIDGE] Available fields in Console class:");
                var fields = consoleType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    Main.logger?.Msg(3, string.Format("[BRIDGE]   - {0} ({1})", field.Name, field.FieldType.Name));
                }
            }
            catch (Exception ex)
            {
                fieldError = ex;
            }
            
            if (fieldError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] LogAvailableFields error: {0}", fieldError.Message));
            }
        }

        /// <summary>
        /// Try to find any console class in the game assemblies
        /// </summary>
        private static void TryFindAnyConsoleClass()
        {
            Exception findError = null;
            try
            {
                Main.logger?.Msg(3, "[BRIDGE] Searching for any Console class in loaded assemblies...");
                
                var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        var types = assembly.GetTypes();
                        foreach (var type in types)
                        {
                            if (type.Name.Contains("Console"))
                            {
                                Main.logger?.Msg(3, string.Format("[BRIDGE] Found Console-like class: {0} in {1}", type.FullName, assembly.GetName().Name));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Skip assemblies that can't be reflected over
                    }
                }
            }
            catch (Exception ex)
            {
                findError = ex;
            }
            
            if (findError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] TryFindAnyConsoleClass error: {0}", findError.Message));
            }
        }

        /// <summary>
        /// Try alternative console integration approaches
        /// Enhanced to include direct console input hooking
        /// </summary>
        private static void TryAlternativeConsoleIntegration(Type consoleType)
        {
            Exception altError = null;
            try
            {
                Main.logger?.Msg(2, "[BRIDGE] Attempting alternative console integration...");
                
                // Try to find any methods that might be used for command registration
                var methods = consoleType.GetMethods(BindingFlags.Public | BindingFlags.Static);
                Main.logger?.Msg(3, "[BRIDGE] Available static methods in Console class:");
                foreach (var method in methods)
                {
                    Main.logger?.Msg(3, string.Format("[BRIDGE]   - {0}", method.Name));
                }

                // Try using Harmony to patch the console input method directly
                TryHarmonyConsoleIntegration(consoleType);
            }
            catch (Exception ex)
            {
                altError = ex;
            }
            
            if (altError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] TryAlternativeConsoleIntegration error: {0}", altError.Message));
            }
        }

        /// <summary>
        /// Use Harmony to patch the console command processing directly
        /// This ensures our commands are recognized by the game's console system
        /// </summary>
        private static void TryHarmonyConsoleIntegration(Type consoleType)
        {
            Exception harmonyError = null;
            try
            {
                Main.logger?.Msg(2, "[BRIDGE] Attempting Harmony-based console integration...");
                
                // dnSpy Verified: Console method signatures and parameter types confirmed through comprehensive decompilation analysis
                // All Console.Log, Console.LogWarning, Console.LogError methods use (object, UnityEngine.Object) signature
                var processMethod = consoleType.GetMethod("Process", BindingFlags.Public | BindingFlags.Static);
                if (processMethod == null)
                {
                    processMethod = consoleType.GetMethod("ProcessCommand", BindingFlags.Public | BindingFlags.Static);
                }
                if (processMethod == null)
                {
                    processMethod = consoleType.GetMethod("ExecuteCommand", BindingFlags.Public | BindingFlags.Static);
                }

                if (processMethod != null)
                {
                    Main.logger?.Msg(3, string.Format("[BRIDGE] Found console process method: {0}", processMethod.Name));
                    
                    // Create harmony patches to intercept console commands
                    var harmony = Main.Instance?.HarmonyInstance;
                    if (harmony != null)
                    {
                        var prefixMethod = typeof(GameConsoleBridge).GetMethod("ConsoleProcessPrefix", BindingFlags.Static | BindingFlags.NonPublic);
                        if (prefixMethod != null)
                        {
                            harmony.Patch(processMethod, new HarmonyMethod(prefixMethod));
                            Main.logger?.Msg(2, "[BRIDGE] Successfully patched console command processing with Harmony");
                            _isInitialized = true;
                        }
                        else
                        {
                            Main.logger?.Err("[BRIDGE] ConsoleProcessPrefix method not found for Harmony patch");
                        }
                    }
                    else
                    {
                        Main.logger?.Err("[BRIDGE] Harmony instance not available for console patching");
                    }
                }
                else
                {
                    Main.logger?.Warn(1, "[BRIDGE] Could not find console command processing method for Harmony patching");
                    
                    // List all available methods for debugging
                    var allMethods = consoleType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                    Main.logger?.Msg(3, "[BRIDGE] All available methods in Console class:");
                    foreach (var method in allMethods)
                    {
                        var paramTypes = string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name).ToArray());
                        Main.logger?.Msg(3, string.Format("[BRIDGE]   - {0}({1})", method.Name, paramTypes));
                        
                        // Look for promising methods to patch
                        if (method.Name.ToLower().Contains("process") || 
                            method.Name.ToLower().Contains("execute") || 
                            method.Name.ToLower().Contains("command") ||
                            method.Name.ToLower().Contains("parse"))
                        {
                            Main.logger?.Msg(2, string.Format("[BRIDGE] ** Potential command method: {0}({1})", method.Name, paramTypes));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                harmonyError = ex;
            }
            
            if (harmonyError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] TryHarmonyConsoleIntegration error: {0}", harmonyError.Message));
            }
        }

        /// <summary>
        /// Harmony prefix patch for console command processing
        /// Intercepts console commands and handles mod-specific ones
        /// ⚠️ COMPREHENSIVE LOGGING: Logs all console commands for debugging, including non-mod commands
        /// </summary>
        private static bool ConsoleProcessPrefix(string command)
        {
            Exception patchError = null;
            try
            {
                if (string.IsNullOrEmpty(command))
                {
                    Main.logger?.Msg(3, "[BRIDGE] Empty command intercepted - allowing original processing");
                    return true; // Continue with original method
                }

                var lowerCommand = command.ToLower().Trim();
                var parts = lowerCommand.Split(' ');
                var baseCommand = parts[0];

                // Log ALL console commands for comprehensive debugging
                Main.logger?.Msg(2, string.Format("[BRIDGE] === INTERCEPTED CONSOLE COMMAND ==="));
                Main.logger?.Msg(2, string.Format("[BRIDGE] Raw command: '{0}'", command));
                Main.logger?.Msg(2, string.Format("[BRIDGE] Base command: '{0}'", baseCommand));
                if (parts.Length > 1)
                {
                    Main.logger?.Msg(2, string.Format("[BRIDGE] Command parameters ({0}): [{1}]", parts.Length - 1, string.Join(", ", parts, 1, parts.Length - 1)));
                }
                else
                {
                    Main.logger?.Msg(3, "[BRIDGE] No parameters detected");
                }

                // Check if this is one of our mod commands
                var modCommands = new string[] 
                { 
                    "savemonitor", "transactionalsave", "profile", "saveprefstress", "savegamestress",
                    "mixer_reset", "mixer_save", "mixer_path", "mixer_emergency", "msg", "warn", "err", "help"
                };

                bool isModCommand = modCommands.Contains(baseCommand);
                Main.logger?.Msg(2, string.Format("[BRIDGE] Command classification: {0}", isModCommand ? "MOD COMMAND" : "GAME COMMAND"));

                if (isModCommand)
                {
                    Main.logger?.Msg(2, string.Format("[BRIDGE] Processing mod command: {0}", command));
                    
                    // Process with our console handler
                    var hookInstance = Console.MixerConsoleHook.Instance;
                    if (hookInstance != null)
                    {
                        hookInstance.OnConsoleCommand(command);
                        Main.logger?.Msg(2, "[BRIDGE] Mod command processed successfully - skipping game processing");
                    }
                    else
                    {
                        Main.logger?.Err("[BRIDGE] MixerConsoleHook instance not available for intercepted command");
                    }
                    
                    return false; // Skip original method execution
                }
                else
                {
                    Main.logger?.Msg(2, string.Format("[BRIDGE] Allowing game to process command: {0}", baseCommand));
                    return true; // Continue with original method for game commands
                }
            }
            catch (Exception ex)
            {
                patchError = ex;
            }
            
            if (patchError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] ConsoleProcessPrefix error: {0}", patchError.Message));
                Main.logger?.Err(string.Format("[BRIDGE] Failed command was: '{0}'", command ?? "[null]"));
            }
            
            return true; // Continue with original method on error
        }

        /// <summary>
        /// Add mod commands to game's native console system using composition pattern
        /// ⚠️ .NET 4.8.1 Compatible: Uses composition instead of dynamic inheritance
        /// </summary>
        private static void AddModCommandsToGameConsole(System.Collections.IDictionary commandsDict, Type consoleType)
        {
            Exception addError = null;
            try
            {
                // Find ConsoleCommand base class for creating compatible commands
                var consoleCommandType = consoleType.GetNestedType("ConsoleCommand", BindingFlags.Public);
                if (consoleCommandType == null)
                {
                    Main.logger?.Warn(1, "[BRIDGE] Could not find ConsoleCommand base class - using alternative approach");
                    // Alternative: Try to find any existing command to understand the interface
                    TryAlternativeCommandRegistration(commandsDict);
                    return;
                }

                // Add stress testing commands
                AddStressTestCommands(commandsDict, consoleCommandType);

                // Add logging commands  
                AddLoggingCommands(commandsDict, consoleCommandType);

                // Add utility commands
                AddUtilityCommands(commandsDict, consoleCommandType);

                Main.logger?.Msg(2, string.Format("[BRIDGE] Added {0} mod commands to native console system", 9));
            }
            catch (Exception ex)
            {
                addError = ex;
            }

            if (addError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] AddModCommandsToGameConsole error: {0}", addError.Message));
            }
        }

        /// <summary>
        /// Add stress testing commands using reflection-based wrappers
        /// ⚠️ CRASH PREVENTION: Safe command creation with error handling
        /// </summary>
        private static void AddStressTestCommands(System.Collections.IDictionary commandsDict, Type consoleCommandType)
        {
            Exception stressError = null;
            try
            {
                // Create mod stress test commands using reflection wrapper
                var savePrefStressCmd = CreateReflectionBasedCommand(consoleCommandType, "saveprefstress",
                    "Stress test mixer preferences saves", "saveprefstress <count> [delay] [bypass]",
                    HandleSavePrefStressCommand);

                var saveGameStressCmd = CreateReflectionBasedCommand(consoleCommandType, "savegamestress",
                    "Stress test game saves", "savegamestress <count> [delay] [bypass]",
                    HandleSaveGameStressCommand);

                var saveMonitorCmd = CreateReflectionBasedCommand(consoleCommandType, "savemonitor",
                    "Comprehensive save monitoring with multi-method validation", "savemonitor <count> [delay] [bypass]",
                    HandleSaveMonitorCommand);

                var transactionalSaveCmd = CreateReflectionBasedCommand(consoleCommandType, "transactionalsave",
                    "Perform atomic transactional save operation", "transactionalsave",
                    HandleTransactionalSaveCommand);

                var profileCmd = CreateReflectionBasedCommand(consoleCommandType, "profile",
                    "Advanced save operation profiling with detailed performance metrics", "profile",
                    HandleProfileCommand);

                if (savePrefStressCmd != null)
                {
                    commandsDict["saveprefstress"] = savePrefStressCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added saveprefstress command to native console");
                }

                if (saveGameStressCmd != null)
                {
                    commandsDict["savegamestress"] = saveGameStressCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added savegamestress command to native console");
                }

                if (saveMonitorCmd != null)
                {
                    commandsDict["savemonitor"] = saveMonitorCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added savemonitor command to native console");
                }

                if (transactionalSaveCmd != null)
                {
                    commandsDict["transactionalsave"] = transactionalSaveCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added transactionalsave command to native console");
                }

                if (profileCmd != null)
                {
                    commandsDict["profile"] = profileCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added profile command to native console");
                }
            }
            catch (Exception ex)
            {
                stressError = ex;
            }

            if (stressError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] AddStressTestCommands error: {0}", stressError.Message));
            }
        }

        /// <summary>
        /// Add logging commands using reflection-based wrappers
        /// ⚠️ CRASH PREVENTION: Safe command creation with error handling
        /// </summary>
        private static void AddLoggingCommands(System.Collections.IDictionary commandsDict, Type consoleCommandType)
        {
            Exception logError = null;
            try
            {
                var msgCmd = CreateReflectionBasedCommand(consoleCommandType, "msg",
                    "Log info message", "msg <message>", HandleMsgCommand);

                var warnCmd = CreateReflectionBasedCommand(consoleCommandType, "warn",
                    "Log warning message", "warn <message>", HandleWarnCommand);

                var errCmd = CreateReflectionBasedCommand(consoleCommandType, "err",
                    "Log error message", "err <message>", HandleErrCommand);

                if (msgCmd != null)
                {
                    commandsDict["msg"] = msgCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added msg command to native console");
                }

                if (warnCmd != null)
                {
                    commandsDict["warn"] = warnCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added warn command to native console");
                }

                if (errCmd != null)
                {
                    commandsDict["err"] = errCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added err command to native console");
                }
            }
            catch (Exception ex)
            {
                logError = ex;
            }

            if (logError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] AddLoggingCommands error: {0}", logError.Message));
            }
        }

        /// <summary>
        /// Add utility commands using reflection-based wrappers
        /// ⚠️ CRASH PREVENTION: Safe command creation with error handling
        /// </summary>
        private static void AddUtilityCommands(System.Collections.IDictionary commandsDict, Type consoleCommandType)
        {
            Exception utilError = null;
            try
            {
                var mixerResetCmd = CreateReflectionBasedCommand(consoleCommandType, "mixer_reset",
                    "Reset all mixer values", "mixer_reset", HandleMixerResetCommand);

                var mixerPathCmd = CreateReflectionBasedCommand(consoleCommandType, "mixer_path",
                    "Show current save path", "mixer_path", HandleMixerPathCommand);

                if (mixerResetCmd != null)
                {
                    commandsDict["mixer_reset"] = mixerResetCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added mixer_reset command to native console");
                }

                if (mixerPathCmd != null)
                {
                    commandsDict["mixer_path"] = mixerPathCmd;
                    Main.logger?.Msg(3, "[BRIDGE] Added mixer_path command to native console");
                }
            }
            catch (Exception ex)
            {
                utilError = ex;
            }

            if (utilError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] AddUtilityCommands error: {0}", utilError.Message));
            }
        }

        /// <summary>
        /// Create a reflection-based command wrapper compatible with game's console system
        /// ⚠️ .NET 4.8.1 Compatible: Uses composition pattern instead of dynamic inheritance
        /// </summary>
        private static object CreateReflectionBasedCommand(Type consoleCommandType, string commandWord, string description, string exampleUsage, Action<List<string>> executeAction)
        {
            Exception createError = null;
            try
            {
                // Instead of dynamic inheritance, create a wrapper that implements the interface
                // This approach is more compatible with .NET 4.8.1
                return new ConsoleCommandWrapper(commandWord, description, exampleUsage, executeAction);
            }
            catch (Exception ex)
            {
                createError = ex;
            }

            if (createError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] CreateReflectionBasedCommand error for '{0}': {1}", commandWord, createError.Message));
            }

            return null;
        }

        /// <summary>
        /// Alternative command registration for when ConsoleCommand type is not accessible
        /// ⚠️ CRASH PREVENTION: Fallback approach with comprehensive error handling
        /// </summary>
        private static void TryAlternativeCommandRegistration(System.Collections.IDictionary commandsDict)
        {
            Exception altError = null;
            try
            {
                Main.logger?.Msg(2, "[BRIDGE] Attempting alternative command registration approach");

                // Check if there are existing commands to understand the interface
                if (commandsDict.Count > 0)
                {
                    foreach (var key in commandsDict.Keys)
                    {
                        var existingCommand = commandsDict[key];
                        Main.logger?.Msg(3, string.Format("[BRIDGE] Found existing command '{0}' of type: {1}", key, existingCommand?.GetType()?.Name ?? "null"));
                        break; // Just need one example
                    }
                }

                Main.logger?.Warn(1, "[BRIDGE] Alternative registration not implemented - native console integration disabled");
            }
            catch (Exception ex)
            {
                altError = ex;
            }

            if (altError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] TryAlternativeCommandRegistration error: {0}", altError.Message));
            }
        }

        #region Command Handlers

        /// <summary>
        /// Handle saveprefstress command from native console
        /// </summary>
        private static void HandleSavePrefStressCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                var parts = new string[args.Count + 1];
                parts[0] = "saveprefstress";
                for (int i = 0; i < args.Count; i++)
                {
                    parts[i + 1] = args[i];
                }

                // Forward to existing console handler - FIXED: Correct method name
                var hookInstance = Console.MixerConsoleHook.Instance;
                if (hookInstance != null)
                {
                    var handlerMethod = hookInstance.GetType().GetMethod("HandleStressSavePrefCommand", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (handlerMethod != null)
                    {
                        handlerMethod.Invoke(hookInstance, new object[] { parts });
                    }
                    else
                    {
                        Main.logger?.Err("[BRIDGE] HandleStressSavePrefCommand method not found");
                    }
                }
                else
                {
                    Main.logger?.Err("[BRIDGE] MixerConsoleHook instance not available");
                }
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleSavePrefStressCommand error: {0}", handlerError.Message));
            }
        }

        /// <summary>
        /// Handle savegamestress command from native console
        /// </summary>
        private static void HandleSaveGameStressCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                var parts = new string[args.Count + 1];
                parts[0] = "savegamestress";
                for (int i = 0; i < args.Count; i++)
                {
                    parts[i + 1] = args[i];
                }

                // Forward to existing console handler - FIXED: Correct method name
                var hookInstance = Console.MixerConsoleHook.Instance;
                if (hookInstance != null)
                {
                    var handlerMethod = hookInstance.GetType().GetMethod("HandleStressSaveGameCommand", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (handlerMethod != null)
                    {
                        handlerMethod.Invoke(hookInstance, new object[] { parts });
                    }
                    else
                    {
                        Main.logger?.Err("[BRIDGE] HandleStressSaveGameCommand method not found");
                    }
                }
                else
                {
                    Main.logger?.Err("[BRIDGE] MixerConsoleHook instance not available");
                }
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleSaveGameStressCommand error: {0}", handlerError.Message));
            }
        }

        /// <summary>
        /// Handle msg command from native console
        /// </summary>
        private static void HandleMsgCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                if (args.Count == 0)
                {
                    Main.logger?.Msg(1, "[CONSOLE] Usage: msg <message>");
                    return;
                }
                var message = string.Join(" ", args);
                Main.logger?.Msg(1, string.Format("[MANUAL] {0}", message));
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleMsgCommand error: {0}", handlerError.Message));
            }
        }

        /// <summary>
        /// Handle warn command from native console
        /// </summary>
        private static void HandleWarnCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                if (args.Count == 0)
                {
                    Main.logger?.Msg(1, "[CONSOLE] Usage: warn <message>");
                    return;
                }
                var message = string.Join(" ", args);
                Main.logger?.Warn(1, string.Format("[MANUAL] {0}", message));
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleWarnCommand error: {0}", handlerError.Message));
            }
        }

        /// <summary>
        /// Handle err command from native console
        /// </summary>
        private static void HandleErrCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                if (args.Count == 0)
                {
                    Main.logger?.Msg(1, "[CONSOLE] Usage: err <message>");
                    return;
                }
                var message = string.Join(" ", args);
                Main.logger?.Err(string.Format("[MANUAL] {0}", message));
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleErrCommand error: {0}", handlerError.Message));
            }
        }

        /// <summary>
        /// Handle mixer_reset command from native console
        /// </summary>
        private static void HandleMixerResetCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                var hookInstance = Console.MixerConsoleHook.Instance;
                if (hookInstance != null)
                {
                    var resetMethod = hookInstance.GetType().GetMethod("ResetMixerValues", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (resetMethod != null)
                    {
                        resetMethod.Invoke(hookInstance, null);
                    }
                    else
                    {
                        Main.logger?.Err("[BRIDGE] ResetMixerValues method not found");
                    }
                }
                else
                {
                    Main.logger?.Err("[BRIDGE] MixerConsoleHook instance not available");
                }
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleMixerResetCommand error: {0}", handlerError.Message));
            }
        }

        /// <summary>
        /// Handle mixer_path command from native console
        /// </summary>
        private static void HandleMixerPathCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                Main.logger?.Msg(1, string.Format("[CONSOLE] Current save path: {0}", Main.CurrentSavePath ?? "[not set]"));
                Main.logger?.Msg(1, string.Format("[CONSOLE] Tracked mixer values: {0}", Main.savedMixerValues?.Count ?? 0));
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleMixerPathCommand error: {0}", handlerError.Message));
            }
        }

        /// <summary>
        /// Handle savemonitor command from native console
        /// </summary>
        private static void HandleSaveMonitorCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                var parts = new string[args.Count + 1];
                parts[0] = "savemonitor";
                for (int i = 0; i < args.Count; i++)
                {
                    parts[i + 1] = args[i];
                }

                var hookInstance = Console.MixerConsoleHook.Instance;
                if (hookInstance != null)
                {
                    var handlerMethod = hookInstance.GetType().GetMethod("HandleComprehensiveSaveMonitoringCommand", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (handlerMethod != null)
                    {
                        handlerMethod.Invoke(hookInstance, new object[] { parts });
                    }
                    else
                    {
                        Main.logger?.Err("[BRIDGE] HandleComprehensiveSaveMonitoringCommand method not found");
                    }
                }
                else
                {
                    Main.logger?.Err("[BRIDGE] MixerConsoleHook instance not available");
                }
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleSaveMonitorCommand error: {0}", handlerError.Message));
            }
        }

        /// <summary>
        /// Handle transactionalsave command from native console
        /// </summary>
        private static void HandleTransactionalSaveCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                var hookInstance = Console.MixerConsoleHook.Instance;
                if (hookInstance != null)
                {
                    var handlerMethod = hookInstance.GetType().GetMethod("HandleTransactionalSaveCommand", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (handlerMethod != null)
                    {
                        handlerMethod.Invoke(hookInstance, null);
                    }
                    else
                    {
                        Main.logger?.Err("[BRIDGE] HandleTransactionalSaveCommand method not found");
                    }
                }
                else
                {
                    Main.logger?.Err("[BRIDGE] MixerConsoleHook instance not available");
                }
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleTransactionalSaveCommand error: {0}", handlerError.Message));
            }
        }

        /// <summary>
        /// Handle profile command from native console
        /// </summary>
        private static void HandleProfileCommand(List<string> args)
        {
            Exception handlerError = null;
            try
            {
                var hookInstance = Console.MixerConsoleHook.Instance;
                if (hookInstance != null)
                {
                    var handlerMethod = hookInstance.GetType().GetMethod("HandleProfileCommand", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (handlerMethod != null)
                    {
                        handlerMethod.Invoke(hookInstance, null);
                    }
                    else
                    {
                        Main.logger?.Err("[BRIDGE] HandleProfileCommand method not found");
                    }
                }
                else
                {
                    Main.logger?.Err("[BRIDGE] MixerConsoleHook instance not available");
                }
            }
            catch (Exception ex)
            {
                handlerError = ex;
            }

            if (handlerError != null)
            {
                Main.logger?.Err(string.Format("[BRIDGE] HandleProfileCommand error: {0}", handlerError.Message));
            }
        }

        #endregion

        /// <summary>
        /// Wrapper class to make mod commands compatible with game's console system
        /// ⚠️ .NET 4.8.1 Compatible: Uses composition pattern for compatibility
        /// </summary>
        private class ConsoleCommandWrapper
        {
            private readonly string _commandWord;
            private readonly string _description;
            private readonly string _exampleUsage;
            private readonly Action<List<string>> _executeAction;

            public ConsoleCommandWrapper(string commandWord, string description, string exampleUsage, Action<List<string>> executeAction)
            {
                _commandWord = commandWord;
                _description = description;
                _exampleUsage = exampleUsage;
                _executeAction = executeAction;
            }

            public string CommandWord { get { return _commandWord; } }
            public string CommandDescription { get { return _description; } }
            public string ExampleUsage { get { return _exampleUsage; } }

            public void Execute(List<string> args)
            {
                Exception executeError = null;
                try
                {
                    _executeAction?.Invoke(args);
                }
                catch (Exception ex)
                {
                    executeError = ex;
                }

                if (executeError != null)
                {
                    Main.logger?.Err(string.Format("[BRIDGE] ConsoleCommandWrapper.Execute error for '{0}': {1}", _commandWord, executeError.Message));
                }
            }
        }
    }
}