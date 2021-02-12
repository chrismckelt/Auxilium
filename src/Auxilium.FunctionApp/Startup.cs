using System.Reflection;
using Auxilium.Core;
using Auxilium.Core.Configuration;
using Auxilium.Core.Dtos;
using Auxilium.Core.Interfaces;
using Auxilium.Core.Services;
using Auxilium.FunctionApp;
using Auxilium.Infrastructure;
using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Constants = Auxilium.Core.Configuration.Constants;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Auxilium.FunctionApp
{
	internal class Startup : FunctionsStartup
	{
		private IConfigurationRoot _configurationRoot;
		
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.AddSwashBuckle(Assembly.GetExecutingAssembly());
			
			builder.Services.AddOptions<CosmosConfig>().Configure<IConfiguration>((settings, configuration) =>
			{
				configuration.GetSection(Constants.CosmosConfig).Bind(settings);
			});
			builder.Services.AddScoped<ICosmosRepository<AzureTenantDto>, TenantRepository>();
			builder.Services.AddScoped<ITenantService, TenantService>();
		}
	}
}