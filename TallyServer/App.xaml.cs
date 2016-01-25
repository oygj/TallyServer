using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TallyServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private Views.MainView _mainView;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _mainView = new Views.MainView();
            _mainView.DataContext = new ViewModels.MainViewModel();
            _mainView.Show();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (_mainView != null)
                _mainView.Close();
        }

    }
}
