using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public static async Task<string> GetBearer(string tenant, string appId, string password)
        {
            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", appId),
                new KeyValuePair<string, string>("client_secret", password),
                new KeyValuePair<string, string>("resource", "https://management.azure.com/")
            };

            var url = $"https://login.microsoftonline.com/{tenant}/oauth2/token";

            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new FormUrlEncodedContent(nvc)
                };

                var res = await client.SendAsync(req);
                var jsonString = await res.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<dynamic>(jsonString);
                return json.access_token;
            }
        }
    }
}
