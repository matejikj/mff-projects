using Harmonogram.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ExpeditionUserControl.xaml
    /// </summary>
    public partial class ExpeditionUserControl : UserControl
    {
        Visibility adresaVisibility;

        ExpeditionViewModel expeditionVM;
        HarmonogramDBEntities db;
        public ExpeditionUserControl()
        {
            InitializeComponent();
            expeditionVM = new ExpeditionViewModel();
            UserViewModel uvm = new UserViewModel();
            DataContext = expeditionVM;


            ExpeditionDataGrid.ItemsSource = expeditionVM.Expeditions;

            User logUser = uvm.Users.Where(p => p.Username == StaticResources.UserName).FirstOrDefault();
            adresaVisibility = Visibility.Hidden;
            CollapseAllNotification();
            db = new HarmonogramDBEntities();
            Cislo.Visibility = Visibility.Collapsed;
            Ulice.Visibility = Visibility.Collapsed;
            PSC.Visibility = Visibility.Collapsed;
        }

        public void CollapseAllNotification()
        {
            VyberVarovani.Visibility = Visibility.Collapsed;
            CisloVarovani.Visibility = Visibility.Collapsed;
            Ulozeno.Visibility = Visibility.Collapsed;
            Validita.Visibility = Visibility.Collapsed;
        }

        public void btnDown_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllNotification();

            if (expeditionVM.Current_expedition != null)
            {
                int id = expeditionVM.Current_expedition.OrderId;
                if (expeditionVM.Expeditions.Count > expeditionVM.Current_expedition.ExpeditedSortNr)
                {
                    Expedition currE = expeditionVM.Current_expedition;
                    Expedition nextE = expeditionVM.Expeditions.Where(p => p.ExpeditedSortNr == currE.ExpeditedSortNr + 1).FirstOrDefault();
                    Expedition expedition = db.Expeditions.Find(id);
                    if (expedition == null)
                    {
                        Expedition removeE = expeditionVM.Expeditions.Where(p => p.ExpeditionId == expeditionVM.Current_expedition.OrderId).SingleOrDefault();
                        expeditionVM.Expeditions.Remove(removeE);
                        MessageBox.Show("Vybraná objednávka již byla dříve odstraněna jiným uživatelem");
                    }
                    else
                    {
                        Expedition nextExpedition = db.Expeditions.FirstOrDefault(p => p.ExpeditedSortNr == expeditionVM.Current_expedition.ExpeditedSortNr.Value + 1 );
                        if (nextE.OrderId == nextExpedition.OrderId && currE.OrderId == expedition.OrderId)
                        {
                            nextExpedition.ExpeditedSortNr = nextExpedition.ExpeditedSortNr - 1;
                            nextE.ExpeditedSortNr = nextE.ExpeditedSortNr - 1;

                            expedition.ExpeditedSortNr = expedition.ExpeditedSortNr + 1;
                            currE.ExpeditedSortNr = currE.ExpeditedSortNr + 1;
                            db.Expeditions.AddOrUpdate(nextExpedition);
                            db.Expeditions.AddOrUpdate(expedition);
                            db.SaveChanges();
                            int index = expeditionVM.Expeditions.IndexOf(currE);
                            expeditionVM.Expeditions[index] = nextE;
                            expeditionVM.Expeditions[index + 1] = currE;
                            expeditionVM.Current_expedition = currE;
                        }
                        else
                        {
                            MessageBox.Show("Databaze ma jiz nove polozky, prosim obnovte zobrazeni a pote opakujte");
                        }
                    }
                }
            } else
            {
                VyberVarovani.Visibility = Visibility.Visible;
            }
        }

        private void HandleAddressLinkClick(object sender, RoutedEventArgs e)
        {
            CollapseAllNotification();
            Expedition selectedExpedition = expeditionVM.Current_expedition;
            String ulice;
            String mesto;
            String cislo;
            ulice = selectedExpedition.Order.Ulice;
            mesto = selectedExpedition.Order.Mesto;
            cislo = selectedExpedition.Order.Cislo;
            String navigateUri = "https://www.google.cz/maps/search/" + ulice + " " + cislo + ", " + mesto;
            try
            {
                Process.Start(new ProcessStartInfo(navigateUri));
                e.Handled = true;
            }
            catch
            {
                MessageBox.Show("Cesta k souboru neexistuje");
            }
        }

        public void btnUp_Click(object sender, EventArgs e)
        {
            CollapseAllNotification();

            if (expeditionVM.Current_expedition != null)
            {
                int id = expeditionVM.Current_expedition.OrderId;
                if (expeditionVM.Expeditions.Count > expeditionVM.Current_expedition.ExpeditedSortNr)
                {
                    Expedition currE = expeditionVM.Current_expedition;
                    Expedition nextE = expeditionVM.Expeditions.Where(p => p.ExpeditedSortNr == currE.ExpeditedSortNr - 1).FirstOrDefault();
                    Expedition expedition = db.Expeditions.Find(id);
                    if (expedition == null)
                    {
                        Expedition removeE = expeditionVM.Expeditions.Where(p => p.ExpeditionId == expeditionVM.Current_expedition.OrderId).SingleOrDefault();
                        expeditionVM.Expeditions.Remove(removeE);
                        MessageBox.Show("Vybraná objednávka již byla dříve odstraněna jiným uživatelem");
                    }
                    else
                    {
                        Expedition nextExpedition = db.Expeditions.FirstOrDefault(p => p.ExpeditedSortNr == expeditionVM.Current_expedition.ExpeditedSortNr.Value - 1);
                        if (nextE.OrderId == nextExpedition.OrderId && currE.OrderId == expedition.OrderId)
                        {
                            nextExpedition.ExpeditedSortNr = nextExpedition.ExpeditedSortNr + 1;
                            nextE.ExpeditedSortNr = nextE.ExpeditedSortNr + 1;

                            expedition.ExpeditedSortNr = expedition.ExpeditedSortNr - 1;
                            currE.ExpeditedSortNr = currE.ExpeditedSortNr - 1;
                            db.Expeditions.AddOrUpdate(nextExpedition);
                            db.Expeditions.AddOrUpdate(expedition);
                            db.SaveChanges();
                            int index = expeditionVM.Expeditions.IndexOf(currE);
                            expeditionVM.Expeditions[index] = nextE;
                            expeditionVM.Expeditions[index - 1] = currE;
                            expeditionVM.Current_expedition = currE;
                        }
                        else
                        {
                            MessageBox.Show("Databaze ma jiz nove polozky, prosim obnovte zobrazeni a pote opakujte");
                        }
                    }
                }
            }
            else
            {
                VyberVarovani.Visibility = Visibility.Visible;
            }
        }

        public void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllNotification();
            expeditionVM = new ExpeditionViewModel();
            ExpeditionDataGrid.ItemsSource = expeditionVM.Expeditions;
        }

        public void btnDelete_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Opravdu chcete odstranit vybranou expedici?\nPočet vyexpedovaných kusů se opět přičte zpět.", "", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    CollapseAllNotification();

                    if (expeditionVM.Current_expedition == null)
                    {
                        VyberVarovani.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        var deleteExpedition = db.Expeditions.Where(m => !m.Order.IsDeleted && m.ExpeditionId == expeditionVM.Current_expedition.ExpeditionId).Single();
                        int exsortnr = deleteExpedition.ExpeditedSortNr.HasValue ? deleteExpedition.ExpeditedSortNr.Value : 0;
                        var exlist = db.Expeditions.ToList();
                        foreach (var item in exlist)
                        {
                            int n = item.ExpeditedSortNr.HasValue ? item.ExpeditedSortNr.Value : 0;
                            if (n > exsortnr)
                            {
                                item.ExpeditedSortNr--;
                            }
                        }
                        expeditionVM.Expeditions.Remove(expeditionVM.Current_expedition);
                        foreach ( var item in expeditionVM.Expeditions)
                        {
                            int n = item.ExpeditedSortNr.HasValue ? item.ExpeditedSortNr.Value : 0;
                            if (n > exsortnr)
                            {
                                item.ExpeditedSortNr--;
                            }
                        }
                        
                        int orderid = deleteExpedition.OrderId;

                        var expeds = db.Expeditions.Where(o => !o.Order.IsDeleted && o.OrderId == orderid).ToList();
                        int i = 0;
                        int ks = 0;

                        foreach (Expedition expedition in expeds)
                        {
                            if (int.TryParse(expedition.KridlaKsExpedovanych, out ks))
                            {
                                i = i + ks;
                            }
                        }

                        int.TryParse(deleteExpedition.KridlaKsExpedovanych, out ks);
                        i = i - ks;


                        var updateOrder = db.Orders.Where(o => !o.IsDeleted && o.OrderId == orderid).FirstOrDefault();
                        updateOrder.KridlaKsExpedovanych = i.ToString();
                        int b = 0;
                        if (int.TryParse(updateOrder.KridlaKs, out b))
                        {
                            updateOrder.KridlaText = (b - i).ToString();
                        }

                        var expedis = db.Expeditions.Where(o => !o.Order.IsDeleted && o.OrderId == orderid).ToList();
                        int j = 0;
                        int kszar = 0;

                        foreach (Expedition expedition in expedis)
                        {
                            if (int.TryParse(expedition.ZarubneKsExpedovanych, out kszar))
                            {
                                j = j + kszar;
                            }
                        }

                        int.TryParse(deleteExpedition.ZarubneKsExpedovanych, out kszar);
                        j = j - kszar;

                        var updateOrderzarubne = db.Orders.Where(o => !o.IsDeleted && o.OrderId == orderid).FirstOrDefault();

                        updateOrderzarubne.ZarubneKsExpedovanych = j.ToString();
                        int a = 0;
                        if (int.TryParse(updateOrderzarubne.ZarubneKs, out a))
                        {
                            updateOrderzarubne.ZarubneText = (a - j).ToString();
                        }
                        db.Expeditions.Remove(deleteExpedition);
                        db.SaveChanges();
                    }

                    break;
                case MessageBoxResult.No:
                    CollapseAllNotification();
                    break;
            }
        }

        private void dp_SelectedDateChanged(object sender, EventArgs e)
        {
            if (expeditionVM.Current_expedition != null)
            {
                DatePicker dp = sender as DatePicker;
                DateTime expdat = dp.SelectedDate.HasValue ? dp.SelectedDate.Value : DateTime.Now;
                var updateExpedition = db.Expeditions.Where(o => !o.Order.IsDeleted && o.ExpeditionId == expeditionVM.Current_expedition.ExpeditionId).Single();
                updateExpedition.ExpediceDatum = expdat;
                db.SaveChanges();
            }
        }

        private void CheckBoxHeader_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllNotification();

            var columnHeader = e.OriginalSource as DataGridColumnHeader;
            string label = columnHeader.Content as string;
            switch (label)
            {

                case "Mesto":
                    if (adresaVisibility == Visibility.Visible)
                    {
                        adresaVisibility = Visibility.Collapsed;
                        Cislo.Visibility = Visibility.Collapsed;
                        Ulice.Visibility = Visibility.Collapsed;
                        PSC.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        adresaVisibility = Visibility.Visible;
                        Cislo.Visibility = Visibility.Visible;
                        Ulice.Visibility = Visibility.Visible;
                        PSC.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void expeditionColorBtn_changed(object sender, MouseButtonEventArgs e)
        {
            VyberVarovani.Visibility = Visibility.Collapsed;
            CisloVarovani.Visibility = Visibility.Collapsed;
            Ulozeno.Visibility = Visibility.Collapsed; ;
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                var updateExpedition = db.Expeditions.Where(o => !o.Order.IsDeleted && o.ExpeditionId == expeditionVM.Current_expedition.ExpeditionId).Single();
                updateExpedition.ExepdiceColor = scb.ToString();
                expeditionVM.Current_expedition.ExepdiceColor = scb.ToString();
                db.SaveChanges();
            }
        }

        private void TypAutaColorBtn_changed(object sender, MouseButtonEventArgs e)
        {
            VyberVarovani.Visibility = Visibility.Collapsed;
            CisloVarovani.Visibility = Visibility.Collapsed;
            Ulozeno.Visibility = Visibility.Collapsed; ;
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                var updateExpedition = db.Expeditions.Where(o => !o.Order.IsDeleted && o.ExpeditionId == expeditionVM.Current_expedition.ExpeditionId).Single();
                updateExpedition.AutoColor = scb.ToString();
                expeditionVM.Current_expedition.AutoColor = scb.ToString();
                db.SaveChanges();
            }
        }

        private void Material_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = e.OriginalSource as System.Windows.Controls.TextBox;
            string s = tb.Text;
            Expedition exp = db.Expeditions.Where(p => p.ExpeditionId == expeditionVM.Current_expedition.ExpeditionId).FirstOrDefault();
            exp.ExpedicePoradi = s;
            db.SaveChanges();
        }

        private void TypAuta_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = e.OriginalSource as System.Windows.Controls.TextBox;
            string s = tb.Text;
            Expedition exp = db.Expeditions.Where(p => p.ExpeditionId == expeditionVM.Current_expedition.ExpeditionId).FirstOrDefault();
            exp.ExpediceAuto = s;
            db.SaveChanges();
        }
    }
}
