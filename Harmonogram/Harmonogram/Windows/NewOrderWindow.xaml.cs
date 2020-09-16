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
    /// Interaction logic for NewOrderWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        string warning;
        int department;

        public NewOrderWindow(int dep)
        {
            InitializeComponent();
            department = dep;
            dp_zahajeniDatum.SelectedDate = DateTime.Today;
            dp_Dokonceni.SelectedDate = DateHelper.add30WorkDays(DateTime.Today);
            dp_pozadovany.SelectedDate = dp_Dokonceni.SelectedDate;
            tb_technik.Text = StaticResources.UserName;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cb_materials.ItemsSource = StaticResources.Materials;

        }



        private void cb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBox cb = e.OriginalSource as ComboBox;
            tb_material.Text = (string)cb.SelectedItem;

        }


        public void btnAdd_Click(object sender, EventArgs e)
        {
            warning = "";

            Order order = new Order();

            order.Cislo = tb_cislo.Text;
            order.Ulice = tb_ulice.Text;
            order.Mesto = tb_mesto.Text;
            order.PSC = tb_PSC.Text;

            order.Adresa = order.Ulice + " " + order.Cislo + ", " + order.Mesto;

            order.Department = department;

            order.VyrobniNr = "";
            order.DocUrl = "http://www.lignis.cz";
            order.ArchivedDate = null;
            order.IsInProccess = false;
            order.IsDeleted = false;
            order.NotBlank = true;


            order.Priprava = "";
            order.Oblozky = "";
            order.Stredove = "";
            order.Ramecek = "";
            order.Klapacka = "";
            order.Posuv = "";
            order.Dvere = "";
            order.Dokonceni = "";
            order.Kompletace = "";
            order.Sklad = "";

            order.BrouseniDyhaDokonceni = "";
            order.BrouseniZakladDokonceni = "";
            order.CentrumDvere = "";
            order.CentrumStredove = "";
            order.CncDvere = "";
            order.CncOblozky = "";
            order.Dokumentace = "";
            order.DorazovaPosuv = "";
            order.DvereKompletace = "";
            order.FormatkaDvere = "";
            order.FrezovaniStredove = "";
            order.GarnyzPosuv = "";
            order.HranolPosuv = "";
            order.InDoca = "";
            order.KorpusyDvere = "";
            order.KovaniSklad = "";
            order.LisDvere = "";
            order.LisStredove = "";
            order.ObalovaniKlapacka = "";
            order.ObalovaniRamecek = "";
            order.ObalovatOblozky = "";
            order.OlepovackaDvere = "";
            order.OlepovaniStredove = "";
            order.PgmDvere = "";
            order.RezaniKlapacka = "";
            order.RezaniRamecek = "";
            order.RezatOblozky = "";
            order.SesazenkyDvere = "";
            order.SkloSklad = "";
            order.TypRamecek = "";
            order.VrchDokonceni = "";
            order.ZakladDokonceni = "";
            order.ZarubenKompletace = "";

            order.PripravaHli = "";
            order.DokumentPripravaHli = "";
            order.IndocaPripravaHli = "";
            order.NarezHli = "";
            order.ProfilNarezHli = "";
            order.ListyNarezHli = "";
            order.CncHli = "";
            order.SplnenoCncHli = "";
            order.FrezaHli = "";
            order.SplnenoFrezaHli = "";
            order.Priprava2Hli = "";
            order.SplnenoPriprava2Hli = "";
            order.StrikarnaHli = "";
            order.SplnenoStrikarnaHli = "";
            order.KompletaceHli = "";
            order.SplnenoKompletaceHli = "";
            order.SkladHli = "";
            order.ProfilSkladHli = "";
            order.BarvaSkladHli = "";
            order.KovaniSkladHli = "";
            order.SkloSkladHli = "";

            //zarubne
            order.PripravaZar = "";
            order.DokumentPripravaZar = "";
            order.InDocaPripravaZar = "";
            order.LaserZar = "";
            order.PohLaserZar = "";
            order.NepohLaserZar = "";
            order.PoutecLaserZar = "";
            order.NuzkyZar = "";
            order.SplnenoNuzkyZar = "";
            order.OhybackaZar = "";
            order.PohOhybackaZar = "";
            order.NepohOhybackaZar = "";
            order.PoutecOhybackaZar = "";
            order.PilaZar = "";
            order.PohPilaZar = "";
            order.NepohPilaZar = "";
            order.VyrazeckaZar = "";
            order.SplnenoVyrazeckaZar = "";
            order.BodovaniZar = "";
            order.SplnenoBodovaniZar = "";
            order.SvarovaniZar = "";
            order.SplnenoSvarovaniZar = "";
            order.BrouseniZar = "";
            order.SplnenoBrouseniZar = "";
            order.StrikarnaZar = "";
            order.SplnenoStrikarnaZar = "";
            order.RameckyZar = "";
            order.NarezRameckyZar = "";
            order.FrezRameckyZar = "";
            order.SvarRameckyZar = "";
            order.StriRameckyZar = "";
            order.KompletaceZar = "";
            order.SplnenoKompletaceZar = "";
            order.SkladZar = "";
            order.PlechSkladZar = "";
            order.BarvaSkladZar = "";
            order.KovaniSkladZar = "";
            order.SkloSkladZar = "";

            order.PripravaKrid = "";
            order.DokumentPripravaKrid = "";
            order.IndocaPripravaKrid = "";
            order.LaserKrid = "";
            order.PohLaserKrid = "";
            order.NepohLaserKrid = "";
            order.KlapackaLaserKrid = "";
            order.OkopLaserKrid = "";
            order.OhybackaKrid = "";
            order.PohOhybackaKrid = "";
            order.NepohOhybackaKrid = "";
            order.KlapackaOhybackaKrid = "";
            order.BodovaniKrid = "";
            order.SplnenoBodovaniKrid = "";
            order.ThermacolKrid = "";
            order.NarezThermacolKrid = "";
            order.LepeniThermacolKrid = "";
            order.CncThermacolKrid = "";
            order.LepeniKrid = "";
            order.SplnenoLepeniKrid = "";
            order.StrikarnaKrid = "";
            order.SplnenoStrikarnaKrid = "";
            order.RameckyKrid = "";
            order.NarezRameckyKrid = "";
            order.FrezRameckyKrid = "";
            order.SvarRameckyKrid = "";
            order.StriRameckyKrid = "";
            order.KompletaceKrid = "";
            order.SplnenoKompletaceKrid = "";
            order.SkladKrid = "";
            order.PlechSkladKrid = "";
            order.KovaniSkladKrid = "";
            order.BarvaSkladKrid = "";
            order.SkloSkladKrid = "";

            order.PripravaC = "Yellow";
            order.OblozkyC = "Yellow";
            order.StredoveC = "Yellow";
            order.RamecekC = "Yellow";
            order.KlapackaC = "Yellow";
            order.PosuvC = "Yellow";
            order.DvereC = "Yellow";
            order.DokonceniC = "Yellow";
            order.KompletaceC = "Yellow";
            order.SkladC = "Yellow";

            order.BrouseniDyhaDokonceniC = "Yellow";
            order.BrouseniZakladDokonceniC = "Yellow";
            order.CentrumDvereC = "Yellow";
            order.CentrumStredoveC = "Yellow";
            order.CncDvereC = "Yellow";
            order.CncOblozkyC = "Yellow";
            order.DokumentaceC = "Yellow";
            order.DorazovaPosuvC = "Yellow";
            order.DvereKompletaceC = "Yellow";
            order.FormatkaDvereC = "Yellow";
            order.FrezovaniStredoveC = "Yellow";
            order.GarnyzPosuvC = "Yellow";
            order.HranolPosuvC = "Yellow";
            order.InDocaC = "Yellow";
            order.KorpusyDvereC = "Yellow";
            order.KovaniSkladC = "Yellow";
            order.LisDvereC = "Yellow";
            order.LisStredoveC = "Yellow";
            order.ObalovaniKlapackaC = "Yellow";
            order.ObalovaniRamecekC = "Yellow";
            order.ObalovatOblozkyC = "Yellow";
            order.OlepovackaDvereC = "Yellow";
            order.OlepovaniStredoveC = "Yellow";
            order.PgmDvereC = "Yellow";
            order.RezaniKlapackaC = "Yellow";
            order.RezaniRamecekC = "Yellow";
            order.RezatOblozkyC = "Yellow";
            order.SesazenkyDvereC = "Yellow";
            order.SkloSkladC = "Yellow";
            order.TypRamecekC = "Yellow";
            order.VrchDokonceniC = "Yellow";
            order.ZakladDokonceniC = "Yellow";
            order.ZarubenKompletaceC = "Yellow";

            order.PripravaHliC = "Yellow";
            order.DokumentPripravaHliC = "Yellow";
            order.IndocaPripravaHliC = "Yellow";
            order.NarezHliC = "Yellow";
            order.ProfilNarezHliC = "Yellow";
            order.ListyNarezHliC = "Yellow";
            order.CncHliC = "Yellow";
            order.SplnenoCncHliC = "Yellow";
            order.FrezaHliC = "Yellow";
            order.SplnenoFrezaHliC = "Yellow";
            order.Priprava2HliC = "Yellow";
            order.SplnenoPriprava2HliC = "Yellow";
            order.StrikarnaHliC = "Yellow";
            order.SplnenoStrikarnaHliC = "Yellow";
            order.KompletaceHliC = "Yellow";
            order.SplnenoKompletaceHliC = "Yellow";
            order.SkladHliC = "Yellow";
            order.ProfilSkladHliC = "Yellow";
            order.BarvaSkladHliC = "Yellow";
            order.KovaniSkladHliC = "Yellow";
            order.SkloSkladHliC = "Yellow";

            //zarubne
            order.PripravaZarC = "Yellow";
            order.DokumentPripravaZarC = "Yellow";
            order.InDocaPripravaZarC = "Yellow";
            order.LaserZarC = "Yellow";
            order.PohLaserZarC = "Yellow";
            order.NepohLaserZarC = "Yellow";
            order.PoutecLaserZarC = "Yellow";
            order.NuzkyZarC = "Yellow";
            order.SplnenoNuzkyZarC = "Yellow";
            order.OhybackaZarC = "Yellow";
            order.PohOhybackaZarC = "Yellow";
            order.NepohOhybackaZarC = "Yellow";
            order.PoutecOhybackaZarC = "Yellow";
            order.PilaZarC = "Yellow";
            order.PohPilaZarC = "Yellow";
            order.NepohPilaZarC = "Yellow";
            order.VyrazeckaZarC = "Yellow";
            order.SplnenoVyrazeckaZarC = "Yellow";
            order.BodovaniZarC = "Yellow";
            order.SplnenoBodovaniZarC = "Yellow";
            order.SvarovaniZarC = "Yellow";
            order.SplnenoSvarovaniZarC = "Yellow";
            order.BrouseniZarC = "Yellow";
            order.SplnenoBrouseniZarC = "Yellow";
            order.StrikarnaZarC = "Yellow";
            order.SplnenoStrikarnaZarC = "Yellow";
            order.RameckyZarC = "Yellow";
            order.NarezRameckyZarC = "Yellow";
            order.FrezRameckyZarC = "Yellow";
            order.SvarRameckyZarC = "Yellow";
            order.StriRameckyZarC = "Yellow";
            order.KompletaceZarC = "Yellow";
            order.SplnenoKompletaceZarC = "Yellow";
            order.SkladZarC = "Yellow";
            order.PlechSkladZarC = "Yellow";
            order.BarvaSkladZarC = "Yellow";
            order.KovaniSkladZarC = "Yellow";
            order.SkloSkladZarC = "Yellow";

            order.PripravaKridC = "Yellow";
            order.DokumentPripravaKridC = "Yellow";
            order.IndocaPripravaKridC = "Yellow";
            order.LaserKridC = "Yellow";
            order.PohLaserKridC = "Yellow";
            order.NepohLaserKridC = "Yellow";
            order.KlapackaLaserKridC = "Yellow";
            order.OkopLaserKridC = "Yellow";
            order.OhybackaKridC = "Yellow";
            order.PohOhybackaKridC = "Yellow";
            order.NepohOhybackaKridC = "Yellow";
            order.KlapackaOhybackaKridC = "Yellow";
            order.BodovaniKridC = "Yellow";
            order.SplnenoBodovaniKridC = "Yellow";
            order.ThermacolKridC = "Yellow";
            order.NarezThermacolKridC = "Yellow";
            order.LepeniThermacolKridC = "Yellow";
            order.CncThermacolKridC = "Yellow";
            order.LepeniKridC = "Yellow";
            order.SplnenoLepeniKridC = "Yellow";
            order.StrikarnaKridC = "Yellow";
            order.SplnenoStrikarnaKridC = "Yellow";
            order.RameckyKridC = "Yellow";
            order.NarezRameckyKridC = "Yellow";
            order.FrezRameckyKridC = "Yellow";
            order.SvarRameckyKridC = "Yellow";
            order.StriRameckyKridC = "Yellow";
            order.KompletaceKridC = "Yellow";
            order.SplnenoKompletaceKridC = "Yellow";
            order.SkladKridC = "Yellow";
            order.PlechSkladKridC = "Yellow";
            order.KovaniSkladKridC = "Yellow";
            order.BarvaSkladKridC = "Yellow";
            order.SkloSkladKridC = "Yellow";

            int i;

            if (int.TryParse(tb_kridelks.Text, out i))
            {
                order.KridlaKs = tb_kridelks.Text;
            }
            else
            {
                lbl_varovani.Visibility = Visibility.Visible;
                order.KridlaKs = "0";
                return;
            }

            order.KridlaText = order.KridlaKs.ToString();

            order.Material = tb_material.Text;
            order.MaterialColor = "white";

            order.IsArchived = false;

            order.ZahajeniDatum = Convert.ToDateTime(dp_zahajeniDatum.Text);
            order.DokonceniDatum = Convert.ToDateTime(dp_Dokonceni.Text);
            order.PozadovanyDatum = Convert.ToDateTime(dp_pozadovany.Text);

            order.Poznamky = tb_poznamky.Text;

            order.Technik = StaticResources.UserName;

            if (tb_zakazka.Text == "")
            {
                warning += "jmeno zakazky, ";

            }
            else
            {
                order.Zakazka = tb_zakazka.Text;

            }


            if (tb_zakazkaNr.Text == "")
            {
                warning += "cislo zakazky, ";

            }
            else
            {
                order.ZakazkaNr = tb_zakazkaNr.Text;
            }

            if (int.TryParse(tb_zarubniks.Text, out i))
            {
                order.ZarubneKs = tb_zarubniks.Text;
            }
            else
            {
                lbl_varovani.Visibility = Visibility.Visible;
                order.ZarubneKs = "0";
                return;
            }
            order.ZarubneText = order.ZarubneKs.ToString();

            order.KridlaKsExpedovanych = "0";
            order.ZarubneKsExpedovanych = "0";

            if (warning != "")
            {
                tbl_varovani.Text = "Koukni se ještě jednou a doplň: " + warning;
            }
            else
            {
                using (var context = new HarmonogramDBEntities())
                {
                    order.OrderId = context.Orders.Max(x => x.OrderId) + 1;
                    order.SortNr = null;
                    StaticResources.lastAddedOrder = order;
                    context.Orders.Add(order);
                    context.SaveChanges();
                }

                this.DialogResult = true;
                this.Close();
            }
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

