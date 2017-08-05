using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;


[assembly: Dependency(typeof(MatchMaker.UI.Services.ApiClient.IApiClientService))]
namespace MatchMaker.UI.Services.ApiClient
{
    public class ApiClient : IApiClientService
    {
        private readonly HttpClient _client;

        public ApiClient()
        {
            this._client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000,
                BaseAddress = new Uri("")
            };
        }

        public async Task Login()
        {

        }

        public async Task Register(string userName, string password)
        {
            //var user = JsonConvert.SerializeObject();

            var response = await this._client.PostAsync("accounts/register", new StringContent("", Encoding.UTF8, "application/json"));
        }
    }
}