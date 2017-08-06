using System.Threading.Tasks;
using MatchMaker.UI.Common;
using MatchMaker.UI.Services.Navigation;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.Main
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        public AsyncCommand NewMatchdayCommand { get; }

        public MainViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.NewMatchdayCommand = new AsyncCommand(_ => this.NewMatchday());
        }

        private async Task NewMatchday()
        {
            this._navigationService.NavigateToNewMatchDay();
        }
    }
}