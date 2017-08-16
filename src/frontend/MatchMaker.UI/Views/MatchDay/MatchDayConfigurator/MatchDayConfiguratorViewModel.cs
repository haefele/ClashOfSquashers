using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MatchMaker.Shared.Accounts;
using MatchMaker.Shared.MatchDays;
using MatchMaker.UI.Common;
using MatchMaker.UI.Exceptions;
using MatchMaker.UI.Services.Accounts;
using MatchMaker.UI.Services.Alert;
using MatchMaker.UI.Services.Exception;
using MatchMaker.UI.Services.MatchDays;
using MatchMaker.UI.Services.Navigation;
using Xamarin.Forms;

namespace MatchMaker.UI.Views.MatchDay.MatchDayConfigurator
{
    public class MatchDayConfiguratorViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMatchDaysService _matchDaysService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IAccountService _accountService;

        private string _searchText;

        private ObservableCollection<Account> _foundAccounts;
        private Account _selectedAccount;

        public string SearchText
        {
            get { return this._searchText; }
            set { this.SetProperty(ref this._searchText, value); }
        }

        public ObservableCollection<Account> FoundAccounts
        {
            get { return this._foundAccounts; }
            set { this.SetProperty(ref this._foundAccounts, value); }
        }

        public Account SelectedFoundAccount
        {
            get { return this._selectedAccount; }
            set { this.SetProperty(ref this._selectedAccount, value); }
        }

        public AsyncCommand SearchCommand { get; }

        public AsyncCommand CreateCommand { get; }

        public Command CancelCommand { get; }

        public MatchDayConfiguratorViewModel(INavigationService navigationService, IMatchDaysService matchDaysService, IExceptionHandler exceptionHandler, IAccountService accountService)
        {
            this._navigationService = navigationService;
            this._matchDaysService = matchDaysService;
            this._exceptionHandler = exceptionHandler;
            this._accountService = accountService;

            this.SearchCommand = new AsyncCommand(_ => this.Search());

            this.CreateCommand = new AsyncCommand(_ => this.Create());
            this.CancelCommand = new Command(_ => this.MatchCreationCanceled?.Invoke(this, EventArgs.Empty));
        }

        private async Task Search()
        {
            try
            {
                var accounts = await this._accountService.SearchAccounts(this.SearchText);
            }
            catch (UserNotFoundException)
            {
                this.FoundAccounts = new ObservableCollection<Account>();

                for (int i = 0; i < 20; i++)
                {
                    this.FoundAccounts.Add(new Account{EmailAddress = i+"@1.de"});
                }
            }
            catch (Exception)
            {

            }
        }

        private async Task Create()
        {
            try
            {
                var response = await this._matchDaysService.CreateNewMatchDay(new List<int> { 1, 2 }, DateTime.Now);
                this.MatchCreated?.Invoke(this, new MatchDayCreatedEventArgs(response));
            }
            catch (Exception e)
            {
                await this._exceptionHandler.Handle(e);
                await this._navigationService.PopModalWindow();
            }
        }

        public event EventHandler<MatchDayCreatedEventArgs> MatchCreated;

        public EventHandler MatchCreationCanceled;

        public class MatchDayCreatedEventArgs : EventArgs
        {
            public Shared.MatchDays.MatchDay MatchDay { get; }

            public MatchDayCreatedEventArgs(Shared.MatchDays.MatchDay matchDay)
            {
                this.MatchDay = matchDay;
            }
        }
    }
}