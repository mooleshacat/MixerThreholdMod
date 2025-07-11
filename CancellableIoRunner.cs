using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
            logger?.Invoke(1, $"Error during cancellable I/O operation: {ex}");
            return false;
        }
    }
}