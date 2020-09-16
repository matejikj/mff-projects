using Harmonogram.Helper;
using Harmonogram.UserControls;
using Harmonogram.ViewModels;
using Harmonogram.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Harmonogram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LoginDataHelper.init();
            // StaticResources.woodOrderVM = new WoodOrderViewModel();
            try
            {
                if (new LoginWindow().ShowDialog() == true)
                {
                    new MainWindow().ShowDialog();
                }
                else
                {
                    MessageBox.Show("Přihlášení se nezdařilo");
                }
            }
            finally
            {
                Shutdown();
            }
        }
    }
}
