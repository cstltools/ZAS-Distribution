using System;
using System.IO;

namespace Library.DAL.InternalCls
{
    // No logging framework exists in this codebase yet. This is a minimal file-based logger
    // so database failures swallowed by SaveAction/UpdateAction are at least observable on disk
    // instead of silently vanishing. Logging failures are swallowed on purpose - a broken log
    // must never take down the original database call.
    internal static class ClsErrorLogger
    {
        private static readonly string LogFilePath =
            AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/') + "\\App_Data\\DatabaseErrors.log";

        internal static void Log(string operation, string storeProcedureName, Exception ex)
        {
            try
            {
                string directory = Path.GetDirectoryName(LogFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string entry = string.Format(
                    "{0:yyyy-MM-dd HH:mm:ss.fff}\tOperation={1}\tStoredProcedure={2}\t{3}\r\n",
                    DateTime.Now, operation, storeProcedureName, ex);

                File.AppendAllText(LogFilePath, entry);
            }
            catch
            {
                // Logging must never mask the original database failure.
            }
        }
    }
}
