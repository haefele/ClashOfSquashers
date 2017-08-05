using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MatchMaker.Api.Entities;
using MatchMaker.Shared.MatchDays;

namespace MatchMaker.Api.Database
{
    public static class AccountQueries
    {
        public static async Task<Account> QueryAccountByEmail(this IDbConnection self, string emailAddress, IDbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var sql = "SELECT Id, EmailAddress, PasswordHash FROM dbo.Accounts WHERE EmailAddress = @EmailAddress";
            var parameters = new
            {
                EmailAddress = emailAddress
            };
            var def = new CommandDefinition(commandText: sql, parameters: parameters, cancellationToken: cancellationToken, transaction:transaction);

            return (Account)await self.QueryFirstOrDefaultAsync(typeof(Account), def);
        }

        public static async Task CreateAccount(this IDbConnection self, Account account, IDbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var sql = "INSERT INTO dbo.Accounts(EmailAddress, PasswordHash) VALUES (@EmailAddress, @PasswordHash); SELECT SCOPE_IDENTITY();";
            var parameters = new
            {
                EmailAddress = account.EmailAddress,
                PasswordHash = account.PasswordHash
            };
            var def = new CommandDefinition(sql, parameters, cancellationToken:cancellationToken, transaction:transaction);
            var id = await self.ExecuteScalarAsync<int>(def);

            account.Id = id;
        }

        public static async Task<List<AccountCompact>> GetAccountCompacts(this IDbConnection self, List<int> accountIds, IDbTransaction transaction, CancellationToken cancellationToken = default(CancellationToken))
        {
            var sql = "SELECT Id, EmailAddress FROM dbo.Accounts WHERE Id in @AccountIds";
            var parameters = new
            {
                AccountIds = accountIds
            };
            var def = new CommandDefinition(sql, parameters, cancellationToken:cancellationToken, transaction:transaction);
            return (await self.QueryAsync<AccountCompact>(def)).ToList();
        }
    }
}