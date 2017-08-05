using System;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;

        public string UserName
        {
            get { return this._userName; }
            set { this.SetProperty(ref this._userName, value); }
        }

        public string Password
        {
            get { return this._password; }
            set { this.SetProperty(ref this._password, value); }
        }

        public Command LoginCommand { get; }

        public LoginViewModel()
        {

        }
        
    }
}