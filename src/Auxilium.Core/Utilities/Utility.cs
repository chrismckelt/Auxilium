using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Auxilium.Core.Utilities
{
    public static class Utility
    {
        private static HttpClient _httpClient = new HttpClient();
        public static async Task<TResponse> GetResourceAsync<TResponse>(string requestUri, string token, bool print = false)
        {
            Console.WriteLine(requestUri);
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(requestUri);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                content = content.Replace("@variables", "variables");
                try
                {
                    if (print)
                    {
                        Console.WriteLine("-----------------------------------------");
                        Console.WriteLine(content);
                        Console.WriteLine("-----------------------------------------");
                    }

                    var data = JsonConvert.DeserializeObject<TResponse>(content, new JsonSerializerSettings
                    {
                        Error = HandleDeserializationError
                    });
                    if (data == null)
                        throw new InvalidOperationException($"Unable to perform ARM Post. Response: {content}");

                    return data;
                }
                catch (Exception e)
                {
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine(content);
                    Console.WriteLine("-----------------------------------------");
                    var instance = Activator.CreateInstance<TResponse>();
                    return instance;
                }
            }

            response.EnsureSuccessStatusCode();
            throw new ApplicationException(response.ReasonPhrase);
        }

        public static async Task<string> DeleteResourceAsync<TResponse>(string requestUri, string token)
        {
            Console.WriteLine(requestUri);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public static async Task<string> CreateUpdateResourceAsync<TResponse>(string requestUri, string token, string requestContent)
        {
            Console.WriteLine(requestUri);
            if (string.IsNullOrWhiteSpace(requestUri) ||
                string.IsNullOrWhiteSpace(token) ||
                string.IsNullOrWhiteSpace(requestContent))
                return null;

            //  requestContent = System.IO.File.ReadAllText(@"c:\temp\zzz.txt");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpContent = new StringContent(requestContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(requestUri, httpContent);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
          
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public static void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            Trace.TraceWarning(currentError);
            errorArgs.ErrorContext.Handled = true;
        }

        [DebuggerStepThrough]
        public static string TryParseHttpResponseContentData(string data)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<HttpResponse>(data);
                if (obj != null)
                {
                    string found = obj.Body.ContentData ?? obj.Body.Content;
                    if (!string.IsNullOrEmpty(found))
                    {
                        var bytes = Convert.FromBase64String(found);
                        {
                            string decoded =  Encoding.UTF8.GetString(bytes);
                            return decoded;
                        }
                    }

                    throw new ApplicationException(data);
                }

            }
            catch (Exception ee)
            {
                Trace.TraceWarning(ee.ToString());
                try
                {
                    var obj2 = JsonConvert.DeserializeObject<HttpResponse2>(data);
                    if (obj2 == null) return data;
                    if (!string.IsNullOrEmpty(obj2.Body) && obj2.Body.TryParseJson<Body>(out var body))
                    {
                        var str = body?.ContentData ?? body?.Content;
                        if (!string.IsNullOrEmpty(str))
                        {
                            var bytes = Convert.FromBase64String(str);
                            {
                                return Encoding.UTF8.GetString(bytes);
                            }
                        }
                    }
                    else
                    {
                        return obj2.Body;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return data;
                }
            }

            return data;
        }

        //TODO clean up this mess
        public static async Task<string> TryExtractContentAsync(string data)
        {
            //Console.WriteLine(data);

            if (Uri.IsWellFormedUriString(data, UriKind.Absolute)) data = await GetContent(data).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(data))
                try
                {
                    if (data.TryParseJson(out HttpPayload obj))
                        if (IsBase64String(obj?.Body?.ContentData))
                        {
                            var bytes = Convert.FromBase64String(obj.Body?.ContentData);
                            {
                                return Encoding.UTF8.GetString(bytes);
                            }
                        }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    try
                    {
                        var jObject = (JObject) JsonConvert.DeserializeObject(data);

                        if (jObject?["body"] != null)
                        {
                            var val = jObject["body"];
                            data = val.ToString();

                            if (data.IsJson())
                                data = GetFirstPropertyNameFromJson(data);
                            else if (data.IsValidXml()) return GetFirstPropertyNameFromXml(data);
                        }
                    }
                    catch (Exception e3)
                    {
                        Console.WriteLine(e3);
                        return "ERROR";
                    }
                }

            return data;
        }

        public static bool IsBase64String(string base64String)
        {
            // Credit: oybek https://stackoverflow.com/users/794764/oybek
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
                                                   || base64String.Contains(" ") || base64String.Contains("\t") ||
                                                   base64String.Contains("\r") || base64String.Contains("\n"))
                return false;

            try
            {
                var res = Convert.FromBase64String(base64String);
                Trace.WriteLine(res);
                return true;
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.ToString());
            }

            return false;
        }

        public static string GetFirstPropertyNameFromJson(string json)
        {
            if (string.IsNullOrEmpty(json)) return null;
            var jObject = (JObject) JsonConvert.DeserializeObject(json);

            if (jObject.Children()[0] != null)
            {
                var val = jObject.Properties().Select(p => p.Name).FirstOrDefault();
                return val;
            }

            return null;
        }

        public static bool IsJson(this string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        // [DebuggerHidden]
        public static bool TryParseJson<T>(this string @this, out T result)
        {
            var success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    success = false;
                    args.ErrorContext.Handled = true;
                },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(@this, settings);
            return success;
        }

        public static bool IsValidXml(this string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string GetFirstPropertyNameFromXml(string xmlString)
        {
            var element = XElement.Parse(xmlString)
                .Descendants()
                .FirstOrDefault();

            return element?.Name.LocalName;
        }

        public static async Task<string> GetContent(string uri)
        {
            if (string.IsNullOrEmpty(uri)) return null;
            using (var client = new HttpClient())
            {
                //       client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);  --> this causes an ERROR ???
                var response = await client.GetAsync(uri).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                throw new ApplicationException(response.ReasonPhrase);
            }
        }

        public static string GetContentFromFile(string filePath, string fileName, string fileExtension)
        {
            if (string.IsNullOrWhiteSpace(filePath) ||
                string.IsNullOrWhiteSpace(fileName) ||
                string.IsNullOrWhiteSpace(fileExtension) ||
                !System.IO.File.Exists($"{filePath}\\{fileName}.{fileExtension}"))
                return null;

            return System.IO.File.ReadAllText($"{filePath}\\{fileName}.{fileExtension}");
        }
    }
}