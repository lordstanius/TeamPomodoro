using System;
using System.Windows.Input;

namespace ViewModel
{
    public class CustomCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _execute;

        public event EventHandler CanExecuteChanged;

        public CustomCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}