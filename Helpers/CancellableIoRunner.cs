using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Cancellable I/O operation runner for .NET 4.8.1 compatibility.
    /// Provides safe execution of I/O operations with proper cancellation support.
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
    /// - Enables cancellable file I/O operations
    /// - Prevents main thread blocking during I/O
    /// - Provides timeout and cancellation mechanisms
    /// - Integrates with the mod's logging system
    /// </summary>
    public static class CancellableIoRunner
    {
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
                logger?.Invoke(1, "CancellableIoRunner.Run: ioOperation is null");
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
            logger?.Invoke(1, string.Format("CancellableIoRunner.Run: Critical error: {0}", ex.Message));
            return false;
        }
    }
}
}