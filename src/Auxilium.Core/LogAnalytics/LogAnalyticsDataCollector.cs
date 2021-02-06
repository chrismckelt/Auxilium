using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Auxilium.Core.Utilities;

namespace Auxilium.Core.LogAnalytics
{
    /// <summary>
    /// HTTP Data Collector API wrapper https://github.com/ealsur/HTTPDataCollectorAPI/blob/master/HTTPDataCollectorAPI/ICollector.cs
    /// </summary>
    public class LogAnalyticsDataCollector : ILogAnalyticsDataCollector
    {
        private readonly string _workspaceId;
        private readonly string _sharedKey;
		private readonly byte[] _sharedKeyBytes;
        private readonly string _serviceNamespace;

        
        /// <summary>
        /// Wrapper for reporting custom JSON events to Azure Log Analytics
        /// </summary>
        /// <param name="workspaceId">Workspace Id obtained from your Microsoft Operations Management Suite account, Settings > Connected Sources.</param>
        /// <param name="sharedKey">Primary or Secondary Key obtained from your Microsoft Operations Management Suite account, Settings > Connected Sources.</param>
        /// <param name="serviceNamespace">Optional. Allows to change the service endpoint to use the library in different clouds.</param>
        public LogAnalyticsDataCollector(string workspaceId, string sharedKey, string serviceNamespace = "ods.opinsights.azure.com")
        {
            if (string.IsNullOrEmpty(workspaceId))
            {
                throw new ArgumentNullException(nameof(workspaceId));
            }

            if (string.IsNullOrEmpty(sharedKey))
            {
                throw new ArgumentNullException(nameof(sharedKey));
            }

            _workspaceId = workspaceId;
            _sharedKey = sharedKey;
			_sharedKeyBytes = Convert.FromBase64String(_sharedKey);
            _serviceNamespace = serviceNamespace;
		}

        /// <summary>
        /// Collect a JSON log to Azure Log Analytics
        /// </summary>
        /// <param name="logType">Name of the Type of Log. Can be any name you want to appear on Azure Log Analytics.</param>
        /// <param name="objectToSerialize">Object to serialize and collect.</param>
        /// <param name="apiVersion">Optional. Api Version.</param>
        /// <param name="timeGeneratedPropertyName"></param>
        public async Task Collect(object objectToSerialize,string logType = "ApplicationLog", string apiVersion="2016-04-01", string timeGeneratedPropertyName = null)
        {
            await Collect(Newtonsoft.Json.JsonConvert.SerializeObject(objectToSerialize), logType, apiVersion, timeGeneratedPropertyName);
        }

        /// <summary>
        /// Collect a JSON log to Azure Log Analytics
        /// </summary>
        /// <param name="logType">Name of the Type of Log. Can be any name you want to appear on Azure Log Analytics.</param>
        /// <param name="json">JSON string. Can be an array or single object.</param>
        /// <param name="apiVersion">Optional. Api Version.</param>
        /// <param name="timeGeneratedPropertyName"></param>
        public async Task Collect(string json, string logType = "ApplicationLog", string apiVersion="2016-04-01", string timeGeneratedPropertyName = null)
        {
            if (!json.Contains("{"))
            {
                await Collect(LogMessage.Log(json));
                return; // dont double log
            }

            string logUri = $"https://{_workspaceId}.ods.opinsights.azure.com/api/logs?api-version={apiVersion}";
            var dateTimeNow = DateTime.UtcNow.ToString("r");
            var authSignature = GetAuthSignature(json, dateTimeNow);

            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Authorization", authSignature);
                client.DefaultRequestHeaders.Add("Log-Type", logType);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("x-ms-date", dateTimeNow);
                client.DefaultRequestHeaders.Add("time-generated-field", ""); // if we want to extend this in the future to support custom date fields from the entity etc.

                HttpContent httpContent = new StringContent(json, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(new Uri(logUri), httpContent);
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                Trace.WriteLine("Log Analytics collection result: " + result);
            }
            catch (Exception excep)
            {
                Consoler.Error("API Post Exception", excep);

                #if DEBUG
                    throw;
                #endif
            }
        }


        private string GetAuthSignature(string serializedJsonObject, string dateString)
        {
            string stringToSign = $"POST\n{serializedJsonObject.Length}\napplication/json\nx-ms-date:{dateString}\n/api/logs";
            string signedString;

            var encoding = new ASCIIEncoding();
            var sharedKeyBytes = Convert.FromBase64String(_sharedKey);
            var stringToSignBytes = encoding.GetBytes(stringToSign);
            using (var hmacsha256Encryption = new HMACSHA256(sharedKeyBytes))
            {
                var hashBytes = hmacsha256Encryption.ComputeHash(stringToSignBytes);
                signedString = Convert.ToBase64String(hashBytes);
            }

            return $"SharedKey {_workspaceId}:{signedString}";
        }
    }
}