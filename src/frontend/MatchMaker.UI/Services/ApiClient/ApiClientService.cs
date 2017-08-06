using MatchMaker.Shared.Accounts;
using MatchMaker.Shared.Common;
using MatchMaker.Shared.MatchDays;
using MatchMaker.UI.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


[assembly: Dependency(typeof(MatchMaker.UI.Services.ApiClient.ApiClientService))]
namespace MatchMaker.UI.Services.ApiClient
{
    public class ApiClientService : IApiClientService
    {
        private readonly HttpClient _client;
        private bool UseMockData => true;

        public ApiClientService()
        {
            if (this.UseMockData)
                return;

            this._client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000,
                BaseAddress = new Uri("http://desktop-haefele:52940")
            };
        }


        public async Task Register(string email, string password)
        {
            Guard.NotNullOrWhiteSpace(email, nameof(email));
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            if (this.UseMockData)
                return;

            var json = JsonConvert.SerializeObject(new RegisterData { EmailAddress = email, Password = password });
            var request = new HttpRequestMessage(HttpMethod.Post, "accounts/register") { Content = new StringContent(json, Encoding.UTF8, "application/json") };
            var response = await this.Send(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new EmailAlreadyInUseException();
                default:
                    return;
            }
        }

        public async Task<string> Login(string email, string password)
        {
            Guard.NotNullOrWhiteSpace(email, nameof(email));
            Guard.NotNullOrWhiteSpace(password, nameof(password));

            if (this.UseMockData)
                return "this is irrelephant";

            var json = JsonConvert.SerializeObject(new RegisterData { EmailAddress = email, Password = password });
            var request = new HttpRequestMessage(HttpMethod.Post, "accounts/login") { Content = new StringContent(json, Encoding.UTF8, "application/json") };
            var response = await this.Send(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new UserNotFoundException();
                case HttpStatusCode.Unauthorized:
                    throw new InvalidPasswordException();
                case HttpStatusCode.OK:
                    var contentJson = await response.Content.ReadAsStringAsync();
                    var content = JsonConvert.DeserializeObject<LoginResult>(contentJson);
                    return content.Token;
                default:
                    throw new System.Exception(response.StatusCode.ToString());
            }
        }

        public async Task<MatchDayCompactDTO> CreateNewMatchDay(List<int> participantIds, DateTime when)
        {
            Guard.NotNullOrEmpty(participantIds, nameof(participantIds));
            Guard.NotInvalidDateTime(when, nameof(when));

            if (this.UseMockData)
                return new MatchDayCompactDTO
                {
                    Id = 1,
                    MatchCount = 10,
                    Participants = new List<AccountCompactDTO>(participantIds.Select(f => new AccountCompactDTO { Id = f, EmailAddress = f + "lul@lulz.com" })),
                    When = DateTime.Now
                };

            var json = JsonConvert.SerializeObject(new CreateMatchDayData { ParticipantIds = participantIds, When = when });
            var request = new HttpRequestMessage(HttpMethod.Post, "matchdays") { Content = new StringContent(json, Encoding.UTF8, "application/json") };
            var response = await this.Send(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new System.Exception();
                case HttpStatusCode.Created:
                    var contentJson = await response.Content.ReadAsStringAsync();
                    var content = JsonConvert.DeserializeObject<MatchDayCompactDTO>(contentJson);
                    return content;
                default:
                    throw new System.Exception(response.StatusCode.ToString());
            }
        }

        public async Task<MatchDTO> GetNextMatch(int matchDayId)
        {
            Guard.NotLessOrEqual(matchDayId, 0, nameof(matchDayId));

            if (this.UseMockData)
                return new MatchDTO
                {
                    MatchDayId = matchDayId,
                    Id = 1,
                    CreatedBy = new AccountCompactDTO { Id = 1, EmailAddress = "lel@lel.com"},
                    Participant1 = new AccountCompactDTO { Id = 2, EmailAddress = "rofl@lel.com" },
                    Participant2 = new AccountCompactDTO { Id = 3, EmailAddress = "lulz@lel.com" },
                    StartTime = DateTime.Now
                    
                };

            var json = JsonConvert.SerializeObject(matchDayId);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{matchDayId}/nextmatch") { Content = new StringContent(json, Encoding.UTF8, "application/json") };
            var response = await this.Send(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new System.Exception();
                case HttpStatusCode.OK:
                    var contentJson = await response.Content.ReadAsStringAsync();
                    var content = JsonConvert.DeserializeObject<MatchDTO>(contentJson);
                    return content;
                default:
                    throw new System.Exception(response.StatusCode.ToString());
            }
        }
        
        public async Task<MatchDTO> SaveMatch(int matchDayId, MatchDTO match)
        {
            if (this.UseMockData)
            {
                match.EndTime = DateTime.Now;
                return match;
            }

            throw new NotImplementedException();
        }

        private async Task<HttpResponseMessage> Send(HttpRequestMessage request)
        {
            try
            {
                return await this._client.SendAsync(request);
            }
            catch (HttpRequestException e)
            {
                throw new NoConnectionException(e.Message, e.InnerException);
            }
        }
    }
}