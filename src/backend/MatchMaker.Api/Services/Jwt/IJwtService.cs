namespace MatchMaker.Api.Services.Jwt
{
    public interface IJwtService
    {
        string Create(int accountId, string emailAddress);
        bool Validate(string token);
    }
}