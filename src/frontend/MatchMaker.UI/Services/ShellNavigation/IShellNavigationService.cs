using MatchMaker.UI.Views.Shell;

namespace MatchMaker.UI.Services.ShellNavigation
{
    public interface IShellNavigationService
    {
        ShellView Shell { get; }

        void RegisterShell(ShellView shell);

        void SetShellDetailPage(ShellViewMenuItem item);
        void ActivateShellItem(ShellViewMenuItem item);
        void ReactivateShellItem(ShellViewMenuItem item, bool overrideExisting);

    }
}