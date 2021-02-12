using System.Reflection;
using Auxilium.FunctionApp;
using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Hosting;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Auxilium.FunctionApp
{
	internal class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
		}
	}
}