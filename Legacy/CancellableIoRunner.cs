

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MixerThreholdMod_1_0_0.Helpers
{
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