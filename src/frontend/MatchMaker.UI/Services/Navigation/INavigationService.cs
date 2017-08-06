using System.Threading.Tasks;
using Xamarin.Forms;

namespace MatchMaker.UI.Services.Navigation
{
    public interface INavigationService
    {
        void NavigateToShell();
        void NavigateToNewMatch();

        Task ShowModalWindow(ContentPage page);
    }
}