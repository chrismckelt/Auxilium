using System;
using System.Diagnostics;

namespace Auxilium.Host
{
    public static class AuthUtil
    {
        public static void SetAuthToken(string token)
        {
           Environment.SetEnvironmentVariable("azure-token", token, EnvironmentVariableTarget.User);
           
        }
        public static string GetTokenFromConsole()
        {
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
                Console.WriteLine("------------------");
                Console.WriteLine("TOKEN");
                Console.WriteLine("------------------");
                Console.WriteLine(trim);
                return trim;
            }

            string errors = process.StandardError.ReadToEnd();

            throw new ApplicationException(errors);
        }


    }
}
