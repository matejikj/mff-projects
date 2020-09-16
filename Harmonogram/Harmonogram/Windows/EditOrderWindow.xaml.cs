using Harmonogram.Helper;
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
    /// Interaction logic for EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        int departmentId;
        int orderId;

        public EditOrderWindow(int dep, int id)
        {
            InitializeComponent();
            departmentId = dep;
            orderId = id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cb_materials.ItemsSource = StaticResources.Materials;

            using (var context = new HarmonogramDBEntities())
            {
                Order order = context.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();
                tb_technik.Text = order.Technik;
                tb_zakazka.Text = order.Zakazka;
                tb_zakazkaNr.Text = order.ZakazkaNr;
                tb_kridelks.Text = order.KridlaKs;
                tb_ulice.Text = order.Ulice;
                tb_PSC.Text = order.PSC;
                tb_cislo.Text = order.Cislo;
                tb_mesto.Text = order.Mesto;
                tb_poznamky.Text = order.Poznamky;
                tb_zarubniks.Text = order.ZarubneKs;
                dp_pozadovany.SelectedDate = order.PozadovanyDatum;
                dp_zahajeniDatum.SelectedDate = order.ZahajeniDatum;
                dp_Dokonceni.SelectedDate = order.DokonceniDatum;
                tb_material.Text = order.Material;

                context.SaveChanges();
            }
        }
        private void cb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBox cb = e.OriginalSource as ComboBox;
            tb_material.Text = (string)cb.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int i;
            if (int.TryParse(tb_zarubniks.Text, out i))
            {
            }
            else
            {
                lbl_varovani.Visibility = Visibility.Visible;
                return;
            }
            if (int.TryParse(tb_kridelks.Text, out i))
            {
            }
            else
            {
                lbl_varovani.Visibility = Visibility.Visible;
                return;
            }

            using (var context = new HarmonogramDBEntities())
            {
                Order order = context.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();
                order.Technik = tb_technik.Text;
                order.Zakazka = tb_zakazka.Text;
                order.ZakazkaNr = tb_zakazkaNr.Text;
                order.KridlaKs = tb_kridelks.Text;
                order.Ulice = tb_ulice.Text;
                order.PSC = tb_PSC.Text;
                order.Cislo = tb_cislo.Text;
                order.Mesto = tb_mesto.Text;
                order.Poznamky = tb_poznamky.Text;
                order.ZarubneKs = tb_zarubniks.Text;
                order.PozadovanyDatum = dp_pozadovany.SelectedDate;
                order.ZahajeniDatum = dp_zahajeniDatum.SelectedDate;
                order.DokonceniDatum = dp_Dokonceni.SelectedDate;
                order.Material = tb_material.Text;

                context.SaveChanges();
            }

            this.DialogResult = true;
            this.Close();


        }


        private void dp_SelectedDateChanged(object sender, EventArgs e)
        {
            DateTime zahaj = dp_zahajeniDatum.SelectedDate.HasValue ? dp_zahajeniDatum.SelectedDate.Value : DateTime.Now;
            DateTime dokon = dp_Dokonceni.SelectedDate.HasValue ? dp_Dokonceni.SelectedDate.Value : DateTime.Now; ;
            DateTime spravne = DateHelper.add30WorkDays(zahaj);
            DateTime pozad = dp_pozadovany.SelectedDate.HasValue ? dp_pozadovany.SelectedDate.Value : DateTime.Now; ;

            var rozdil = pozad.Subtract(spravne).Days;
            if (rozdil == 0)
            {
                lbl_rozdil.Text = "Na den přesně s požadovaným datem";
                lbl_rozdil.Background = Brushes.White;

            }
            else
            {
                if (rozdil > 0)
                {
                    lbl_rozdil.Text = Math.Abs(rozdil).ToString() + " dní více než požadované datum";
                    lbl_rozdil.Background = Brushes.Red;
                }
                else
                {
                    lbl_rozdil.Text = Math.Abs(rozdil).ToString() + " dní méně než požadované datum";
                    lbl_rozdil.Background = Brushes.White;

                }
            }
        }

    }
}
