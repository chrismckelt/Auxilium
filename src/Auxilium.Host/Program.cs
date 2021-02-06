using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Auxilium.Core;
using Auxilium.Core.LogicApps;
using Newtonsoft.Json;


namespace Auxilium.Host
{
    internal class Program
    {
        private static Extractor _extractor;
        /*
            Console App To Query Azure using .Net SDK & custom extension to analyse and configure monitoring and alerts
        */
      
        const string ExportFolder = @"c:\temp\logic-app-extracts\";

        static IApiClient Client { get; set; }

        public static async Task Main(string[] args)
        {
            Console.WriteLine("--------- Start ---------");
            await Run(args).ConfigureAwait(true);

            Console.WriteLine("--------- Completed ---------");
            Console.ReadLine();
        }

        private static async Task Run(string[] args)
        {
            _extractor = new Extractor();
            _extractor.Data = LoadDataFromDisk();
            var max = _extractor.Data.Select(x => x.StartTimeUtc).Max();
            await _extractor.Run(max);
            await  Export();
        }
        private static List<LogicAppExtract> LoadDataFromDisk()
        {
           var data = new List<LogicAppExtract>();
            foreach (var f in Directory.GetFiles(ExportFolder, "*.json"))
            {
                var content = System.IO.File.ReadAllText(f);
                if (!string.IsNullOrEmpty(content))
                {
                    var list = JsonConvert.DeserializeObject<LogicAppExtract[]>(content);
                    foreach (var d in list)
                    {
                        if (!data.Any(x => x.RunId == d.ActionName && x.ActionName == d.ActionName))
                        {
                            data.Add(d);
                        }
                    }

                }
            }

            return data;
        }

        private static async Task Export()
        {
            foreach (var logicAppName in _extractor.Data.Select(x=>x.LogicAppName).Distinct())
            {
                var exportItems = new List<LogicAppExtract>();
                string file = Path.Combine(ExportFolder, $"{logicAppName}.json");
                if (File.Exists(file))
                {
                    string content = await System.IO.File.ReadAllTextAsync(file);
                    var list = JsonConvert.DeserializeObject<LogicAppExtract[]>(content);
                    if (list.Any())
                    {
                        var unique = list.Except(_extractor.Data.Where(x=>x.LogicAppName== logicAppName));
                        exportItems.AddRange(unique);
                    }
                    else
                    {
                        exportItems.AddRange(_extractor.Data);
                    }
                }
             
                exportItems.SaveToFile(file);
            }
        }

    }
};