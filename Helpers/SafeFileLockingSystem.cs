using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MixerThreholdMod_1_0_0;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Thread-safe file locking helper for .NET 4.8.1 compatibility.
    /// Provides exclusive and shared locking mechanisms to prevent file corruption.
    /// 
    /// ⚠️ THREAD SAFETY: This class is thread-safe and can be used across multiple threads.
    /// All lock operations are atomic and protected against race conditions.
    /// 
    /// ⚠️ MAIN THREAD WARNING: Synchronous lock methods (AcquireSharedLock, AcquireExclusiveLock) 
    /// use Thread.Sleep and blocking operations. Do NOT call from Unity's main thread as they 
    /// can cause UI freezes and deadlocks. Use async alternatives when possible.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses compatible async/await patterns with ConfigureAwait(false)
    /// - Proper timeout mechanisms instead of infinite blocking
    /// - Compatible exception handling and resource disposal
    /// - Thread-safe lock acquisition with retry logic
    /// 
    /// Locking Strategy:
    /// - Exclusive locks: For write operations (FileShare.None)
    /// - Shared locks: For read operations (FileShare.Read)
    /// - Automatic cleanup and timeout protection
    /// - Proper IDisposable implementation for resource cleanup
    /// </summary>
    public class FileLockHelper : IDisposable
    {
        private readonly string lockFilePath;
        private FileStream lockStream;
        private volatile bool isLocked;
        private readonly object lockObject = new object();
        private bool disposed = false;
        private const int DefaultTimeoutMs = 5000;
        private const int RetryDelayMs = 50;

        public FileLockHelper(string lockFilePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lockFilePath))
                {
                    Main.logger?.Err("FileLockHelper constructor: lockFilePath is null or empty");
                    throw new ArgumentException("Lock file path cannot be null or empty", "lockFilePath");
                }

                this.lockFilePath = lockFilePath;
                Main.logger?.Msg(3, string.Format("{0} FileLockHelper created for: {1}", FILE_LOCK_PREFIX, lockFilePath));
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} FileLockHelper constructor failed: {1}\n{2}", FILE_LOCK_PREFIX, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Attempts to acquire a shared (read) lock with timeout.
        /// 
        /// ⚠️ MAIN THREAD WARNING: This method uses Thread.Sleep and blocks the calling thread.
        /// Do NOT call from main thread. Use AcquireSharedLockAsync() for main thread safety.
        /// 
        /// Thread Safety: Uses proper locking mechanisms but blocks calling thread during retries.
        /// </summary>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>True if lock acquired successfully, false on timeout or error</returns>
        public bool AcquireSharedLock(int timeoutMs = DefaultTimeoutMs)
        {
            try
            {
                if (disposed)
                {
                    Main.logger?.Warn(1, "FileLockHelper.AcquireSharedLock: Object is disposed");
                    return false;
                }

                lock (lockObject)
                {
                    if (isLocked)
                    {
                        Main.logger?.Warn(2, "FileLockHelper.AcquireSharedLock: Lock already acquired");
                        return true;
                    }

                    Main.logger?.Msg(3, string.Format("{0} FileLockHelper.AcquireSharedLock: Attempting shared lock on {1}", FILE_LOCK_PREFIX, lockFilePath));

                    var startTime = DateTime.Now;
                    var timeout = TimeSpan.FromMilliseconds(timeoutMs);

                    while (DateTime.Now - startTime < timeout)
                    {
                        try
                        {
                            lockStream = new FileStream(
                                lockFilePath,
                                FileMode.OpenOrCreate,
                                FileAccess.Read,
                                FileShare.Read,
                                DEFAULT_FILE_BUFFER_SIZE,
                                FileOptions.None
                            );
                            isLocked = true;
                            Main.logger?.Msg(2, string.Format("{0} FileLockHelper.AcquireSharedLock: Successfully acquired shared lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                            return true;
                        }
                        catch (IOException)
                        {
                            // File is locked, wait and retry
                            // WARNING: Thread.Sleep blocks calling thread - consider using async version for main thread safety
                            Thread.Sleep(RetryDelayMs);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            // Permission denied, wait and retry
                            // WARNING: Thread.Sleep blocks calling thread - consider using async version for main thread safety
                            Thread.Sleep(RetryDelayMs);
                        }
                    }

                    Main.logger?.Warn(1, string.Format("{0} FileLockHelper.AcquireSharedLock: Timeout acquiring shared lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} FileLockHelper.AcquireSharedLock: Caught exception: {1}\n{2}", FILE_LOCK_PREFIX, ex.Message, ex.StackTrace));
                try
                {
                    ReleaseLock();
                }
                catch (Exception releaseEx)
                {
                    Main.logger?.Err(string.Format("{0} Error during lock release in exception handler: {1}", FILE_LOCK_PREFIX, releaseEx.Message));
                }
                return false;
            }
        }

        /// <summary>
        /// Attempts to acquire an exclusive (write) lock with timeout.
        /// 
        /// ⚠️ MAIN THREAD WARNING: This method uses Thread.Sleep and blocks the calling thread.
        /// Do NOT call from main thread. Use AcquireExclusiveLockAsync() for main thread safety.
        /// 
        /// Thread Safety: Uses proper locking mechanisms but blocks calling thread during retries.
        /// </summary>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>True if lock acquired successfully, false on timeout or error</returns>
        public bool AcquireExclusiveLock(int timeoutMs = DefaultTimeoutMs)
        {
            try
            {
                if (disposed)
                {
                    Main.logger?.Warn(1, "FileLockHelper.AcquireExclusiveLock: Object is disposed");
                    return false;
                }

                lock (lockObject)
                {
                    if (isLocked)
                    {
                        Main.logger?.Warn(2, "FileLockHelper.AcquireExclusiveLock: Lock already acquired");
                        return true;
                    }

                    Main.logger?.Msg(3, string.Format("{0} FileLockHelper.AcquireExclusiveLock: Attempting exclusive lock on {1}", FILE_LOCK_PREFIX, lockFilePath));

                    var startTime = DateTime.Now;
                    var timeout = TimeSpan.FromMilliseconds(timeoutMs);

                    while (DateTime.Now - startTime < timeout)
                    {
                        try
                        {
                            lockStream = new FileStream(
                                lockFilePath,
                                FileMode.OpenOrCreate,
                                FileAccess.ReadWrite,
                                FileShare.None,
                                DEFAULT_FILE_BUFFER_SIZE,
                                FileOptions.None
                            );
                            isLocked = true;
                            Main.logger?.Msg(2, string.Format("{0} FileLockHelper.AcquireExclusiveLock: Successfully acquired exclusive lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                            return true;
                        }
                        catch (IOException)
                        {
                            // File is locked, wait and retry
                            // WARNING: Thread.Sleep blocks calling thread - consider using async version for main thread safety
                            Thread.Sleep(RetryDelayMs);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            // Permission denied, wait and retry
                            // WARNING: Thread.Sleep blocks calling thread - consider using async version for main thread safety
                            Thread.Sleep(RetryDelayMs);
                        }
                    }

                    Main.logger?.Warn(1, string.Format("{0} FileLockHelper.AcquireExclusiveLock: Timeout acquiring exclusive lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} FileLockHelper.AcquireExclusiveLock: Caught exception: {1}\n{2}", FILE_LOCK_PREFIX, ex.Message, ex.StackTrace));
                try
                {
                    ReleaseLock();
                }
                catch (Exception releaseEx)
                {
                    Main.logger?.Err(string.Format("{0} Error during lock release in exception handler: {1}", FILE_LOCK_PREFIX, releaseEx.Message));
                }
                return false;
            }
        }

        /// <summary>
        /// Asynchronously acquires a shared (read) lock with timeout.
        /// 
        /// Thread Safety: This method is fully async and safe to call from main thread.
        /// Uses Task.Delay instead of Thread.Sleep to avoid blocking.
        /// 
        /// .NET 4.8.1 Compatibility: Uses ConfigureAwait(false) to prevent deadlocks.
        /// Supports cancellation tokens for cooperative cancellation.
        /// </summary>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <param name="cancellationToken">Cancellation token for cooperative cancellation</param>
        /// <returns>Task returning true if lock acquired successfully, false on timeout/cancellation</returns>
        public async Task<bool> AcquireSharedLockAsync(int timeoutMs = DefaultTimeoutMs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (disposed)
                {
                    Main.logger?.Warn(1, "FileLockHelper.AcquireSharedLockAsync: Object is disposed");
                    return false;
                }

                if (isLocked)
                {
                    Main.logger?.Warn(2, "FileLockHelper.AcquireSharedLockAsync: Lock already acquired");
                    return true;
                }

                Main.logger?.Msg(3, string.Format("{0} FileLockHelper.AcquireSharedLockAsync: Attempting async shared lock on {1}", FILE_LOCK_PREFIX, lockFilePath));

                var startTime = DateTime.Now;
                var timeout = TimeSpan.FromMilliseconds(timeoutMs);

                while (DateTime.Now - startTime < timeout && !cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        lock (lockObject)
                        {
                            if (!isLocked)
                            {
                                lockStream = new FileStream(
                                    lockFilePath,
                                    FileMode.OpenOrCreate,
                                    FileAccess.Read,
                                    FileShare.Read,
                                    DEFAULT_FILE_BUFFER_SIZE,
                                    FileOptions.None
                                );
                                isLocked = true;
                                Main.logger?.Msg(2, string.Format("{0} FileLockHelper.AcquireSharedLockAsync: Successfully acquired async shared lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                                return true;
                            }
                        }
                    }
                    catch (IOException)
                    {
                        // File is locked, wait and retry
                        await Task.Delay(RetryDelayMs, cancellationToken).ConfigureAwait(false);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Permission denied, wait and retry
                        await Task.Delay(RetryDelayMs, cancellationToken).ConfigureAwait(false);
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    Main.logger?.Warn(1, string.Format("{0} FileLockHelper.AcquireSharedLockAsync: Cancelled acquiring shared lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                }
                else
                {
                    Main.logger?.Warn(1, string.Format("{0} FileLockHelper.AcquireSharedLockAsync: Timeout acquiring shared lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                }

                return false;
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} FileLockHelper.AcquireSharedLockAsync: Caught exception: {1}\n{2}", FILE_LOCK_PREFIX, ex.Message, ex.StackTrace));
                return false;
            }
        }

        /// <summary>
        /// Asynchronously acquires an exclusive (write) lock with timeout.
        /// 
        /// Thread Safety: This method is fully async and safe to call from main thread.
        /// Uses Task.Delay instead of Thread.Sleep to avoid blocking.
        /// 
        /// .NET 4.8.1 Compatibility: Uses ConfigureAwait(false) to prevent deadlocks.
        /// Supports cancellation tokens for cooperative cancellation.
        /// </summary>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <param name="cancellationToken">Cancellation token for cooperative cancellation</param>
        /// <returns>Task returning true if lock acquired successfully, false on timeout/cancellation</returns>
        public async Task<bool> AcquireExclusiveLockAsync(int timeoutMs = DefaultTimeoutMs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (disposed)
                {
                    Main.logger?.Warn(1, "FileLockHelper.AcquireExclusiveLockAsync: Object is disposed");
                    return false;
                }

                if (isLocked)
                {
                    Main.logger?.Warn(2, "FileLockHelper.AcquireExclusiveLockAsync: Lock already acquired");
                    return true;
                }

                Main.logger?.Msg(3, string.Format("{0} FileLockHelper.AcquireExclusiveLockAsync: Attempting async exclusive lock on {1}", FILE_LOCK_PREFIX, lockFilePath));

                var startTime = DateTime.Now;
                var timeout = TimeSpan.FromMilliseconds(timeoutMs);

                while (DateTime.Now - startTime < timeout && !cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        lock (lockObject)
                        {
                            if (!isLocked)
                            {
                                lockStream = new FileStream(
                                    lockFilePath,
                                    FileMode.OpenOrCreate,
                                    FileAccess.ReadWrite,
                                    FileShare.None,
                                    DEFAULT_FILE_BUFFER_SIZE,
                                    FileOptions.None
                                );
                                isLocked = true;
                                Main.logger?.Msg(2, string.Format("{0} FileLockHelper.AcquireExclusiveLockAsync: Successfully acquired async exclusive lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                                return true;
                            }
                        }
                    }
                    catch (IOException)
                    {
                        // File is locked, wait and retry
                        await Task.Delay(RetryDelayMs, cancellationToken).ConfigureAwait(false);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Permission denied, wait and retry
                        await Task.Delay(RetryDelayMs, cancellationToken).ConfigureAwait(false);
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    Main.logger?.Warn(1, string.Format("{0} FileLockHelper.AcquireExclusiveLockAsync: Cancelled acquiring exclusive lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                }
                else
                {
                    Main.logger?.Warn(1, string.Format("{0} FileLockHelper.AcquireExclusiveLockAsync: Timeout acquiring exclusive lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                }

                return false;
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} FileLockHelper.AcquireExclusiveLockAsync: Caught exception: {1}\n{2}", FILE_LOCK_PREFIX, ex.Message, ex.StackTrace));
                return false;
            }
        }

        /// <summary>
        /// Releases the current lock.
        /// </summary>
        public void ReleaseLock()
        {
            try
            {
                lock (lockObject)
                {
                    if (isLocked && lockStream != null)
                    {
                        Main.logger?.Msg(3, string.Format("{0} FileLockHelper.ReleaseLock: Releasing lock on {1}", FILE_LOCK_PREFIX, lockFilePath));
                        lockStream.Dispose();
                        lockStream = null;
                        isLocked = false;

                        // Clean up lock file if it exists
                        try
                        {
                            if (File.Exists(lockFilePath))
                            {
                                File.Delete(lockFilePath);
                                Main.logger?.Msg(3, string.Format("{0} FileLockHelper.ReleaseLock: Deleted lock file {1}", FILE_LOCK_PREFIX, lockFilePath));
                            }
                        }
                        catch (Exception deleteEx)
                        {
                            Main.logger?.Warn(1, string.Format("{0} FileLockHelper.ReleaseLock: Could not delete lock file {1}: {2}", FILE_LOCK_PREFIX, lockFilePath, deleteEx.Message));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("{0} FileLockHelper.ReleaseLock: Caught exception: {1}\n{2}", FILE_LOCK_PREFIX, ex.Message, ex.StackTrace));
                // Force cleanup in case of catastrophic failure
                isLocked = false;
                lockStream = null;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                try
                {
                    Main.logger?.Msg(3, string.Format("{0} FileLockHelper.Dispose: Disposing lock helper for {1}", FILE_LOCK_PREFIX, lockFilePath));
                    ReleaseLock();
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("{0} FileLockHelper.Dispose: Caught exception: {1}\n{2}", FILE_LOCK_PREFIX, ex.Message, ex.StackTrace));
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}