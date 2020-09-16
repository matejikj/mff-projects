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
    /// Interaction logic for NewBlankRow.xaml
    /// </summary>
    public partial class NewBlankRow : Window
    {
        int department;
        public NewBlankRow(int dep)
        {
            InitializeComponent();
            this.department = dep;
            StaticResources.blankRowColor = "NULL";
        }

        private void changeColor_changed(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                changeColor.Background = scb;
                StaticResources.blankRowColor = scb.ToString();
            }            
        }

        private void saveRow_changed(object sender, RoutedEventArgs e)
        {
            StaticResources.blankRowName = tb_zakazka.Text;
            this.DialogResult = true;
            this.Close();
        }
    }
}
