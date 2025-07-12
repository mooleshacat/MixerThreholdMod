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
                    throw new ArgumentException("Path cannot be null or empty", nameof(path));
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
                        Main.logger?.Warn(1, $"FileOperations.SafeWriteAllTextAsync: Could not acquire exclusive lock on [{path}]");
                        throw new TimeoutException($"Could not acquire exclusive lock on file: {path}");
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"SafeWriteAllTextAsync: Caught exception for [{path}]: {ex.Message}\n{ex.StackTrace}");
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
                    throw new ArgumentException("Path cannot be null or empty", nameof(path));
                }

                if (!File.Exists(path))
                {
                    Main.logger?.Warn(2, $"SafeReadAllTextAsync: File does not exist: {path}");
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
                        Main.logger?.Warn(1, $"FileOperations.SafeReadAllTextAsync: Could not acquire shared lock on [{path}]");
                        throw new TimeoutException($"Could not acquire shared lock on file: {path}");
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"SafeReadAllTextAsync: Caught exception for [{path}]: {ex.Message}\n{ex.StackTrace}");
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
                Main.logger?.Err($"SafeWriteAllText: Caught exception: {ex.Message}\n{ex.StackTrace}");
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
                Main.logger?.Err($"SafeReadAllText: Caught exception: {ex.Message}\n{ex.StackTrace}");
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
                Main.logger?.Msg(2, $"Write operation canceled for: {path}");
                throw;
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"SafeWriteAllTextWithCancellationAsync: Caught exception: {ex.Message}\n{ex.StackTrace}");
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
                    throw new ArgumentException("Source file path cannot be null or empty", nameof(sourceFile));
                }

                if (string.IsNullOrWhiteSpace(targetFile))
                {
                    Main.logger?.Warn(1, "SafeCopyAsync: targetFile is null or empty");
                    throw new ArgumentException("Target file path cannot be null or empty", nameof(targetFile));
                }

                if (!File.Exists(sourceFile))
                {
                    Main.logger?.Warn(1, $"SafeCopyAsync: Source file does not exist: {sourceFile}");
                    throw new FileNotFoundException($"Source file not found: {sourceFile}");
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
                        Main.logger?.Warn(1, $"FileOperations.SafeCopyAsync: Could not acquire shared lock on [{sourceFile}] for copying.");
                        throw new TimeoutException($"Could not acquire shared lock on source file: {sourceFile}");
                    }
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err($"SafeCopyAsync: Caught exception copying [{sourceFile}] to [{targetFile}]: {ex.Message}\n{ex.StackTrace}");
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
                Main.logger?.Err($"SafeCopy: Caught exception: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}