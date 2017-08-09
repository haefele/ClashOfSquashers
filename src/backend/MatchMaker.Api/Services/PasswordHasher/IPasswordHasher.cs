namespace MatchMaker.Api.Databases
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Validate(string hash, string password);
    }
}