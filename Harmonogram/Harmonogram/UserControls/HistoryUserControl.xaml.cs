using Harmonogram.ViewModels;
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

namespace Harmonogram.UserControls
{
    /// <summary>
    /// Interaction logic for HistoryUserControl.xaml
    /// </summary>
    public partial class HistoryUserControl : UserControl
    {
        HistoryOrderViewModel orderVM;
        HarmonogramDBEntities db;

        public HistoryUserControl()
        {
            InitializeComponent();
            orderVM = new HistoryOrderViewModel();
            DataContext = orderVM;
            HistoryDataGrid.ItemsSource = orderVM.Orders;
            db = new HarmonogramDBEntities();

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                Order o = db.Orders.Where(p => p.OrderId == orderVM.Current_order.OrderId).FirstOrDefault();
                orderVM.Orders.Remove(orderVM.Current_order);
                o.SortNr = db.Orders.Where(p => p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == o.Department).ToList().Count + 1;
                o.IsDeleted = false;
                o.IsInProccess = true;
                o.KridlaText = o.KridlaKs + "/0";
                o.ZarubneText = o.ZarubneKs + "/0";


                db.SaveChanges();
            }
        }
    }
}
