using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
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

        #region SQL
        private Task<bool> EmailExistsAsync(string emailAddress, CancellationToken token)
        {
            const string sql = "SELECT TOP 1 1 " +
                               "FROM dbo.Accounts A " +
                               "WHERE A.EmailAddress = @EmailAddress";

            return this.Connection.ExecuteScalarAsync<bool>(this.AsCommand(sql, new {EmailAddress = emailAddress}, token));
        }

        private Task<int> InsertIntoAccountsAsync(string emailAddress, string passwordHash, CancellationToken token)
        {
            const string sql = "INSERT INTO dbo.Accounts (EmailAddress, PasswordHash)" +
                               "VALUES (@EmailAddress, @PasswordHash);" +

                               "SELECT SCOPE_IDENTITY();";
            return this.Connection.ExecuteScalarAsync<int>(this.AsCommand(sql, new { EmailAddress = emailAddress, PasswordHash = passwordHash }, token));
        }

        private async Task<AccountWithPasswordHash> GetAccountWithPasswordHashAsync(string emailAddress, CancellationToken token)
        {
            const string sql = "SELECT A.Id, A.EmailAddress, A.PasswordHash " +
                               "FROM dbo.Accounts A " +
                               "WHERE A.EmailAddress = @EmailAddress";
            return (await this.Connection.QueryAsync<AccountWithPasswordHash>(this.AsCommand(sql, new {EmailAddress = emailAddress}, token))).FirstOrDefault();
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