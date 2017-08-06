using NPoco.FluentMappings;

namespace MatchMaker.Api.Entities
{
    public class MatchMaps : Map<Match>
    {
        public MatchMaps()
        {
            this.TableName("Matches");
            this.PrimaryKey(f => f.Id);
        }
    }
}