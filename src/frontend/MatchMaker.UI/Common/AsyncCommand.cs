using MatchMaker.UI.Services.Exception;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MatchMaker.UI.Common
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<object, Task> _func;
        private readonly Func<object, bool> _canExecute;

        private int _callRunning;

        public IExceptionHandler ExceptionHandler => DependencyService.Get<IExceptionHandler>();

        public event EventHandler CanExecuteChanged;

        public AsyncCommand(Func<object, Task> func, Func<object, bool> canExecute)
        {
            this._func = func;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this._callRunning == 0 && this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (Interlocked.CompareExchange(ref this._callRunning, 1, 0) == 1)
                return;

            this.OnCanExecuteChagend();

            this._func(parameter).ContinueWith((task, _) => this.ExecuteFinished(task), null, TaskContinuationOptions.ExecuteSynchronously);
        }

        private void ExecuteFinished(Task task)
        {
            Interlocked.Exchange(ref this._callRunning, 0);

            if (task.IsFaulted)
            {
                this.ExceptionHandler.Handle(task.Exception);
            }

            this.OnCanExecuteChagend();
        }

        public void OnCanExecuteChagend()
        {
            var handler = this.CanExecuteChanged;

            handler?.Invoke(this, EventArgs.Empty);
        }


    }
}