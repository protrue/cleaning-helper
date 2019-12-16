using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningHelper.ViewModel
{
    public delegate void CommandExecute(object parameter);

    public delegate bool CommandCanExecute(object parameter);
}
