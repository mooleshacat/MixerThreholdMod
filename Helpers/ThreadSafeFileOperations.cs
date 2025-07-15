using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Thread-safe file operations for .NET 4.8.1 compatibility.
    /// Provides atomic file operations with proper error handling.
    /// 
    /// ⚠️ THREAD SAFETY: All file operations are thread-safe and use proper locking.
    /// ⚠️ MAIN THREAD WARNING: These operations should not be called from Unity's main thread.
    /// 
    /// .NET 4.8.1 Compatibility:
    /// - Uses Task.Run instead of newer async patterns
    /// - Compatible exception handling
    /// - Proper file locking mechanisms
    /// </summary>
    public static class ThreadSafeFileOperations
    {
        private static readonly object _fileLock = new object();

        /// <summary>
        /// Safely read all text from a file asynchronously
        /// </summary>
        public static async Task<string> SafeReadAllTextAsync(string filePath, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                Main.logger?.Msg(3, string.Format("SafeReadAllTextAsync: Reading from {0}", filePath));

                return await Task.Run(() =>
                {
                    lock (_fileLock)
                    {
                        if (!File.Exists(filePath))
                        {
                            Main.logger?.Warn(2, string.Format("SafeReadAllTextAsync: File does not exist: {0}", filePath));
                            return string.Empty;
                        }

                        return File.ReadAllText(filePath);
                    }
                }, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeReadAllTextAsync error for {0}: {1}", filePath, ex));
                throw;
            }
        }

        /// <summary>
        /// Safely write all text to a file asynchronously with atomic operation
        /// </summary>
        public static async Task SafeWriteAllTextAsync(string filePath, string content, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                Main.logger?.Msg(3, string.Format("SafeWriteAllTextAsync: Writing to {0}", filePath));

                await Task.Run(() =>
                {
                    lock (_fileLock)
                    {
                        string tempFile = filePath + ".tmp";

                        // Atomic write operation
                        File.WriteAllText(tempFile, content);

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        File.Move(tempFile, filePath);
                    }
                }, ct).ConfigureAwait(false);

                Main.logger?.Msg(3, string.Format("SafeWriteAllTextAsync: Successfully wrote to {0}", filePath));
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeWriteAllTextAsync error for {0}: {1}", filePath, ex));
                throw;
            }
        }

        /// <summary>
        /// Safely copy a file with overwrite option
        /// </summary>
        public static void SafeCopy(string sourceFile, string destFile, bool overwrite = false)
        {
            try
            {
                Main.logger?.Msg(3, string.Format("SafeCopy: Copying {0} to {1}, overwrite: {2}", sourceFile, destFile, overwrite));

                lock (_fileLock)
                {
                    if (!File.Exists(sourceFile))
                    {
                        Main.logger?.Warn(2, string.Format("SafeCopy: Source file does not exist: {0}", sourceFile));
                        return;
                    }

                    // Ensure destination directory exists
                    string destDir = Path.GetDirectoryName(destFile);
                    if (!Directory.Exists(destDir))
                    {
                        Directory.CreateDirectory(destDir);
                    }

                    File.Copy(sourceFile, destFile, overwrite);
                }

                Main.logger?.Msg(3, string.Format("SafeCopy: Successfully copied {0} to {1}", sourceFile, destFile));
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeCopy error copying {0} to {1}: {2}", sourceFile, destFile, ex));
                throw;
            }
        }

        /// <summary>
        /// Check if a file exists safely
        /// </summary>
        public static bool SafeFileExists(string filePath)
        {
            try
            {
                return File.Exists(filePath);
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeFileExists error for {0}: {1}", filePath, ex));
                return false;
            }
        }

        /// <summary>
        /// Safely delete a file if it exists
        /// </summary>
        public static bool SafeDelete(string filePath)
        {
            try
            {
                lock (_fileLock)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Main.logger?.Msg(3, string.Format("SafeDelete: Deleted {0}", filePath));
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Main.logger?.Err(string.Format("SafeDelete error for {0}: {1}", filePath, ex));
                return false;
            }
        }
    }
}