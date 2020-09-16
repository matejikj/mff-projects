using Harmonogram.Helper;
using Harmonogram.ViewModels;
using Harmonogram.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Harmonogram.UserControls
{
    /// <summary>
    /// Interaction logic for AluminiumUserControl.xaml
    /// </summary>
    public partial class AluminiumUserControl : UserControl
    {
        AluminiumOrderViewModel orderVM;
        private bool isShopShowed;
        List<string> filterList;

        HarmonogramDBEntities db;

        public AluminiumUserControl()
        {
            InitializeComponent();
        }

        private static readonly Dispatcher UIDispatcher = Dispatcher.CurrentDispatcher;
        private void ShowWaitingCursor()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            UIDispatcher.BeginInvoke((System.Action)(() => { Mouse.OverrideCursor = null; }),
                         DispatcherPriority.ContextIdle);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowWaitingCursor();

            orderVM = new AluminiumOrderViewModel();
            DataContext = orderVM;
            AluminiumDatagrid.ItemsSource = orderVM.Orders;
            NewAluminiumDatagrid.ItemsSource = orderVM.NewOrders;
            db = new HarmonogramDBEntities();
            //hide shop part
            isShopShowed = false;
            NewDatagridRow.Height = new GridLength(0, GridUnitType.Star);
            NewTitleRow.Height = new GridLength(0, GridUnitType.Pixel);
            NewToolbarRow.Height = new GridLength(0, GridUnitType.Pixel);

            listBoxColumns.ItemsSource = columns;

            ZakazkaNr.IsReadOnly = !StaticResources.User.PrepisCisloZakazky.Value;

            string cesta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "harmonogramlignis");
            string soubor = Path.Combine(cesta, "aluminiumWidths.json");
            if (File.Exists(soubor))
            {
                List<DataGridLength> list = JsonConvert.DeserializeObject<List<DataGridLength>>(File.ReadAllText(soubor));

                if (list.Count == AluminiumDatagrid.Columns.Count)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        DataGridLength j = list[i];
                        AluminiumDatagrid.Columns[i].Width = j;
                    }
                }
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string name = b.Name;
            filterList = new List<string>();
            switch (name)
            {
                case "btnTechnikFilter":
                    listBoxTechnik.ItemsSource = orderVM.Orders.Select(p => p.Technik).Distinct().OrderBy(q => q);
                    TechnikFilter.IsOpen = true;
                    break;
                case "btnZakazkaFilter":
                    listBoxZakazka.ItemsSource = orderVM.Orders.Select(p => p.Zakazka).Distinct().OrderBy(q => q);
                    ZakazkaFilter.IsOpen = true;
                    break;
                case "btnZakazkaNrFilter":
                    listBoxZakazkaNr.ItemsSource = orderVM.Orders.Select(p => p.ZakazkaNr).Distinct().OrderBy(q => q);
                    ZakazkaNrFilter.IsOpen = true;
                    break;
                case "btnMaterialFilter":
                    listBoxMaterial.ItemsSource = orderVM.Orders.Select(p => p.Material).Distinct().OrderBy(q => q);
                    MaterialFilter.IsOpen = true;
                    break;
                case "btnVyrobniNrFilter":
                    listBoxVyrobniNr.ItemsSource = orderVM.Orders.Select(p => p.VyrobniNr).Distinct().OrderBy(q => q);
                    VyrobniNrFilter.IsOpen = true;
                    break;
                case "btnMestoFilter":
                    listBoxMesto.ItemsSource = orderVM.Orders.Select(p => p.Mesto).Distinct().OrderBy(q => q);
                    MestoFilter.IsOpen = true;
                    break;
            }
        }

        private void btnFilter_checked_Click(object sender, RoutedEventArgs e)
        {
            var j = sender as CheckBox;
            string st = j.DataContext as string;
            filterList.Add(st);
            Console.WriteLine();
        }

        private void btnFilter_unchecked_Click(object sender, RoutedEventArgs e)
        {
            var j = sender as CheckBox;
            string st = j.DataContext as string;
            filterList.Remove(st);
            Console.WriteLine();
        }

        private void filtrujBtn_click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string name = b.Name;
            ShowWaitingCursor();
            switch (name)
            {
                case "TechnikFiltrujBtn":
                    AluminiumDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Technik) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 2));
                    break;
                case "ZakazkaFiltrujBtn":
                    AluminiumDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Zakazka) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 2));
                    break;
                case "ZakazkaNrFiltrujBtn":
                    AluminiumDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.ZakazkaNr) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 2));
                    break;
                case "MaterialFiltrujBtn":
                    AluminiumDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Material) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 2));
                    break;
                case "VyrobniNrFiltrujBtn":
                    AluminiumDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.VyrobniNr) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 2));
                    break;
                case "MestoFiltrujBtn":
                    AluminiumDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Mesto) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 2));
                    break;
            }

        }

        private void allBtn_click(object sender, RoutedEventArgs e)
        {
            ShowWaitingCursor();
            orderVM = new AluminiumOrderViewModel();
            DataContext = orderVM;
            AluminiumDatagrid.ItemsSource = orderVM.Orders;
            NewAluminiumDatagrid.ItemsSource = orderVM.NewOrders;
        }

        private void zakazkaColorBtn_changed(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {

                System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                    var updateOrder = db.Orders.Where(o => o.OrderId == orderVM.Current_order.OrderId).FirstOrDefault();
                    updateOrder.ZakazkaColor = scb.ToString();
                    orderVM.Current_order.ZakazkaColor = scb.ToString();
                    db.SaveChanges();
                }
            }
        }


        private void materialColorBtn_changed(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                    var updateOrder = db.Orders.Where(o => o.OrderId == orderVM.Current_order.OrderId).FirstOrDefault();
                    updateOrder.MaterialColor = scb.ToString();
                    orderVM.Current_order.MaterialColor = scb.ToString();
                    db.SaveChanges();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                ShowWaitingCursor();
                Order order = orderVM.Current_order;
                int sortnr = order.SortNr.Value;
                Order dbOrder = db.Orders.Find(order.OrderId);
                dbOrder.IsDeleted = true;
                dbOrder.IsInProccess = false;
                dbOrder.DeletedBy = StaticResources.UserName;
                dbOrder.DeletedDate = DateTime.Now;
                orderVM.Orders.Remove(order);
                orderVM.Orders.Where(x => x.SortNr > sortnr).ToList().ForEach(y => y.SortNr--);
                var rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - 1 WHERE Department = 2 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr > @i", new SqlParameter("@i", sortnr));
                rows = db.Database.ExecuteSqlCommand("DELETE FROM [Expedition] WHERE OrderId = @i", new SqlParameter("@i", order.OrderId));
                rows = db.Database.ExecuteSqlCommand("SELECT ExpeditionId = T.ExpeditionId, ExpeditedSortNr = T.ExpeditedSortNr INTO #Temp FROM [Expedition] AS T UNION ALL SELECT ExpeditionId = 0, ExpeditedSortNr = 0 ORDER BY ExpeditedSortNr, ExpeditionId;DECLARE @id int;DECLARE @i int = 1;SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;DELETE #Temp WHERE ExpeditionId = @id;WHILE (SELECT Count(*) FROM #Temp) > 0 BEGIN SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;UPDATE [Expedition] SET [ExpeditedSortNr] = @i WHERE [ExpeditionId] = @id;SET @i = @i + 1;DELETE #Temp WHERE ExpeditionId = @id;END");
                db.SaveChanges();
            }
        }

        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            PopPresunText.Text = "1";
            PopPresun.IsOpen = true;
        }

        private void PopPresun_Click(object sender, RoutedEventArgs e)
        {

            PopPresun.IsOpen = false;
            int number = 0;
            if (int.TryParse(PopPresunText.Text, out number))
            {
                var ordery = new List<Order>();
                foreach (var row in AluminiumDatagrid.SelectedItems)
                {
                    Order o = row as Order;
                    ordery.Add(o);
                }
                var indexy = ordery.OrderBy(x => x.SortNr).Select(x => x.SortNr.Value).ToList();
                if (indexy[0] + indexy.Count - 1 == indexy.Last())
                {
                    var array = ordery.OrderBy(x => x.SortNr).Select(x => x.OrderId).ToList();
                    if (array.Count > 0)
                    {
                        if (number + array.Count - 1 <= orderVM.Orders.Count && number > 0)
                        {
                            ShowWaitingCursor();
                            int pos = orderVM.Orders.FirstOrDefault(x => x.OrderId == array[0]).SortNr.Value;

                            ordery = new List<Order>();

                            for (int i = 0; i < array.Count; i++)
                            {
                                ordery.Add(orderVM.Orders.FirstOrDefault(x => x.OrderId == array[i]));
                            }
                            int delka = ordery.Count;

                            if (number < pos)
                            {
                                var count = orderVM.Orders.Where(x => x.SortNr >= number && x.SortNr < pos).OrderBy(p => p.SortNr);
                                List<int> nrArray = new List<int>();
                                foreach (var order in count)
                                {
                                    nrArray.Add(order.OrderId);
                                }
                                nrArray.Reverse();
                                int rows;

                                foreach (var element in nrArray)
                                {
                                    Order c = orderVM.Orders.FirstOrDefault(x => x.OrderId == element);
                                    int newpos = c.SortNr.Value;
                                    newpos += delka;
                                    c.SortNr = newpos;
                                    orderVM.Orders[newpos - 1] = c;
                                    rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] + @delka WHERE OrderId = @id", new SqlParameter("@delka", delka), new SqlParameter("@id", c.OrderId));
                                }
                                for (int i = 0; i < ordery.Count; i++)
                                {
                                    orderVM.Orders[number + i - 1] = ordery[i];
                                    ordery[i].SortNr = number + i;
                                    rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = @number + @i WHERE OrderId = @id", new SqlParameter("@number", number), new SqlParameter("@i", i), new SqlParameter("@id", ordery[i].OrderId));
                                }
                                db.SaveChanges();
                            }
                            else
                            {
                                if (pos < number)
                                {
                                    var count = orderVM.Orders.Where(x => x.SortNr > (pos + delka - 1) && x.SortNr < (number + delka)).OrderBy(p => p.SortNr);
                                    List<int> nrArray = new List<int>();
                                    foreach (var order in count)
                                    {
                                        nrArray.Add(order.OrderId);
                                    }
                                    int rows;

                                    foreach (var element in nrArray)
                                    {
                                        Order c = orderVM.Orders.FirstOrDefault(x => x.OrderId == element);
                                        int newpos = c.SortNr.Value;
                                        newpos -= delka;
                                        c.SortNr = newpos;
                                        orderVM.Orders[newpos - 1] = c;
                                        rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - @delka WHERE OrderId = @id", new SqlParameter("@delka", delka), new SqlParameter("@id", c.OrderId));
                                    }
                                    for (int i = 0; i < ordery.Count; i++)
                                    {
                                        orderVM.Orders[number + i - 1] = ordery[i];
                                        ordery[i].SortNr = number + i;
                                        rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = @number + @i WHERE OrderId = @id", new SqlParameter("@number", number), new SqlParameter("@i", i), new SqlParameter("@id", ordery[i].OrderId));
                                    }
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Zadano cislo vetsi nez pocet objednavek");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Neni vybrana objednavka");
                    }
                }
                else
                {
                    MessageBox.Show("Vybirej radky jdouci po sobe");
                }
            }
        }

        private void btnNewRow_Click(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                int id = orderVM.Current_order.OrderId;
                if (orderVM.Current_order.SortNr > 0)
                {
                    Order o = orderVM.Orders.FirstOrDefault(x => x.OrderId == id);
                    if (new NewBlankRow(2).ShowDialog() == true)
                    {
                        ShowWaitingCursor();
                        Order newOrder = OrderCreator.createOrder(2);
                        StaticResources.blankRowColor = "NULL";
                        StaticResources.blankRowName = "";
                        newOrder.OrderId = db.Orders.Max(x => x.OrderId) + 1;
                        newOrder.SortNr = o.SortNr;
                        newOrder.IsInProccess = true;
                        db.Orders.Add(newOrder);
                        orderVM.Orders.Insert(o.SortNr.Value - 1, newOrder);
                        orderVM.Orders.Where(x => !x.IsDeleted && x.Department == 2 && x.IsInProccess && x.SortNr.Value > o.SortNr.Value).ToList().ForEach(a => a.SortNr++);
                        int rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] + 1 WHERE Department = 2 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr >= @i", new SqlParameter("@i", o.SortNr));
                        o.SortNr++;
                        db.SaveChanges();
                    }
                }
            }
        }

        private void btnArchive_Click(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                Order archivedOrder = orderVM.Current_order;
                ShowWaitingCursor();
                int sortnr = archivedOrder.SortNr.Value;
                int rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - 1 WHERE Department = 2 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr > @i", new SqlParameter("@i", sortnr));
                orderVM.Orders.Where(x => x.SortNr > sortnr).ToList().ForEach(y => y.SortNr--);
                var dbArchivedOrder = db.Orders.Find(archivedOrder.OrderId);
                dbArchivedOrder.SortNr = null;
                dbArchivedOrder.ArchivedBy = StaticResources.UserName;
                dbArchivedOrder.ArchivedDate = DateTime.Now;
                dbArchivedOrder.IsInProccess = false;
                dbArchivedOrder.IsArchived = true;
                orderVM.Orders.Remove(archivedOrder);
                rows = db.Database.ExecuteSqlCommand("DELETE FROM [Expedition] WHERE OrderId = @i", new SqlParameter("@i", archivedOrder.OrderId));
                rows = db.Database.ExecuteSqlCommand("SELECT ExpeditionId = T.ExpeditionId, ExpeditedSortNr = T.ExpeditedSortNr INTO #Temp FROM [Expedition] AS T UNION ALL SELECT ExpeditionId = 0, ExpeditedSortNr = 0 ORDER BY ExpeditedSortNr, ExpeditionId;DECLARE @id int;DECLARE @i int = 1;SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;DELETE #Temp WHERE ExpeditionId = @id;WHILE (SELECT Count(*) FROM #Temp) > 0 BEGIN SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;UPDATE [Expedition] SET [ExpeditedSortNr] = @i WHERE [ExpeditionId] = @id;SET @i = @i + 1;DELETE #Temp WHERE ExpeditionId = @id;END");
                db.SaveChanges();

            }
        }

        private void btnExpedition_Click(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                ShowWaitingCursor();
                int id = orderVM.Current_order.OrderId;
                Order o = db.Orders.Find(id);
                Expedition expedition = new Expedition();
                var list = db.Expeditions.ToList();
                expedition.ExpeditedSortNr = list.Count + 1;
                expedition.OrderId = id;
                var kridlaList = list.Where(p => p.OrderId == id).ToList();
                int sumKridla = 0;
                foreach (var s in kridlaList)
                {
                    int a;
                    if (int.TryParse(s.KridlaKsExpedovanych, out a))
                    {
                        sumKridla = sumKridla + a;
                    }
                }
                var sumOrderKridla = int.Parse(o.KridlaKs);
                expedition.KridlaKsZbyvaExpedovat = (sumOrderKridla - sumKridla).ToString();
                var zarubneList = list.Where(p => p.OrderId == id).ToList();
                int sumZarubne = 0;
                foreach (var s in zarubneList)
                {
                    int a;
                    if (int.TryParse(s.ZarubneKsExpedovanych, out a))
                    {
                        sumZarubne = sumZarubne + a;
                    }
                }
                var sumOrderZarubne = int.Parse(o.ZarubneKs);
                expedition.ZarubneKsZbyvaExpedovat = (sumOrderZarubne - sumZarubne).ToString();
                db.Expeditions.Add(expedition);
                db.SaveChanges();
            }
        }

        private void HandleEditLinkClick(object sender, RoutedEventArgs e)
        {
            StaticResources.lastUrl = orderVM.Current_order.DocUrl;
            new EditUrlWindow().ShowDialog();
            var updateOrder = db.Orders.Where(o => !o.IsDeleted && o.OrderId == orderVM.Current_order.OrderId).Single();
            updateOrder.DocUrl = StaticResources.lastUrl;
            orderVM.Current_order.DocUrl = StaticResources.lastUrl;
            db.SaveChanges();
        }

        private void HandleLinkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(orderVM.Current_order.DocUrl));
                e.Handled = true;
            }
            catch
            {
                MessageBox.Show("Cesta k souboru neexistuje");
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ShowWaitingCursor();
            orderVM = new AluminiumOrderViewModel();
            DataContext = orderVM;
            AluminiumDatagrid.ItemsSource = orderVM.Orders;
            NewAluminiumDatagrid.ItemsSource = orderVM.NewOrders;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            new NewOrderWindow(2).ShowDialog();
            if (StaticResources.lastAddedOrder != null)
            {
                orderVM.NewOrders.Add(StaticResources.lastAddedOrder);
                StaticResources.lastAddedOrder = null;
            }
        }

        public void pozadDatum_MouseEnter(object sender, RoutedEventArgs e)
        {

            if ((sender as DatePicker).ToolTip != null)
            {
                var toolTip = (sender as DatePicker).ToolTip as TextBlock;

                var baseobj = sender as FrameworkElement;
                var myObject = baseobj.DataContext as Order;

                DateTime zahaj = myObject.ZahajeniDatum.HasValue ? myObject.ZahajeniDatum.Value : DateTime.Now;
                DateTime spravne = DateHelper.add30WorkDays(zahaj);
                var rozdil = myObject.DokonceniDatum.Value.Subtract(spravne).Days;
                if (rozdil == 0)
                {
                    toolTip.Text = "Na den přesně 6 týdnů od zahájení";
                }
                else
                {
                    if (rozdil > 0)
                    {
                        toolTip.Text = Math.Abs(rozdil).ToString() + " dní více než 6 týdnů od zahájení";
                    }
                    else
                    {
                        toolTip.Text = Math.Abs(rozdil).ToString() + " dní méně než 6 týdnů od zahájení";
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            string cesta = "";
            try
            {
                cesta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "harmonogramlignis");
                if (!Directory.Exists(cesta))
                    Directory.CreateDirectory(cesta);
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku {0}, zkontrolujte prosím svá oprávnění.", cesta);
            }
            string fileName = System.IO.Path.Combine(cesta, "print_previw.xps");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            XpsDocument doc = new XpsDocument(fileName, FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            SerializerWriterCollator preview_Document = writer.CreateVisualsCollator();
            preview_Document.BeginBatchWrite();
            preview_Document.Write(AluminiumDatagrid);
            preview_Document.EndBatchWrite();
            FixedDocumentSequence preview = doc.GetFixedDocumentSequence();
            var window = new Window();
            window.Content = new DocumentViewer { Document = preview };
            window.ShowDialog();
            doc.Close();
        }

        private void btnDeleteNew_Click(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_new_order != null)
            {
                ShowWaitingCursor();
                Order o = orderVM.Current_new_order;
                orderVM.NewOrders.Remove(o);
                Order order = db.Orders.Where(p => p.OrderId == o.OrderId).FirstOrDefault();
                db.Orders.Remove(order);
                db.SaveChanges();
            }
        }

        private void btnProccess_Click(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_new_order != null)
            {
                ShowWaitingCursor();
                Order o = orderVM.Current_new_order;
                orderVM.NewOrders.Remove(o);
                o.SortNr = orderVM.Orders.Count + 1;
                o.IsInProccess = true;
                orderVM.Orders.Add(o);
                Order order = db.Orders.Where(p => p.OrderId == o.OrderId).FirstOrDefault();
                order.SortNr = o.SortNr;
                order.IsInProccess = true;
                db.SaveChanges();
            }
            else
            {
                MessageBox.Show("Neni vybrana zadna zakazka k predani, prosim oznacte ji a pak predejte");
            }
        }

        private void HandleAddressLinkClick(object sender, RoutedEventArgs e)
        {
            Order o = null;
            if (orderVM.Current_order != null)
                o = orderVM.Current_order;
            else
                if (orderVM.Current_order != null)
                o = orderVM.Current_new_order;
            String ulice = "";
            String mesto = "";
            String cislo = "";

            if (o.Ulice != null)
                ulice = o.Ulice;
            if (o.Mesto != null)
                mesto = o.Mesto;
            if (o.Cislo != null)
                cislo = o.Cislo;
            String navigateUri = "https://www.google.cz/maps/search/" + ulice + " " + cislo + ", " + mesto;
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }

        private void btnShowHideShop_Click(object sender, RoutedEventArgs e)
        {
            isShopShowed = !isShopShowed;
            if (isShopShowed)
            {
                NewDatagridRow.Height = new GridLength(12, GridUnitType.Star);
                NewTitleRow.Height = new GridLength(24, GridUnitType.Pixel);
                NewToolbarRow.Height = new GridLength(24, GridUnitType.Pixel);
                btnShowHideShop.Content = "Skrýt obchod";
            }
            else
            {
                NewDatagridRow.Height = new GridLength(0, GridUnitType.Star);
                NewTitleRow.Height = new GridLength(0, GridUnitType.Pixel);
                NewToolbarRow.Height = new GridLength(0, GridUnitType.Pixel);
                btnShowHideShop.Content = "Zobrazit obchod";
            }
        }

        private void btnBackToShop_Click(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                ShowWaitingCursor();
                Order order = orderVM.Current_order;
                int sortnr = order.SortNr.Value;
                orderVM.Orders.Where(x => x.SortNr > sortnr).ToList().ForEach(y => y.SortNr--);
                var rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - 1 WHERE Department = 2 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr > @i", new SqlParameter("@i", sortnr));
                var dbOrder = db.Orders.Find(order.OrderId);
                dbOrder.SortNr = null;
                dbOrder.IsInProccess = false;
                orderVM.Orders.Remove(order);
                orderVM.NewOrders.Add(order);
                db.SaveChanges();

            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                DatePicker dp = sender as DatePicker;
                DateTime expdat = dp.SelectedDate.HasValue ? dp.SelectedDate.Value : DateTime.Now;
                if (orderVM.Current_order != null)
                {
                    int i = orderVM.Current_order.OrderId;
                    var update = db.Orders.Where(o => o.OrderId == i).FirstOrDefault();
                    update.ZahajeniDatum = expdat;
                    db.SaveChanges();
                }
            }
        }

        private void DatePicker_SelectedDateChanged1(object sender, SelectionChangedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                DatePicker dp = sender as DatePicker;
                DateTime expdat = dp.SelectedDate.HasValue ? dp.SelectedDate.Value : DateTime.Now;
                if (orderVM.Current_order != null)
                {
                    int i = orderVM.Current_order.OrderId;
                    var update = db.Orders.Where(o => o.OrderId == i).FirstOrDefault();
                    update.PozadovanyDatum = expdat;
                    db.SaveChanges();
                }

            }
        }

        private void DatePicker_SelectedDateChanged2(object sender, SelectionChangedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                var update = orderVM.Orders.Where(x => x.OrderId == orderVM.Current_order.OrderId).FirstOrDefault();

                if (update != null)
                {
                    DatePicker dp = sender as DatePicker;
                    DateTime expdat = dp.SelectedDate.HasValue ? dp.SelectedDate.Value : DateTime.Now;


                    update.DokonceniDatum = expdat;

                    DateTime zahaj = update.ZahajeniDatum.HasValue ? update.ZahajeniDatum.Value : DateTime.Now;
                    DateTime spravne = DateHelper.add30WorkDays(zahaj);

                    var rozdil = update.DokonceniDatum.Value.Subtract(spravne).Days;
                    if (rozdil > 0)
                    {
                        update.DokonceniColor = "Red";
                    }
                    else
                    {
                        update.DokonceniColor = null;
                    }
                    db.SaveChanges();
                }

            }
        }

        private void DatePicker_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                    var updateOrder = db.Orders.Where(o => o.OrderId == orderVM.Current_order.OrderId).FirstOrDefault();
                    updateOrder.DokonceniColor = scb.ToString();
                    orderVM.Current_order.DokonceniColor = scb.ToString();
                    db.SaveChanges();
                }
            }
        }

        private void DatePickerPoz_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));
                    var updateOrder = db.Orders.Where(o => o.OrderId == orderVM.Current_order.OrderId).FirstOrDefault();
                    updateOrder.DokonceniColor = scb.ToString();
                    orderVM.Current_order.PozadovanyColor = scb.ToString();
                    db.SaveChanges();
                }
            }
        }


        private void DatePicker_SelectedDateChangedNew(object sender, SelectionChangedEventArgs e)
        {
            if (orderVM.Current_new_order != null)
            {
                DatePicker dp = sender as DatePicker;
                DateTime expdat = dp.SelectedDate.HasValue ? dp.SelectedDate.Value : DateTime.Now;
                if (orderVM.Current_order != null)
                {
                    int i = orderVM.Current_order.OrderId;
                    var update = db.Orders.Where(o => o.OrderId == i).FirstOrDefault();
                    update.ZahajeniDatum = expdat;
                    db.SaveChanges();
                }
            }
        }

        private void CheckBoxHeader_Click(object sender, RoutedEventArgs e)
        {
            var columnHeader = e.OriginalSource as DataGridColumnHeader;
            string text = columnHeader.Content as string;
            switch (text)
            {
                case "Priprava":
                    if (DokumentPripravaHli.Visibility == Visibility.Visible)
                    {
                        DokumentPripravaHli.Visibility = Visibility.Hidden;
                        IndocaPripravaHli.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        IndocaPripravaHli.Visibility = Visibility.Visible;
                        DokumentPripravaHli.Visibility = Visibility.Visible;
                    }
                    break;

                case "Narez":
                    if (ProfilNarezHli.Visibility == Visibility.Visible)
                    {
                        ProfilNarezHli.Visibility = Visibility.Hidden;
                        ListyNarezHli.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        ProfilNarezHli.Visibility = Visibility.Visible;
                        ListyNarezHli.Visibility = Visibility.Visible;
                    }
                    break;

                case "CNC":
                    if (SplnenoCncHli.Visibility == Visibility.Visible)
                    {
                        SplnenoCncHli.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoCncHli.Visibility = Visibility.Visible;
                    }
                    break;

                case "Freza":
                    if (SplnenoFrezaHli.Visibility == Visibility.Visible)
                    {
                        SplnenoFrezaHli.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoFrezaHli.Visibility = Visibility.Visible;
                    }
                    break;

                case "Priprava_":
                    if (SplnenoPriprava2Hli.Visibility == Visibility.Visible)
                    {
                        SplnenoPriprava2Hli.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoPriprava2Hli.Visibility = Visibility.Visible;
                    }
                    break;

                case "Strikarna":
                    if (SplnenoStrikarnaHli.Visibility == Visibility.Visible)
                    {
                        SplnenoStrikarnaHli.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        SplnenoStrikarnaHli.Visibility = Visibility.Visible;
                    }
                    break;
                case "Kompletace":
                    if (SplnenoKompletaceHli.Visibility == Visibility.Visible)
                    {
                        SplnenoKompletaceHli.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoKompletaceHli.Visibility = Visibility.Visible;
                    }
                    break;
                case "Sklad":
                    if (BarvaSkladHli.Visibility == Visibility.Visible)
                    {
                        BarvaSkladHli.Visibility = Visibility.Hidden;
                        KovaniSkladHli.Visibility = Visibility.Hidden;
                        ProfilSkladHli.Visibility = Visibility.Hidden;
                        SkloSkladHli.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        BarvaSkladHli.Visibility = Visibility.Visible;
                        KovaniSkladHli.Visibility = Visibility.Visible;
                        ProfilSkladHli.Visibility = Visibility.Visible;
                        SkloSkladHli.Visibility = Visibility.Visible;
                    }
                    break;
                case "Město":
                    if (Psc.Visibility == Visibility.Visible)
                    {
                        Psc.Visibility = Visibility.Hidden;
                        Cislo.Visibility = Visibility.Hidden;
                        Ulice.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        Psc.Visibility = Visibility.Visible;
                        Cislo.Visibility = Visibility.Visible;
                        Ulice.Visibility = Visibility.Visible;
                    }
                    break;
            }
        }

        private void PopupColorBtn_changed(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog dlg = new System.Windows.Forms.ColorDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(dlg.Color.A, dlg.Color.R, dlg.Color.G, dlg.Color.B));

                popupColorBtn.Background = scb;
                activeColor = scb.ToString();
            }

            Popupek.IsOpen = true;
        }

        string activePopupName;
        string activeColor;
        string activeName;

        private void Proces_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string name = b.Name;

            Popupek.PlacementTarget = b;
            Popupek.IsOpen = true;

            Order o = orderVM.Current_order;
            activePopupName = name;

            switch (activePopupName)
            {
                case "DokumentPripravaHli":
                    activeColor = o.DokumentPripravaHliC;
                    break;
                case "IndocaPripravaHli":
                    activeColor = o.IndocaPripravaHliC;
                    break;
                case "ProfilNarezHli":
                    activeColor = o.ProfilNarezHliC;
                    break;
                case "ListyNarezHli":
                    activeColor = o.ListyNarezHliC;
                    break;
                case "SplnenoCncHli":
                    activeColor = o.SplnenoCncHliC;
                    break;
                case "SplnenoFrezaHli":
                    activeColor = o.SplnenoFrezaHliC;
                    break;
                case "SplnenoPriprava2Hli":
                    activeColor = o.SplnenoPriprava2HliC;
                    break;
                case "SplnenoStrikarnaHli":
                    activeColor = o.SplnenoStrikarnaHliC;
                    break;
                case "SplnenoKompletaceHli":
                    activeColor = o.SplnenoKompletaceHliC;
                    break;
                case "ProfilSkladHli":
                    activeColor = o.ProfilSkladHliC;
                    break;
                case "BarvaSkladHli":
                    activeColor = o.BarvaSkladHliC;
                    break;
                case "KovaniSkladHli":
                    activeColor = o.KovaniSkladHliC;
                    break;
                case "SkloSkladHli":
                    activeColor = o.SkloSkladHliC;
                    break;
            }

            switch (activePopupName)
            {
                case "DokumentPripravaHli":
                    activeName = o.DokumentPripravaHli;
                    break;
                case "IndocaPripravaHli":
                    activeName = o.IndocaPripravaHli;
                    break;
                case "ProfilNarezHli":
                    activeName = o.ProfilNarezHli;
                    break;
                case "ListyNarezHli":
                    activeName = o.ListyNarezHli;
                    break;
                case "SplnenoCncHli":
                    activeName = o.SplnenoCncHli;
                    break;
                case "SplnenoFrezaHli":
                    activeName = o.SplnenoFrezaHli;
                    break;
                case "SplnenoPriprava2Hli":
                    activeName = o.SplnenoPriprava2Hli;
                    break;
                case "SplnenoStrikarnaHli":
                    activeName = o.SplnenoStrikarnaHli;
                    break;
                case "SplnenoKompletaceHli":
                    activeName = o.SplnenoKompletaceHli;
                    break;
                case "ProfilSkladHli":
                    activeName = o.ProfilSkladHli;
                    break;
                case "BarvaSkladHli":
                    activeName = o.BarvaSkladHli;
                    break;
                case "KovaniSkladHli":
                    activeName = o.KovaniSkladHli;
                    break;
                case "SkloSkladHli":
                    activeName = o.SkloSkladHli;
                    break;

            }

            if (activeColor != null)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString(activeColor);

                popupColorBtn.Background = brush;
            }

            popupText2.Text = activeName;
        }


        private void choiceRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            popupColorBtn.Visibility = Visibility.Visible;
            popupText1.Visibility = Visibility.Visible;
            popupText2.Visibility = Visibility.Visible;
            popupText3.Visibility = Visibility.Visible;
        }

        private void choiceRadioBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            popupColorBtn.Visibility = Visibility.Collapsed;
            popupText1.Visibility = Visibility.Collapsed;
            popupText2.Visibility = Visibility.Collapsed;
            popupText3.Visibility = Visibility.Collapsed;
        }



        private void PopupSave_Click(object sender, RoutedEventArgs e)
        {

            Order o = orderVM.Orders.Where(p => p.OrderId == orderVM.Current_order.OrderId).FirstOrDefault();

            string value = "";

            if (popupChck1.IsChecked.Value)
            {
                value = "✓";
                activeColor = "LimeGreen";
            }
            else
            {
                if (popupChck2.IsChecked.Value)
                {
                    value = "✗";
                    activeColor = "Yellow";
                }
                else
                {
                    if (popupChck3.IsChecked.Value)
                    {
                        value = popupText2.Text;
                        activeColor = popupColorBtn.Background.ToString();
                    }
                    else
                    {
                        if (popupChck4.IsChecked.Value)
                        {
                            value = "";
                            activeColor = "Yellow";
                        }
                    }
                }
            }


            switch (activePopupName)
            {
                case "DokumentPripravaHli":
                    o.DokumentPripravaHli = value;
                    break;
                case "IndocaPripravaHli":
                    o.IndocaPripravaHli = value;
                    break;
                case "ProfilNarezHli":
                    o.ProfilNarezHli = value;
                    break;
                case "ListyNarezHli":
                    o.ListyNarezHli = value;
                    break;
                case "SplnenoCncHli":
                    o.SplnenoCncHli = value;
                    break;
                case "SplnenoFrezaHli":
                    o.SplnenoFrezaHli = value;
                    break;
                case "SplnenoPriprava2Hli":
                    o.SplnenoPriprava2Hli = value;
                    break;
                case "SplnenoStrikarnaHli":
                    o.SplnenoStrikarnaHli = value;
                    break;
                case "SplnenoKompletaceHli":
                    o.SplnenoKompletaceHli = value;
                    break;
                case "ProfilSkladHli":
                    o.ProfilSkladHli = value;
                    break;
                case "BarvaSkladHli":
                    o.BarvaSkladHli = value;
                    break;
                case "KovaniSkladHli":
                    o.KovaniSkladHli = value;
                    break;
                case "SkloSkladHli":
                    o.SkloSkladHli = value;
                    break;
            }

            if (activeColor != null)
            {
                switch (activePopupName)
                {
                    case "DokumentPripravaHli":
                        o.DokumentPripravaHliC = activeColor;
                        break;
                    case "IndocaPripravaHli":
                        o.IndocaPripravaHliC = activeColor;
                        break;
                    case "ProfilNarezHli":
                        o.ProfilNarezHliC = activeColor;
                        break;
                    case "ListyNarezHli":
                        o.ListyNarezHliC = activeColor;
                        break;
                    case "SplnenoCncHli":
                        o.SplnenoCncHliC = activeColor;
                        break;
                    case "SplnenoFrezaHli":
                        o.SplnenoFrezaHliC = activeColor;
                        break;
                    case "SplnenoPriprava2Hli":
                        o.SplnenoPriprava2HliC = activeColor;
                        break;
                    case "SplnenoStrikarnaHli":
                        o.SplnenoStrikarnaHliC = activeColor;
                        break;
                    case "SplnenoKompletaceHli":
                        o.SplnenoKompletaceHliC = activeColor;
                        break;
                    case "ProfilSkladHli":
                        o.ProfilSkladHliC = activeColor;
                        break;
                    case "BarvaSkladHli":
                        o.BarvaSkladHliC = activeColor;
                        break;
                    case "KovaniSkladHli":
                        o.KovaniSkladHliC = activeColor;
                        break;
                    case "SkloSkladHli":
                        o.SkloSkladHliC = activeColor;
                        break;
                }
            }

            o.PripravaHliC = "Yellow";
            o.NarezHliC = "Yellow";
            o.CncHliC = "Yellow";
            o.FrezaHliC = "Yellow";
            o.Priprava2HliC = "Yellow";
            o.StrikarnaHliC = "Yellow";
            o.KompletaceHliC = "Yellow";
            o.SkladHliC = "Yellow";


            if ((o.IndocaPripravaHli == "✓" || o.IndocaPripravaHli == "✗" ) && (o.DokumentPripravaHli == "✓" || o.DokumentPripravaHli == "✗" ))
            {
                o.PripravaHliC = "LimeGreen";
            }
            if ((o.ProfilNarezHli == "✓" || o.ProfilNarezHli == "✗" ) && (o.ListyNarezHli == "✓" || o.ListyNarezHli == "✗") )
            {
                o.NarezHliC = "LimeGreen";
            }
            if ((o.SplnenoCncHli == "✓" || o.SplnenoCncHli == "✗" ))
            {
                o.CncHliC = "LimeGreen";
            }
            if ((o.SplnenoFrezaHli == "✓" || o.SplnenoFrezaHli == "✗" ))
            {
                o.FrezaHliC = "LimeGreen";
            }
            if ((o.SplnenoPriprava2Hli == "✓" || o.SplnenoPriprava2Hli == "✗" ))
            {
                o.Priprava2HliC = "LimeGreen";
            }
            if ((o.SplnenoStrikarnaHli == "✓" || o.SplnenoStrikarnaHli == "✗" ))
            {
                o.StrikarnaHliC = "LimeGreen";
            }
            if ((o.SplnenoKompletaceHli == "✓" || o.SplnenoKompletaceHli == "✗" ))
            {
                o.KompletaceHliC = "LimeGreen";
            }
            if ( (o.ProfilSkladHli == "✓" || o.ProfilSkladHli == "✗" ) && (o.BarvaSkladHli == "✓" || o.BarvaSkladHli == "✗") &&
                (o.KovaniSkladHli == "✓" || o.KovaniSkladHli == "✗" ) && (o.SkloSkladHli == "✓" || o.SkloSkladHli == "✗"))
            {
                o.SkladHliC = "LimeGreen";
            }
            db.SaveChanges();
            Popupek.IsOpen = false;
        }

        List<string> columns = new List<string> { "Zahajeni Datum", "Technik", "Zakazka",
            "Poznamky", "Material", "Cislo Zakazky", "Vyrobni Cislo","Soubor", "Upravit soubor", "Kridla",
            "Zarubne", "Priprava", "Narez", "CNC", "Freza", "Priprava_", "Strikarna", "Kompletace",
            "Sklad", "Pozadovany Datum", "Datum Dokonceni", "Mesto", "Mapa", "Plosny material", "Hotovo bloky"};

        private void btnColumnsFilter_Click(object sender, RoutedEventArgs e)
        {
            ColumnsFilter.IsOpen = true;
        }

        private void ColumnsChecked(object sender, RoutedEventArgs e)
        {
            var j = sender as CheckBox;
            string st = j.DataContext as string;
            switch (st)
            {
                case "Zahajeni Datum":
                    ZahajeniDatum.Visibility = ZahajeniDatum.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Technik":
                    Technik.Visibility = Technik.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Zakazka":
                    Zakazka.Visibility = Zakazka.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Poznamky":
                    Poznamky.Visibility = Poznamky.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;


                    break;
                case "Material":
                    material.Visibility = material.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Cislo Zakazky":
                    ZakazkaNr.Visibility = ZakazkaNr.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Vyrobni Cislo":
                    VyrobniNr.Visibility = VyrobniNr.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Soubor":
                    Soubor.Visibility = Soubor.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Upravit soubor":
                    editUrl.Visibility = editUrl.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Kridla":
                    KridlaText.Visibility = ZahajeniDatum.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Zarubne":
                    ZarubneText.Visibility = ZarubneText.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Priprava":
                    PripravaHli.Visibility = PripravaHli.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Narez":
                    NarezHli.Visibility = NarezHli.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "CNC":
                    CncHli.Visibility = CncHli.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Freza":
                    FrezaHli.Visibility = FrezaHli.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Priprava_":
                    Priprava2Hli.Visibility = Priprava2Hli.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Strikarna":
                    StrikarnaHli.Visibility = StrikarnaHli.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Kompletace":
                    KompletaceHli.Visibility = KompletaceHli.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Sklad":
                    SkladHli.Visibility = SkladHli.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Pozadovany Datum":
                    PozadovanyDatum.Visibility = PozadovanyDatum.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Datum Dokonceni":
                    DokonceniDatum.Visibility = DokonceniDatum.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Mesto":
                    Mesto.Visibility = Mesto.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Mapa":
                    Mapa.Visibility = Mapa.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Plosny material":
                    PlosnyMaterial.Visibility = PlosnyMaterial.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Hotovo bloky":
                    HotovoBloky.Visibility = HotovoBloky.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
            }
        }

        private void ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            ColumnsFilter.IsOpen = false;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

            try
            {
                string cesta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "harmonogramlignis");
                if (Directory.Exists(cesta))
                {
                    List<DataGridLength> lista = new List<DataGridLength>();

                    AluminiumDatagrid.Columns.ToList().ForEach(x => lista.Add(x.ActualWidth));

                    if (lista.Count != 0)
                    {
                        string fileName = System.IO.Path.Combine(cesta, "aluminiumWidths.json");

                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        var jsonString = JsonConvert.SerializeObject(lista);
                        File.WriteAllText(fileName, jsonString);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku {0}, zkontrolujte prosím svá oprávnění.");
            }
        }
    }
}
