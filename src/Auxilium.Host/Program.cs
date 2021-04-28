using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Auxilium.Core;
using Auxilium.Core.LogicApps;
using Auxilium.Core.Utilities;
using Microsoft.Azure.Management.ServiceBus.Fluent.Subscription.Definition;
using Microsoft.WindowsAzure;
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
            var dt = DateTime.Parse("16/04/2021  6:13:51 AM");
            int minutesAgo = Convert.ToInt32((DateTime.UtcNow-dt).TotalMinutes) * -1;
            int minutesToAdd = 10;
            //_extractor = new Extractor();
           // await _extractor.Load();

            if (args.Any())
            {
                minutesAgo = Convert.ToInt32(args[0]);
            }

            while (minutesAgo < minutesToAdd)
            {
                try
                {
                    Consoler.TitleStart($"minutes ago {minutesAgo}");
                    await Extract(minutesAgo, minutesToAdd);
                    Consoler.TitleEnd($"minutes ago {minutesAgo}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                minutesAgo = minutesAgo + minutesToAdd;
            }

            Console.ReadLine();
        }

        private static async Task Extract(int minutesAgo, int minutesToExtract = 1)
        {
          //  DateTime? startDate = DateTime.UtcNow.AddMinutes(minutesAgo);
          //  DateTime? endDate = DateTime.UtcNow.AddMinutes(minutesAgo+ minutesToExtract);

            DateTime? startDate = DateTime.Parse("16/04/2021  6:13:50 AM");
            DateTime? endDate = DateTime.Parse("16/04/2021  6:15:03 AM");

            ////if (!startDate.HasValue) startDate = _extractor.Data.Select(x => x.StartTimeUtc).Max();
            Consoler.Information($"start {startDate}");
            Consoler.Information($"end {endDate}");

            //await _extractor.Run(startDate, endDate);

            var list = new List<string>()
            {

            };

            var tasks = new List<Task>();
            var extractor = new Extractor();
            await extractor.Load();
            foreach (var la in list)
            {
                var item = extractor.LogicApps.SingleOrDefault(x => x.Value == la);
                await extractor.ExtractLogicApp(item.Key, item.Value, false, true, startDate, endDate);

                //var task = Task.Run(async () => {
                  

                //    //while (startDate > endDate && rg.Value!=null)
                //    //{
                //    //    var dt = startDate.Value.AddMinutes(5);
                       
                //    //    startDate = dt;
                //    //}
                //});

                //tasks.Add(task);

            }

          //  await Task.WhenAll(tasks);

            //await _extractor.Run(startDate, endDate);
            await Export(extractor);
        
             
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

        private static async Task Export(Extractor extractor)
        {
            var las = extractor.LogicApps.Select(x => x.Value).Distinct();
            foreach (var logicAppName in las)
            {
                var exportItems = new List<LogicAppExtract>();
                string file = Path.Combine(ExportFolder, $"{logicAppName}.json");
                var data = extractor.Data.Where(x => x.LogicAppName == logicAppName);
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