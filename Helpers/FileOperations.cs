using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MixerThreholdMod_0_0_1
{
    public static class FileOperations
    {
        private const int DefaultTimeoutMs = 5000;
        private const int BufferSize = 4096;

        /// <summary>
        /// Writes text to a file with exclusive lock (async version).
        /// </summary>
        public static async Task SafeWriteAllTextAsync(string path, string output, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    Main.logger?.Err("SafeWriteAllTextAsync: path is null or empty");
                    throw new ArgumentException("Path cannot be null or empty", "path"); // .NET 4.8.1 compatible
                }

                if (output == null)
                {
                    Main.logger?.Warn(2, "SafeWriteAllTextAsync: output is null, treating as empty string");
                    output = string.Empty;
                }

                // Ensure directory exists
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var locker = new FileLockHelper(path + ".lock"))
                {
                    if (await locker.AcquireExclusiveLockAsync(DefaultTimeoutMs, cancellationToken).ConfigureAwait(false))
                    {
                        // Use async file operations to avoid blocking the main thread
                        using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, FileOptions.SequentialScan))
                        using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                        {
                            await writer.WriteAsync(output).ConfigureAwait(false);
                            await writer.FlushAsync().ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        Main.logger?.Warn(1, string.Format("FileOperations.SafeWriteAllTextAsync: Could not acquire exclusive lock on [{0}]", path));
                        throw new TimeoutException(string.Format("Could not acquire exclusive lock on file: {0}", path));
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeWriteAllTextAsync: Caught exception for [{0}]: {1}\n{2}", path, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Reads text from a file with shared lock (async version).
        /// </summary>
        public static async Task<string> SafeReadAllTextAsync(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    Main.logger?.Err("SafeReadAllTextAsync: path is null or empty");
                    throw new ArgumentException("Path cannot be null or empty", "path"); // .NET 4.8.1 compatible
                }

                if (!File.Exists(path))
                {
                    Main.logger?.Warn(2, string.Format("SafeReadAllTextAsync: File does not exist: {0}", path));
                    return string.Empty;
                }

                using (var locker = new FileLockHelper(path + ".lock"))
                {
                    if (await locker.AcquireSharedLockAsync(DefaultTimeoutMs, cancellationToken).ConfigureAwait(false))
                    {
                        // Use async file operations to avoid blocking the main thread
                        using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOptions.SequentialScan))
                        using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                        {
                            return await reader.ReadToEndAsync().ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        Main.logger?.Warn(1, string.Format("FileOperations.SafeReadAllTextAsync: Could not acquire shared lock on [{0}]", path));
                        throw new TimeoutException(string.Format("Could not acquire shared lock on file: {0}", path));
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeReadAllTextAsync: Caught exception for [{0}]: {1}\n{2}", path, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Writes text to a file with exclusive lock (sync version - use sparingly).
        /// </summary>
        public static void SafeWriteAllText(string path, string output)
        {
            try
            {
                // Run on thread pool to avoid blocking main thread
                Task.Run(async () => await SafeWriteAllTextAsync(path, output).ConfigureAwait(false))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeWriteAllText: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Reads text from a file with shared lock (sync version - use sparingly).
        /// </summary>
        public static string SafeReadAllText(string path)
        {
            try
            {
                // Run on thread pool to avoid blocking main thread
                return Task.Run(async () => await SafeReadAllTextAsync(path).ConfigureAwait(false))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeReadAllText: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Writes text to a file with exclusive lock and cancellation support.
        /// </summary>
        public static async Task SafeWriteAllTextWithCancellationAsync(
            string path,
            string output,
            CancellationToken cancellationToken)
        {
            try
            {
                await SafeWriteAllTextAsync(path, output, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                Main.logger?.Msg(2, string.Format("Write operation canceled for: {0}", path));
                throw;
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeWriteAllTextWithCancellationAsync: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Copies a file with shared lock on source.
        /// </summary>
        public static async Task SafeCopyAsync(string sourceFile, string targetFile, bool overwrite, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceFile))
                {
                    Main.logger?.Warn(1, "SafeCopyAsync: sourceFile is null or empty");
                    throw new ArgumentException("Source file path cannot be null or empty", "sourceFile"); // .NET 4.8.1 compatible
                }

                if (string.IsNullOrWhiteSpace(targetFile))
                {
                    Main.logger?.Warn(1, "SafeCopyAsync: targetFile is null or empty");
                    throw new ArgumentException("Target file path cannot be null or empty", "targetFile"); // .NET 4.8.1 compatible
                }

                if (!File.Exists(sourceFile))
                {
                    Main.logger?.Warn(1, string.Format("SafeCopyAsync: Source file does not exist: {0}", sourceFile));
                    throw new FileNotFoundException(string.Format("Source file not found: {0}", sourceFile));
                }

                using (var locker = new FileLockHelper(sourceFile + ".lock"))
                {
                    if (await locker.AcquireSharedLockAsync(DefaultTimeoutMs, cancellationToken).ConfigureAwait(false))
                    {
                        // Ensure target directory exists
                        var targetDir = Path.GetDirectoryName(targetFile);
                        if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }

                        // Perform copy operation on thread pool
                        await Task.Run(() => File.Copy(sourceFile, targetFile, overwrite), cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        Main.logger?.Warn(1, string.Format("FileOperations.SafeCopyAsync: Could not acquire shared lock on [{0}] for copying.", sourceFile));
                        throw new TimeoutException(string.Format("Could not acquire shared lock on source file: {0}", sourceFile));
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeCopyAsync: Caught exception copying [{0}] to [{1}]: {2}\n{3}", sourceFile, targetFile, ex.Message, ex.StackTrace));
                throw;
            }
        }

        /// <summary>
        /// Copies a file with shared lock on source (sync version).
        /// </summary>
        public static void SafeCopy(string sourceFile, string targetFile, bool overwrite)
        {
            try
            {
                Task.Run(async () => await SafeCopyAsync(sourceFile, targetFile, overwrite).ConfigureAwait(false))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeCopy: Caught exception: {0}\n{1}", ex.Message, ex.StackTrace));
                throw;
            }
        }
    }

    // Add the missing FileLockHelper class for .NET 4.8.1 compatibility
    public class FileLockHelper : IDisposable
    {
        private readonly string _lockFilePath;
        private FileStream _lockStream;
        private bool _disposed = false;

        public FileLockHelper(string lockFilePath)
        {
            _lockFilePath = lockFilePath ?? throw new ArgumentNullException("lockFilePath");
        }

        public async Task<bool> AcquireExclusiveLockAsync(int timeoutMs, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await TryAcquireLockAsync(FileAccess.Write, FileShare.None, timeoutMs, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> AcquireSharedLockAsync(int timeoutMs, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await TryAcquireLockAsync(FileAccess.Read, FileShare.Read, timeoutMs, cancellationToken).ConfigureAwait(false);
        }

        private async Task<bool> TryAcquireLockAsync(FileAccess access, FileShare share, int timeoutMs, CancellationToken cancellationToken)
        {
            var startTime = DateTime.Now;
            var timeout = TimeSpan.FromMilliseconds(timeoutMs);

            while (DateTime.Now - startTime < timeout && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _lockStream = new FileStream(_lockFilePath, FileMode.Create, access, share);
                    return true;
                }
                catch (IOException)
                {
                    // File is locked, wait and retry
                    await Task.Delay(50, cancellationToken).ConfigureAwait(false);
                }
                catch (UnauthorizedAccessException)
                {
                    // Permission denied, wait and retry
                    await Task.Delay(50, cancellationToken).ConfigureAwait(false);
                }
            }

            return false;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    _lockStream?.Dispose();
                    if (File.Exists(_lockFilePath))
                    {
                        File.Delete(_lockFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Main.logger?.Warn(1, string.Format("FileLockHelper.Dispose: Error cleaning up lock file: {0}", ex.Message));
                }
                finally
                {
                    _disposed = true;
                }
            }
        }
    }
}