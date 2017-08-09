using MatchMaker.Shared.Common;

namespace MatchMaker.Api.Databases
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Validate(string hash, string password)
        {
            Guard.NotNullOrWhiteSpace(hash, nameof(hash));
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}