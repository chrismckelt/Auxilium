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
            var dt = DateTime.Parse("20/02/2021  0:00:00 PM");
            int hoursAgo = Convert.ToInt32((DateTime.UtcNow-dt).TotalHours) * -1;
            int hoursToAdd = 12;
            //_extractor = new Extractor();
           // await _extractor.Load();

            if (args.Any())
            {
                hoursAgo = Convert.ToInt32(args[0]);
            }

            while (hoursAgo < hoursToAdd)
            {
                try
                {
                    Consoler.TitleStart($"Hours ago {hoursAgo}");
                    await Extract(hoursAgo, hoursToAdd);
                    Consoler.TitleEnd($"Hours ago {hoursAgo}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                hoursAgo = hoursAgo + hoursToAdd;
            }

            Console.ReadLine();
        }

        private static async Task Extract(int hoursAgo, int hoursToExtract = 1)
        {
            DateTime? startDate = DateTime.UtcNow.AddHours(hoursAgo);
            DateTime? endDate = DateTime.UtcNow.AddHours(hoursAgo+ hoursToExtract);

            ////if (!startDate.HasValue) startDate = _extractor.Data.Select(x => x.StartTimeUtc).Max();
            Consoler.Information($"start {startDate}");
            Consoler.Information($"end {endDate}");

            //await _extractor.Run(startDate, endDate);

            var list = new List<string>()
            {
                "GX-Syd-P-ALA-POS-ClickAndCollect-Execute-SalesOrdersUpdate-Magento",
                "GX-Syd-P-ALA-LSCentral-BLoyal-Save-Transaction",
                "GX-Syd-P-ALA-WMS-Purecomm-Create-Shipping-Request",
                "GX-Syd-P-ALA-WMS-Purecomm-Create-CaseInfo-Request",
                "GX-Syd-P-ALA-POS-ClickAndCollect-Execute-SalesOrdersCreation-LSCentral",
                "GX-Syd-P-ALA-POS-ClickAndCollect-Process-SalesOrders-LSCentral"

            };

            var tasks = new List<Task>();

            foreach (var item in list)
            {
                var task = Task.Run(async () => {
                    var extractor = new Extractor();
                    await extractor.Load();
                    var rg = extractor.LogicApps.Single(x => x.Key == item).Value;
                    await extractor.ExtractLogicApp(rg, item, true, false, startDate, endDate);
                });

                tasks.Add(task);

            }

            await Task.WhenAll(tasks);

            //await _extractor.Run(startDate, endDate);
            await Export();
        
             
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