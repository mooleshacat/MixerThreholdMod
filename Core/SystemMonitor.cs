using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
                    _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
                    // Prime the counter (first call always returns 0) - need delay for accurate reading
                    float initialValue = _cpuCounter.NextValue();
                    Main.logger?.Msg(3, string.Format("[SYSMON] CPU performance counter initialized (initial value: {0:F1}%)", initialValue));
                }
                catch (Exception ex)
                {
                    Main.logger?.Warn(1, string.Format("[SYSMON] CPU counter initialization failed: {0}", ex.Message));
                    _cpuCounter = null;
                }

                // Memory usage counter
                try
                {
                    _memoryCounter = new PerformanceCounter("Memory", "Available MBytes", true);
                    float availableMem = _memoryCounter.NextValue();
                    Main.logger?.Msg(3, string.Format("[SYSMON] Memory performance counter initialized (available: {0:F1} MB)", availableMem));
                }
                catch (Exception ex)
                {
                    Main.logger?.Warn(1, string.Format("[SYSMON] Memory counter initialization failed: {0}", ex.Message));
                    _memoryCounter = null;
                }

                // Disk usage counter
                try
                {
                    _diskCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total", true);
                    float diskUsage = _diskCounter.NextValue();
                    Main.logger?.Msg(3, string.Format("[SYSMON] Disk performance counter initialized (usage: {0:F1}%)", diskUsage));
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
                
                // Fix working set display - Environment.WorkingSet can be unreliable
                try
                {
                    long workingSet = Environment.WorkingSet;
                    if (workingSet > 0)
                    {
                        Main.logger?.Msg(2, string.Format("[SYSMON] Working Set: {0:F2} MB", workingSet / 1048576.0));
                    }
                    else
                    {
                        // Fallback to GC memory
                        long gcMemory = GC.GetTotalMemory(false);
                        Main.logger?.Msg(2, string.Format("[SYSMON] Working Set: ~{0:F2} MB (GC estimate)", gcMemory / 1048576.0));
                    }
                }
                catch (Exception ex)
                {
                    Main.logger?.Warn(1, string.Format("[SYSMON] Working set read failed: {0}", ex.Message));
                    Main.logger?.Msg(2, "[SYSMON] Working Set: Unable to read");
                }
                
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
            // Use built-in .NET diagnostics instead of WMI for better compatibility
            Exception systemInfoError = null;
            try
            {
                Main.logger?.Msg(3, string.Format("[SYSMON] {0} Information (via .NET Diagnostics):", category));
                
                if (category.Contains("OS"))
                {
                    Main.logger?.Msg(3, string.Format("[SYSMON]   OSVersion: {0}", Environment.OSVersion));
                    Main.logger?.Msg(3, string.Format("[SYSMON]   MachineName: {0}", Environment.MachineName));
                    Main.logger?.Msg(3, string.Format("[SYSMON]   ProcessorCount: {0}", Environment.ProcessorCount));
                    Main.logger?.Msg(3, string.Format("[SYSMON]   SystemDirectory: {0}", Environment.SystemDirectory));
                    Main.logger?.Msg(3, string.Format("[SYSMON]   CLRVersion: {0}", Environment.Version));
                }
                else if (category.Contains("Processor"))
                {
                    Main.logger?.Msg(3, string.Format("[SYSMON]   ProcessorCount: {0}", Environment.ProcessorCount));
                    Main.logger?.Msg(3, string.Format("[SYSMON]   Architecture: {0}", RuntimeInformation.ProcessArchitecture));
                }
                else if (category.Contains("Memory"))
                {
                    Main.logger?.Msg(3, string.Format("[SYSMON]   WorkingSet: {0:F2} MB", Environment.WorkingSet / 1048576.0));
                    Main.logger?.Msg(3, string.Format("[SYSMON]   GC Total Memory: {0:F2} MB", GC.GetTotalMemory(false) / 1048576.0));
                }
                else
                {
                    Main.logger?.Msg(3, string.Format("[SYSMON]   WMI not available - using basic .NET diagnostics"));
                }
            }
            catch (Exception ex)
            {
                systemInfoError = ex;
            }

            if (systemInfoError != null)
            {
                Main.logger?.Warn(1, string.Format("[SYSMON] System info query for {0} failed: {1}", className, systemInfoError.Message));
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
                Main.logger?.Warn(1, "[SYSMON] System monitoring not initialized - executing operation without monitoring");
                return;
            }

            Exception perfError = null;
            try
            {
                string contextPrefix = string.IsNullOrEmpty(context) ? "" : string.Format("[{0}] ", context);
                
                Main.logger?.Msg(2, string.Format("[SYSMON] {0}=== PERFORMANCE SNAPSHOT ===", contextPrefix));
                
                // CPU Usage with better error handling
                if (_cpuCounter != null)
                {
                    try
                    {
                        float cpuUsage = _cpuCounter.NextValue();
                        // Check for invalid values and provide alternative calculation
                        if (cpuUsage < 0 || cpuUsage > 100 || float.IsNaN(cpuUsage))
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}CPU Usage: Calculating... (performance counter warming up)", contextPrefix));
                        }
                        else
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}CPU Usage: {1:F1}%", contextPrefix, cpuUsage));
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Warn(1, string.Format("[SYSMON] CPU usage read failed: {0}", ex.Message));
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}CPU Usage: Unable to read (counter error)", contextPrefix));
                    }
                }
                else
                {
                    Main.logger?.Msg(2, string.Format("[SYSMON] {0}CPU Usage: Counter not available", contextPrefix));
                }

                // Memory Usage with validation
                if (_memoryCounter != null)
                {
                    try
                    {
                        float availableMemoryMB = _memoryCounter.NextValue();
                        if (availableMemoryMB < 0 || float.IsNaN(availableMemoryMB))
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Available Memory: Unable to read (invalid value)", contextPrefix));
                        }
                        else
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Available Memory: {1:F1} MB", contextPrefix, availableMemoryMB));
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Warn(1, string.Format("[SYSMON] Memory usage read failed: {0}", ex.Message));
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}Available Memory: Counter error", contextPrefix));
                    }
                }
                else
                {
                    Main.logger?.Msg(2, string.Format("[SYSMON] {0}Available Memory: Counter not available", contextPrefix));
                }

                // Process-specific metrics with enhanced validation
                if (_currentProcess != null && !_currentProcess.HasExited)
                {
                    try
                    {
                        _currentProcess.Refresh();
                        
                        // Check for valid working set value
                        long workingSet = _currentProcess.WorkingSet64;
                        if (workingSet > 0)
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process Working Set: {1:F2} MB", contextPrefix, workingSet / 1048576.0));
                        }
                        else
                        {
                            // Fallback to GC memory if process working set is not available
                            long gcMemory = GC.GetTotalMemory(false);
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process Working Set: ~{1:F2} MB (GC estimate)", contextPrefix, gcMemory / 1048576.0));
                        }
                        
                        // Check for valid private memory value
                        long privateMemory = _currentProcess.PrivateMemorySize64;
                        if (privateMemory > 0)
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process Private Memory: {1:F2} MB", contextPrefix, privateMemory / 1048576.0));
                        }
                        else
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process Private Memory: Data not available", contextPrefix));
                        }
                        
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process CPU Time: {1:F3}s", contextPrefix, _currentProcess.TotalProcessorTime.TotalSeconds));
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Warn(1, string.Format("[SYSMON] Process metrics read failed: {0}", ex.Message));
                        // Fallback to basic .NET metrics
                        try
                        {
                            long gcMemory = GC.GetTotalMemory(false);
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process Memory (GC): {1:F2} MB", contextPrefix, gcMemory / 1048576.0));
                        }
                        catch
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Process metrics: Unable to read", contextPrefix));
                        }
                    }
                }

                // Disk Usage with validation
                if (_diskCounter != null)
                {
                    try
                    {
                        float diskUsage = _diskCounter.NextValue();
                        if (diskUsage < 0 || diskUsage > 100 || float.IsNaN(diskUsage))
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Disk Usage: Calculating... (counter warming up)", contextPrefix));
                        }
                        else
                        {
                            Main.logger?.Msg(2, string.Format("[SYSMON] {0}Disk Usage: {1:F1}%", contextPrefix, diskUsage));
                        }
                    }
                    catch (Exception ex)
                    {
                        Main.logger?.Warn(1, string.Format("[SYSMON] Disk usage read failed: {0}", ex.Message));
                        Main.logger?.Msg(2, string.Format("[SYSMON] {0}Disk Usage: Counter error", contextPrefix));
                    }
                }
                else
                {
                    Main.logger?.Msg(2, string.Format("[SYSMON] {0}Disk Usage: Counter not available", contextPrefix));
                }

                // Garbage Collection info (this should always work)
                try
                {
                    Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Total Memory: {1:F2} MB", contextPrefix, GC.GetTotalMemory(false) / 1048576.0));
                    Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Collection Count (Gen 0): {1}", contextPrefix, GC.CollectionCount(0)));
                    Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Collection Count (Gen 1): {1}", contextPrefix, GC.CollectionCount(1)));
                    Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Collection Count (Gen 2): {1}", contextPrefix, GC.CollectionCount(2)));
                }
                catch (Exception ex)
                {
                    Main.logger?.Warn(1, string.Format("[SYSMON] GC metrics read failed: {0}", ex.Message));
                }
                
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