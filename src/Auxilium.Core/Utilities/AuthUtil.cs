using System;
using System.Diagnostics;

namespace Auxilium.Core.Utilities
{
    public static class AuthUtil
    {
        public static string GetTokenFromConsole()
        {
            Consoler.TitleStart("Login GetTokenFromConsole");
            string ps = "$(az account get-access-token --query 'accessToken' -o tsv)";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = $@"-Command echo {ps}";
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
