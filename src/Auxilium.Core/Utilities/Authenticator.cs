using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json.Linq;

namespace Auxilium.Core.Utilities
{
    public static class Authenticator
    {
        /// <summary>
        /// https://stackoverflow.com/questions/39407952/adal-net-core-nuget-package-does-not-support-userpasswordcredential/52846875
        /// </summary>
        public const string Saml11Bearer = "urn:ietf:params:oauth:grant-type:saml1_1-bearer";
        public const string Saml20Bearer = "urn:ietf:params:oauth:grant-type:saml2-bearer";
        public const string JwtBearer = "urn:ietf:params:oauth:grant-type:jwt-bearer";

        /// <summary>
        ///     Acquire an AAD authentication token silently for an AAD App (Native) with an AAD account
        ///     NOTE: This process was ported from the Microsoft.IdentityModel.Clients.ActiveDirectory's
        ///     AuthenticationContext.AcquireTokenAsync method, which can silently authenticate using the UserPasswordCredential
        ///     class.
        ///     Since this class is missing from .NET Core, this method can be used to perform the same without any dependencies.
        /// </summary>
        /// <param name="user">AAD login</param>
        /// <param name="password">AAD pass</param>
        /// <param name="tenantId">Tenant ID</param>
        /// <param name="resourceUrl">Resource ID: the Azure app that will be accessed</param>
        /// <param name="clientId">
        ///     The Application ID of the calling app. This guid can be obtained from Azure Portal > app auth
        ///     setup > Advanced Settings
        /// </param>
        public static async Task<string> GetAuthTokenForAadNativeApp(string user, string password, string tenantId,
            string resourceUrl, string clientId)
        {
            var pass = ConvertToSecureString(password);
            var tokenForUser = string.Empty;
            var authority = "https://login.microsoftonline.com/" + tenantId; // The AD Authority used for login
            var clientRequestId = Guid.NewGuid().ToString();

            // Discover the preferred openid / oauth2 endpoint for the tenant (by authority)
            var api =
                "https://login.microsoftonline.com/common/discovery/instance?api-version=1.1&authorization_endpoint=" +
                authority + "/oauth2/authorize";
            var openIdPreferredNetwork = string.Empty;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("client-request-id", clientRequestId);
            client.DefaultRequestHeaders.Add("return-client-request-id", "true");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var responseTask = client.GetAsync(api);
            responseTask.Wait();
            if (responseTask.Result.Content != null)
            {
                var responseString = responseTask.Result.Content.ReadAsStringAsync();
                responseString.Wait();
                try
                {
                    dynamic json = JObject.Parse(responseString.Result);
                    openIdPreferredNetwork = json.metadata[0].preferred_network; // e.g. login.microsoftonline.com
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(openIdPreferredNetwork))
                openIdPreferredNetwork = "login.microsoftonline.com";

            // Get the federation metadata url & federation active auth url by user realm (by user domain)
            responseTask = client.GetAsync("https://" + openIdPreferredNetwork + "/common/userrealm/" + user +
                                           "?api-version=1.0");
            responseTask.Wait();
            var federationMetadataUrl = string.Empty;
            var federationActiveAuthUrl = string.Empty;
            if (responseTask.Result.Content != null)
            {
                var responseString = responseTask.Result.Content.ReadAsStringAsync();
                responseString.Wait();
                try
                {
                    dynamic json = JObject.Parse(responseString.Result);
                    federationMetadataUrl =
                        json.federation_metadata_url; // e.g. https://sts.{domain}.com.au/adfs/services/trust/mex
                    federationActiveAuthUrl =
                        json.federation_active_auth_url; // e.g. https://sts.{domain}.com.au/adfs/services/trust/2005/usernamemixed
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(federationMetadataUrl) || string.IsNullOrEmpty(federationActiveAuthUrl))
                return string.Empty;

            // Get federation metadata
            responseTask = client.GetAsync(federationMetadataUrl);
            responseTask.Wait();
            string federationMetadataXml = null;
            if (responseTask.Result.Content != null)
            {
                var responseString = responseTask.Result.Content.ReadAsStringAsync();
                responseString.Wait();
                try
                {
                    federationMetadataXml = responseString.Result;
                }
                catch
                {
                }
            }

            if (string.IsNullOrEmpty(federationMetadataXml))
                return string.Empty;

            // Post credential to the federation active auth URL
            var messageId = Guid.NewGuid().ToString("D").ToLower();
            var postData = @"
<s:Envelope xmlns:s='http://www.w3.org/2003/05/soap-envelope' xmlns:a='http://www.w3.org/2005/08/addressing' xmlns:u='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'>
<s:Header>
<a:Action s:mustUnderstand='1'>http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue</a:Action>
<a:MessageID>urn:uuid:" + messageId + @"</a:MessageID>
<a:ReplyTo>
<a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address>
</a:ReplyTo>
<a:To s:mustUnderstand='1'>" + federationActiveAuthUrl + @"</a:To>
<o:Security s:mustUnderstand='1' xmlns:o='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'>
<u:Timestamp u:Id='_0'>
<u:Created>" + DateTime.Now.ToString("o") + @"</u:Created>
<u:Expires>" + DateTime.Now.AddMinutes(10).ToString("o") + @"</u:Expires>
</u:Timestamp>
<o:UsernameToken u:Id='uuid-" + Guid.NewGuid().ToString("D").ToLower() + @"'>
<o:Username>" + user + @"</o:Username>
<o:Password>" + FromSecureString(pass) + @"</o:Password>
</o:UsernameToken>
</o:Security>
</s:Header>
<s:Body>
<trust:RequestSecurityToken xmlns:trust='http://schemas.xmlsoap.org/ws/2005/02/trust'>
<wsp:AppliesTo xmlns:wsp='http://schemas.xmlsoap.org/ws/2004/09/policy'>
<a:EndpointReference>
  <a:Address>urn:federation:MicrosoftOnline</a:Address>
</a:EndpointReference>
</wsp:AppliesTo>
<trust:KeyType>http://schemas.xmlsoap.org/ws/2005/05/identity/NoProofKey</trust:KeyType>
<trust:RequestType>http://schemas.xmlsoap.org/ws/2005/02/trust/Issue</trust:RequestType>
</trust:RequestSecurityToken>
</s:Body>
</s:Envelope>";
            var content = new StringContent(postData, Encoding.UTF8, "application/soap+xml");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("SOAPAction", "http://schemas.xmlsoap.org/ws/2005/02/trust/RST/Issue");
            client.DefaultRequestHeaders.Add("client-request-id", clientRequestId);
            client.DefaultRequestHeaders.Add("return-client-request-id", "true");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await client.PostAsync(federationActiveAuthUrl, content);
            
            var xml = new XmlDocument();
            var assertion = string.Empty;
            var grant_type = string.Empty;
            if (response.Content != null)
            {
               
                var respcontent = await response.Content.ReadAsStringAsync();
                 
                try
                {
                    xml.LoadXml(respcontent);
                }
                catch
                {
                }

                var nodeList = xml.GetElementsByTagName("saml:Assertion");
                if (nodeList.Count > 0)
                {
                    assertion = nodeList[0].OuterXml;
                    // The grant type depends on the assertion value returned previously <saml:Assertion MajorVersion="1" MinorVersion="1"...>
                    grant_type = Saml11Bearer;
                    var majorVersion = nodeList[0].Attributes["MajorVersion"] != null
                        ? nodeList[0].Attributes["MajorVersion"].Value
                        : string.Empty;
                    if (majorVersion == "1")
                        grant_type = Saml11Bearer;
                    if (majorVersion == "2")
                        grant_type = Saml20Bearer;
                    else
                        grant_type = Saml11Bearer; // Default to Saml11Bearer
                }
            }

            // Post to obtain an oauth2 token to for the resource 
            // (*) Pass in the assertion XML node encoded to base64 in the post, as is done here https://blogs.msdn.microsoft.com/azuredev/2018/01/22/accessing-the-power-bi-apis-in-a-federated-azure-ad-setup/
            var ua = new UserAssertion(assertion, grant_type, Uri.EscapeDataString(user));
            var encoding = new UTF8Encoding();
            var byteSource = encoding.GetBytes(ua.Assertion);
            var base64ua = Uri.EscapeDataString(Convert.ToBase64String(byteSource));
            postData =
                "resource={resourceUrl}&client_id={clientId}&grant_type={grantType}&assertion={assertion}&scope=openid"
                    .Replace("{resourceUrl}", Uri.EscapeDataString(resourceUrl))
                    .Replace("{clientId}", Uri.EscapeDataString(clientId))
                    .Replace("{grantType}", Uri.EscapeDataString(grant_type))
                    .Replace("{assertion}", base64ua);
            content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("client-request-id", clientRequestId);
            client.DefaultRequestHeaders.Add("return-client-request-id", "true");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            responseTask = client.PostAsync("https://" + openIdPreferredNetwork + "/common/oauth2/token", content);
            responseTask.Wait();
            if (responseTask.Result.Content != null)
            {
                var responseString = responseTask.Result.Content.ReadAsStringAsync();
                responseString.Wait();
                try
                {
                    dynamic json = JObject.Parse(responseString.Result);
                    tokenForUser = json.access_token;
                }
                catch(Exception e)
                {
                    Trace.Fail(e.ToString());
                }
            }

            if (string.IsNullOrEmpty(federationMetadataXml))
                return string.Empty;


            return tokenForUser;
        }

        private static string FromSecureString(SecureString value)
        {
            string stringBstr;
            var bSTR = Marshal.SecureStringToBSTR(value);
            if (bSTR == IntPtr.Zero) return string.Empty;
            try
            {
                stringBstr = Marshal.PtrToStringBSTR(bSTR);
            }
            finally
            {
                Marshal.FreeBSTR(bSTR);
            }

            return stringBstr;
        }

        public static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();

            foreach (var c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }
    }
}