using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Auxilium.Core.LogAnalytics
{
    public class LogAnalyticsService : ServiceBase, ILogAnalyticsService
    {
        public LogAnalyticsService(string token = null) : base(token)
        {
        }

        public async Task<T> LogAnalyticsSearch<T>(LogAnalyticsQuery search, string workspaceId)
        {

            using (var client = new HttpClient())
            {
                var url = $@"https://api.loganalytics.io/v1/workspaces/{workspaceId}/query";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                var json = JsonConvert.SerializeObject(search,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                var postData = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, postData).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var data = JsonConvert.DeserializeObject<T>(content);
                return data;
            }
        }
    }
}