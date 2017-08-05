using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.Shared.Accounts;
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
                BaseAddress = new Uri("http://192.168.1.20:52940/")
            };
        }
        

        public async Task Register(string email, string password)
        {
            var json = JsonConvert.SerializeObject(new RegisterData { EmailAddress = email, Password = password });
            var response = await this._client.PostAsync("accounts/register", new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public async Task Login(string email, string password)
        {
            try
            {
                var json = JsonConvert.SerializeObject(new RegisterData { EmailAddress = email, Password = password });
                var response = await this._client.PostAsync("accounts/login", new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (Exception e)
            {
                
            }
        }
    }
}