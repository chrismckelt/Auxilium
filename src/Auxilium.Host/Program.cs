using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Auxilium.Core;
using Auxilium.Core.LogicApps;
using Auxilium.Core.Utilities;
using Microsoft.Azure.Management.ServiceBus.Fluent.Subscription.Definition;
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
            Consoler.TitleStart("Auxilium");
            await Run(args).ConfigureAwait(true);

            Console.WriteLine("--------- Completed ---------");

            if (Debugger.IsAttached)
            {
                Console.ReadLine();
            }
        }

        private static async Task Run(string[] args)
        {
            DateTime? startDate = null;
            if (args.Any())
            {
                startDate = DateTime.Parse(args[0]);
            }

            _extractor = new Extractor();
            _extractor.Data = LoadDataFromDisk();
            //if (!startDate.HasValue) startDate = _extractor.Data.Select(x => x.StartTimeUtc).Max();

            await _extractor.Run(startDate);
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
            var las = _extractor.LogicApps.Select(x => x.Value).Distinct();
            foreach (var logicAppName in las)
            {
                var exportItems = new List<LogicAppExtract>();
                string file = Path.Combine(ExportFolder, $"{logicAppName}.json");
                var data = _extractor.Data.Where(x => x.LogicAppName == logicAppName);
                if (File.Exists(file))
                {
                    // load existing data from file and resave without duplicates
                    string content = await System.IO.File.ReadAllTextAsync(file);
                    var list = JsonConvert.DeserializeObject<LogicAppExtract[]>(content).ToList();
                    if (list.Any())
                    {
                        foreach (var d in data)
                        {
                            if (!list.Any(x => x.RunId == d.RunId && x.ActionName == d.ActionName))
                            {
                                list.Add(d);
                            }
                        }
                    }
                    else
                    {
                        list.AddRange(data);
                    }

                    exportItems.AddRange(list);
                }
                else
                {
                    exportItems.AddRange(data);
                }

                if (exportItems.Any())
                {
                    exportItems.SaveToFile(file);
                }
            }
        }

    }
};