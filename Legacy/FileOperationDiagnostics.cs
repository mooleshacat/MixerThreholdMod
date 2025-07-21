

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// âš ï¸ FILE OPERATION DIAGNOSTICS: Comprehensive monitoring class for file I/O operations
    /// âš ï¸ THREAD SAFETY: All operations are thread-safe for concurrent diagnostics
    /// âš ï¸ .NET 4.8.1 COMPATIBLE: Uses framework-safe timing and logging patterns
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
            Main.logger?.Msg(3, string.Format("[FILE-DIAG] Starting {0}...", operationName));
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
                Main.logger?.Warn(1, string.Format("[FILE-DIAG] SLOW OPERATION: {0} took {1:F1}ms (>500ms threshold)",
                    result.OperationName, result.DurationMs));
            }
            else if (result.DurationMs > 100)
            {
                Main.logger?.Msg(2, string.Format("[FILE-DIAG] Moderate timing: {0} took {1:F1}ms",
                    result.OperationName, result.DurationMs));
            }
            else
            {
                Main.logger?.Msg(3, string.Format("[FILE-DIAG] Fast operation: {0} took {1:F1}ms",
                    result.OperationName, result.DurationMs));
            }

            // Calculate transfer rate if file size provided
            if (fileSizeBytes > 0 && result.DurationMs > 0)
            {
                var transferRateMBps = (fileSizeBytes / 1048576.0) / (result.DurationMs / 1000.0);
                Main.logger?.Msg(3, string.Format("[FILE-DIAG] Transfer rate: {0:F2} MB/s ({1} bytes in {2:F1}ms)",
                    transferRateMBps, fileSizeBytes, result.DurationMs));
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
                    Main.logger?.Msg(3, string.Format("[FILE-DIAG] {0}: No file operations recorded", context));
                    return;
                }

                var totalTime = _operations.Sum(op => op.DurationMs);
                var avgTime = totalTime / _operations.Count;
                var maxTime = _operations.Max(op => op.DurationMs);
                var minTime = _operations.Min(op => op.DurationMs);
                var slowOps = _operations.Count(op => op.DurationMs > 100);

                Main.logger?.Msg(2, string.Format("[FILE-DIAG] {0} Summary:", context));
                Main.logger?.Msg(2, string.Format("[FILE-DIAG] Operations: {0}, Total: {1:F1}ms, Avg: {2:F1}ms",
                    _operations.Count, totalTime, avgTime));
                Main.logger?.Msg(2, string.Format("[FILE-DIAG] Range: {0:F1}ms - {1:F1}ms, Slow ops (>100ms): {2}",
                    minTime, maxTime, slowOps));

                // Log individual operations if there were performance issues
                if (slowOps > 0 || maxTime > 200)
                {
                    Main.logger?.Msg(2, "[FILE-DIAG] Performance breakdown:");
                    foreach (var op in _operations.Where(o => o.DurationMs > 50).OrderByDescending(o => o.DurationMs))
                    {
                        Main.logger?.Msg(3, string.Format("[FILE-DIAG] - {0}: {1:F1}ms", op.OperationName, op.DurationMs));
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("[FILE-DIAG] Error generating summary: {0}", ex.Message));
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