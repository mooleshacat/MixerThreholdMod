using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

<<<<<<< HEAD
<<<<<<< HEAD
=======
<<<<<<<< HEAD:Helpers/FileLockerHelper.cs
namespace MixerThreholdMod_0_0_1
{
========
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
    public class FileLockHelper : IDisposable
    {
        private readonly string lockFilePath;
        private FileStream lockStream;
        private volatile bool isLocked;
        private readonly object lockObject = new object();
        private bool disposed = false;
        private const int DefaultTimeoutMs = 5000;
        private const int RetryDelayMs = 50;
<<<<<<< HEAD
=======
>>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025):Helpers/SafeFileLockingSystem.cs
    public class FileLockHelper : IDisposable
    {
        private readonly string _lockFilePath;
        private FileStream _lockStream;
        private volatile bool _isLocked;
        private readonly object _lockObject = new object();
        private const int DefaultTimeoutMs = 5000;
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)

        public FileLockHelper(string lockFilePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lockFilePath))
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                {
                    Main.logger?.Err("FileLockHelper constructor: lockFilePath is null or empty");
                    throw new ArgumentException("Lock file path cannot be null or empty", "lockFilePath");
                }

                this.lockFilePath = lockFilePath;
                Main.logger?.Msg(3, string.Format("FileLockHelper created for: {0}", lockFilePath));
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("FileLockHelper constructor failed: {0}\n{1}", ex.Message, ex.StackTrace));
<<<<<<< HEAD
=======
                    throw new ArgumentException("Lock file path cannot be null or empty", nameof(lockFilePath));

                _lockFilePath = lockFilePath;
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"FileLockHelper constructor failed: {ex.Message}\n{ex.StackTrace}");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                throw;
            }
        }

        /// <summary>
        /// Attempts to acquire a shared (read) lock with timeout.
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        /// 
        /// ⚠️ MAIN THREAD WARNING: This method uses Thread.Sleep and blocks the calling thread.
        /// Do NOT call from main thread. Use AcquireSharedLockAsync() for main thread safety.
        /// 
        /// Thread Safety: Uses proper locking mechanisms but blocks calling thread during retries.
        /// </summary>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>True if lock acquired successfully, false on timeout or error</returns>
<<<<<<< HEAD
=======
        /// </summary>
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        public bool AcquireSharedLock(int timeoutMs = DefaultTimeoutMs)
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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

                    Main.logger?.Msg(3, string.Format("FileLockHelper.AcquireSharedLock: Attempting shared lock on {0}", lockFilePath));

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
                                4096,
                                FileOptions.None
                            );
                            isLocked = true;
                            Main.logger?.Msg(2, string.Format("FileLockHelper.AcquireSharedLock: Successfully acquired shared lock on {0}", lockFilePath));
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

                    Main.logger?.Warn(1, string.Format("FileLockHelper.AcquireSharedLock: Timeout acquiring shared lock on {0}", lockFilePath));
<<<<<<< HEAD
=======
                lock (_lockObject)
                {
                    if (_isLocked)
                    {
                        Main.logger?.Warn(2, "Lock already acquired");
                        return true;
                    }

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
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    return false;
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
<<<<<<< HEAD
                Main.logger?.Err(string.Format("FileLockHelper.AcquireSharedLock: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
=======
                Main.logger?.Err($"AcquireSharedLock: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
                Main.logger?.Err(string.Format("FileLockHelper.AcquireSharedLock: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                try
                {
                    ReleaseLock();
                }
                catch (Exception releaseEx)
                {
<<<<<<< HEAD
<<<<<<< HEAD
                    Main.logger?.Err(string.Format("Error during lock release in exception handler: {0}", releaseEx.Message));
=======
                    Main.logger?.Err($"Error during lock release in exception handler: {releaseEx.Message}");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
                    Main.logger?.Err(string.Format("Error during lock release in exception handler: {0}", releaseEx.Message));
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }
                return false;
            }
        }

        /// <summary>
        /// Attempts to acquire an exclusive (write) lock with timeout.
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        /// 
        /// ⚠️ MAIN THREAD WARNING: This method uses Thread.Sleep and blocks the calling thread.
        /// Do NOT call from main thread. Use AcquireExclusiveLockAsync() for main thread safety.
        /// 
        /// Thread Safety: Uses proper locking mechanisms but blocks calling thread during retries.
        /// </summary>
        /// <param name="timeoutMs">Timeout in milliseconds</param>
        /// <returns>True if lock acquired successfully, false on timeout or error</returns>
<<<<<<< HEAD
=======
        /// </summary>
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        public bool AcquireExclusiveLock(int timeoutMs = DefaultTimeoutMs)
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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

                    Main.logger?.Msg(3, string.Format("FileLockHelper.AcquireExclusiveLock: Attempting exclusive lock on {0}", lockFilePath));

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
                                4096,
                                FileOptions.None
                            );
                            isLocked = true;
                            Main.logger?.Msg(2, string.Format("FileLockHelper.AcquireExclusiveLock: Successfully acquired exclusive lock on {0}", lockFilePath));
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

                    Main.logger?.Warn(1, string.Format("FileLockHelper.AcquireExclusiveLock: Timeout acquiring exclusive lock on {0}", lockFilePath));
<<<<<<< HEAD
=======
                lock (_lockObject)
                {
                    if (_isLocked)
                    {
                        Main.logger?.Warn(2, "Lock already acquired");
                        return true;
                    }

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
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    return false;
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
<<<<<<< HEAD
                Main.logger?.Err(string.Format("FileLockHelper.AcquireExclusiveLock: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
=======
                Main.logger?.Err($"AcquireExclusiveLock: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
                Main.logger?.Err(string.Format("FileLockHelper.AcquireExclusiveLock: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                try
                {
                    ReleaseLock();
                }
                catch (Exception releaseEx)
                {
<<<<<<< HEAD
<<<<<<< HEAD
                    Main.logger?.Err(string.Format("Error during lock release in exception handler: {0}", releaseEx.Message));
=======
                    Main.logger?.Err($"Error during lock release in exception handler: {releaseEx.Message}");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
                    Main.logger?.Err(string.Format("Error during lock release in exception handler: {0}", releaseEx.Message));
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                }
                return false;
            }
        }

        /// <summary>
        /// Asynchronously acquires a shared (read) lock with timeout.
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
=======
        /// </summary>
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        public async Task<bool> AcquireSharedLockAsync(int timeoutMs = DefaultTimeoutMs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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

                Main.logger?.Msg(3, string.Format("FileLockHelper.AcquireSharedLockAsync: Attempting async shared lock on {0}", lockFilePath));

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
                                    4096,
                                    FileOptions.None
                                );
                                isLocked = true;
                                Main.logger?.Msg(2, string.Format("FileLockHelper.AcquireSharedLockAsync: Successfully acquired async shared lock on {0}", lockFilePath));
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
                    Main.logger?.Warn(1, string.Format("FileLockHelper.AcquireSharedLockAsync: Cancelled acquiring shared lock on {0}", lockFilePath));
                }
                else
                {
                    Main.logger?.Warn(1, string.Format("FileLockHelper.AcquireSharedLockAsync: Timeout acquiring shared lock on {0}", lockFilePath));
                }

                return false;
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("FileLockHelper.AcquireSharedLockAsync: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
<<<<<<< HEAD
=======
                if (_isLocked)
                {
                    Main.logger?.Warn(2, "Lock already acquired");
                    return true;
                }

                using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    cts.CancelAfter(timeoutMs);
                    var ct = cts.Token;

                    var tcs = new TaskCompletionSource<bool>();
                    using (var timer = new Timer(_ => tcs.TrySetCanceled(), null, timeoutMs, Timeout.Infinite))
                    {
                        await Task.Run(async () =>
                        {
                            while (!ct.IsCancellationRequested)
                            {
                                try
                                {
                                    lock (_lockObject)
                                    {
                                        if (!_isLocked)
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
                                        }
                                    }
                                    timer.Dispose();
                                    tcs.TrySetResult(true);
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    Main.logger?.Warn(3, $"Retry shared lock attempt: {ex.Message}");
                                    await Task.Delay(100, ct).ConfigureAwait(false);
                                }
                            }
                            timer.Dispose();
                            tcs.TrySetCanceled();
                        }, ct).ConfigureAwait(false);

                        return await tcs.Task.ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"AcquireSharedLockAsync: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                return false;
            }
        }

        /// <summary>
        /// Asynchronously acquires an exclusive (write) lock with timeout.
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
=======
        /// </summary>
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
        public async Task<bool> AcquireExclusiveLockAsync(int timeoutMs = DefaultTimeoutMs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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

                Main.logger?.Msg(3, string.Format("FileLockHelper.AcquireExclusiveLockAsync: Attempting async exclusive lock on {0}", lockFilePath));

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
                                    4096,
                                    FileOptions.None
                                );
                                isLocked = true;
                                Main.logger?.Msg(2, string.Format("FileLockHelper.AcquireExclusiveLockAsync: Successfully acquired async exclusive lock on {0}", lockFilePath));
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
                    Main.logger?.Warn(1, string.Format("FileLockHelper.AcquireExclusiveLockAsync: Cancelled acquiring exclusive lock on {0}", lockFilePath));
                }
                else
                {
                    Main.logger?.Warn(1, string.Format("FileLockHelper.AcquireExclusiveLockAsync: Timeout acquiring exclusive lock on {0}", lockFilePath));
                }

                return false;
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("FileLockHelper.AcquireExclusiveLockAsync: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
<<<<<<< HEAD
=======
                if (_isLocked)
                {
                    Main.logger?.Warn(2, "Lock already acquired");
                    return true;
                }

                using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    cts.CancelAfter(timeoutMs);
                    var ct = cts.Token;

                    var tcs = new TaskCompletionSource<bool>();
                    using (var timer = new Timer(_ => tcs.TrySetCanceled(), null, timeoutMs, Timeout.Infinite))
                    {
                        await Task.Run(async () =>
                        {
                            while (!ct.IsCancellationRequested)
                            {
                                try
                                {
                                    lock (_lockObject)
                                    {
                                        if (!_isLocked)
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
                                        }
                                    }
                                    timer.Dispose();
                                    tcs.TrySetResult(true);
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    Main.logger?.Warn(3, $"Retry exclusive lock attempt: {ex.Message}");
                                    await Task.Delay(100, ct).ConfigureAwait(false);
                                }
                            }
                            timer.Dispose();
                            tcs.TrySetCanceled();
                        }, ct).ConfigureAwait(false);

                        return await tcs.Task.ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"AcquireExclusiveLockAsync: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
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
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                lock (lockObject)
                {
                    if (isLocked && lockStream != null)
                    {
                        Main.logger?.Msg(3, string.Format("FileLockHelper.ReleaseLock: Releasing lock on {0}", lockFilePath));
                        lockStream.Dispose();
                        lockStream = null;
                        isLocked = false;

                        // Clean up lock file if it exists
                        try
                        {
                            if (File.Exists(lockFilePath))
                            {
                                File.Delete(lockFilePath);
                                Main.logger?.Msg(3, string.Format("FileLockHelper.ReleaseLock: Deleted lock file {0}", lockFilePath));
                            }
                        }
                        catch (Exception deleteEx)
                        {
                            Main.logger?.Warn(1, string.Format("FileLockHelper.ReleaseLock: Could not delete lock file {0}: {1}", lockFilePath, deleteEx.Message));
                        }
<<<<<<< HEAD
=======
                lock (_lockObject)
                {
                    if (_isLocked && _lockStream != null)
                    {
                        _lockStream.Dispose();
                        _lockStream = null;
                        _isLocked = false;
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                    }
                }
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
                Main.logger?.Err(string.Format("FileLockHelper.ReleaseLock: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                // Force cleanup in case of catastrophic failure
                isLocked = false;
                lockStream = null;
<<<<<<< HEAD
=======
                Main.logger?.Err($"ReleaseLock: Caught exception: {ex.Message}\n{ex.StackTrace}");
                // Force cleanup in case of catastrophic failure
                _isLocked = false;
                _lockStream = null;
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            }
        }

        public void Dispose()
        {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            if (!disposed)
            {
                try
                {
                    Main.logger?.Msg(3, string.Format("FileLockHelper.Dispose: Disposing lock helper for {0}", lockFilePath));
                    ReleaseLock();
                }
                catch (Exception ex)
                {
                    Main.logger?.Err(string.Format("FileLockHelper.Dispose: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                }
                finally
                {
                    disposed = true;
                }
<<<<<<< HEAD
=======
            try
            {
                ReleaseLock();
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"Dispose: Caught exception: {ex.Message}\n{ex.StackTrace}");
>>>>>>> bd55758 (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
=======
>>>>>>> c6170fc (Merge branch 'copilot/fix-7f635d0c-3e41-4d2d-ba44-3f2ddfc5a4c6' into copilot/fix-6fb822ce-3d96-449b-9617-05ee31c54025)
            }
        }
    }
}