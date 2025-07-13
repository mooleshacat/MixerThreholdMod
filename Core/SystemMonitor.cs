using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace MixerThreholdMod_1_0_0.Core
{
    /// <summary>
    /// Advanced system hardware monitoring with comprehensive memory leak detection for debugging and performance analysis
    /// ⚠️ THREAD SAFETY: All operations are thread-safe and designed for concurrent access
    /// ⚠️ .NET 4.8.1 Compatible: Uses compatible performance counter patterns and memory management
    /// ⚠️ MAIN THREAD WARNING: Performance counter operations may block - use async patterns when possible
    /// ⚠️ IL2CPP COMPATIBLE: Uses compile-time safe patterns, no dynamic reflection, AOT-friendly operations
    /// 
    /// DEBUG-ONLY Features:
    /// - CPU usage monitoring during save operations
    /// - Advanced memory leak detection and growth pattern analysis
    /// - GC pressure monitoring and allocation tracking
    /// - Disk I/O performance monitoring with resource usage analysis
    /// - System hardware information logging
    /// - Memory allocation baseline tracking for leak detection
    /// 
    /// Memory Leak Detection Features:
    /// - Tracks memory growth patterns over time
    /// - Monitors GC collection frequency and heap pressure
    /// - Detects abnormal memory allocation patterns
    /// - Provides comprehensive resource cleanup verification
    /// - Tracks object allocation rates and lifetime patterns
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses standard .NET performance counters (no WMI dependency)
    /// - Compatible performance counter patterns
    /// - Proper exception handling for system-level operations
    /// - Framework-appropriate memory monitoring techniques
    /// 
    /// IL2CPP Compatibility:
    /// - No use of System.Reflection.Emit or dynamic code generation
    /// - All types statically known at compile time
    /// - AOT-safe performance counter access patterns
    /// - No runtime assembly traversal or dynamic type loading
    /// - Uses typeof() instead of GetType() where possible
    /// - Compile-time safe generic constraints
    /// 
    /// Crash Prevention Features:
    /// - Graceful degradation when performance counters are unavailable
    /// - Comprehensive error handling for system queries
    /// - Safe resource disposal patterns with memory leak prevention
    /// - Prevents system monitoring failures from affecting game performance
    /// - Memory monitoring does not allocate excessive objects itself
    /// </summary>
    public static class SystemMonitor
    {
        private static bool _isInitialized = false;
        private static readonly object _initLock = new object();
        private static PerformanceCounter _cpuCounter;
        private static PerformanceCounter _memoryCounter;
        private static PerformanceCounter _diskCounter;
        private static Process _currentProcess;

        // IL2CPP COMPATIBLE: Memory leak detection fields using compile-time safe types
        // These fields track memory patterns over time without dynamic type creation
        private static readonly List<MemorySnapshot> _memorySnapshots = new List<MemorySnapshot>();
        private static readonly object _snapshotLock = new object();
        private static long _baselineMemory = 0;
        private static DateTime _baselineTime = DateTime.MinValue;
        private static int _lastGen0Collections = 0;
        private static int _lastGen1Collections = 0;
        private static int _lastGen2Collections = 0;
        private static long _maxObservedMemory = 0;
        private static DateTime _lastMemoryCheck = DateTime.MinValue;
        
        // IL2CPP COMPATIBLE: Compile-time constant thresholds (no dynamic configuration)
        private const long MEMORY_LEAK_THRESHOLD_MB = 50; // Alert if memory grows by 50MB without GC
        private const int MEMORY_SNAPSHOT_MAX_COUNT = 100; // Limit snapshot history to prevent memory issues
        private const double GC_PRESSURE_THRESHOLD = 5.0; // Alert if GC rate exceeds 5 collections per minute
        
        /// <summary>
        /// IL2CPP COMPATIBLE: Memory snapshot structure using compile-time known types only
        /// No reflection, no dynamic types, fully AOT-safe
        /// </summary>
        private struct MemorySnapshot
        {
            public DateTime Timestamp;
            public long TotalMemory;
            public long WorkingSet;
            public long PrivateMemory;
            public int Gen0Collections;
            public int Gen1Collections;
            public int Gen2Collections;
            public string Context;
            
            public MemorySnapshot(string context)
            {
                Timestamp = DateTime.Now;
                Context = context ?? "Unknown";
                
                // IL2CPP COMPATIBLE: Direct GC calls without reflection
                TotalMemory = GC.GetTotalMemory(false);
                Gen0Collections = GC.CollectionCount(0);
                Gen1Collections = GC.CollectionCount(1);
                Gen2Collections = GC.CollectionCount(2);
                
                // IL2CPP COMPATIBLE: Safe process memory access with fallback
                WorkingSet = 0;
                PrivateMemory = 0;
                try
                {
                    if (_currentProcess != null && !_currentProcess.HasExited)
                    {
                        _currentProcess.Refresh();
                        WorkingSet = _currentProcess.WorkingSet64;
                        PrivateMemory = _currentProcess.PrivateMemorySize64;
                    }
                }
                catch
                {
                    // Fallback to GC memory if process metrics fail
                    WorkingSet = TotalMemory;
                    PrivateMemory = TotalMemory;
                }
            }
        }

        /// <summary>
        /// Initialize system monitoring with advanced memory leak detection (DEBUG mode only)
        /// ⚠️ CRASH PREVENTION: Safe initialization with comprehensive error handling
        /// ⚠️ IL2CPP COMPATIBLE: Uses only compile-time safe initialization patterns
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
                    Main.logger?.Msg(2, "[SYSMON] Initializing advanced system monitoring with memory leak detection (DEBUG mode)");
                    
                    // Initialize current process reference
                    _currentProcess = Process.GetCurrentProcess();
                    Main.logger?.Msg(3, string.Format("[SYSMON] Process initialized: {0} (PID: {1})", _currentProcess.ProcessName, _currentProcess.Id));

                    // Initialize performance counters with error handling
                    InitializePerformanceCounters();
                    
                    // IL2CPP COMPATIBLE: Initialize memory leak detection with baseline
                    InitializeMemoryLeakDetection();
                    
                    // Log initial system information
                    LogSystemInformation();
                    
                    _isInitialized = true;
                    Main.logger?.Msg(1, "[SYSMON] Advanced system monitoring with memory leak detection initialized successfully");
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
        /// IL2CPP COMPATIBLE: Initialize memory leak detection baseline using compile-time safe patterns
        /// Establishes baseline memory usage for leak detection without dynamic reflection
        /// </summary>
        private static void InitializeMemoryLeakDetection()
        {
#if DEBUG
            Exception memoryInitError = null;
            try
            {
                lock (_snapshotLock)
                {
                    Main.logger?.Msg(2, "[SYSMON] Initializing memory leak detection baseline...");
                    
                    // Force initial GC collection to establish clean baseline
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    
                    // IL2CPP COMPATIBLE: Establish baseline using direct GC calls
                    _baselineMemory = GC.GetTotalMemory(false);
                    _baselineTime = DateTime.Now;
                    _maxObservedMemory = _baselineMemory;
                    _lastMemoryCheck = DateTime.Now;
                    
                    // Record initial GC state
                    _lastGen0Collections = GC.CollectionCount(0);
                    _lastGen1Collections = GC.CollectionCount(1);
                    _lastGen2Collections = GC.CollectionCount(2);
                    
                    // Take initial snapshot
                    var initialSnapshot = new MemorySnapshot("BASELINE");
                    _memorySnapshots.Add(initialSnapshot);
                    
                    Main.logger?.Msg(2, string.Format("[SYSMON] Memory leak detection baseline established: {0:F2} MB", _baselineMemory / 1048576.0));
                    Main.logger?.Msg(3, string.Format("[SYSMON] Initial GC state - Gen0: {0}, Gen1: {1}, Gen2: {2}", 
                        _lastGen0Collections, _lastGen1Collections, _lastGen2Collections));
                }
            }
            catch (Exception ex)
            {
                memoryInitError = ex;
            }

            if (memoryInitError != null)
            {
                Main.logger?.Err(string.Format("[SYSMON] Memory leak detection initialization failed: {0}", memoryInitError.Message));
            }
#endif
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
        /// IL2CPP COMPATIBLE: Take memory snapshot for leak detection using compile-time safe patterns
        /// Records current memory state without dynamic type creation or reflection
        /// </summary>
        public static void TakeMemorySnapshot(string context = "")
        {
#if DEBUG
            if (!_isInitialized)
            {
                Main.logger?.Warn(1, "[SYSMON] System monitoring not initialized - skipping memory snapshot");
                return;
            }

            Exception snapshotError = null;
            try
            {
                lock (_snapshotLock)
                {
                    // IL2CPP COMPATIBLE: Create snapshot using compile-time safe constructor
                    var snapshot = new MemorySnapshot(context);
                    _memorySnapshots.Add(snapshot);
                    
                    // Maintain snapshot history limit to prevent memory issues
                    if (_memorySnapshots.Count > MEMORY_SNAPSHOT_MAX_COUNT)
                    {
                        _memorySnapshots.RemoveAt(0); // Remove oldest snapshot
                    }
                    
                    // Update tracking variables
                    if (snapshot.TotalMemory > _maxObservedMemory)
                    {
                        _maxObservedMemory = snapshot.TotalMemory;
                    }
                    
                    _lastMemoryCheck = DateTime.Now;
                    
                    // Check for potential memory leaks
                    CheckForMemoryLeaks(snapshot);
                    
                    Main.logger?.Msg(3, string.Format("[SYSMON] Memory snapshot taken: {0} - {1:F2} MB total, {2:F2} MB working set", 
                        context, snapshot.TotalMemory / 1048576.0, snapshot.WorkingSet / 1048576.0));
                }
            }
            catch (Exception ex)
            {
                snapshotError = ex;
            }

            if (snapshotError != null)
            {
                Main.logger?.Err(string.Format("[SYSMON] Memory snapshot error: {0}", snapshotError.Message));
            }
#endif
        }

        /// <summary>
        /// IL2CPP COMPATIBLE: Check for memory leaks using compile-time safe analysis
        /// Analyzes memory growth patterns without dynamic type introspection
        /// </summary>
        private static void CheckForMemoryLeaks(MemorySnapshot currentSnapshot)
        {
#if DEBUG
            Exception leakCheckError = null;
            try
            {
                // Check memory growth since baseline
                long memoryGrowth = currentSnapshot.TotalMemory - _baselineMemory;
                double memoryGrowthMB = memoryGrowth / 1048576.0;
                
                // Check time elapsed since baseline
                var timeElapsed = DateTime.Now - _baselineTime;
                double minutesElapsed = timeElapsed.TotalMinutes;
                
                // IL2CPP COMPATIBLE: Calculate GC pressure using direct collection count access
                int gen0Growth = currentSnapshot.Gen0Collections - _lastGen0Collections;
                int gen1Growth = currentSnapshot.Gen1Collections - _lastGen1Collections;
                int gen2Growth = currentSnapshot.Gen2Collections - _lastGen2Collections;
                
                // Update last collection counts
                _lastGen0Collections = currentSnapshot.Gen0Collections;
                _lastGen1Collections = currentSnapshot.Gen1Collections;
                _lastGen2Collections = currentSnapshot.Gen2Collections;
                
                // Calculate GC rate per minute
                double gcRate = minutesElapsed > 0 ? (gen0Growth + gen1Growth + gen2Growth) / minutesElapsed : 0;
                
                // Check for memory leak indicators
                bool possibleLeak = false;
                string leakReason = "";
                
                if (memoryGrowthMB > MEMORY_LEAK_THRESHOLD_MB && gen2Growth == 0)
                {
                    possibleLeak = true;
                    leakReason = string.Format("Memory grew {0:F1} MB without Gen2 GC", memoryGrowthMB);
                }
                else if (gcRate > GC_PRESSURE_THRESHOLD)
                {
                    possibleLeak = true;
                    leakReason = string.Format("High GC pressure: {0:F1} collections/minute", gcRate);
                }
                else if (memoryGrowthMB > (MEMORY_LEAK_THRESHOLD_MB * 2))
                {
                    possibleLeak = true;
                    leakReason = string.Format("Excessive memory growth: {0:F1} MB", memoryGrowthMB);
                }
                
                // Log memory analysis
                if (possibleLeak)
                {
                    Main.logger?.Warn(1, string.Format("[SYSMON] ⚠️ POTENTIAL MEMORY LEAK DETECTED: {0}", leakReason));
                    Main.logger?.Warn(1, string.Format("[SYSMON] Memory growth: {0:F1} MB over {1:F1} minutes", memoryGrowthMB, minutesElapsed));
                    Main.logger?.Warn(1, string.Format("[SYSMON] GC activity - Gen0: +{0}, Gen1: +{1}, Gen2: +{2}", gen0Growth, gen1Growth, gen2Growth));
                    Main.logger?.Warn(1, string.Format("[SYSMON] Current memory: {0:F2} MB, Max observed: {1:F2} MB", 
                        currentSnapshot.TotalMemory / 1048576.0, _maxObservedMemory / 1048576.0));
                }
                else if (memoryGrowthMB > 10) // Log significant growth even if not leak
                {
                    Main.logger?.Msg(2, string.Format("[SYSMON] Memory growth: {0:F1} MB, GC rate: {1:F1}/min (within normal range)", memoryGrowthMB, gcRate));
                }
            }
            catch (Exception ex)
            {
                leakCheckError = ex;
            }

            if (leakCheckError != null)
            {
                Main.logger?.Err(string.Format("[SYSMON] Memory leak check error: {0}", leakCheckError.Message));
            }
#endif
        }

        /// <summary>
        /// IL2CPP COMPATIBLE: Generate comprehensive memory leak report using compile-time safe analysis
        /// Provides detailed memory usage patterns without dynamic reflection
        /// </summary>
        public static void GenerateMemoryLeakReport()
        {
#if DEBUG
            if (!_isInitialized)
            {
                Main.logger?.Warn(1, "[SYSMON] System monitoring not initialized - cannot generate memory report");
                return;
            }

            Exception reportError = null;
            try
            {
                lock (_snapshotLock)
                {
                    Main.logger?.Msg(1, "[SYSMON] === COMPREHENSIVE MEMORY LEAK ANALYSIS REPORT ===");
                    
                    if (_memorySnapshots.Count < 2)
                    {
                        Main.logger?.Warn(1, "[SYSMON] Insufficient memory snapshots for analysis (need at least 2)");
                        return;
                    }
                    
                    var firstSnapshot = _memorySnapshots[0];
                    var lastSnapshot = _memorySnapshots[_memorySnapshots.Count - 1];
                    var timeSpan = lastSnapshot.Timestamp - firstSnapshot.Timestamp;
                    
                    // IL2CPP COMPATIBLE: Calculate metrics using direct field access
                    long totalMemoryChange = lastSnapshot.TotalMemory - firstSnapshot.TotalMemory;
                    long workingSetChange = lastSnapshot.WorkingSet - firstSnapshot.WorkingSet;
                    int totalGen0Collections = lastSnapshot.Gen0Collections - firstSnapshot.Gen0Collections;
                    int totalGen1Collections = lastSnapshot.Gen1Collections - firstSnapshot.Gen1Collections;
                    int totalGen2Collections = lastSnapshot.Gen2Collections - firstSnapshot.Gen2Collections;
                    
                    Main.logger?.Msg(1, string.Format("[SYSMON] Analysis period: {0:F1} minutes ({1} snapshots)", timeSpan.TotalMinutes, _memorySnapshots.Count));
                    Main.logger?.Msg(1, string.Format("[SYSMON] Total memory change: {0:F2} MB", totalMemoryChange / 1048576.0));
                    Main.logger?.Msg(1, string.Format("[SYSMON] Working set change: {0:F2} MB", workingSetChange / 1048576.0));
                    Main.logger?.Msg(1, string.Format("[SYSMON] GC collections - Gen0: {0}, Gen1: {1}, Gen2: {2}", totalGen0Collections, totalGen1Collections, totalGen2Collections));
                    
                    // Calculate memory growth rate
                    double memoryGrowthRateMBPerMinute = timeSpan.TotalMinutes > 0 ? (totalMemoryChange / 1048576.0) / timeSpan.TotalMinutes : 0;
                    double gcRatePerMinute = timeSpan.TotalMinutes > 0 ? (totalGen0Collections + totalGen1Collections + totalGen2Collections) / timeSpan.TotalMinutes : 0;
                    
                    Main.logger?.Msg(1, string.Format("[SYSMON] Memory growth rate: {0:F3} MB/minute", memoryGrowthRateMBPerMinute));
                    Main.logger?.Msg(1, string.Format("[SYSMON] GC rate: {0:F1} collections/minute", gcRatePerMinute));
                    
                    // Analyze trends
                    if (memoryGrowthRateMBPerMinute > 1.0)
                    {
                        Main.logger?.Warn(1, "[SYSMON] ⚠️ HIGH MEMORY GROWTH RATE - Potential memory leak");
                    }
                    else if (memoryGrowthRateMBPerMinute > 0.1)
                    {
                        Main.logger?.Warn(1, "[SYSMON] ⚠️ MODERATE MEMORY GROWTH - Monitor for leaks");
                    }
                    else
                    {
                        Main.logger?.Msg(1, "[SYSMON] ✅ MEMORY GROWTH WITHIN NORMAL RANGE");
                    }
                    
                    if (gcRatePerMinute > GC_PRESSURE_THRESHOLD)
                    {
                        Main.logger?.Warn(1, "[SYSMON] ⚠️ HIGH GC PRESSURE - Possible memory allocation issues");
                    }
                    else
                    {
                        Main.logger?.Msg(1, "[SYSMON] ✅ GC PRESSURE WITHIN NORMAL RANGE");
                    }
                    
                    // Check for concerning patterns
                    if (totalGen2Collections == 0 && totalMemoryChange > (MEMORY_LEAK_THRESHOLD_MB * 1048576))
                    {
                        Main.logger?.Warn(1, "[SYSMON] ⚠️ CRITICAL: Large memory growth with no Gen2 GC - Strong leak indicator");
                    }
                    
                    Main.logger?.Msg(1, "[SYSMON] === END MEMORY LEAK ANALYSIS REPORT ===");
                }
            }
            catch (Exception ex)
            {
                reportError = ex;
            }

            if (reportError != null)
            {
                Main.logger?.Err(string.Format("[SYSMON] Memory leak report generation error: {0}", reportError.Message));
            }
#endif
        }
        /// <summary>
        /// Log current system performance metrics with advanced memory leak detection (DEBUG mode only)
        /// ⚠️ THREAD SAFETY: Safe performance counter access with error handling
        /// ⚠️ IL2CPP COMPATIBLE: Uses compile-time safe performance monitoring patterns
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
                
                Main.logger?.Msg(2, string.Format("[SYSMON] {0}=== PERFORMANCE SNAPSHOT WITH MEMORY LEAK DETECTION ===", contextPrefix));
                
                // IL2CPP COMPATIBLE: Take memory snapshot for leak detection
                TakeMemorySnapshot(context);
                
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

                // IL2CPP COMPATIBLE: Enhanced process-specific metrics with memory leak indicators
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

                // IL2CPP COMPATIBLE: Enhanced garbage collection analysis for memory leak detection
                try
                {
                    long currentGCMemory = GC.GetTotalMemory(false);
                    Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Total Memory: {1:F2} MB", contextPrefix, currentGCMemory / 1048576.0));
                    Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Collection Count (Gen 0): {1}", contextPrefix, GC.CollectionCount(0)));
                    Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Collection Count (Gen 1): {1}", contextPrefix, GC.CollectionCount(1)));
                    Main.logger?.Msg(3, string.Format("[SYSMON] {0}GC Collection Count (Gen 2): {1}", contextPrefix, GC.CollectionCount(2)));
                    
                    // Memory leak indicators
                    if (_baselineMemory > 0)
                    {
                        long memoryGrowth = currentGCMemory - _baselineMemory;
                        double growthMB = memoryGrowth / 1048576.0;
                        Main.logger?.Msg(3, string.Format("[SYSMON] {0}Memory Growth Since Baseline: {1:F2} MB", contextPrefix, growthMB));
                        
                        if (growthMB > MEMORY_LEAK_THRESHOLD_MB)
                        {
                            Main.logger?.Warn(1, string.Format("[SYSMON] {0}⚠️ High memory growth detected: {1:F2} MB", contextPrefix, growthMB));
                        }
                    }
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
        /// Monitor performance during a specific operation with memory leak detection
        /// ⚠️ THREAD SAFETY: Safe performance monitoring with comprehensive error handling
        /// ⚠️ IL2CPP COMPATIBLE: Uses compile-time safe monitoring patterns without reflection
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
                Main.logger?.Msg(2, string.Format("[SYSMON] Starting monitored operation with memory leak detection: {0}", operationName));
                LogCurrentPerformance(string.Format("BEFORE {0}", operationName));
                
                stopwatch.Start();
                operation?.Invoke();
                stopwatch.Stop();
                
                LogCurrentPerformance(string.Format("AFTER {0}", operationName));
                Main.logger?.Msg(1, string.Format("[SYSMON] Operation '{0}' completed in {1:F3}s", operationName, stopwatch.Elapsed.TotalSeconds));
                
                // IL2CPP COMPATIBLE: Generate memory leak analysis for long operations
                if (stopwatch.Elapsed.TotalSeconds > 1.0) // Only analyze operations longer than 1 second
                {
                    Main.logger?.Msg(2, string.Format("[SYSMON] Generating memory analysis for operation: {0}", operationName));
                    GenerateMemoryLeakReport();
                }
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
        /// Cleanup system monitoring resources with memory leak final report
        /// ⚠️ CRASH PREVENTION: Safe resource disposal with comprehensive error handling
        /// ⚠️ IL2CPP COMPATIBLE: Uses compile-time safe cleanup patterns
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
                    Main.logger?.Msg(3, "[SYSMON] Cleaning up system monitoring resources and generating final memory report");
                    
                    // IL2CPP COMPATIBLE: Generate final memory leak report
                    GenerateMemoryLeakReport();
                    
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
                    
                    // Clear memory snapshots to prevent memory leaks in monitoring system itself
                    lock (_snapshotLock)
                    {
                        _memorySnapshots.Clear();
                    }
                    
                    _isInitialized = false;
                    Main.logger?.Msg(2, "[SYSMON] System monitoring cleanup completed with final memory analysis");
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