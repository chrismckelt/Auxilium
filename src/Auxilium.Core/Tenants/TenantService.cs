using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Auxilium.Core.Utilities;
using Newtonsoft.Json.Linq;

namespace Auxilium.Core.Tenants
{
    public class TenantService : ServiceBase, ITenantService
    {
        public TenantService(string token = null) : base(token)
        {
        }

        public async Task<string> GetTenantIdAsync()
        {
            const string url = "https://management.azure.com/tenants?api-version=2016-06-01";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false); ;
                    return JObject.Parse(content).SelectToken("tenantId").ToString();
                }

                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        public async Task<AzureResultList<AzureTenantValue>> ListTenantsAsync()
        {
            var url = "https://management.azure.com/tenants?api-version=2019-06-01";
            var results = await Utility.GetResourceAsync<AzureResultList<AzureTenantValue>>(url, Token).ConfigureAwait(false);
            return results;
        }
    }
}