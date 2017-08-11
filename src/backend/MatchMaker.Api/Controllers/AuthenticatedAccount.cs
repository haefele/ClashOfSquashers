namespace MatchMaker.Api.Controllers
{
    public class AuthenticatedAccount
    {
        public AuthenticatedAccount(int accountId, string emailAddress)
        {
            this.AccountId = accountId;
            this.EmailAddress = emailAddress;
        }

        public int AccountId { get; }
        public string EmailAddress { get; }
    }
}