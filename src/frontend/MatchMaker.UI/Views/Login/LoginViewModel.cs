using System;
using System.Threading.Tasks;
using MatchMaker.UI.Services.Authentication;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.Login
{
    public class LoginViewModel : BaseViewModel
    {
        public IAuthService AuthService => DependencyService.Get<IAuthService>();

        private string _eMail;
        private string _password;
        
        public string EMail
        {
            get { return this._eMail; }
            set { this.SetProperty(ref this._eMail, value); }
        }

        public string Password
        {
            get { return this._password; }
            set { this.SetProperty(ref this._password, value); }
        }

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public LoginViewModel()
        {

            this.LoginCommand = new Command(async () => { await this.Login(); });
            this.RegisterCommand = new Command(async () => { await this.Register(); });

            this.EMail = "1@1.de";
            this.Password = "123456";
        }

        private async Task Register()
        {
            await this.AuthService.Register(this.EMail, this.Password);
        }

        private async Task Login()
        {
            await this.AuthService.Login(this.EMail, this.Password);
        }
    }
}