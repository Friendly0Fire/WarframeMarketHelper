using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WarframeMarketHelper
{
    class ShowWindowCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var app = Application.Current as App;
            app.ShowWindow();
        }
    }
}
