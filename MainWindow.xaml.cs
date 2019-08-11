using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WarframeMarketHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            RestartHelper();
        }

        private void BtnOK_OnClick(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Token = txtToken.Text;
            if (chkStartWithWindows.IsChecked ?? false)
                app.InStartup = true;
            else
                app.InStartup = false;

            RestartHelper();
            Close();
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            RestartHelper();
            Close();
        }

        private void RestartHelper()
        {
            var app = Application.Current as App;
            app.Start();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;
            app.Stop();
            txtToken.Text = app.Token;
            chkStartWithWindows.IsChecked = app.InStartup;
        }
    }
}
