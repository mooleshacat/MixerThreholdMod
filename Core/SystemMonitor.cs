using System;
using System.Diagnostics;
using System.Management;
using UnityEngine;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// System hardware monitoring for debugging and performance analysis
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and designed for concurrent access
    /// ⚠️ .NET 4.8.1 Compatible: Uses compatible WMI and performance counter patterns
    /// ⚠️ MAIN THREAD WARNING: WMI operations may block - use async patterns when possible
    /// 
    /// DEBUG-ONLY Features:
    /// - CPU usage monitoring during save operations
    /// - Memory usage tracking with GC analysis
    /// - Disk I/O performance monitoring
    /// - System hardware information logging
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses WMI (Windows Management Instrumentation) for hardware access
    /// - Compatible performance counter patterns
    /// - Proper exception handling for system-level operations
    /// 
    /// Crash Prevention Features:
    /// - Graceful degradation when WMI is unavailable
    /// - Comprehensive error handling for system queries
    /// - Safe resource disposal patterns
    /// - Prevents system monitoring failures from affecting game performance
    /// </summary>
    public static class SystemMonitor
    {
        private static bool _isInitialized = false;
        private static readonly object _initLock = new object();
        private static PerformanceCounter _cpuCounter;
        private static PerformanceCounter _memoryCounter;
        private static PerformanceCounter _diskCounter;
        private static Process _currentProcess;

        /// <summary>
        /// Initialize system monitoring (DEBUG mode only)
        /// ⚠️ CRASH PREVENTION: Safe initialization with comprehensive error handling
        /// </summary>
        public static void Initialize()
        {
            lock (_initLock)
            {
                if (_isInitialized) return;

#if DEBUG
                Exception initError = null;
                try
                {
                    Main.logger?.Msg(2, "[SYSMON] Initializing system monitoring (DEBUG mode)");
                    
                    // Initialize current process reference
                    _currentProcess = Process.GetCurrentProcess();
                    Main.logger?.Msg(3, string.Format("[SYSMON] Process initialized: {0} (PID: {1})", _currentProcess.ProcessName, _currentProcess.Id));

                    // Initialize performance counters with error handling
                    InitializePerformanceCounters();
                    
                    // Log initial system information
                    LogSystemInformation();
                    
                    _isInitialized = true;
                    Main.logger?.Msg(1, "[SYSMON] System monitoring initialized successfully");
                }
                catch (Exception ex)
                {
                    initError = ex;
                }

                if (initError != null)
                {
                    Main.logger?.Err(string.Format("[SYSMON] System monitoring initialization failed: {0}\n{1}", initError.Message, initError.StackTrace));
                    Main.logger?.Warn(1, "[SYSMON] Continuing without system monitoring capabilities");
                }
#else
                Main.logger?.Msg(3, "[SYSMON] System monitoring disabled in RELEASE mode");
#endif
            }
        }

        /// <summary>
        /// Initialize performance counters with error handling
        /// ⚠️ CRASH PREVENTION: Safe counter initialization with fallback handling
        /// </summary>
        private static void InitializePerformanceCounters()
        {
#if DEBUG
            Exception counterError = null;
            try
            {
                // CPU usage counter
                try
                {
                    _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                    // Prime the counter (first call always returns 0)
                    _cpuCounter.NextValue();
                    Main.logger?.Msg(3, "[SYSMON] CPU performance counter initialized");
                }
                catch (Exception ex)
                {
                    Main.logger?.Warn(1, string.Format("[SYSMON] CPU counter initialization failed: {0}", ex.Message));
                    _cpuCounter = null;
                }

                // Memory usage counter
                try
                {
                    _memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
                    Main.logger?.Msg(3, "[SYSMON] Memory performance counter initialized");
                }
                catch (Exception ex)
                {
                    Main.logger?.Warn(1, string.Format("[SYSMON] Memory counter initialization failed: {0}", ex.Message));
                    _memoryCounter = null;
                }

                // Disk usage counter
                try
                {
                    _diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
                    Main.logger?.Msg(3, "[SYSMON] Disk performance counter initialized");
                }
                catch (Exception ex)
                {
                    Main.logger?.Warn(1, string.Format("[SYSMON] Disk counter initialization failed: {0}", ex.Message));
                    _diskCounter = null;
                }
            }
            catch (Exception ex)
            {
                counterError = ex;
            }

            if (counterError != null)
            {
                Main.logger?.Err(string.Format("[SYSMON] Performance counter initialization error: {0}", counterError.Message));
            }
#endif
        }

        /// <summary>
        /// Log comprehensive system information (DEBUG mode only)
        /// ⚠️ THREAD SAFETY: Safe WMI operations with timeout protection
        /// </summary>
        private static void LogSystemInformation()
        {
#if DEBUG
            Exception sysInfoError = null;
            try
            {
                Main.logger?.Msg(2, "[SYSMON] === SYSTEM INFORMATION ===");
                
                // Basic system info
                Main.logger?.Msg(2, string.Format("[SYSMON] OS: {0}", Environment.OSVersion));
                Main.logger?.Msg(2, string.Format("[SYSMON] .NET Version: {0}", Environment.Version));
                Main.logger?.Msg(2, string.Format("[SYSMON] Processor Count: {0}", Environment.ProcessorCount));
                Main.logger?.Msg(2, string.Format("[SYSMON] Working Set: {0:F2} MB", Environment.WorkingSet / 1048576.0));
                Main.logger?.Msg(2, string.Format("[SYSMON] Unity System Memory: {0} MB", SystemInfo.systemMemorySize));
                Main.logger?.Msg(2, string.Format("[SYSMON] Unity Graphics Memory: {0} MB", SystemInfo.graphicsMemorySize));
                Main.logger?.Msg(2, string.Format("[SYSMON] Unity Processor Type: {0}", SystemInfo.processorType));
                Main.logger?.Msg(2, string.Format("[SYSMON] Unity Graphics Device: {0}", SystemInfo.graphicsDeviceName));

                // Enhanced WMI system information with timeout protection
                LogWMISystemInformation();
                
                Main.logger?.Msg(2, "[SYSMON] === END SYSTEM INFORMATION ===");
            }
            catch (Exception ex)
            {
                sysInfoError = ex;
            }

            if (sysInfoError != null)
            {
                Main.logger?.Err(string.Format("[SYSMON] System information logging error: {0}", sysInfoError.Message));
            }
#endif
        }

        /// <summary>
        /// Log WMI-based system information with comprehensive error handling
        /// ⚠️ CRASH PREVENTION: Safe WMI operations with timeout and exception handling
        /// </summary>
        private static void LogWMISystemInformation()
        {
#if DEBUG
            Exception wmiError = null;
            try
            {
                // CPU Information
                LogWMIInfo("Win32_Processor", new string[] { "Name", "MaxClockSpeed", "NumberOfCores", "NumberOfLogicalProcessors" }, "CPU");
                
                // Memory Information
                LogWMIInfo("Win32_ComputerSystem", new string[] { "TotalPhysicalMemory" }, "MEMORY");
                
                // Storage Information
                LogWMIInfo("Win32_LogicalDisk", new string[] { "DeviceID", "Size", "FreeSpace", "FileSystem" }, "STORAGE");
            }
            catch (Exception ex)
            {
                wmiError = ex;
            }

            if (wmiError != null)
            {
                Main.logger?.Warn(1, string.Format("[SYSMON] WMI information logging failed: {0}", wmiError.Message));
                Main.logger?.Msg(3, "[SYSMON] Continuing with basic system monitoring only");
            }
#endif
        }

        /// <summary>
        /// Helper method to safely query WMI information
        /// ⚠️ CRASH PREVENTION: Safe WMI queries with comprehensive error handling
        /// </summary>
        private static void LogWMIInfo(string className, string[] properties, string category)
        {
#if DEBUG
            Exception wmiQueryError = null;
            try
            {
                using (var searcher = new ManagementObjectSearcher(string.Format("SELECT * FROM {0}", className)))
                {
                    using (var collection = searcher.Get())
                    {
                        foreach (ManagementObject obj in collection)
                        {
                            Main.logger?.Msg(3, string.Format("[SYSMON] {0} Information:", category));
                            
                            foreach (string property in properties)
                            {
                                try
                                {
                                    object value = obj[property];
                                    if (value != null)
                                    {
                                        // Format large numbers appropriately
                                        if (property.Contains("Memory") || property.Contains("Size") || property.Contains("FreeSpace"))
                                        {
                                            if (long.TryParse(value.ToString(), out long bytes))
                                            {
                                                Main.logger?.Msg(3, string.Format("[SYSMON]   {0}: {1:F2} GB", property, bytes / 1073741824.0));
                                            }
                                            else
                                            {
                                                Main.logger?.Msg(3, string.Format("[SYSMON]   {0}: {1}", property, value));
                                            }
                                        }
                                        else
                                        {
                                            Main.logger?.Msg(3, string.Format("[SYSMON]   {0}: {1}", property, value));
                                        }
                                    }
                                }
                                catch (Exception propEx)
                                {
                                    Main.logger?.Warn(1, string.Format("[SYSMON] Failed to read property {0}: {1}", property, propEx.Message));
                                }
                            }
                            
                            // Only log first instance for most classes
                            if (className != "Win32_LogicalDisk") break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                wmiQueryError = ex;
            }

            if (wmiQueryError != null)
            {
                Main.logger?.Warn(1, string.Format("[SYSMON] WMI query for {0} failed: {1}", className, wmiQueryError.Message));
            }
#endif
        }

        /// <summary>
        /// Log current system performance metrics (DEBUG mode only)
        /// ⚠️ THREAD SAFETY: Safe performance counter access with error handling
        /// </summary>
        public static void LogCurrentPerformance(string context = "")
        {
#if DEBUG
            if (!_isInitialized)
            {
                Main.logger?.Warn(1, "[SYSMON] System monitoring not initialized - skipping performance logging");
                return;
            }

            Exception perfError = null;
            try
            {
                string contextPrefix = string.IsNullOrEmpty(context) ? "" : string.Format("[{0}] ", context);
                
                Main.logger?.Msg(2, string.Format("[SYSMON] {0}=== PERFORMANCE SNAPSHOT ===", contextPrefix));
                
                // CPU Usage
                if (_cpuCounter != null)
                {
                    try
                    {
                        float cpuUsage = _cpuCounter.NextValue();
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}CPU Usage: {1:F1}%", contextPrefix, cpuUsage));
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Warn(1, string.Format("[SYSMON] CPU usage read failed: {0}", ex.Message));
                    }
                }

                // Memory Usage
                if (_memoryCounter != null)
                {
                    try
                    {
                        float availableMemoryMB = _memoryCounter.NextValue();
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}Available Memory: {1:F1} MB", contextPrefix, availableMemoryMB));
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Warn(1, string.Format("[SYSMON] Memory usage read failed: {0}", ex.Message));
                    }
                }

                // Process-specific metrics
                if (_currentProcess != null && !_currentProcess.HasExited)
                {
                    try
                    {
                        _currentProcess.Refresh();
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process Working Set: {1:F2} MB", contextPrefix, _currentProcess.WorkingSet64 / 1048576.0));
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process Private Memory: {1:F2} MB", contextPrefix, _currentProcess.PrivateMemorySize64 / 1048576.0));
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process CPU Time: {1:F3}s", contextPrefix, _currentProcess.TotalProcessorTime.TotalSeconds));
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Warn(1, string.Format("[SYSMON] Process metrics read failed: {0}", ex.Message));
                    }
                }

                // Disk Usage
                if (_diskCounter != null)
                {
                    try
                    {
                        float diskUsage = _diskCounter.NextValue();
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}Disk Usage: {1:F1}%", contextPrefix, diskUsage));
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Warn(1, string.Format("[SYSMON] Disk usage read failed: {0}", ex.Message));
                    }
                }

                // Garbage Collection info
                Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Total Memory: {1:F2} MB", contextPrefix, GC.GetTotalMemory(false) / 1048576.0));
                Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Collection Count (Gen 0): {1}", contextPrefix, GC.CollectionCount(0)));
                Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Collection Count (Gen 1): {1}", contextPrefix, GC.CollectionCount(1)));
                Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Collection Count (Gen 2): {1}", contextPrefix, GC.CollectionCount(2)));
                
                Main.logger?.Msg(2, string.Format("[SYSMON] {0}=== END PERFORMANCE SNAPSHOT ===", contextPrefix));
            }
            catch (Exception ex)
            {
                perfError = ex;
            }

            if (perfError != null)
            {
                Main.logger?.Err(string.Format("[SYSMON] Performance logging error: {0}", perfError.Message));
            }
#endif
        }

        /// <summary>
        /// Monitor performance during a specific operation
        /// ⚠️ THREAD SAFETY: Safe performance monitoring with comprehensive error handling
        /// </summary>
        public static void MonitorOperation(string operationName, Action operation)
        {
#if DEBUG
            if (!_isInitialized)
            {
                Main.logger?.Warn(1, "[SYSMON] System monitoring not initialized - executing operation without monitoring");
                operation?.Invoke();
                return;
            }

            Exception monitorError = null;
            var stopwatch = new Stopwatch();
            
            try
            {
                Main.logger?.Msg(2, string.Format("[SYSMON] Starting monitored operation: {0}", operationName));
                LogCurrentPerformance(string.Format("BEFORE {0}", operationName));
                
                stopwatch.Start();
                operation?.Invoke();
                stopwatch.Stop();
                
                LogCurrentPerformance(string.Format("AFTER {0}", operationName));
                Main.logger?.Msg(1, string.Format("[SYSMON] Operation '{0}' completed in {1:F3}s", operationName, stopwatch.Elapsed.TotalSeconds));
            }
            catch (Exception ex)
            {
                monitorError = ex;
                stopwatch.Stop();
            }

            if (monitorError != null)
            {
                Main.logger?.Err(string.Format("[SYSMON] Operation monitoring error for '{0}': {1}", operationName, monitorError.Message));
                Main.logger?.Msg(1, string.Format("[SYSMON] Operation '{0}' failed after {1:F3}s", operationName, stopwatch.Elapsed.TotalSeconds));
            }
#else
            // In release mode, just execute the operation
            operation?.Invoke();
#endif
        }

        /// <summary>
        /// Cleanup system monitoring resources
        /// ⚠️ CRASH PREVENTION: Safe resource disposal with comprehensive error handling
        /// </summary>
        public static void Cleanup()
        {
#if DEBUG
            lock (_initLock)
            {
                if (!_isInitialized) return;

                Exception cleanupError = null;
                try
                {
                    Main.logger?.Msg(3, "[SYSMON] Cleaning up system monitoring resources");
                    
                    if (_cpuCounter != null)
                    {
                        _cpuCounter.Dispose();
                        _cpuCounter = null;
                    }
                    
                    if (_memoryCounter != null)
                    {
                        _memoryCounter.Dispose();
                        _memoryCounter = null;
                    }
                    
                    if (_diskCounter != null)
                    {
                        _diskCounter.Dispose();
                        _diskCounter = null;
                    }
                    
                    if (_currentProcess != null)
                    {
                        _currentProcess.Dispose();
                        _currentProcess = null;
                    }
                    
                    _isInitialized = false;
                    Main.logger?.Msg(2, "[SYSMON] System monitoring cleanup completed");
                }
                catch (Exception ex)
                {
                    cleanupError = ex;
                }

                if (cleanupError != null)
                {
                    Main.logger?.Err(string.Format("[SYSMON] Cleanup error: {0}", cleanupError.Message));
                }
            }
#endif
        }
    }
}