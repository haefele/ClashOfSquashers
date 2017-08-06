using System;
using System.Threading.Tasks;
using MatchMaker.UI.Exceptions;
using MatchMaker.UI.Services.Alert;
using MatchMaker.UI.Services.Authentication;
using MatchMaker.UI.Services.Navigation;
using MatchMaker.UI.Views.Shell;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly IAlertService _alertService;

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

        public LoginViewModel(IAuthService authService, INavigationService navigationService, IAlertService alertService)
        {
            this._authService = authService;
            this._navigationService = navigationService;
            this._alertService = alertService;

            this.LoginCommand = new Command(async () => { await this.Login(); });
            this.RegisterCommand = new Command(async () => { await this.Register(); });

            this.EMail = "1@1.de";
            this.Password = "123456";
        }

        private async Task Register()
        {
            try
            {
                await this._authService.Register(this.EMail, this.Password);
            }
            catch (EmailAlreadyInUseException)
            {
                await this._alertService.DisplayAlert("Invalid Sign-Up", "This email-address is already in use.");
            }
        }

        private async Task Login()
        {
            try
            {
                await this._authService.Login(this.EMail, this.Password);
            }
            catch (InvalidPasswordException)
            {
                await this._alertService.DisplayAlert("Invalid Logindata", "Wrong password");
                return;
            }
            catch (UserNotFoundException)
            {
                await this._alertService.DisplayAlert("Invalid Logindata", "User was not found");
                return;
            }

            this._navigationService.NavigateToShell();
        }
    }
}