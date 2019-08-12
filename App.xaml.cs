using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using WarframeMarketHelper.Properties;

namespace WarframeMarketHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Helper _helper;
        private TaskbarIcon _taskIcon;
        private MainWindow _window;

        public void Start() => _helper.Start();

        public void Stop() => _helper.Stop();

        public string Token
        {
            get => _helper.Token;
            set
            {
                _helper.Token = value;
                Settings.Default.token = value;
                Settings.Default.Save();
            }
        }

        public bool InStartup
        {
            get
            {
                using (var key =
                    Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    var refVal = "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"";
                    var curVal = key?.GetValue("Warframe Market Helper", false) as string;
                    return refVal == curVal;
                }
            }
            set
            {
                using (var key =
                    Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (value)
                        key?.SetValue("Warframe Market Helper",
                            "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
                    else
                        key?.SetValue("Warframe Market Helper", false);
                }
            }
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _helper = new Helper();
            _helper.Token = Settings.Default.token;
            _taskIcon = (TaskbarIcon) FindResource("BackgroundIcon");

            _helper.Start();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _helper.Stop();
        }

        public void ShowWindow()
        {
            _window = new MainWindow();
            _window.Show();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Current.Shutdown();
        }
    }
}
