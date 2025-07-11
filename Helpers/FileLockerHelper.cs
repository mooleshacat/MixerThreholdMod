using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

public class FileLockHelper : IDisposable
{
    private FileStream _lockStream;
    private bool _isLocked = false;
    private readonly string _lockFilePath;
    private const int DefaultTimeoutMs = 5000; // 5 seconds

    public FileLockHelper(string lockFilePath)
    {
        _lockFilePath = lockFilePath;
    }

    /// <summary>
    /// Acquires a shared (read) lock on the file.
    /// Allows multiple readers, blocks writers.
    /// </summary>
    public bool AcquireSharedLock(int timeoutMs = DefaultTimeoutMs)
    {
        if (_isLocked) throw new InvalidOperationException("Already locked.");
        try
        {
            _lockStream = new FileStream(
                _lockFilePath,
                FileMode.OpenOrCreate,
                FileAccess.Read,
                FileShare.Read,
                4096,
                FileOptions.None
            );
            _isLocked = true;

            // Simulate timeout via Task + Wait
            if (Task.Run(() => true).Wait(timeoutMs))
                return true;

            ReleaseLock();
            return false;
        }
        catch (Exception ex)
        {
            try
            {
                ReleaseLock();
            }
            catch
            {
                // Ignore disposal errors
            }
            return false;
        }
    }

    /// <summary>
    /// Acquires an exclusive (write) lock on the file.
    /// Blocks all other readers and writers.
    /// </summary>
    public bool AcquireExclusiveLock(int timeoutMs = DefaultTimeoutMs)
    {
        if (_isLocked) throw new InvalidOperationException("Already locked.");
        try
        {
            _lockStream = new FileStream(
                _lockFilePath,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.None,
                4096,
                FileOptions.None
            );
            _isLocked = true;

            if (Task.Run(() => true).Wait(timeoutMs))
                return true;

            ReleaseLock();
            return false;
        }
        catch (Exception ex)
        {
            try
            {
                ReleaseLock();
            }
            catch
            {
                // Ignore disposal errors
            }
            return false;
        }
    }

    /// <summary>
    /// Asynchronously acquires a shared (read) lock with timeout.
    /// </summary>
    public async Task<bool> AcquireSharedLockAsync(CancellationToken ct = default, int timeoutMs = DefaultTimeoutMs)
    {
        if (_isLocked) throw new InvalidOperationException("Already locked.");

        var tcs = new TaskCompletionSource<bool>();
        var timer = new Timer(_ => tcs.TrySetResult(false), null, timeoutMs, Timeout.Infinite);
        try
        {
            await Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        _lockStream = new FileStream(
                            _lockFilePath,
                            FileMode.OpenOrCreate,
                            FileAccess.Read,
                            FileShare.Read,
                            4096,
                            FileOptions.None
                        );
                        _isLocked = true;
                        timer.Dispose();
                        tcs.TrySetResult(true);
                        return;
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(100, ct).ConfigureAwait(false);
                    }
                }
                timer.Dispose();
                tcs.TrySetCanceled();
            }, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            timer.Dispose();
            return false;
        }
        return await tcs.Task.ConfigureAwait(false);
    }
    
    /// <summary>
    /// Asynchronously acquires an exclusive (write) lock with timeout.
    /// </summary>
    public async Task<bool> AcquireExclusiveLockAsync(CancellationToken ct = default, int timeoutMs = DefaultTimeoutMs)
    {
        if (_isLocked) throw new InvalidOperationException("Already locked.");
        var tcs = new TaskCompletionSource<bool>();
        var timer = new Timer(_ => tcs.TrySetResult(false), null, timeoutMs, Timeout.Infinite);
        try
        {
            await Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        _lockStream = new FileStream(
                            _lockFilePath,
                            FileMode.OpenOrCreate,
                            FileAccess.ReadWrite,
                            FileShare.None,
                            4096,
                            FileOptions.None
                        );
                        _isLocked = true;
                        timer.Dispose();
                        tcs.TrySetResult(true);
                        return;
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(100, ct).ConfigureAwait(false);
                    }
                }
                timer.Dispose();
                tcs.TrySetCanceled();
            }, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            timer.Dispose();
            return false;
        }
        return await tcs.Task.ConfigureAwait(false);
    }
    /// <summary>
    /// Releases the current lock.
    /// </summary>
    public void ReleaseLock()
    {
        try
        {
            if (_isLocked && _lockStream != null)
            {
                _lockStream.Dispose();
                _lockStream = null;
                _isLocked = false;
            }
        }
        catch (Exception ex)
        {
<<<<<<< HEAD
            // Log if Main.logger is available, otherwise silently handle
            try
            {
                MixerThreholdMod_0_0_1.Main.logger.Warn(1, $"FileLockHelper.ReleaseLock: Error releasing lock: {ex.Message}");
            }
            catch
            {
                // Ignore logging errors during cleanup
            }
=======
            // Log the error but don't throw - we're in cleanup
            _isLocked = false;
            _lockStream = null;
>>>>>>> f184e29 (Fix sync-over-async patterns, improve file operations, and add defensive programming)
        }
    }
    
    public void Dispose()
    {
        try
        {
            ReleaseLock();
        }
        catch (Exception ex)
        {
            // Log if Main.logger is available, otherwise silently handle
            try
            {
                MixerThreholdMod_0_0_1.Main.logger.Warn(1, $"FileLockHelper.Dispose: Error during disposal: {ex.Message}");
            }
            catch
            {
                // Ignore logging errors during cleanup
            }
        }
    }
}