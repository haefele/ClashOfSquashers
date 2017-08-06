using NPoco.FluentMappings;

namespace MatchMaker.Api.Entities
{
    public class MatchDayMaps : Map<MatchDay>
    {
        public MatchDayMaps()
        {
            this.TableName("MatchDays");
            this.PrimaryKey(f => f.Id);
        }
    }
}