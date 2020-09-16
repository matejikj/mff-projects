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
using System.Windows.Shapes;

namespace Harmonogram.Windows
{
    /// <summary>
    /// Interaction logic for EditUrlWindow.xaml
    /// </summary>
    public partial class EditUrlWindow : Window
    {
        public EditUrlWindow()
        {
            InitializeComponent();
            tb_url.Text = StaticResources.lastUrl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StaticResources.lastUrl = tb_url.Text;
            Console.Write(StaticResources.lastUrl);
            Close();

        }
    }
}
