using System.Threading.Tasks;

namespace MatchMaker.UI.Services.Exception
{
    public interface IExceptionHandler
    {
        Task Handle(System.Exception exception);
    }
}