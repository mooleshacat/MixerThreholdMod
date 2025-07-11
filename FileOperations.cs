using MelonLoader.Utils;
using ScheduleOne.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MixerThreholdMod_0_0_1
{
    internal class FileOperations
    {
        public static readonly string LockFilePath = Path.Combine(MelonEnvironment.UserDataDirectory, ".mixersavelock").Replace('/', '\\');
        public static readonly string TempFilePath = Path.Combine(MelonEnvironment.UserDataDirectory, ".mixersave.tmp").Replace('/', '\\');
        public const int DefaultTimeoutMs = 1000;
        public const int MaxRetries = 3;
        public const int RetryDelayMs = 500;

        /// <summary>
        /// Writes text to a file with exclusive lock, async with retry.
        /// </summary>
        public static async Task SafeWriteAllTextAsync(string path, string output, CancellationToken ct = default)
        {
            string tempFile = null;
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    using (var locker = new FileLockHelper(LockFilePath))
                    {
                        bool acquired = await locker.AcquireExclusiveLockAsync(ct, DefaultTimeoutMs);
                        if (!acquired)
                        {
                            if (attempt < MaxRetries)
                            {
                                Main.logger.Warn(1, $"FileOperations.SafeWriteAllText: Failed to acquire exclusive lock for [{path}] (attempt {attempt}). Retrying...");
                                await Task.Delay(RetryDelayMs * attempt, ct);
                                continue;
                            }
                            else
                            {
                                Main.logger.Warn(1, $"FileOperations.SafeWriteAllText: Failed to acquire exclusive lock for [{path}] after {MaxRetries} attempts.");
                                return;
                            }
                        }

                        // Write to temp file first
                        tempFile = Path.GetTempFileName();
                        using (var writer = new StreamWriter(tempFile, false, Encoding.UTF8))
                        {
                            await writer.WriteAsync(output);
                        }
                        // Safely move to final destination
                        SafeMove(tempFile, path);
                    }
                    return; // Success
                }
                catch (Exception ex) when (!ct.IsCancellationRequested)
                {
                    Main.logger.Warn(1, $"Caught exception while writing to [{path}] in FileOperations.SafeWriteAllTextAsync: {ex}");
                    if (tempFile != null && File.Exists(tempFile))
                        File.Delete(tempFile);
                    await Task.Delay(RetryDelayMs * attempt, ct);
                }
            }
        }

        /// <summary>
        /// Reads text from a file with shared lock, async with retry.
        /// </summary>
        public static async Task<string> SafeReadAllTextAsync(string path, CancellationToken ct = default)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    using (var locker = new FileLockHelper(LockFilePath))
                    {
                        bool acquired = await locker.AcquireSharedLockAsync(ct, DefaultTimeoutMs);
                        if (!acquired)
                        {
                            if (attempt < MaxRetries)
                            {
                                Main.logger.Warn(1, $"FileOperations.SafeReadAllText: Failed to acquire shared lock for [{path}] (attempt {attempt}). Retrying...");
                                await Task.Delay(RetryDelayMs * attempt, ct);
                                continue;
                            }
                            else
                            {
                                Main.logger.Warn(1, $"FileOperations.SafeReadAllText: Failed to acquire shared lock for [{path}] after {MaxRetries} attempts.");
                                return null;
                            }
                        }
                        using (var reader = new StreamReader(path))
                        {
                            return await reader.ReadToEndAsync();
                        }
                    }
                }
                catch (Exception ex) when (!ct.IsCancellationRequested)
                {
                    Main.logger.Warn(1, $"Caught exception while reading from [{path}] in FileOperations.SafeReadAllTextAsync: {ex}");
                    await Task.Delay(RetryDelayMs * attempt, ct);
                }
            }

            return null;
        }
        /// <summary>
        /// Writes text to a file with exclusive lock (sync version).
        /// </summary>
        public static void SafeWriteAllText(string path, string output)
        {
            SafeWriteAllTextAsync(path, output).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Reads text from a file with shared lock (sync version).
        /// </summary>
        public static string SafeReadAllText(string path)
        {
            return SafeReadAllTextAsync(path).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Safe write text to a file with cancellation.
        /// </summary>
        public static async Task SafeWriteAllTextWithCancellationAsync(
            string path,
            string output,
            CancellationToken ct,
            Action<int, string> logger = null)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                if (ct.IsCancellationRequested)
                {
                    logger?.Invoke(1, $"Operation canceled during SafeWriteAllTextWithCancellation for [{path}]");
                    ct.ThrowIfCancellationRequested();
                }

                try
                {
                    using (var locker = new FileLockHelper(LockFilePath))
                    {
                        if (!await locker.AcquireExclusiveLockAsync())
                        {
                            if (attempt < MaxRetries)
                            {
                                logger?.Invoke(1, $"Could not acquire lock for [{path}], retrying...");
                                Thread.Sleep(RetryDelayMs * attempt);
                                continue;
                            }
                            else
                            {
                                logger?.Invoke(1, $"Failed to acquire lock after {MaxRetries} attempts for [{path}]");
                                return;
                            }
                        }

                        File.WriteAllText(path, output);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    logger?.Invoke(1, $"Error writing to file [{path}]: {ex}");
                    if (attempt < MaxRetries)
                    {
                        Thread.Sleep(RetryDelayMs * attempt);
                    }
                }
            }
        }




        /// <summary>
        /// Copies a file with shared lock on source.
        /// </summary>
        /// <summary>
        /// Copies a file with shared lock on source.
        /// </summary>
        public static void SafeCopy(string sourceFile, string targetFile, bool overwrite)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFile) || string.IsNullOrEmpty(targetFile))
                {
                    Main.logger.Warn(1, $"FileOperations.SafeCopy: Invalid file paths - source: {sourceFile}, target: {targetFile}");
                    return;
                }

                if (!File.Exists(sourceFile))
                {
                    Main.logger.Warn(1, $"FileOperations.SafeCopy: Source file does not exist: {sourceFile}");
                    return;
                }

                string lockFile = sourceFile + ".lock";
                using (var locker = new FileLockHelper(lockFile))
                {
                    bool lockAcquired = locker.AcquireSharedLock();
                    if (!lockAcquired)
                    {
                        // Try once more
                        lockAcquired = locker.AcquireSharedLock();
                    }

                    if (!lockAcquired)
                    {
                        Main.logger.Warn(1, $"FileOperations.SafeCopy: Could not acquire shared lock on [{sourceFile}] for copying.");
                        return;
                    }

                    string targetDir = Path.GetDirectoryName(targetFile);
                    if (!string.IsNullOrEmpty(targetDir))
                    {
                        Utils.EnsureDirectoryExists(targetDir, "SafeCopy target directory");
                    }

                    File.Copy(sourceFile, targetFile, overwrite);
                    Main.logger.Msg(3, $"FileOperations.SafeCopy: Successfully copied {sourceFile} to {targetFile}");
                }
            }
            catch (Exception ex)
            {
                Main.logger.Warn(1, $"Caught exception while copying [{sourceFile} → {targetFile}] in FileOperations.SafeCopy: {ex.Message}\n{ex.StackTrace}");
            }
        }


        /// <summary>
        /// Safely deletes a file.
        /// </summary>
        public static void SafeDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Main.logger.Warn(1, $"Caught exception while deleting [{path}] in FileOperations.SafeDelete: {ex}");
            }
        }

        /// <summary>
        /// Safely moves a file using a temporary file as buffer.
        /// </summary>
        public static void SafeMove(string sourceFile, string targetFile)
        {
            try
            {
                SafeCopy(sourceFile, targetFile, true);
                SafeDelete(sourceFile);
            }
            catch (Exception ex)
            {
                Main.logger.Warn(1, $"caught exception while moving [{sourceFile} → {targetFile}] in FileOperations.SafeMove: {ex}");
            }
        }

    }
}
