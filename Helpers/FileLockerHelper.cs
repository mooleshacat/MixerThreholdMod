using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MixerThreholdMod_0_0_1
{
    public class FileLockHelper : IDisposable
    {
        private readonly string _lockFilePath;
        private FileStream _lockStream;
        private volatile bool _isLocked;
        private readonly object _lockObject = new object();
        private const int DefaultTimeoutMs = 5000;

        public FileLockHelper(string lockFilePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lockFilePath))
                    throw new ArgumentException("Lock file path cannot be null or empty", nameof(lockFilePath));

                _lockFilePath = lockFilePath;
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"FileLockHelper constructor failed: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Attempts to acquire a shared (read) lock with timeout.
        /// </summary>
        public bool AcquireSharedLock(int timeoutMs = DefaultTimeoutMs)
        {
            try
            {
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
                    return false;
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"AcquireSharedLock: Caught exception: {ex.Message}\n{ex.StackTrace}");
                try
                {
                    ReleaseLock();
                }
                catch (Exception releaseEx)
                {
                    Main.logger?.Err($"Error during lock release in exception handler: {releaseEx.Message}");
                }
                return false;
            }
        }

        /// <summary>
        /// Attempts to acquire an exclusive (write) lock with timeout.
        /// </summary>
        public bool AcquireExclusiveLock(int timeoutMs = DefaultTimeoutMs)
        {
            try
            {
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
                    return false;
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"AcquireExclusiveLock: Caught exception: {ex.Message}\n{ex.StackTrace}");
                try
                {
                    ReleaseLock();
                }
                catch (Exception releaseEx)
                {
                    Main.logger?.Err($"Error during lock release in exception handler: {releaseEx.Message}");
                }
                return false;
            }
        }

        /// <summary>
        /// Asynchronously acquires a shared (read) lock with timeout.
        /// </summary>
        public async Task<bool> AcquireSharedLockAsync(int timeoutMs = DefaultTimeoutMs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
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
                return false;
            }
        }

        /// <summary>
        /// Asynchronously acquires an exclusive (write) lock with timeout.
        /// </summary>
        public async Task<bool> AcquireExclusiveLockAsync(int timeoutMs = DefaultTimeoutMs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
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
                lock (_lockObject)
                {
                    if (_isLocked && _lockStream != null)
                    {
                        _lockStream.Dispose();
                        _lockStream = null;
                        _isLocked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"ReleaseLock: Caught exception: {ex.Message}\n{ex.StackTrace}");
                // Force cleanup in case of catastrophic failure
                _isLocked = false;
                _lockStream = null;
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
                Main.logger?.Err($"Dispose: Caught exception: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}