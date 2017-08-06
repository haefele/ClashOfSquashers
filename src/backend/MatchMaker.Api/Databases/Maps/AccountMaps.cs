using NPoco.FluentMappings;

namespace MatchMaker.Api.Entities
{
    public class AccountMaps : Map<Account>
    {
        public AccountMaps()
        {
            this.TableName("Accounts");
            this.PrimaryKey(f => f.Id);
        }
    }
}