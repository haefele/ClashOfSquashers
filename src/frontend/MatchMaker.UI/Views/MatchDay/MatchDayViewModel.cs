using MatchMaker.UI.Services.Navigation;
using MatchMaker.UI.Views.MatchDay.MatchDayConfigurator;

namespace MatchMaker.UI.Views.MatchDay
{
    public class MatchDayViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        private bool _isNew;

        public bool IsNew
        {
            get { return this._isNew; }
            set { this.SetProperty(ref this._isNew, value); }
        }


        public MatchDayViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.IsNew = true;
        }

        public void OnActivate()
        {
            if (this.IsNew)
            {
                var configView = new MatchDayConfiguratorView();

                this._navigationService.ShowModalWindow(configView);

                this.IsNew = false;
            }
        }
    }
}