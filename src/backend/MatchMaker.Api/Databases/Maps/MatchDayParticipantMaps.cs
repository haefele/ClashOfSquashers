using NPoco.FluentMappings;

namespace MatchMaker.Api.Entities
{
    public class MatchDayParticipantMaps : Map<MatchDayParticipant>
    {
        public MatchDayParticipantMaps()
        {
            this.TableName("MatchDayParticipants");
            this.PrimaryKey(f => f.Id);
        }
    }
}