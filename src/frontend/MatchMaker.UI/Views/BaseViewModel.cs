using MatchMaker.UI.Helpers;
using MatchMaker.UI.Services;
using Xamarin.Forms;

namespace MatchMaker.UI.Views
{
    public class BaseViewModel : ObservableObject
    {
        private bool _isBusy;
        private string _title;

        public bool IsBusy
        {
            get { return this._isBusy; }
            set { this.SetProperty(ref this._isBusy, value); }
        }

        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }
    }
}

