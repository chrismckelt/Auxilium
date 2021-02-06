namespace Auxilium.Core
{
    public abstract class ServiceBase : IHaveAToken
    {
        public const string AzureManagementUrl = "https://management.azure.com/subscriptions";

        protected ServiceBase(string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                Token = token;
            }
        }

        public string Token { get; protected set; }

        public void SetToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                Token = token;
            }
        }
    }
}
