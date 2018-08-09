using System;
using System.Windows.Input;

namespace Touch.ViewModels
{
    public class CommandHandler : ICommand
    {
        private readonly Action<object> _action;

        public CommandHandler(Action<object> action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}