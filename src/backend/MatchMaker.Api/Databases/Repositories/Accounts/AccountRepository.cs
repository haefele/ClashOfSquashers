using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MatchMaker.Api.Services.PasswordHasher;
using MatchMaker.Shared.Accounts;
using MatchMaker.Shared.Common;

namespace MatchMaker.Api.Databases.Repositories.Accounts
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        private readonly IPasswordHasher _passwordHasher;

        public AccountRepository(IDbTransaction transaction, IPasswordHasher passwordHasher) 
            : base(transaction)
        {
            Guard.NotNull(passwordHasher, nameof(passwordHasher));
            
            this._passwordHasher = passwordHasher;
        }

        public async Task<Account> CreateAsync(string emailAddress, string password, CancellationToken token)
        {
            Guard.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            if (await this.EmailExistsAsync(emailAddress, token))
                throw new Exception("Email address is already in use.");

            var cleanEmailAddress = emailAddress.Trim();
            var passwordHash = this._passwordHasher.Hash(password);

            var id = await this.InsertIntoAccountsAsync(cleanEmailAddress, passwordHash, token);

            return new Account
            {
                Id = id,
                EmailAddress = emailAddress,
            };
        }

        public async Task<(Account account, string passwordHash)> GetAccountAndPasswordHashFromEmailAddressAsync(string emailAddress, CancellationToken token)
        {
            Guard.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));

            var cleanedEmailAddress = emailAddress.Trim();

            AccountWithPasswordHash found = await this.GetAccountWithPasswordHashAsync(cleanedEmailAddress, token);

            if (found == null)
                return (null, null);

            return (new Account {Id = found.Id, EmailAddress = found.EmailAddress}, found.PasswordHash);
        }

        public Task<Account> GetAccountByIdAsync(int accountId, CancellationToken token)
        {
            Guard.NotZeroOrNegative(accountId, nameof(accountId));

            return this.GetAccountByIdAsyncInternal(accountId, token);
        }

        public Task<List<Account>> SearchAccountsAsync(string searchText, CancellationToken token)
        {
            Guard.NotNullOrWhiteSpace(searchText, nameof(searchText));

            return this.GetAccountsBySearchTextAsync(searchText, token);
        }

        #region SQL
        private Task<bool> EmailExistsAsync(string emailAddress, CancellationToken token)
        {
            Guard.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));

            const string sql = @"
SELECT TOP 1 1 
FROM dbo.Accounts A 
WHERE A.EmailAddress = @EmailAddress";

            return this.Connection.ExecuteScalarAsync<bool>(this.AsCommand(sql, new {EmailAddress = emailAddress}, token));
        }

        private Task<int> InsertIntoAccountsAsync(string emailAddress, string passwordHash, CancellationToken token)
        {
            Guard.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));
            Guard.NotNullOrWhiteSpace(passwordHash, nameof(passwordHash));

            const string sql = @"
INSERT INTO dbo.Accounts (EmailAddress, PasswordHash) 
VALUES (@EmailAddress, @PasswordHash); 

SELECT SCOPE_IDENTITY();";

            return this.Connection.ExecuteScalarAsync<int>(this.AsCommand(sql, new { EmailAddress = emailAddress, PasswordHash = passwordHash }, token));
        }

        private async Task<AccountWithPasswordHash> GetAccountWithPasswordHashAsync(string emailAddress, CancellationToken token)
        {
            Guard.NotNullOrWhiteSpace(emailAddress, nameof(emailAddress));

            const string sql = @"
SELECT A.Id, A.EmailAddress, A.PasswordHash 
FROM dbo.Accounts A 
WHERE A.EmailAddress = @EmailAddress";

            return (await this.Connection.QueryAsync<AccountWithPasswordHash>(this.AsCommand(sql, new {EmailAddress = emailAddress}, token))).FirstOrDefault();
        }

        private async Task<Account> GetAccountByIdAsyncInternal(int accountId, CancellationToken token)
        {
            Guard.NotZeroOrNegative(accountId, nameof(accountId));

            const string sql = @"
SELECT A.Id, A.EmailAddress
FROM dbo.Accounts A
WHERE A.Id = @AccountId";

            return (await this.Connection.QueryAsync<Account>(this.AsCommand(sql, new {AccountId = accountId}, token))).FirstOrDefault();
        }

        private async Task<List<Account>> GetAccountsBySearchTextAsync(string searchText, CancellationToken token)
        {
            Guard.NotNullOrWhiteSpace(searchText, nameof(searchText));

            const string sql = @"
SELECT A.Id, A.EmailAddress
FROM dbo.Accounts A
WHERE A.EmailAddress LIKE @SearchText";

            return (await this.Connection.QueryAsync<Account>(this.AsCommand(sql, new {SearchText = $"%{searchText}%"},token))).ToList();
        }

        private class AccountWithPasswordHash
        {
            public int Id { get; set; }
            public string EmailAddress { get; set; }
            public string PasswordHash { get; set; }
        }
        #endregion
    }
}