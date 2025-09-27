using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BetterFormat
{
    public class FormatHelper
    {
        public static Task FormatDriveAsync(string driveLetter, string fileSystem, string volumeLabel, bool quickFormat, uint clusterSize)
        {
            return Task.Run(() =>
            {
                string drive = driveLetter.Substring(0, 2);
                string args = $"/C format {drive} /FS:{fileSystem}";

                if (quickFormat)
                {
                    args += " /Q";
                }

                if (!string.IsNullOrEmpty(volumeLabel))
                {
                    args += $" /V:{volumeLabel}";
                }

                if (clusterSize > 0)
                {
                    args += $" /A:{clusterSize}";
                }

                // Force format without user prompt in the console
                args += " /Y";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception($"格式化失败。退出代码: {process.ExitCode}. 错误信息: {error}");
                    }
                }
            });
        }
    }
}
