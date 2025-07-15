using System;
using System.IO;
using static MixerThreholdMod_1_0_0.Constants.ModConstants;

namespace MixerThreholdMod_1_0_0.Helpers
{
    /// <summary>
    /// Basic file operations for backward compatibility
    /// ⚠️ THREAD SAFETY: All operations are thread-safe
    /// ⚠️ .NET 4.8.1 Compatible
    /// </summary>
    public static class FileOperations
    {
        /// <summary>
        /// Safe file copy operation
        /// </summary>
        public static bool SafeCopy(string sourceFile, string targetFile, bool overwrite = false)
        {
            Exception copyError = null;
            try
            {
                if (string.IsNullOrEmpty(sourceFile) || string.IsNullOrEmpty(targetFile))
                {
                    Main.logger.Err("[FILE-OPS] SafeCopy: Source or target file path is null/empty");
                    return false;
                }

                if (!File.Exists(sourceFile))
                {
                    Main.logger.Err(string.Format("[FILE-OPS] SafeCopy: Source file does not exist: {0}", sourceFile));
                    return false;
                }

                // Ensure target directory exists
                var targetDir = Path.GetDirectoryName(targetFile);
                if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                File.Copy(sourceFile, targetFile, overwrite);
                Main.logger.Msg(3, string.Format("[FILE-OPS] SafeCopy: Successfully copied {0} to {1}", sourceFile, targetFile));
                return true;
            }
            catch (Exception ex)
            {
                copyError = ex;
                return false;
            }
            finally
            {
                if (copyError != null)
                {
                    Main.logger.Err(string.Format("[FILE-OPS] SafeCopy: Error copying {0} to {1}: {2}",
                        sourceFile, targetFile, copyError.Message));
                }
            }
        }
    }
}