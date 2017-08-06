namespace MatchMaker.Api.Entities
{
    public class MatchDayParticipant
    {
        public int Id { get; set; }
        public int MatchDayId { get; set; }
        public int AccountId { get; set; }
    }
}