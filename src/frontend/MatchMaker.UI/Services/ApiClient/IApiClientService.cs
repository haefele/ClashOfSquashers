using MatchMaker.Shared.MatchDays;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatchMaker.UI.Services.ApiClient
{
    public interface IApiClientService
    {
        Task Register(string email, string password);

        Task<string> Login(string email, string password);

        Task<MatchDayCompactDTO> CreateNewMatchDay(List<int> participantIds, DateTime when);

        Task<Match> GetNextMatch(int matchDayId);

        Task<Match> SaveMatch(int matchDayId, Match match);
    }
}