namespace MatchMaker.UI.Services.Token
{
    public interface ITokenService
    {
        string Token { get; }

        void SaveToken(string token);
    }
}