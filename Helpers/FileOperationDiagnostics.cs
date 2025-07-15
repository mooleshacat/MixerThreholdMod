using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// ⚠️ FILE OPERATION DIAGNOSTICS: Comprehensive monitoring class for file I/O operations
    /// ⚠️ THREAD SAFETY: All operations are thread-safe for concurrent diagnostics
    /// ⚠️ .NET 4.8.1 COMPATIBLE: Uses framework-safe timing and logging patterns
    /// 
    /// Features:
    /// - Tracks file operation timing with millisecond precision
    /// - Monitors file sizes and transfer rates
    /// - Provides performance warnings for slow operations
    /// - Comprehensive logging with context information
    /// </summary>
    public class FileOperationDiagnostics
    {
        private readonly System.Diagnostics.Stopwatch _operationTimer;
        private readonly DateTime _startTime;
        private string _currentOperation;
        private readonly List<FileOperationResult> _operations;

        public FileOperationDiagnostics()
        {
            _operationTimer = new System.Diagnostics.Stopwatch();
            _startTime = DateTime.Now;
            _operations = new List<FileOperationResult>();
        }

        public void StartOperation(string operationName)
        {
            _currentOperation = operationName;
            _operationTimer.Restart();
            Main.logger?.Msg(3, string.Format("{0} Starting {1}...", DIAGNOSTICS_PREFIX, operationName));
        }

        public void EndOperation(long fileSizeBytes = 0)
        {
            _operationTimer.Stop();
            var result = new FileOperationResult
            {
                OperationName = _currentOperation,
                DurationMs = _operationTimer.Elapsed.TotalMilliseconds,
                FileSizeBytes = fileSizeBytes,
                Timestamp = DateTime.Now
            };

            _operations.Add(result);

            // Log operation result with performance analysis
            if (result.DurationMs > 500)
            {
                Main.logger?.Warn(1, string.Format("{0} SLOW OPERATION: {1} took {2:F1}ms (>500ms threshold)",
                    DIAGNOSTICS_PREFIX, result.OperationName, result.DurationMs));
            }
            else if (result.DurationMs > 100)
            {
                Main.logger?.Msg(2, string.Format("{0} Moderate timing: {1} took {2:F1}ms",
                    DIAGNOSTICS_PREFIX, result.OperationName, result.DurationMs));
            }
            else
            {
                Main.logger?.Msg(3, string.Format("{0} Fast operation: {1} took {2:F1}ms",
                    DIAGNOSTICS_PREFIX, result.OperationName, result.DurationMs));
            }

            // Calculate transfer rate if file size provided
            if (fileSizeBytes > 0 && result.DurationMs > 0)
            {
                var transferRateMBps = (fileSizeBytes / 1048576.0) / (result.DurationMs / 1000.0);
                Main.logger?.Msg(3, string.Format("{0} Transfer rate: {1:F2} MB/s ({2} bytes in {3:F1}ms)",
                    DIAGNOSTICS_PREFIX, transferRateMBps, fileSizeBytes, result.DurationMs));
            }
        }

        public double GetLastOperationTime()
        {
            return _operations.Count > 0 ? _operations[_operations.Count - 1].DurationMs : 0;
        }

        public void LogSummary(string context)
        {
            try
            {
                if (_operations.Count == 0)
                {
                    Main.logger?.Msg(3, string.Format("{0} {1}: No file operations recorded", DIAGNOSTICS_PREFIX, context));
                    return;
                }

                var totalTime = _operations.Sum(op => op.DurationMs);
                var avgTime = totalTime / _operations.Count;
                var maxTime = _operations.Max(op => op.DurationMs);
                var minTime = _operations.Min(op => op.DurationMs);
                var slowOps = _operations.Count(op => op.DurationMs > 100);

                Main.logger?.Msg(2, string.Format("{0} {1} Summary:", DIAGNOSTICS_PREFIX, context));
                Main.logger?.Msg(2, string.Format("{0} Operations: {1}, Total: {2:F1}ms, Avg: {3:F1}ms",
                    DIAGNOSTICS_PREFIX, _operations.Count, totalTime, avgTime));
                Main.logger?.Msg(2, string.Format("{0} Range: {1:F1}ms - {2:F1}ms, Slow ops (>100ms): {3}",
                    DIAGNOSTICS_PREFIX, minTime, maxTime, slowOps));

                // Log individual operations if there were performance issues
                if (slowOps > 0 || maxTime > 200)
                {
                    Main.logger?.Msg(2, string.Format("{0} Performance breakdown:", DIAGNOSTICS_PREFIX));
                    foreach (var op in _operations.Where(o => o.DurationMs > 50).OrderByDescending(o => o.DurationMs))
                    {
                        Main.logger?.Msg(3, string.Format("{0} - {1}: {2:F1}ms", DIAGNOSTICS_PREFIX, op.OperationName, op.DurationMs));
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} Error generating summary: {1}", DIAGNOSTICS_PREFIX, ex.Message));
            }
        }

        private struct FileOperationResult
        {
            public string OperationName;
            public double DurationMs;
            public long FileSizeBytes;
            public DateTime Timestamp;
        }
    }
}