using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CleaningHelper.ViewModel
{
    public class Command : ICommand
    {
        private CommandExecute _execute;
        private CommandCanExecute _canExecute;

        public bool CanExecute(object parameter = null)
        {
            return _canExecute?.Invoke(parameter) == null;
        }

        public void Execute(object parameter = null)
        {
            _execute?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public Command(CommandExecute execute, CommandCanExecute canExecute = null, EventHandler canExecuteChanged = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            CanExecuteChanged += canExecuteChanged;
        }
    }
}
