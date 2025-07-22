using System;
using System.Reflection;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

using UnityEngine;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// IL2CPP COMPATIBLE: Safe type resolution for both MONO and IL2CPP build environments
    ///  THREAD SAFETY: All operations are thread-safe and designed for concurrent access
    ///  .NET 4.8.1 Compatible: Uses compatible reflection patterns and error handling
    ///  IL2CPP COMPATIBLE: Uses string-based type loading safe for AOT compilation
    /// 
    /// Type Loading Strategy:
    /// - MONO builds: Direct type references work normally
    /// - IL2CPP builds: Use Assembly.GetType() with string-based type names
    /// - Graceful degradation when types are not available in either environment
    /// - Comprehensive error handling and logging for debugging type loading issues
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses Assembly.GetType() instead of newer reflection APIs
    /// - Compatible exception handling patterns
    /// - Safe string-based type resolution
    /// </summary>
    public static class IL2CPPTypeResolver
    {
        private static readonly Logger logger = new Logger();
        private static bool _isIL2CPP;
        private static bool _buildEnvironmentDetected = false;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Detect build environment (MONO vs IL2CPP) for appropriate type loading strategy
        /// </summary>
        public static bool IsIL2CPPBuild
        {
            get
            {
                if (!_buildEnvironmentDetected)
                {
                    lock (_lockObject)
                    {
                        if (!_buildEnvironmentDetected)
                        {
                            DetectBuildEnvironment();
                            _buildEnvironmentDetected = true;
                        }
                    }
                }
                return _isIL2CPP;
            }
        }

        private static void DetectBuildEnvironment()
        {
            try
            {
                // IL2CPP builds have specific runtime characteristics
                // Check for IL2CPP-specific assemblies or runtime features
                var unityPlayerType = Type.GetType("UnityEngine.UnityPlayer", false);
                if (unityPlayerType != null)
                {
                    // Check for IL2CPP-specific fields or properties
                    var il2cppField = unityPlayerType.GetField("unityVersion", BindingFlags.Static | BindingFlags.Public);
                    _isIL2CPP = il2cppField != null;
                }

                // Alternative detection: Check assembly location patterns
                if (!_isIL2CPP)
                {
                    var currentAssembly = Assembly.GetExecutingAssembly();
                    var location = currentAssembly.Location;
                    _isIL2CPP = string.IsNullOrEmpty(location) || location.Contains("Il2Cpp");
                }

                logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[TYPE_RESOLVER] Build environment detected: {0}", _isIL2CPP ? IL2CPP_BUILD : MONO_BUILD));
            }
            catch (Exception ex)
            {
                logger.Warn(LOG_LEVEL_CRITICAL, string.Format("[TYPE_RESOLVER] Build detection failed, assuming MONO: {0}", ex.Message));
                _isIL2CPP = false;
            }
        }

        /// <summary>
        /// IL2CPP-safe type resolution for MixingStationConfiguration
        /// </summary>
        public static Type GetMixingStationConfigurationType()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                if (IsIL2CPPBuild)
                {
                    // IL2CPP: Use string-based type loading
                    var assemblyCSharp = Assembly.Load("Assembly-CSharp");
                    if (assemblyCSharp != null)
                    {
                        var type = assemblyCSharp.GetType("ScheduleOne.Management.MixingStationConfiguration");
                        if (type != null)
                        {
                            stopwatch.Stop();
                            AdvancedSystemPerformanceMonitor.LogIL2CPPTypeLoadingPerformance("MixingStationConfiguration (Assembly-CSharp)", stopwatch.Elapsed.TotalMilliseconds, true);
                            logger.Msg(3, "[TYPE_RESOLVER] IL2CPP: MixingStationConfiguration found via Assembly.GetType()");
                            return type;
                        }
                    }

                    // Fallback: Try different assembly loading approaches
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        var type = assembly.GetType("ScheduleOne.Management.MixingStationConfiguration");
                        if (type != null)
                        {
                            stopwatch.Stop();
                            AdvancedSystemPerformanceMonitor.LogIL2CPPTypeLoadingPerformance(string.Format("MixingStationConfiguration ({0})", assembly.FullName), stopwatch.Elapsed.TotalMilliseconds, true);
                            logger.Msg(LOG_LEVEL_VERBOSE, string.Format("[TYPE_RESOLVER] IL2CPP: MixingStationConfiguration found in assembly: {0}", assembly.FullName));
                            return type;
                        }
                    }

                    stopwatch.Stop();
                    AdvancedSystemPerformanceMonitor.LogIL2CPPTypeLoadingPerformance("MixingStationConfiguration (NOT_FOUND)", stopwatch.Elapsed.TotalMilliseconds, false);
                    logger.Warn(1, "[TYPE_RESOLVER] IL2CPP: MixingStationConfiguration not found via string-based loading");
                    return null;
                }
                else
                {
                    // MONO: Direct type reference (should work normally)
                    stopwatch.Stop();
                    AdvancedSystemPerformanceMonitor.LogIL2CPPTypeLoadingPerformance("MixingStationConfiguration (MONO_DIRECT)", stopwatch.Elapsed.TotalMilliseconds, true);
                    logger.Msg(3, "[TYPE_RESOLVER] MONO: Using direct type reference for MixingStationConfiguration");
                    return typeof(ScheduleOne.Management.MixingStationConfiguration);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                AdvancedSystemPerformanceMonitor.LogIL2CPPTypeLoadingPerformance("MixingStationConfiguration (EXCEPTION)", stopwatch.Elapsed.TotalMilliseconds, false);
                logger.Err(string.Format("[TYPE_RESOLVER] Failed to resolve MixingStationConfiguration: {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// IL2CPP-safe type resolution for SaveManager
        /// </summary>
        public static Type GetSaveManagerType()
        {
            try
            {
                if (IsIL2CPPBuild)
                {
                    // IL2CPP: Use string-based type loading
                    var assemblyCSharp = Assembly.Load("Assembly-CSharp");
                    if (assemblyCSharp != null)
                    {
                        var type = assemblyCSharp.GetType("ScheduleOne.Persistence.SaveManager");
                        if (type != null)
                        {
                            logger.Msg(3, "[TYPE_RESOLVER] IL2CPP: SaveManager found via Assembly.GetType()");
                            return type;
                        }
                    }

                    // Fallback: Try different assembly loading approaches
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        var type = assembly.GetType("ScheduleOne.Persistence.SaveManager");
                        if (type != null)
                        {
                            logger.Msg(3, string.Format("[TYPE_RESOLVER] IL2CPP: SaveManager found in assembly: {0}", assembly.FullName));
                            return type;
                        }
                    }

                    logger.Warn(1, "[TYPE_RESOLVER] IL2CPP: SaveManager not found via string-based loading");
                    return null;
                }
                else
                {
                    // MONO: Direct type reference (should work normally)
                    logger.Msg(3, "[TYPE_RESOLVER] MONO: Using direct type reference for SaveManager");
                    return typeof(ScheduleOne.Persistence.SaveManager);
                }
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("[TYPE_RESOLVER] Failed to resolve SaveManager: {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Get constructor for MixingStationConfiguration with proper IL2CPP handling
        /// </summary>
        public static ConstructorInfo GetMixingStationConfigurationConstructor()
        {
            try
            {
                var mixingStationType = GetMixingStationConfigurationType();
                if (mixingStationType == null)
                {
                    logger.Warn(1, "[TYPE_RESOLVER] Cannot get constructor - MixingStationConfiguration type not found");
                    return null;
                }

                var constructor = mixingStationType.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    new[] {
                        GetTypeByName("ScheduleOne.Management.ConfigurationReplicator"),
                        GetTypeByName("ScheduleOne.Management.IConfigurable"),
                        GetTypeByName("ScheduleOne.ObjectScripts.MixingStation")
                    },
                    null
                );

                if (constructor != null)
                {
                    logger.Msg(3, "[TYPE_RESOLVER] MixingStationConfiguration constructor found successfully");
                }
                else
                {
                    logger.Warn(1, "[TYPE_RESOLVER] MixingStationConfiguration constructor not found");
                }

                return constructor;
            }
            catch (Exception ex)
            {
                logger.Err(string.Format("[TYPE_RESOLVER] Failed to get MixingStationConfiguration constructor: {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Generic type resolution by name with IL2CPP compatibility
        /// </summary>
        public static Type GetTypeByName(string typeName)
        {
            try
            {
                if (IsIL2CPPBuild)
                {
                    // IL2CPP: String-based loading
                    var assemblyCSharp = Assembly.Load("Assembly-CSharp");
                    if (assemblyCSharp != null)
                    {
                        var type = assemblyCSharp.GetType(typeName);
                        if (type != null)
                        {
                            return type;
                        }
                    }

                    // Fallback search
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        var type = assembly.GetType(typeName);
                        if (type != null)
                        {
                            return type;
                        }
                    }
                    return null;
                }
                else
                {
                    // MONO: Standard type loading
                    return Type.GetType(typeName, false);
                }
            }
            catch (Exception ex)
            {
                logger.Warn(3, string.Format("[TYPE_RESOLVER] Failed to resolve type {0}: {1}", typeName, ex.Message));
                return null;
            }
        }

        /// <summary>
        /// Safe type check that works in both MONO and IL2CPP
        /// </summary>
        public static bool IsTypeAvailable(string typeName)
        {
            return GetTypeByName(typeName) != null;
        }

        /// <summary>
        /// Log comprehensive type availability information for debugging
        /// </summary>
        public static void LogTypeAvailability()
        {
            logger.Msg(1, "[TYPE_RESOLVER] === TYPE AVAILABILITY REPORT ===");
            logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[TYPE_RESOLVER] Build Environment: {0}", IsIL2CPPBuild ? IL2CPP_BUILD : MONO_BUILD));
            
            var mixingStationType = GetMixingStationConfigurationType();
            logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[TYPE_RESOLVER] MixingStationConfiguration: {0}", mixingStationType != null ? AVAILABLE_STATUS : "NOT FOUND"));
            
            var saveManagerType = GetSaveManagerType();
            logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[TYPE_RESOLVER] SaveManager: {0}", saveManagerType != null ? AVAILABLE_STATUS : "NOT FOUND"));
            
            var constructor = GetMixingStationConfigurationConstructor();
            logger.Msg(LOG_LEVEL_CRITICAL, string.Format("[TYPE_RESOLVER] MixingStationConfiguration Constructor: {0}", constructor != null ? AVAILABLE_STATUS : "NOT FOUND"));

            // List available assemblies for debugging
            logger.Msg(2, "[TYPE_RESOLVER] Available Assemblies:");
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                logger.Msg(3, string.Format("[TYPE_RESOLVER]   - {0}", assembly.FullName));
            }
        }
    }
}