using MatchMaker.UI.Services.Navigation;

namespace MatchMaker.UI.Views.MatchDay.MatchDayConfigurator
{
    public class MatchDayConfiguratorViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public MatchDayConfiguratorViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

        }
    }
}