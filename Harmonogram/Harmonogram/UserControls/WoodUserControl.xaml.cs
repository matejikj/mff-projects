using Harmonogram.Helper;
using Harmonogram.ViewModels;
using Harmonogram.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    /// Interaction logic for WoodUserControl.xaml
    /// </summary>
    public partial class WoodUserControl : UserControl
    {
        WoodOrderViewModel orderVM;
        private bool isShopShowed;
        List<string> filterList;
        List<string> printList;
        HarmonogramDBEntities db;

        public WoodUserControl()
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
            orderVM = new WoodOrderViewModel();
            DataContext = orderVM;
            WoodDatagrid.ItemsSource = orderVM.Orders;
            NewWoodDatagrid.ItemsSource = orderVM.NewOrders;
            db = new HarmonogramDBEntities();
            //hide shop part
            isShopShowed = false;
            NewDatagridRow.Height = new GridLength(0, GridUnitType.Star);
            NewTitleRow.Height = new GridLength(0, GridUnitType.Pixel);
            NewToolbarRow.Height = new GridLength(0, GridUnitType.Pixel);

            listBoxColumns.ItemsSource = columns;

            Cislo_zakazky.IsReadOnly = !StaticResources.User.PrepisCisloZakazky.Value;

            string cesta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "harmonogramlignis");
            string soubor = Path.Combine(cesta, "woodWidths.json");
            if (File.Exists(soubor))
            {
                List<DataGridLength> list = JsonConvert.DeserializeObject<List<DataGridLength>>(File.ReadAllText(soubor));

                if (list.Count == WoodDatagrid.Columns.Count)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        DataGridLength j = list[i];
                        WoodDatagrid.Columns[i].Width = j;
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

        private void allBtn_click(object sender, RoutedEventArgs e)
        {
            ShowWaitingCursor();
            orderVM = new WoodOrderViewModel();
            DataContext = orderVM;
            WoodDatagrid.ItemsSource = orderVM.Orders;
            NewWoodDatagrid.ItemsSource = orderVM.NewOrders;
        }

        private void btnPrint_checked_Click(object sender, RoutedEventArgs e)
        {
            var j = sender as CheckBox;
            string st = j.DataContext as string;
            printList.Add(st);
            Console.WriteLine();
            foreach (var i in printList)
            {
                Console.WriteLine(i);
            }
        }

        private void btnPrint_unchecked_Click(object sender, RoutedEventArgs e)
        {
            var j = sender as CheckBox;
            string st = j.DataContext as string;
            printList.Remove(st);
            Console.WriteLine();
        }

        private void btnFilter_checked_Click(object sender, RoutedEventArgs e)
        {
            var j = sender as CheckBox;
            string st = j.DataContext as string;
            filterList.Add(st);
        }

        private void btnFilter_unchecked_Click(object sender, RoutedEventArgs e)
        {
            var j = sender as CheckBox;
            string st = j.DataContext as string;
            filterList.Remove(st);
        }

        private void filtrujBtn_click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string name = b.Name;
            ShowWaitingCursor();
            switch (name)
            {
                case "TechnikFiltrujBtn":
                    WoodDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Technik) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 1));
                    break;
                case "ZakazkaFiltrujBtn":
                    WoodDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Zakazka) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 1));
                    break;
                case "ZakazkaNrFiltrujBtn":
                    WoodDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.ZakazkaNr) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 1));
                    break;
                case "MaterialFiltrujBtn":
                    WoodDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Material) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 1));
                    break;
                case "VyrobniNrFiltrujBtn":
                    WoodDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.VyrobniNr) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 1));
                    break;
                case "MestoFiltrujBtn":
                    WoodDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Mesto) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 1));
                    break;
            }
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
                var order = orderVM.Current_order;
                int sortnr = order.SortNr.Value;
                Order dbOrder = db.Orders.Find(order.OrderId);
                dbOrder.IsDeleted = true;
                dbOrder.IsInProccess = false;
                dbOrder.DeletedBy = StaticResources.UserName;
                dbOrder.DeletedDate = DateTime.Now;
                orderVM.Orders.Remove(order);
                orderVM.Orders.Where(x => x.SortNr > sortnr).ToList().ForEach(y => y.SortNr--);
                var rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - 1 WHERE Department = 1 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr > @i", new SqlParameter("@i", sortnr));
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
                foreach (var row in WoodDatagrid.SelectedItems)
                {
                    Order o = row as Order;
                    ordery.Add(o);
                }
                var indexy = ordery.OrderBy(x => x.SortNr).Select(x => x.SortNr.Value).ToList();
                if ( indexy[0] + indexy.Count - 1 == indexy.Last() )
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
                                var vmOrdersArray = orderVM.Orders.Where(x => x.SortNr >= number && x.SortNr < pos).OrderBy(p => p.SortNr);

                                int rows;

                                foreach (var element in vmOrdersArray)
                                {
                                    int newpos = element.SortNr.Value;
                                    newpos += delka;
                                    element.SortNr = newpos;
                                }
                                for (int i = 0; i < ordery.Count; i++)
                                {
                                    orderVM.Orders.Remove(ordery[i]);
                                    orderVM.Orders.Insert(number + i - 1, ordery[i]);
                                    ordery[i].SortNr = number + i;
                                }
                                db.SaveChanges();
                                //var count = orderVM.Orders.Where(x => x.SortNr >= number && x.SortNr < pos).OrderBy(p => p.SortNr);
                                //List<int> nrArray = new List<int>();
                                //foreach (var order in count)
                                //{
                                //    nrArray.Add(order.OrderId);
                                //}
                                //nrArray.Reverse();
                                //int rows;

                                //foreach (var element in nrArray)
                                //{
                                //    Order c = orderVM.Orders.FirstOrDefault(x => x.OrderId == element);
                                //    int newpos = c.SortNr.Value;
                                //    newpos += delka;
                                //    c.SortNr = newpos;
                                //    orderVM.Orders[newpos - 1] = c;
                                //    rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = @cislo WHERE OrderId = @id", new SqlParameter("@cislo", c.SortNr), new SqlParameter("@id", c.OrderId));
                                //}
                                //for (int i = 0; i < ordery.Count; i++)
                                //{
                                //    orderVM.Orders[number + i - 1] = ordery[i];
                                //    ordery[i].SortNr = number + i;
                                //    rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = @number + @i WHERE OrderId = @id", new SqlParameter("@number", number), new SqlParameter("@i", i), new SqlParameter("@id", ordery[i].OrderId));
                                //}
                                //db.SaveChanges();
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
                                        rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = @cislo WHERE OrderId = @id", new SqlParameter("@cislo", c.SortNr), new SqlParameter("@id", c.OrderId));
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

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            // int rows = db.Database.ExecuteSqlCommand("SELECT ExpeditionId = T.ExpeditionId, ExpeditedSortNr = T.ExpeditedSortNr INTO #Temp FROM [Expedition] AS T UNION ALL SELECT ExpeditionId = 0, ExpeditedSortNr = 0 ORDER BY ExpeditedSortNr, ExpeditionId;DECLARE @id int;DECLARE @i int = 1;SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;DELETE #Temp WHERE ExpeditionId = @id;WHILE (SELECT Count(*) FROM #Temp) > 0 BEGIN SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;UPDATE [Expedition] SET [ExpeditedSortNr] = @i WHERE [ExpeditionId] = @id;SET @i = @i + 1;DELETE #Temp WHERE ExpeditionId = @id;END");
            int i = 1;
            db.Orders.ToList().ForEach(x =>
            {
                DateTime zahaj = x.ZahajeniDatum.HasValue ? x.ZahajeniDatum.Value : DateTime.Now;
                DateTime spravne = DateHelper.add30WorkDays(zahaj);

                var rozdil = x.DokonceniDatum.Value.Subtract(spravne).Days;
                if (rozdil > 0)
                {
                    x.DokonceniColor = "Red";
                }
                else
                {
                    x.DokonceniColor = null;
                }
                db.SaveChanges();

            });
            db.SaveChanges();
        }

        private void btnResetExpeditions_Click(object sender, RoutedEventArgs e)
        {
            // int rows = db.Database.ExecuteSqlCommand("SELECT ExpeditionId = T.ExpeditionId, ExpeditedSortNr = T.ExpeditedSortNr INTO #Temp FROM [Expedition] AS T UNION ALL SELECT ExpeditionId = 0, ExpeditedSortNr = 0 ORDER BY ExpeditedSortNr, ExpeditionId;DECLARE @id int;DECLARE @i int = 1;SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;DELETE #Temp WHERE ExpeditionId = @id;WHILE (SELECT Count(*) FROM #Temp) > 0 BEGIN SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;UPDATE [Expedition] SET [ExpeditedSortNr] = @i WHERE [ExpeditionId] = @id;SET @i = @i + 1;DELETE #Temp WHERE ExpeditionId = @id;END");
            int i = 1;
            db.Orders.ToList().ForEach(x =>
            {
                if (x.ZarubneText != null)
                {
                    var parts = x.ZarubneText.Split('/');
                    if (parts.Length > 1)
                    {
                        x.ZarubneText = parts[0];
                    }
                }
                if (x.KridlaText != null)
                {
                    var parts = x.KridlaText.Split('/');
                    if (parts.Length > 1)
                    {
                        x.KridlaText = parts[0];
                    }
                }
            });
            db.SaveChanges();
        }

        private void btnNewRow_Click(object sender, RoutedEventArgs e)
        {
            if (orderVM.Current_order != null)
            {
                int id = orderVM.Current_order.OrderId;
                if (orderVM.Current_order.SortNr > 0)
                {
                    Order o = orderVM.Orders.FirstOrDefault(x => x.OrderId == id);
                    if (new NewBlankRow(1).ShowDialog() == true)
                    {
                        ShowWaitingCursor();
                        Order newOrder = OrderCreator.createOrder(1);
                        StaticResources.blankRowColor = "NULL";
                        StaticResources.blankRowName = "";
                        newOrder.OrderId = db.Orders.Max(x => x.OrderId) + 1;
                        newOrder.SortNr = o.SortNr;
                        newOrder.IsInProccess = true;
                        db.Orders.Add(newOrder);
                        orderVM.Orders.Insert(o.SortNr.Value - 1, newOrder);
                        orderVM.Orders.Where(x => !x.IsDeleted && x.Department == 1 && x.IsInProccess && x.SortNr.Value > o.SortNr.Value).ToList().ForEach(a => a.SortNr++);
                        int rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] + 1 WHERE Department = 1 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr >= @i", new SqlParameter("@i", o.SortNr));
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
                int rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - 1 WHERE Department = 1 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr > @i", new SqlParameter("@i", sortnr));
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
            orderVM = new WoodOrderViewModel();
            DataContext = orderVM;
            WoodDatagrid.ItemsSource = orderVM.Orders;
            NewWoodDatagrid.ItemsSource = orderVM.NewOrders;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            new NewOrderWindow(1).ShowDialog();
            if (StaticResources.lastAddedOrder != null)
            {
                var order = db.Orders.Find(StaticResources.lastAddedOrder.OrderId);
                orderVM.NewOrders.Add(order);
                StaticResources.lastAddedOrder = null;
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            string cesta = "";
            try
            {
                cesta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "harmonogramlignis");
                if (!Directory.Exists(cesta))
                {
                    Directory.CreateDirectory(cesta);
                }
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku {0}, zkontrolujte prosím svá oprávnění.", cesta);
            }
            string fileName = Path.Combine(cesta, "print_previw.xps");

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            XpsDocument doc = new XpsDocument(fileName, FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            SerializerWriterCollator preview_Document = writer.CreateVisualsCollator();
            preview_Document.BeginBatchWrite();
            preview_Document.Write(WoodDatagrid);
            preview_Document.EndBatchWrite();
            FixedDocumentSequence preview = doc.GetFixedDocumentSequence();
            var window = new Window();
            window.Content = new DocumentViewer { Document = preview };
            window.ShowDialog();
            doc.Close();
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

                try
                {
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
                } catch
                {

                }
            }
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
                var order = db.Orders.Find(o.OrderId);
                orderVM.Orders.Add(order);
                order.SortNr = orderVM.Orders.Count;
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
                var rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - 1 WHERE Department = 1 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr > @i", new SqlParameter("@i", sortnr));
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
                case "Příprava":
                    if (Dokumentace.Visibility == Visibility.Visible)
                    {
                        Dokumentace.Visibility = Visibility.Hidden;
                        InDoca.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        Dokumentace.Visibility = Visibility.Visible;
                        InDoca.Visibility = Visibility.Visible;
                    }
                    break;
                case "Obložky":
                    if (ObalovatOblozky.Visibility == Visibility.Visible)
                    {
                        ObalovatOblozky.Visibility = Visibility.Hidden;
                        RezatOblozky.Visibility = Visibility.Hidden;
                        CncOblozky.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        ObalovatOblozky.Visibility = Visibility.Visible;
                        RezatOblozky.Visibility = Visibility.Visible;
                        CncOblozky.Visibility = Visibility.Visible;
                    }
                    break;

                case "Středové":
                    if (CentrumStredove.Visibility == Visibility.Visible)
                    {
                        CentrumStredove.Visibility = Visibility.Hidden;
                        LisStredove.Visibility = Visibility.Hidden;
                        OlepovaniStredove.Visibility = Visibility.Hidden;
                        FrezovaniStredove.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        CentrumStredove.Visibility = Visibility.Visible;
                        LisStredove.Visibility = Visibility.Visible;
                        OlepovaniStredove.Visibility = Visibility.Visible;
                        FrezovaniStredove.Visibility = Visibility.Visible;
                    }
                    break;
                case "Rámeček":
                    if (TypRamecek.Visibility == Visibility.Visible)
                    {
                        TypRamecek.Visibility = Visibility.Hidden;
                        ObalovaniRamecek.Visibility = Visibility.Hidden;
                        RezaniRamecek.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        TypRamecek.Visibility = Visibility.Visible;
                        ObalovaniRamecek.Visibility = Visibility.Visible;
                        RezaniRamecek.Visibility = Visibility.Visible;
                    }
                    break;
                case "Klapačka":
                    if (ObalovaniKlapacka.Visibility == Visibility.Visible)
                    {
                        ObalovaniKlapacka.Visibility = Visibility.Hidden;
                        RezaniKlapacka.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        ObalovaniKlapacka.Visibility = Visibility.Visible;
                        RezaniKlapacka.Visibility = Visibility.Visible;
                    }
                    break;
                case "Posuv":
                    if (HranolPosuv.Visibility == Visibility.Visible)
                    {
                        HranolPosuv.Visibility = Visibility.Hidden;
                        GarnyzPosuv.Visibility = Visibility.Hidden;
                        DorazovaPosuv.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        HranolPosuv.Visibility = Visibility.Visible;
                        GarnyzPosuv.Visibility = Visibility.Visible;
                        DorazovaPosuv.Visibility = Visibility.Visible;
                    }
                    break;
                case "Dveře":
                    if (CentrumDvere.Visibility == Visibility.Visible)
                    {
                        CentrumDvere.Visibility = Visibility.Hidden;
                        KorpusyDvere.Visibility = Visibility.Hidden;
                        SesazenkyDvere.Visibility = Visibility.Hidden;
                        LisDvere.Visibility = Visibility.Hidden;
                        FormatkaDvere.Visibility = Visibility.Hidden;
                        OlepovackaDvere.Visibility = Visibility.Hidden;
                        CncDvere.Visibility = Visibility.Hidden;
                        PgmDvere.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        CentrumDvere.Visibility = Visibility.Visible;
                        KorpusyDvere.Visibility = Visibility.Visible;
                        SesazenkyDvere.Visibility = Visibility.Visible;
                        LisDvere.Visibility = Visibility.Visible;
                        FormatkaDvere.Visibility = Visibility.Visible;
                        OlepovackaDvere.Visibility = Visibility.Visible;
                        CncDvere.Visibility = Visibility.Visible;
                        PgmDvere.Visibility = Visibility.Visible;
                    }
                    break;
                case "Dokončení":
                    if (BrouseniDyhaDokonceni.Visibility == Visibility.Visible)
                    {
                        BrouseniDyhaDokonceni.Visibility = Visibility.Hidden;
                        ZakladDokonceni.Visibility = Visibility.Hidden;
                        BrouseniZakladDokonceni.Visibility = Visibility.Hidden;
                        VrchDokonceni.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        BrouseniDyhaDokonceni.Visibility = Visibility.Visible;
                        ZakladDokonceni.Visibility = Visibility.Visible;
                        BrouseniZakladDokonceni.Visibility = Visibility.Visible;
                        VrchDokonceni.Visibility = Visibility.Visible;
                    }
                    break;
                case "Kompletace":
                    if (DvereKompletace.Visibility == Visibility.Visible)
                    {
                        DvereKompletace.Visibility = Visibility.Hidden;
                        ZarubenKompletace.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        DvereKompletace.Visibility = Visibility.Visible;
                        ZarubenKompletace.Visibility = Visibility.Visible;
                    }
                    break;
                case "Sklad":
                    if (KovaniSklad.Visibility == Visibility.Visible)
                    {
                        KovaniSklad.Visibility = Visibility.Hidden;
                        SkloSklad.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        KovaniSklad.Visibility = Visibility.Visible;
                        SkloSklad.Visibility = Visibility.Visible;
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
                case "Dokumentace":
                    activeColor = o.DokumentaceC;
                    break;
                case "InDoca":
                    activeColor = o.InDocaC;
                    break;
                case "ObalovatOblozky":
                    activeColor = o.ObalovatOblozkyC;
                    break;
                case "RezatOblozky":
                    activeColor = o.RezatOblozkyC;
                    break;
                case "CncOblozky":
                    activeColor = o.CncOblozkyC;
                    break;
                case "CentrumStredove":
                    activeColor = o.CentrumStredoveC;
                    break;
                case "LisStredove":
                    activeColor = o.LisStredoveC;
                    break;
                case "OlepovaniStredove":
                    activeColor = o.OlepovaniStredoveC;
                    break;
                case "FrezovaniStredove":
                    activeColor = o.FrezovaniStredoveC;
                    break;
                case "TypRamecek":
                    activeColor = o.TypRamecekC;
                    break;
                case "ObalovaniRamecek":
                    activeColor = o.ObalovaniRamecekC;
                    break;
                case "RezaniRamecek":
                    activeColor = o.RezaniRamecekC;
                    break;
                case "ObalovaniKlapacka":
                    activeColor = o.ObalovaniKlapackaC;
                    break;
                case "RezaniKlapacka":
                    activeColor = o.RezaniKlapackaC;
                    break;
                case "HranolPosuv":
                    activeColor = o.HranolPosuvC;
                    break;
                case "GarnyzPosuv":
                    activeColor = o.GarnyzPosuvC;
                    break;
                case "DorazovaPosuv":
                    activeColor = o.DorazovaPosuvC;
                    break;
                case "CentrumDvere":
                    activeColor = o.CentrumDvereC;
                    break;
                case "KorpusyDvere":
                    activeColor = o.KorpusyDvereC;
                    break;
                case "SesazenkyDvere":
                    activeColor = o.SesazenkyDvereC;
                    break;
                case "LisDvere":
                    activeColor = o.LisDvereC;
                    break;
                case "FormatkaDvere":
                    activeColor = o.FormatkaDvereC;
                    break;
                case "OlepovackaDvere":
                    activeColor = o.OlepovackaDvereC;
                    break;
                case "CncDvere":
                    activeColor = o.CncDvereC;
                    break;
                case "PgmDvere":
                    activeColor = o.PgmDvereC;
                    break;
                case "BrouseniDyhaDokonceni":
                    activeColor = o.BrouseniDyhaDokonceniC;
                    break;
                case "ZakladDokonceni":
                    activeColor = o.ZakladDokonceniC;
                    break;
                case "BrouseniZakladDokonceni":
                    activeColor = o.BrouseniZakladDokonceniC;
                    break;
                case "VrchDokonceni":
                    activeColor = o.VrchDokonceniC;
                    break;
                case "DvereKompletace":
                    activeColor = o.DvereKompletaceC;
                    break;
                case "ZarubenKompletace":
                    activeColor = o.ZarubenKompletaceC;
                    break;
                case "KovaniSklad":
                    activeColor = o.KovaniSkladC;
                    break;
                case "SkloSklad":
                    activeColor = o.SkloSkladC;
                    break;
            }
            switch (activePopupName)
            {
                case "Dokumentace":
                    activeName = o.Dokumentace;
                    break;
                case "InDoca":
                    activeName = o.InDoca;
                    break;
                case "ObalovatOblozky":
                    activeName = o.ObalovatOblozky;
                    break;
                case "RezatOblozky":
                    activeName = o.RezatOblozky;
                    break;
                case "CncOblozky":
                    activeName = o.CncOblozky;
                    break;
                case "CentrumStredove":
                    activeName = o.CentrumStredove;
                    break;
                case "LisStredove":
                    activeName = o.LisStredove;
                    break;
                case "OlepovaniStredove":
                    activeName = o.OlepovaniStredove;
                    break;
                case "FrezovaniStredove":
                    activeName = o.FrezovaniStredove;
                    break;
                case "TypRamecek":
                    activeName = o.TypRamecek;
                    break;
                case "ObalovaniRamecek":
                    activeName = o.ObalovaniRamecek;
                    break;
                case "RezaniRamecek":
                    activeName = o.RezaniRamecek;
                    break;
                case "ObalovaniKlapacka":
                    activeName = o.ObalovaniKlapacka;
                    break;
                case "RezaniKlapacka":
                    activeName = o.RezaniKlapacka;
                    break;
                case "HranolPosuv":
                    activeName = o.HranolPosuv;
                    break;
                case "GarnyzPosuv":
                    activeName = o.GarnyzPosuv;
                    break;
                case "DorazovaPosuv":
                    activeName = o.DorazovaPosuv;
                    break;
                case "CentrumDvere":
                    activeName = o.CentrumDvere;
                    break;
                case "KorpusyDvere":
                    activeName = o.KorpusyDvere;
                    break;
                case "SesazenkyDvere":
                    activeName = o.SesazenkyDvere;
                    break;
                case "LisDvere":
                    activeName = o.LisDvere;
                    break;
                case "FormatkaDvere":
                    activeName = o.FormatkaDvere;
                    break;
                case "OlepovackaDvere":
                    activeName = o.OlepovackaDvere;
                    break;
                case "CncDvere":
                    activeName = o.CncDvere;
                    break;
                case "PgmDvere":
                    activeName = o.PgmDvere;
                    break;
                case "BrouseniDyhaDokonceni":
                    activeName = o.BrouseniDyhaDokonceni;
                    break;
                case "ZakladDokonceni":
                    activeName = o.ZakladDokonceni;
                    break;
                case "BrouseniZakladDokonceni":
                    activeName = o.BrouseniZakladDokonceni;
                    break;
                case "VrchDokonceni":
                    activeName = o.VrchDokonceni;
                    break;
                case "DvereKompletace":
                    activeName = o.DvereKompletace;
                    break;
                case "ZarubenKompletace":
                    activeName = o.ZarubenKompletace;
                    break;
                case "KovaniSklad":
                    activeName = o.KovaniSklad;
                    break;
                case "SkloSklad":
                    activeName = o.SkloSklad;
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
                case "Dokumentace":
                    o.Dokumentace = value;
                    break;
                case "InDoca":
                    o.InDoca = value;
                    break;

                case "ObalovatOblozky":
                    o.ObalovatOblozky = value;
                    break;
                case "RezatOblozky":
                    o.RezatOblozky = value;
                    break;
                case "CncOblozky":
                    o.CncOblozky = value;
                    break;

                case "CentrumStredove":
                    o.CentrumStredove = value;
                    break;
                case "LisStredove":
                    o.LisStredove = value;
                    break;
                case "OlepovaniStredove":
                    o.OlepovaniStredove = value;
                    break;
                case "FrezovaniStredove":
                    o.FrezovaniStredove = value;
                    break;

                case "TypRamecek":
                    o.TypRamecek = value;
                    break;
                case "ObalovaniRamecek":
                    o.ObalovaniRamecek = value;
                    break;
                case "RezaniRamecek":
                    o.RezaniRamecek = value;
                    break;
                case "ObalovaniKlapacka":
                    o.ObalovaniKlapacka = value;
                    break;
                case "RezaniKlapacka":
                    o.RezaniKlapacka = value;
                    break;

                case "HranolPosuv":
                    o.HranolPosuv = value;
                    break;
                case "GarnyzPosuv":
                    o.GarnyzPosuv = value;
                    break;
                case "DorazovaPosuv":
                    o.DorazovaPosuv = value;
                    break;

                case "CentrumDvere":
                    o.CentrumDvere = value;
                    break;
                case "KorpusyDvere":
                    o.KorpusyDvere = value;
                    break;
                case "SesazenkyDvere":
                    o.SesazenkyDvere = value;
                    break;
                case "LisDvere":
                    o.LisDvere = value;
                    break;
                case "FormatkaDvere":
                    o.FormatkaDvere = value;
                    break;
                case "OlepovackaDvere":
                    o.OlepovackaDvere = value;
                    break;
                case "CncDvere":
                    o.CncDvere = value;
                    break;
                case "PgmDvere":
                    o.PgmDvere = value;
                    break;

                case "BrouseniDyhaDokonceni":
                    o.BrouseniDyhaDokonceni = value;
                    break;
                case "ZakladDokonceni":
                    o.ZakladDokonceni = value;
                    break;
                case "BrouseniZakladDokonceni":
                    o.BrouseniZakladDokonceni = value;
                    break;
                case "VrchDokonceni":
                    o.VrchDokonceni = value;
                    break;
                case "DvereKompletace":
                    o.DvereKompletace = value;
                    break;
                case "ZarubenKompletace":
                    o.ZarubenKompletace = value;
                    break;

                case "KovaniSklad":
                    o.KovaniSklad = value;
                    break;
                case "SkloSklad":
                    o.SkloSklad = value;
                    break;
            }
            if (activeColor != null)
            {
                switch (activePopupName)
                {
                    case "Dokumentace":
                        o.DokumentaceC = activeColor;
                        break;
                    case "InDoca":
                        o.InDocaC = activeColor;
                        break;
                    case "ObalovatOblozky":
                        o.ObalovatOblozkyC = activeColor;
                        break;
                    case "RezatOblozky":
                        o.RezatOblozkyC = activeColor;
                        break;
                    case "CncOblozky":
                        o.CncOblozkyC = activeColor;
                        break;
                    case "CentrumStredove":
                        o.CentrumStredoveC = activeColor;
                        break;
                    case "LisStredove":
                        o.LisStredoveC = activeColor;
                        break;
                    case "OlepovaniStredove":
                        o.OlepovaniStredoveC = activeColor;
                        break;
                    case "FrezovaniStredove":
                        o.FrezovaniStredoveC = activeColor;
                        break;
                    case "TypRamecek":
                        o.TypRamecekC = activeColor;
                        break;
                    case "ObalovaniRamecek":
                        o.ObalovaniRamecekC = activeColor;
                        break;
                    case "RezaniRamecek":
                        o.RezaniRamecekC = activeColor;
                        break;
                    case "ObalovaniKlapacka":
                        o.ObalovaniKlapackaC = activeColor;
                        break;
                    case "RezaniKlapacka":
                        o.RezaniKlapackaC = activeColor;
                        break;
                    case "HranolPosuv":
                        o.HranolPosuvC = activeColor;
                        break;
                    case "GarnyzPosuv":
                        o.GarnyzPosuvC = activeColor;
                        break;
                    case "DorazovaPosuv":
                        o.DorazovaPosuvC = activeColor;
                        break;
                    case "CentrumDvere":
                        o.CentrumDvereC = activeColor;
                        break;
                    case "KorpusyDvere":
                        o.KorpusyDvereC = activeColor;
                        break;
                    case "SesazenkyDvere":
                        o.SesazenkyDvereC = activeColor;
                        break;
                    case "LisDvere":
                        o.LisDvereC = activeColor;
                        break;
                    case "FormatkaDvere":
                        o.FormatkaDvereC = activeColor;
                        break;
                    case "OlepovackaDvere":
                        o.OlepovackaDvereC = activeColor;
                        break;
                    case "CncDvere":
                        o.CncDvereC = activeColor;
                        break;
                    case "PgmDvere":
                        o.PgmDvereC = activeColor;
                        break;
                    case "BrouseniDyhaDokonceni":
                        o.BrouseniDyhaDokonceniC = activeColor;
                        break;
                    case "ZakladDokonceni":
                        o.ZakladDokonceniC = activeColor;
                        break;
                    case "BrouseniZakladDokonceni":
                        o.BrouseniZakladDokonceniC = activeColor;
                        break;
                    case "VrchDokonceni":
                        o.VrchDokonceniC = activeColor;
                        break;
                    case "DvereKompletace":
                        o.DvereKompletaceC = activeColor;
                        break;
                    case "ZarubenKompletace":
                        o.ZarubenKompletaceC = activeColor;
                        break;
                    case "KovaniSklad":
                        o.KovaniSkladC = activeColor;
                        break;
                    case "SkloSklad":
                        o.SkloSkladC = activeColor;
                        break;
                }
            }
            o.PripravaC = "Yellow";
            o.OblozkyC = "Yellow";
            o.StredoveC = "Yellow";
            o.RamecekC = "Yellow";
            o.KlapackaC = "Yellow";
            o.PosuvC = "Yellow";
            o.DvereC = "Yellow";
            o.DokonceniC = "Yellow";
            o.KompletaceC = "Yellow";
            o.SkladC = "Yellow";
            if ((o.Dokumentace == "✓" || o.Dokumentace == "✗") && (o.InDoca == "✓" || o.InDoca == "✗"))
            {
                o.PripravaC = "LimeGreen";
            }
            if ((o.RezatOblozky == "✓" || o.RezatOblozky == "✗") && (o.ObalovatOblozky == "✓" || o.ObalovatOblozky == "✗") && (o.CncOblozky == "✓" || o.CncOblozky == "✗"))
            {
                o.OblozkyC = "LimeGreen";
            }
            if ((o.OlepovaniStredove == "✓" || o.OlepovaniStredove == "✗") && (o.LisStredove == "✓" || o.LisStredove == "✗") &&
                (o.FrezovaniStredove == "✓" || o.FrezovaniStredove == "✗") && (o.CentrumStredove == "✓" || o.CentrumStredove == "✗"))
            {
                o.StredoveC = "LimeGreen";
            }
            if ((o.ObalovaniRamecek == "✓" || o.ObalovaniRamecek == "✗") && (o.RezaniRamecek == "✓" || o.RezaniRamecek == "✗") && (o.TypRamecek == "✓" || o.TypRamecek == "✗"))
            {
                o.RamecekC = "LimeGreen";
            }
            if ((o.ObalovaniKlapacka == "✓" || o.ObalovaniKlapacka == "✗") && (o.RezaniKlapacka == "✓" || o.RezaniKlapacka == "✗"))
            {
                o.KlapackaC = "LimeGreen";
            }
            if ((o.GarnyzPosuv == "✓" || o.GarnyzPosuv == "✗") && (o.DorazovaPosuv == "✓" || o.DorazovaPosuv == "✗") && (o.HranolPosuv == "✓" || o.HranolPosuv == "✗"))
            {
                o.PosuvC = "LimeGreen";
            }
            if ((o.CentrumDvere == "✓" || o.CentrumDvere == "✗") && (o.CncDvere == "✓" || o.CncDvere == "✗") &&
                (o.FormatkaDvere == "✓" || o.FormatkaDvere == "✗") && (o.KorpusyDvere == "✓" || o.KorpusyDvere == "✗") &&
                (o.LisDvere == "✓" || o.LisDvere == "✗") && (o.OlepovackaDvere == "✓" || o.OlepovackaDvere == "✗") &&
                (o.PgmDvere == "✓" || o.PgmDvere == "✗") && (o.SesazenkyDvere == "✓" || o.SesazenkyDvere == "✗"))
            {
                o.DvereC = "LimeGreen";
            }
            if ((o.BrouseniDyhaDokonceni == "✓" || o.BrouseniDyhaDokonceni == "✗") && (o.BrouseniZakladDokonceni == "✓" || o.BrouseniZakladDokonceni == "✗") &&
                (o.VrchDokonceni == "✓" || o.VrchDokonceni == "✗") && (o.ZakladDokonceni == "✓" || o.ZakladDokonceni == "✗"))
            {
                o.DokonceniC = "LimeGreen";
            }
            if ((o.DvereKompletace == "✓" || o.DvereKompletace == "✗") && (o.ZarubenKompletace == "✓" || o.ZarubenKompletace == "✗"))
            {
                o.KompletaceC = "LimeGreen";
            }
            if ((o.KovaniSklad == "✓" || o.KovaniSklad == "✗") && (o.SkloSklad == "✓" || o.SkloSklad == "✗"))
            {
                o.SkladC = "LimeGreen";
            }
            db.SaveChanges();
            Popupek.IsOpen = false;
        }

        List<string> columns = new List<string> { "Zahajeni Datum", "Technik", "Zakazka",
            "Poznamky", "Material", "Cislo Zakazky", "Vyrobni Cislo","Soubor", "Upravit soubor", "Kridla",
            "Zarubne", "Priprava", "Oblozky", "Stredove", "Ramecek", "Klapacka", "Posuv", "Dvere",
            "Dokonceni", "Kompletace", "Sklad", "Pozadovany Datum", "Datum Dokonceni", "Mesto", "Mapa", "Plosny material", "Hotovo bloky"};

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
                    Material.Visibility = Material.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Cislo Zakazky":
                    Cislo_zakazky.Visibility = Cislo_zakazky.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Vyrobni Cislo":
                    Vyrobni_cislo.Visibility = Vyrobni_cislo.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Soubor":
                    UpravitSoubor.Visibility = UpravitSoubor.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Upravit soubor":
                    editUrl.Visibility = editUrl.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Kridla":
                    Kridla.Visibility = Kridla.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Zarubne":
                    Zarubne.Visibility = Zarubne.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;

                case "Priprava":
                    Priprava.Visibility = Priprava.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Oblozky":
                    Oblozky.Visibility = Zarubne.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Stredove":
                    Stredove.Visibility = Oblozky.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Ramecek":
                    Ramecek.Visibility = Ramecek.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Klapacka":
                    Klapacka.Visibility = Klapacka.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Posuv":
                    Posuv.Visibility = Posuv.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Dvere":
                    Dvere.Visibility = Dvere.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Dokonceni":
                    Dokonceni.Visibility = Dokonceni.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Kompletace":
                    Kompletace.Visibility = Kompletace.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Sklad":
                    Sklad.Visibility = Sklad.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;


                case "Pozadovany Datum":
                    PozadovanyDatum.Visibility = PozadovanyDatum.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Datum Dokonceni":
                    DokonceniDatum.Visibility = Zarubne.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Mesto":
                    Mesto.Visibility = Mesto.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Mapa":
                    AddressLink.Visibility = AddressLink.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

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
                    List<DataGridLength> listw = new List<DataGridLength>();

                    WoodDatagrid.Columns.ToList().ForEach(x => listw.Add(x.ActualWidth));

                    if (listw.Count != 0)
                    {
                        string fileName = System.IO.Path.Combine(cesta, "woodWidths.json");

                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        var jsonString = JsonConvert.SerializeObject(listw);
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
