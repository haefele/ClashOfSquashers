using MatchMaker.Shared.Common;
using MatchMaker.UI.Services.Alert;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MatchMaker.UI.Services.Exception
{
    public class ExceptionHandler : IExceptionHandler
    {
        public IAlertService AlertService => DependencyService.Get<IAlertService>();

        public async Task Handle(System.Exception exception)
        {
            Guard.NotNull(exception, nameof(exception));

            await this.AlertService.DisplayAlert("An error occured.", exception.Message);
        }
    }
}