namespace Auxilium.Core
{
    public interface IHaveAToken
    {
        string Token { get;  }
        void SetToken(string token);
    }
}
