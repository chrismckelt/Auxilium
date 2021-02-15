using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Auxilium.Core.Utilities
{
    public static class AuthUtil
    {
        public static string GetTokenFromConsole()
        {
            string cmd = "$(az account get-access-token --query 'accessToken' -o tsv)";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            bool isLinux = RuntimeInformation
                .IsOSPlatform(OSPlatform.Linux);
            if (isLinux)
            {
                startInfo.FileName = @"/bin/bash";
                startInfo.Arguments = $"-c \"echo {cmd}\"";
            }
            else
            {
                startInfo.FileName = @"powershell.exe";
                startInfo.Arguments = $@"-Command echo {cmd}";
            }
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            if (!string.IsNullOrEmpty(output))
            {
                string trim = output.Trim();
                Console.WriteLine("TOKEN");
                Console.WriteLine(trim);
                Consoler.TitleEnd("Login GetTokenFromConsole");
                return trim;
            }

            string errors = process.StandardError.ReadToEnd();

            throw new ApplicationException(errors);
        }


    }
}
