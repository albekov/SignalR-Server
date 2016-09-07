using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Extensions.PlatformAbstractions;

namespace Microsoft.AspNetCore.SignalR.Testing.Common
{
    public static class Utils
    {
        public static string GetSolutionDir(string slnFileName = null)
        {
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                if (string.IsNullOrEmpty(slnFileName))
                {
                    if (directoryInfo.EnumerateFiles("*.sln").Any())
                    {
                        return directoryInfo.FullName;
                    }
                }
                else
                {
                    if (File.Exists(Path.Combine(directoryInfo.FullName, slnFileName)))
                    {
                        return directoryInfo.FullName;
                    }
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new InvalidOperationException($"Solution root could not be found using {applicationBasePath}");
        }

        // TODO: remove/hardcode runner?
        // TODO: can we commit run-jasmine?
        public static int RunPhantomJS(string runner, string tests)
        {
            // TODO: check if phantom installed globally
            var phantomJsPath = FindPhantomJS();
            return RunProgram(phantomJsPath, runner, tests);
        }

        private static string FindPhantomJS()
        {
            var solutionDir = GetSolutionDir();

            var phantomJsPath = Path.Combine(solutionDir, "bin", "nodejs", "phantomjs");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                phantomJsPath = Path.ChangeExtension(phantomJsPath, "cmd");
            }

            if (!File.Exists(phantomJsPath))
            {
                throw new FileNotFoundException("Cannot find phantomjs.", phantomJsPath);
            }

            return phantomJsPath;
        }

        private static int RunProgram(string name, params string[] args)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = name,
                Arguments = string.Join(" ", args),
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var process = Process.Start(processStartInfo);
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Console.WriteLine(output.Trim());
            }

            return process.ExitCode;
        }

    }
}
