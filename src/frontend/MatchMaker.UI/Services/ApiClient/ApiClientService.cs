using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.Shared.Accounts;
using MatchMaker.UI.Exceptions;
using Newtonsoft.Json;
using Xamarin.Forms;


[assembly: Dependency(typeof(MatchMaker.UI.Services.ApiClient.ApiClientService))]
namespace MatchMaker.UI.Services.ApiClient
{
    public class ApiClientService : IApiClientService
    {
        private readonly HttpClient _client;

        public ApiClientService()
        {
            this._client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000,
                BaseAddress = new Uri("http://desktop-haefele:52940")
            };
        }


        public async Task Register(string email, string password)
        {
            var json = JsonConvert.SerializeObject(new RegisterData { EmailAddress = email, Password = password });
            var response = await this._client.PostAsync("accounts/register", new StringContent(json, Encoding.UTF8, "application/json"));

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
            var json = JsonConvert.SerializeObject(new RegisterData { EmailAddress = email, Password = password });
            var response = await this._client.PostAsync("accounts/login", new StringContent(json, Encoding.UTF8, "application/json"));
            
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
                    throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}