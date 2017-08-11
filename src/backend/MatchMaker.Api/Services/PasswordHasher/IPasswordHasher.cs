namespace MatchMaker.Api.Services.PasswordHasher
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Validate(string hash, string password);
    }
}