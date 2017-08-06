using System.Threading.Tasks;
using MatchMaker.Api.Entities;
using MatchMaker.Shared.Accounts;
using NPoco;

namespace MatchMaker.Api.Databases.Queries
{
    public class AccountCompactDTOQuery : IQuery<AccountCompactDTO>
    {
        private readonly int _accountId;

        public static AccountCompactDTOQuery For(int accountId)
        {
            return new AccountCompactDTOQuery(accountId);
        }

        private AccountCompactDTOQuery(int accountId)
        {
            this._accountId = accountId;
        }

        public async Task<AccountCompactDTO> ExecuteAsync(IDatabase database)
        {
            var account = await database.SingleOrDefaultByIdAsync<Account>(this._accountId);
            return new AccountCompactDTO
            {
                Id = account.Id,
                EmailAddress = account.EmailAddress
            };
        }
    }
}