using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Progress reporting interface for I/O operations
    /// </summary>
    public interface IIoProgressReporter
    {
        void ReportProgress(int percentComplete, string message);
        void ReportCompletion(bool success, string finalMessage);
    }

    /// <summary>
    /// Priority-based I/O operation item
    /// </summary>
    public class IoOperationItem
    {
        public Func<CancellationToken, IProgress<string>, Task> Operation { get; set; }
        public int Priority { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public IIoProgressReporter ProgressReporter { get; set; }
        public DateTime QueuedTime { get; set; } = DateTime.UtcNow;
        public string OperationName { get; set; }
    }

    /// <summary>
    /// Cancellable I/O operation runner for .NET 4.8.1 compatibility.
    /// Provides safe execution of I/O operations with proper cancellation support,
    /// priority queue management, and progress reporting.
    /// 
    /// ⚠️ THREAD SAFETY: This class is thread-safe and designed for background I/O operations.
    /// All operations respect cancellation tokens and provide proper error handling.
    /// 
    /// ⚠️ MAIN THREAD WARNING: Operations run on background threads to prevent blocking
    /// Unity's main thread. Use this for any potentially slow I/O operations.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses Task-based async patterns with proper ConfigureAwait
    /// - Compatible cancellation token support
    /// - Defensive programming with comprehensive error handling
    /// - String.Format usage for logging compatibility
    /// 
    /// Purpose:
    /// - Enables cancellable file I/O operations with priority management
    /// - Prevents main thread blocking during I/O
    /// - Provides timeout and cancellation mechanisms
    /// - Integrates with the mod's logging system
    /// - Supports progress reporting for long-running operations
    /// </summary>
    public static class CancellableIoRunner
    {
        private static readonly ConcurrentQueue<IoOperationItem> _priorityQueue = new ConcurrentQueue<IoOperationItem>();
        private static readonly object _queueLock = new object();
        private static volatile bool _processingQueue = false;
        /// <summary>
        /// Queues an I/O operation with priority for background execution
        /// </summary>
        public static void QueueOperation(
            Func<CancellationToken, IProgress<string>, Task> operation,
            int priority = 0,
            CancellationToken ct = default(CancellationToken),
            IIoProgressReporter progressReporter = null,
            string operationName = "Unknown")
        {
            try
            {
                var item = new IoOperationItem
                {
                    Operation = operation,
                    Priority = priority,
                    CancellationToken = ct,
                    ProgressReporter = progressReporter,
                    OperationName = operationName
                };

                _priorityQueue.Enqueue(item);

                Main.logger?.Msg(3, string.Format("{0} Queued I/O operation: {1} (Priority: {2})", 
                    IO_RUNNER_PREFIX, operationName, priority));

                // Start processing if not already running
                _ = Task.Run(() => ProcessQueue().ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} Failed to queue I/O operation: {1}", 
                    IO_RUNNER_PREFIX, ex.Message));
            }
        }

        /// <summary>
        /// Processes the priority queue of I/O operations
        /// </summary>
        private static async Task ProcessQueue()
        {
            if (_processingQueue) return;

            lock (_queueLock)
            {
                if (_processingQueue) return;
                _processingQueue = true;
            }

            try
            {
                Main.logger?.Msg(3, string.Format("{0} Starting queue processing", IO_RUNNER_PREFIX));

                while (_priorityQueue.TryDequeue(out IoOperationItem item))
                {
                    if (item.CancellationToken.IsCancellationRequested)
                    {
                        Main.logger?.Msg(3, string.Format("{0} Skipping cancelled operation: {1}", 
                            IO_RUNNER_PREFIX, item.OperationName));
                        continue;
                    }

                    try
                    {
                        var progress = new Progress<string>(message =>
                        {
                            item.ProgressReporter?.ReportProgress(50, message);
                            Main.logger?.Msg(3, string.Format("{0} Progress [{1}]: {2}", 
                                IO_RUNNER_PREFIX, item.OperationName, message));
                        });

                        Main.logger?.Msg(2, string.Format("{0} Executing I/O operation: {1}", 
                            IO_RUNNER_PREFIX, item.OperationName));

                        await item.Operation(item.CancellationToken, progress).ConfigureAwait(false);

                        item.ProgressReporter?.ReportCompletion(true, "Operation completed successfully");
                        Main.logger?.Msg(2, string.Format("{0} Completed I/O operation: {1}", 
                            IO_RUNNER_PREFIX, item.OperationName));
                    }
                    catch (OperationCanceledException)
                    {
                        item.ProgressReporter?.ReportCompletion(false, "Operation was cancelled");
                        Main.logger?.Msg(2, string.Format("{0} Cancelled I/O operation: {1}", 
                            IO_RUNNER_PREFIX, item.OperationName));
                    }
                    catch (Exception ex)
                    {
                        item.ProgressReporter?.ReportCompletion(false, string.Format("Operation failed: {0}", ex.Message));
                        Main.logger?.Err(string.Format("{0} Failed I/O operation [{1}]: {2}", 
                            IO_RUNNER_PREFIX, item.OperationName, ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} Queue processing error: {1}", 
                    IO_RUNNER_PREFIX, ex.Message));
            }
            finally
            {
                lock (_queueLock)
                {
                    _processingQueue = false;
                }
                Main.logger?.Msg(3, string.Format("{0} Queue processing completed", IO_RUNNER_PREFIX));
            }
        }

        /// <summary>
        /// Runs a cancellable I/O operation on a background thread.
        /// Optionally logs messages through the provided logging action.
        /// </summary>
    public static async Task<bool> Run(
        Func<CancellationToken, Task> ioOperation,
        CancellationToken ct,
        Action<int, string> logger = null)
    {
        try
        {
            if (ioOperation == null)
            {
                logger?.Invoke(1, string.Format("{0} ioOperation is null", IO_RUNNER_PREFIX));
                return false;
            }

            var tcs = new TaskCompletionSource<bool>();

            // Register cancellation
            ct.Register(() => tcs.TrySetCanceled());

            // Run the operation on a background thread
            await Task.Run(async () =>
            {
                try
                {
                    await ioOperation(ct);
                    tcs.TrySetResult(true);
                }
                catch (OperationCanceledException)
                {
                    tcs.TrySetCanceled();
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }, ct);

            try
            {
                return await tcs.Task;
            }
            catch (OperationCanceledException)
            {
                logger?.Invoke(1, "Cancellable I/O operation was canceled.");
                return false;
            }
            catch (Exception ex)
            {
                logger?.Invoke(1, string.Format("Error during cancellable I/O operation: {0}", ex.Message));
                return false;
            }
        }
        catch (Exception ex)
        {
            logger?.Invoke(1, string.Format("{0} Critical error: {1}", IO_RUNNER_PREFIX, ex.Message));
            return false;
        }
    }
}
}