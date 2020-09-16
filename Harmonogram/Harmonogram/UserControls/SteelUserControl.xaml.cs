using Harmonogram.Helper;
using Harmonogram.ViewModels;
using Harmonogram.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
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
    public partial class SteelUserControl : UserControl
    {
        SteelOrderViewModel orderVM;
        private bool isShopShowed;
        List<string> filterList;

        ObservableCollection<Order> orders = new ObservableCollection<Order>();
        ObservableCollection<Order> newOrders = new ObservableCollection<Order>();

        HarmonogramDBEntities db;

        public SteelUserControl()
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

            orderVM = new SteelOrderViewModel();
            DataContext = orderVM;
            SteelDatagrid.ItemsSource = orderVM.Orders;
            NewSteelDatagrid.ItemsSource = orderVM.NewOrders;
            db = new HarmonogramDBEntities();
            //hide shop part
            isShopShowed = false;
            NewDatagridRow.Height = new GridLength(0, GridUnitType.Star);
            NewTitleRow.Height = new GridLength(0, GridUnitType.Pixel);
            NewToolbarRow.Height = new GridLength(0, GridUnitType.Pixel);
            listBoxColumns.ItemsSource = columns;
            ZakazkaNr.IsReadOnly = !StaticResources.User.PrepisCisloZakazky.Value;

            string cesta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "harmonogramlignis");
            string soubor = Path.Combine(cesta, "steelWidths.json");
            if (File.Exists(soubor))
            {
                List<DataGridLength> list = JsonConvert.DeserializeObject<List<DataGridLength>>(File.ReadAllText(soubor));

                if ( list.Count == SteelDatagrid.Columns.Count)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        DataGridLength j = list[i];
                        SteelDatagrid.Columns[i].Width = j;
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
            orderVM = new SteelOrderViewModel();
            DataContext = orderVM;
            SteelDatagrid.ItemsSource = orderVM.Orders;
            NewSteelDatagrid.ItemsSource = orderVM.NewOrders;
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
                    SteelDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Technik) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 3));
                    break;
                case "ZakazkaFiltrujBtn":
                    SteelDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Zakazka) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 3));
                    break;
                case "ZakazkaNrFiltrujBtn":
                    SteelDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.ZakazkaNr) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 3));
                    break;
                case "MaterialFiltrujBtn":
                    SteelDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Material) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 3));
                    break;
                case "VyrobniNrFiltrujBtn":
                    SteelDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.VyrobniNr) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 3));
                    break;
                case "MestoFiltrujBtn":
                    SteelDatagrid.ItemsSource = new ObservableCollection<Order>(orderVM.Orders.OrderBy(p => p.SortNr).Where(p => filterList.Contains(p.Mesto) && p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 3));
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
                Order order = orderVM.Current_order;
                int sortnr = order.SortNr.Value;
                Order dbOrder = db.Orders.Find(order.OrderId);
                dbOrder.IsDeleted = true;
                dbOrder.IsInProccess = false;
                dbOrder.DeletedBy = StaticResources.UserName;
                dbOrder.DeletedDate = DateTime.Now;
                orderVM.Orders.Remove(order);
                orderVM.Orders.Where(x => x.SortNr > sortnr).ToList().ForEach(y => y.SortNr--);
                var rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - 1 WHERE Department = 3 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr > @i", new SqlParameter("@i", sortnr));
                rows = db.Database.ExecuteSqlCommand("DELETE FROM [Expedition] WHERE OrderId = @i", new SqlParameter("@i", order.OrderId));
                rows = db.Database.ExecuteSqlCommand("SELECT ExpeditionId = T.ExpeditionId, ExpeditedSortNr = T.ExpeditedSortNr INTO #Temp FROM [Expedition] AS T UNION ALL SELECT ExpeditionId = 0, ExpeditedSortNr = 0 ORDER BY ExpeditedSortNr, ExpeditionId;DECLARE @id int;DECLARE @i int = 1;SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;DELETE #Temp WHERE ExpeditionId = @id;WHILE (SELECT Count(*) FROM #Temp) > 0 BEGIN SELECT TOP 1 @id = [ExpeditionId] FROM #Temp;UPDATE [Expedition] SET [ExpeditedSortNr] = @i WHERE [ExpeditionId] = @id;SET @i = @i + 1;DELETE #Temp WHERE ExpeditionId = @id;END");
                db.SaveChanges();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            db.Orders.Where(x => x.Department == 3 && x.IsInProccess && !x.IsDeleted).OrderBy(x => x.SortNr).ToList().ForEach(x =>
            {
                x.SortNr = i;
                i++;
            });
            db.SaveChanges();
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
                foreach (var row in SteelDatagrid.SelectedItems)
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
                                var vmOrdersArray = orderVM.Orders.Where(x => x.SortNr >= number && x.SortNr < pos).OrderBy(p => p.SortNr);

                                int rows;
                                foreach (var element in vmOrdersArray)
                                {
                                    int newpos = element.SortNr.Value;
                                    newpos += delka;
                                    element.SortNr = newpos;
                                }
                                rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] + @delka WHERE SortNr >= @number AND SortNr < @pos AND Department = 3 AND IsInProccess = 1 AND IsDeleted = 0", new SqlParameter("@number", number), new SqlParameter("@pos", pos), new SqlParameter("@delka", delka));

                                for (int i = 0; i < ordery.Count; i++)
                                {
                                    orderVM.Orders.Remove(ordery[i]);
                                    orderVM.Orders.Insert(number + i - 1, ordery[i]);
                                    rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = @number + @i WHERE OrderId = @id", new SqlParameter("@number", number), new SqlParameter("@i", i), new SqlParameter("@id", ordery[i].OrderId));
                                }
                                db.SaveChanges();
                            }
                            else
                            {
                                if (pos < number)
                                {
                                    var vmOrdersArray = orderVM.Orders.Where(x => x.SortNr > (pos + delka - 1) && x.SortNr < (number + delka)).OrderBy(p => p.SortNr);

                                    int rows;
                                    foreach (var element in vmOrdersArray)
                                    {
                                        int newpos = element.SortNr.Value;
                                        newpos -= delka;
                                        element.SortNr = newpos;
                                    }
                                    rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - @delka WHERE SortNr > (@pos + @delka - 1) AND SortNr < (@number + @delka) AND Department = 3 AND IsInProccess = 1 AND IsDeleted = 0", new SqlParameter("@number", number), new SqlParameter("@pos", pos), new SqlParameter("@delka", delka));

                                    for (int i = 0; i < ordery.Count; i++)
                                    {
                                        orderVM.Orders.Remove(ordery[i]);
                                        orderVM.Orders.Insert(number + i - 1, ordery[i]);
                                        rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = @number + @i WHERE OrderId = @id", new SqlParameter("@number", number), new SqlParameter("@i", i), new SqlParameter("@id", ordery[i].OrderId));
                                        ordery[i].SortNr = number + i;
                                    }
                                    db.SaveChanges();
                                    //var count = orderVM.Orders.Where(x => x.SortNr > (pos + delka - 1) && x.SortNr < (number + delka)).OrderBy(p => p.SortNr);
                                    //List<int> nrArray = new List<int>();
                                    //foreach (var order in count)
                                    //{
                                    //    nrArray.Add(order.OrderId);
                                    //}
                                    //int rows;

                                    //foreach (var element in nrArray)
                                    //{
                                    //    Order c = orderVM.Orders.FirstOrDefault(x => x.OrderId == element);
                                    //    int newpos = c.SortNr.Value;
                                    //    newpos -= delka;
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
            else
            {
                MessageBox.Show("Neplatne cislo");
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
                    if (new NewBlankRow(3).ShowDialog() == true)
                    {
                        ShowWaitingCursor();
                        Order newOrder = OrderCreator.createOrder(3);
                        StaticResources.blankRowColor = "NULL";
                        StaticResources.blankRowName = "";
                        newOrder.OrderId = db.Orders.Max(x => x.OrderId) + 1;
                        newOrder.SortNr = o.SortNr;
                        newOrder.IsInProccess = true;
                        db.Orders.Add(newOrder);
                        orderVM.Orders.Insert(o.SortNr.Value - 1, newOrder);
                        orderVM.Orders.Where(x => !x.IsDeleted && x.Department == 3 && x.IsInProccess && x.SortNr.Value > o.SortNr.Value).ToList().ForEach(a => a.SortNr++);
                        int rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] + 1 WHERE Department = 3 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr >= @i", new SqlParameter("@i", o.SortNr));
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
                int rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - 1 WHERE Department = 3 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr > @i", new SqlParameter("@i", sortnr));
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
            orderVM = new SteelOrderViewModel();
            DataContext = orderVM;
            SteelDatagrid.ItemsSource = orderVM.Orders;
            NewSteelDatagrid.ItemsSource = orderVM.NewOrders;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            new NewOrderWindow(3).ShowDialog();
            if (StaticResources.lastAddedOrder != null)
            {
                var order = db.Orders.Find(StaticResources.lastAddedOrder.OrderId);
                orderVM.NewOrders.Add(order);
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
                }
                catch
                {

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
            preview_Document.Write(SteelDatagrid);
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
                var rows = db.Database.ExecuteSqlCommand("UPDATE [Order] SET [SortNr] = [SortNr] - 1 WHERE Department = 3 AND IsInProccess = 1 AND IsDeleted = 0 AND SortNr > @i", new SqlParameter("@i", sortnr));
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
                    int id = orderVM.Current_order.OrderId;
                    var update = db.Orders.Where(o => o.OrderId == id).FirstOrDefault();
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
                    int id = orderVM.Current_order.OrderId;
                    var update = db.Orders.Where(o => o.OrderId == id).FirstOrDefault();
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
                    int id = orderVM.Current_order.OrderId;
                    var update = db.Orders.Where(o => o.OrderId == id).FirstOrDefault();
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
                    if (DokumentPripravaKrid.Visibility == Visibility.Visible)
                    {
                        DokumentPripravaKrid.Visibility = Visibility.Hidden;
                        IndocaPripravaKrid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        DokumentPripravaKrid.Visibility = Visibility.Visible;
                        IndocaPripravaKrid.Visibility = Visibility.Visible;
                    }
                    break;

                case "Laser":
                    if (PohLaserKrid.Visibility == Visibility.Visible)
                    {
                        PohLaserKrid.Visibility = Visibility.Hidden;
                        NepohLaserKrid.Visibility = Visibility.Hidden;
                        KlapackaLaserKrid.Visibility = Visibility.Hidden;
                        OkopLaserKrid.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        NepohLaserKrid.Visibility = Visibility.Visible;
                        PohLaserKrid.Visibility = Visibility.Visible;
                        KlapackaLaserKrid.Visibility = Visibility.Visible;
                        OkopLaserKrid.Visibility = Visibility.Visible;
                    }
                    break;

                case "Ohybacka":
                    if (PohOhybackaKrid.Visibility == Visibility.Visible)
                    {
                        PohOhybackaKrid.Visibility = Visibility.Hidden;
                        NepohOhybackaKrid.Visibility = Visibility.Hidden;
                        KlapackaOhybackaKrid.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        PohOhybackaKrid.Visibility = Visibility.Visible;
                        NepohOhybackaKrid.Visibility = Visibility.Visible;
                        KlapackaOhybackaKrid.Visibility = Visibility.Visible;

                    }
                    break;

                case "Bodovani":
                    if (SplnenoBodovaniKrid.Visibility == Visibility.Visible)
                    {
                        SplnenoBodovaniKrid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoBodovaniKrid.Visibility = Visibility.Visible;
                    }
                    break;

                case "Thermacol":
                    if (NarezThermacolKrid.Visibility == Visibility.Visible)
                    {
                        NarezThermacolKrid.Visibility = Visibility.Hidden;
                        LepeniThermacolKrid.Visibility = Visibility.Hidden;
                        CncThermacolKrid.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        NarezThermacolKrid.Visibility = Visibility.Visible;
                        LepeniThermacolKrid.Visibility = Visibility.Visible;
                        CncThermacolKrid.Visibility = Visibility.Visible;

                    }
                    break;

                case "Lepeni":
                    if (SplnenoLepeniKrid.Visibility == Visibility.Visible)
                    {
                        SplnenoLepeniKrid.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        SplnenoLepeniKrid.Visibility = Visibility.Visible;
                    }
                    break;
                case "Strikarna":
                    if (SplnenoStrikarnaKrid.Visibility == Visibility.Visible)
                    {
                        SplnenoStrikarnaKrid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoStrikarnaKrid.Visibility = Visibility.Visible;
                    }
                    break;
                case "Ramecky":
                    if (NarezRameckyKrid.Visibility == Visibility.Visible)
                    {
                        NarezRameckyKrid.Visibility = Visibility.Hidden;
                        FrezRameckyKrid.Visibility = Visibility.Hidden;
                        SvarRameckyKrid.Visibility = Visibility.Hidden;
                        StriRameckyKrid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        NarezRameckyKrid.Visibility = Visibility.Visible;
                        FrezRameckyKrid.Visibility = Visibility.Visible;
                        SvarRameckyKrid.Visibility = Visibility.Visible;
                        StriRameckyKrid.Visibility = Visibility.Visible;
                    }
                    break;

                case "Kompletace":
                    if (SplnenoKompletaceKrid.Visibility == Visibility.Visible)
                    {
                        SplnenoKompletaceKrid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoKompletaceKrid.Visibility = Visibility.Visible;
                    }
                    break;

                case "Sklad":
                    if (PlechSkladKrid.Visibility == Visibility.Visible)
                    {
                        PlechSkladKrid.Visibility = Visibility.Hidden;
                        KovaniSkladKrid.Visibility = Visibility.Hidden;
                        BarvaSkladKrid.Visibility = Visibility.Hidden;
                        SkloSkladKrid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        PlechSkladKrid.Visibility = Visibility.Visible;
                        KovaniSkladKrid.Visibility = Visibility.Visible;
                        BarvaSkladKrid.Visibility = Visibility.Visible;
                        SkloSkladKrid.Visibility = Visibility.Visible;
                    }
                    break;

                case "Priprava.":
                    if (DokumentPripravaZar.Visibility == Visibility.Visible)
                    {
                        DokumentPripravaZar.Visibility = Visibility.Hidden;
                        InDocaPripravaZar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        DokumentPripravaZar.Visibility = Visibility.Visible;
                        InDocaPripravaZar.Visibility = Visibility.Visible;
                    }
                    break;

                case "Laser.":
                    if (PohLaserZar.Visibility == Visibility.Visible)
                    {
                        PohLaserZar.Visibility = Visibility.Hidden;
                        NepohLaserZar.Visibility = Visibility.Hidden;
                        PoutecLaserZar.Visibility = Visibility.Hidden;
                        OkopLaserKrid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        PohLaserZar.Visibility = Visibility.Visible;
                        NepohLaserZar.Visibility = Visibility.Visible;
                        PoutecLaserZar.Visibility = Visibility.Visible;
                        OkopLaserKrid.Visibility = Visibility.Visible;
                    }
                    break;

                case "Nuzky.":
                    if (SplnenoNuzkyZar.Visibility == Visibility.Visible)
                    {
                        SplnenoNuzkyZar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoNuzkyZar.Visibility = Visibility.Visible;
                    }
                    break;

                case "Ohybacka.":
                    if (PohOhybackaZar.Visibility == Visibility.Visible)
                    {
                        PohOhybackaZar.Visibility = Visibility.Hidden;
                        NepohOhybackaZar.Visibility = Visibility.Hidden;
                        PoutecOhybackaZar.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        PohOhybackaZar.Visibility = Visibility.Visible;
                        NepohOhybackaZar.Visibility = Visibility.Visible;
                        PoutecOhybackaZar.Visibility = Visibility.Visible;

                    }
                    break;

                case "Pila.":
                    if (PohPilaZar.Visibility == Visibility.Visible)
                    {
                        PohPilaZar.Visibility = Visibility.Hidden;
                        NepohPilaZar.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        PohPilaZar.Visibility = Visibility.Visible;
                        NepohPilaZar.Visibility = Visibility.Visible;

                    }
                    break;

                case "Vyrazecka.":
                    if (SplnenoVyrazeckaZar.Visibility == Visibility.Visible)
                    {
                        SplnenoVyrazeckaZar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoVyrazeckaZar.Visibility = Visibility.Visible;
                    }
                    break;



                case "Bodovani.":
                    if (SplnenoBodovaniZar.Visibility == Visibility.Visible)
                    {
                        SplnenoBodovaniZar.Visibility = Visibility.Hidden;

                    }
                    else
                    {
                        SplnenoBodovaniZar.Visibility = Visibility.Visible;
                    }
                    break;
                case "Svarovani.":
                    if (SplnenoBodovaniZar.Visibility == Visibility.Visible)
                    {
                        SplnenoSvarovaniZar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoSvarovaniZar.Visibility = Visibility.Visible;
                    }
                    break;

                case "Brouseni.":
                    if (SplnenoBrouseniZar.Visibility == Visibility.Visible)
                    {
                        SplnenoBrouseniZar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoBrouseniZar.Visibility = Visibility.Visible;
                    }
                    break;
                case "Strikarna.":
                    if (SplnenoStrikarnaZar.Visibility == Visibility.Visible)
                    {
                        SplnenoStrikarnaZar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoStrikarnaZar.Visibility = Visibility.Visible;
                    }
                    break;

                case "Ramecky.":
                    if (NarezRameckyZar.Visibility == Visibility.Visible)
                    {
                        NarezRameckyZar.Visibility = Visibility.Hidden;
                        FrezRameckyZar.Visibility = Visibility.Hidden;
                        SvarRameckyZar.Visibility = Visibility.Hidden;
                        StriRameckyZar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        NarezRameckyZar.Visibility = Visibility.Visible;
                        FrezRameckyZar.Visibility = Visibility.Visible;
                        SvarRameckyZar.Visibility = Visibility.Visible;
                        StriRameckyZar.Visibility = Visibility.Visible;
                    }
                    break;

                case "Kompletace.":
                    if (SplnenoKompletaceZar.Visibility == Visibility.Visible)
                    {
                        SplnenoKompletaceZar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        SplnenoKompletaceZar.Visibility = Visibility.Visible;
                    }
                    break;

                case "Sklad.":
                    if (PlechSkladZar.Visibility == Visibility.Visible)
                    {
                        PlechSkladZar.Visibility = Visibility.Hidden;
                        BarvaSkladZar.Visibility = Visibility.Hidden;
                        KovaniSkladZar.Visibility = Visibility.Hidden;
                        SkloSkladZar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        PlechSkladZar.Visibility = Visibility.Visible;
                        BarvaSkladZar.Visibility = Visibility.Visible;
                        KovaniSkladZar.Visibility = Visibility.Visible;
                        SkloSkladZar.Visibility = Visibility.Visible;
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
                case "DokumentPripravaZar":
                    activeColor = o.DokumentPripravaZarC;
                    break;
                case "InDocaPripravaZar":
                    activeColor = o.InDocaPripravaZarC;
                    break;
                case "PohLaserZar":
                    activeColor = o.PohLaserZarC;
                    break;
                case "NepohLaserZar":
                    activeColor = o.NepohLaserZarC;
                    break;
                case "PoutecLaserZar":
                    activeColor = o.PoutecLaserZarC;
                    break;
                case "SplnenoNuzkyZar":
                    activeColor = o.SplnenoNuzkyZarC;
                    break;
                case "PohOhybackaZar":
                    activeColor = o.PohOhybackaZarC;
                    break;
                case "NepohOhybackaZar":
                    activeColor = o.NepohOhybackaZarC;
                    break;
                case "PoutecOhybackaZar":
                    activeColor = o.PoutecOhybackaZarC;
                    break;
                case "PohPilaZar":
                    activeColor = o.PohPilaZarC;
                    break;
                case "NepohPilaZar":
                    activeColor = o.NepohPilaZarC;
                    break;
                case "SplnenoVyrazeckaZar":
                    activeColor = o.SplnenoVyrazeckaZarC;
                    break;
                case "SplnenoBodovaniZar":
                    activeColor = o.SplnenoBodovaniZarC;
                    break;
                case "SplnenoSvarovaniZar":
                    activeColor = o.SplnenoSvarovaniZarC;
                    break;
                case "SplnenoBrouseniZar":
                    activeColor = o.SplnenoBrouseniZarC;
                    break;
                case "SplnenoStrikarnaZar":
                    activeColor = o.SplnenoStrikarnaZarC;
                    break;
                case "NarezRameckyZar":
                    activeColor = o.NarezRameckyZarC;
                    break;
                case "FrezRameckyZar":
                    activeColor = o.FrezRameckyZarC;
                    break;
                case "SvarRameckyZar":
                    activeColor = o.SvarRameckyZarC;
                    break;
                case "StriRameckyZar":
                    activeColor = o.StriRameckyZarC;
                    break;
                case "SplnenoKompletaceZar":
                    activeColor = o.SplnenoKompletaceZarC;
                    break;
                case "PlechSkladZar":
                    activeColor = o.PlechSkladZarC;
                    break;
                case "BarvaSkladZar":
                    activeColor = o.BarvaSkladZarC;
                    break;
                case "KovaniSkladZar":
                    activeColor = o.KovaniSkladZarC;
                    break;
                case "SkloSkladZar":
                    activeColor = o.SkloSkladZarC;
                    break;


                case "DokumentPripravaKrid":
                    activeColor = o.DokumentPripravaKridC;
                    break;
                case "IndocaPripravaKrid":
                    activeColor = o.IndocaPripravaKridC;
                    break;
                case "PohLaserKrid":
                    activeColor = o.PohLaserKridC;
                    break;
                case "NepohLaserKrid":
                    activeColor = o.NepohLaserKridC;
                    break;
                case "KlapackaLaserKrid":
                    activeColor = o.KlapackaLaserKridC;
                    break;
                case "OkopLaserKrid":
                    activeColor = o.OkopLaserKridC;
                    break;
                case "PohOhybackaKrid":
                    activeColor = o.PohOhybackaKridC;
                    break;
                case "NepohOhybackaKrid":
                    activeColor = o.NepohOhybackaKridC;
                    break;
                case "KlapackaOhybackaKrid":
                    activeColor = o.KlapackaOhybackaKridC;
                    break;
                case "SplnenoBodovaniKrid":
                    activeColor = o.SplnenoBodovaniKridC;
                    break;
                case "NarezThermacolKrid":
                    activeColor = o.NarezThermacolKridC;
                    break;
                case "LepeniThermacolKrid":
                    activeColor = o.LepeniThermacolKridC;
                    break;
                case "CncThermacolKrid":
                    activeColor = o.CncThermacolKridC;
                    break;
                case "SplnenoLepeniKrid":
                    activeColor = o.SplnenoLepeniKridC;
                    break;
                case "SplnenoStrikarnaKrid":
                    activeColor = o.SplnenoStrikarnaKridC;
                    break;
                case "NarezRameckyKrid":
                    activeColor = o.NarezRameckyKridC;
                    break;
                case "FrezRameckyKrid":
                    activeColor = o.FrezRameckyKridC;
                    break;
                case "SvarRameckyKrid":
                    activeColor = o.SvarRameckyKridC;
                    break;
                case "StriRameckyKrid":
                    activeColor = o.StriRameckyKridC;
                    break;
                case "SplnenoKompletaceKrid":
                    activeColor = o.SplnenoKompletaceKridC;
                    break;
                case "PlechSkladKrid":
                    activeColor = o.PlechSkladKridC;
                    break;
                case "KovaniSkladKrid":
                    activeColor = o.KovaniSkladKridC;
                    break;
                case "BarvaSkladKrid":
                    activeColor = o.BarvaSkladKridC;
                    break;
                case "SkloSkladKrid":
                    activeColor = o.SkloSkladKridC;
                    break;
            }

            switch (activePopupName)
            {
                case "Dokumentace":
                    activeName = o.Dokumentace;
                    break;

                case "DokumentPripravaZar":
                    activeName = o.DokumentPripravaZar;
                    break;
                case "InDocaPripravaZar":
                    activeName = o.InDocaPripravaZar;
                    break;
                case "PohLaserZar":
                    activeName = o.PohLaserZar;
                    break;
                case "NepohLaserZar":
                    activeName = o.NepohLaserZar;
                    break;
                case "PoutecLaserZar":
                    activeName = o.PoutecLaserZar;
                    break;
                case "SplnenoNuzkyZar":
                    activeName = o.SplnenoNuzkyZar;
                    break;
                case "PohOhybackaZar":
                    activeName = o.PohOhybackaZar;
                    break;
                case "NepohOhybackaZar":
                    activeName = o.NepohOhybackaZar;
                    break;
                case "PoutecOhybackaZar":
                    activeName = o.PoutecOhybackaZar;
                    break;
                case "PohPilaZar":
                    activeName = o.PohPilaZar;
                    break;
                case "NepohPilaZar":
                    activeName = o.NepohPilaZar;
                    break;
                case "SplnenoVyrazeckaZar":
                    activeName = o.SplnenoVyrazeckaZar;
                    break;
                case "SplnenoBodovaniZar":
                    activeName = o.SplnenoBodovaniZar;
                    break;
                case "SplnenoSvarovaniZar":
                    activeName = o.SplnenoSvarovaniZar;
                    break;
                case "SplnenoBrouseniZar":
                    activeName = o.SplnenoBrouseniZar;
                    break;
                case "SplnenoStrikarnaZar":
                    activeName = o.SplnenoStrikarnaZar;
                    break;
                case "NarezRameckyZar":
                    activeName = o.NarezRameckyZar;
                    break;
                case "FrezRameckyZar":
                    activeName = o.FrezRameckyZar;
                    break;
                case "SvarRameckyZar":
                    activeName = o.SvarRameckyZar;
                    break;
                case "StriRameckyZar":
                    activeName = o.StriRameckyZar;
                    break;
                case "SplnenoKompletaceZar":
                    activeName = o.SplnenoKompletaceZar;
                    break;
                case "PlechSkladZar":
                    activeName = o.PlechSkladZar;
                    break;
                case "BarvaSkladZar":
                    activeName = o.BarvaSkladZar;
                    break;
                case "KovaniSkladZar":
                    activeName = o.KovaniSkladZar;
                    break;
                case "SkloSkladZar":
                    activeName = o.SkloSkladZar;
                    break;


                case "DokumentPripravaKrid":
                    activeName = o.DokumentPripravaKrid;
                    break;
                case "IndocaPripravaKrid":
                    activeName = o.IndocaPripravaKrid;
                    break;
                case "PohLaserKrid":
                    activeName = o.PohLaserKrid;
                    break;
                case "NepohLaserKrid":
                    activeName = o.NepohLaserKrid;
                    break;
                case "KlapackaLaserKrid":
                    activeName = o.KlapackaLaserKrid;
                    break;
                case "OkopLaserKrid":
                    activeName = o.OkopLaserKrid;
                    break;
                case "PohOhybackaKrid":
                    activeName = o.PohOhybackaKrid;
                    break;
                case "NepohOhybackaKrid":
                    activeName = o.NepohOhybackaKrid;
                    break;
                case "KlapackaOhybackaKrid":
                    activeName = o.KlapackaOhybackaKrid;
                    break;
                case "SplnenoBodovaniKrid":
                    activeName = o.SplnenoBodovaniKrid;
                    break;
                case "NarezThermacolKrid":
                    activeName = o.NarezThermacolKrid;
                    break;
                case "LepeniThermacolKrid":
                    activeName = o.LepeniThermacolKrid;
                    break;
                case "CncThermacolKrid":
                    activeName = o.CncThermacolKrid;
                    break;
                case "SplnenoLepeniKrid":
                    activeName = o.SplnenoLepeniKrid;
                    break;
                case "SplnenoStrikarnaKrid":
                    activeName = o.SplnenoStrikarnaKrid;
                    break;
                case "NarezRameckyKrid":
                    activeName = o.NarezRameckyKrid;
                    break;
                case "FrezRameckyKrid":
                    activeName = o.FrezRameckyKrid;
                    break;
                case "SvarRameckyKrid":
                    activeName = o.SvarRameckyKrid;
                    break;
                case "StriRameckyKrid":
                    activeName = o.StriRameckyKrid;
                    break;
                case "SplnenoKompletaceKrid":
                    activeName = o.SplnenoKompletaceKrid;
                    break;
                case "PlechSkladKrid":
                    activeName = o.PlechSkladKrid;
                    break;
                case "KovaniSkladKrid":
                    activeName = o.KovaniSkladKrid;
                    break;
                case "BarvaSkladKrid":
                    activeName = o.BarvaSkladKrid;
                    break;
                case "SkloSkladKrid":
                    activeName = o.SkloSkladKrid;
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
                    } else
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
                case "DokumentPripravaZar":
                    o.DokumentPripravaZar = value;
                    
                    break;
                case "InDocaPripravaZar":
                    o.InDocaPripravaZar = value;    
                    break;
                case "PohLaserZar":
                    o.PohLaserZar = value;
                    break;
                case "NepohLaserZar":
                    o.NepohLaserZar = value;
                    break;
                case "PoutecLaserZar":
                    o.PoutecLaserZar = value;
                    break;
                case "SplnenoNuzkyZar":
                    o.SplnenoNuzkyZar = value;
                    break;
                case "PohOhybackaZar":
                    o.PohOhybackaZar = value;
                    break;
                case "NepohOhybackaZar":
                    o.NepohOhybackaZar = value;
                    break;
                case "PoutecOhybackaZar":
                    o.PoutecOhybackaZar = value;
                    break;
                case "PohPilaZar":
                    o.PohPilaZar = value;
                    break;
                case "NepohPilaZar":
                    o.NepohPilaZar = value;
                    break;
                case "SplnenoVyrazeckaZar":
                    o.SplnenoVyrazeckaZar = value;
                    break;
                case "SplnenoBodovaniZar":
                    o.SplnenoBodovaniZar = value;
                    break;
                case "SplnenoSvarovaniZar":
                    o.SplnenoSvarovaniZar = value;
                    break;
                case "SplnenoBrouseniZar":
                    o.SplnenoBrouseniZar = value;
                    break;
                case "SplnenoStrikarnaZar":
                    o.SplnenoStrikarnaZar = value;
                    break;
                case "NarezRameckyZar":
                    o.NarezRameckyZar = value;
                    break;
                case "FrezRameckyZar":
                    o.FrezRameckyZar = value;
                    break;
                case "SvarRameckyZar":
                    o.SvarRameckyZar = value;
                    break;
                case "StriRameckyZar":
                    o.StriRameckyZar = value;
                    break;
                case "SplnenoKompletaceZar":
                    o.SplnenoKompletaceZar = value;
                    break;
                case "PlechSkladZar":
                    o.PlechSkladZar = value;
                    break;
                case "BarvaSkladZar":
                    o.BarvaSkladZar = value;
                    break;
                case "KovaniSkladZar":
                    o.KovaniSkladZar = value;
                    break;
                case "SkloSkladZar":
                    o.SkloSkladZar = value;
                    break;


                case "DokumentPripravaKrid":
                    o.DokumentPripravaKrid = value;
                    break;
                case "IndocaPripravaKrid":
                    o.IndocaPripravaKrid = value;
                    break;
                case "PohLaserKrid":
                    o.PohLaserKrid = value;
                    break;
                case "NepohLaserKrid":
                    o.NepohLaserKrid = value;
                    break;
                case "KlapackaLaserKrid":
                    o.KlapackaLaserKrid = value;
                    break;
                case "OkopLaserKrid":
                    o.OkopLaserKrid = value;
                    break;
                case "PohOhybackaKrid":
                    o.PohOhybackaKrid = value;
                    break;
                case "NepohOhybackaKrid":
                    o.NepohOhybackaKrid = value;
                    break;
                case "KlapackaOhybackaKrid":
                    o.KlapackaOhybackaKrid = value;
                    break;
                case "SplnenoBodovaniKrid":
                    o.SplnenoBodovaniKrid = value;
                    break;
                case "NarezThermacolKrid":
                    o.NarezThermacolKrid = value;
                    break;
                case "LepeniThermacolKrid":
                    o.LepeniThermacolKrid = value;
                    break;
                case "CncThermacolKrid":
                    o.CncThermacolKrid = value;
                    break;
                case "SplnenoLepeniKrid":
                    o.SplnenoLepeniKrid = value;
                    break;
                case "SplnenoStrikarnaKrid":
                    o.SplnenoStrikarnaKrid = value;
                    break;
                case "NarezRameckyKrid":
                    o.NarezRameckyKrid = value;
                    break;
                case "FrezRameckyKrid":
                    o.FrezRameckyKrid = value;
                    break;
                case "SvarRameckyKrid":
                    o.SvarRameckyKrid = value;
                    break;
                case "StriRameckyKrid":
                    o.StriRameckyKrid = value;
                    break;
                case "SplnenoKompletaceKrid":
                    o.SplnenoKompletaceKrid = value;
                    break;
                case "PlechSkladKrid":
                    o.PlechSkladKrid = value;
                    break;
                case "KovaniSkladKrid":
                    o.KovaniSkladKrid = value;
                    break;
                case "BarvaSkladKrid":
                    o.BarvaSkladKrid = value;
                    break;
                case "SkloSkladKrid":
                    o.SkloSkladKrid = value;
                    break;
            }

            if (activeColor != null)
            {
                switch (activePopupName)
                {

                    case "DokumentPripravaZar":
                        o.DokumentPripravaZarC = activeColor;
                        break;
                    case "InDocaPripravaZar":
                        o.InDocaPripravaZarC = activeColor;
                        break;
                    case "PohLaserZar":
                        o.PohLaserZarC = activeColor;
                        break;
                    case "NepohLaserZar":
                        o.NepohLaserZarC = activeColor;
                        break;
                    case "PoutecLaserZar":
                        o.PoutecLaserZarC = activeColor;
                        break;
                    case "SplnenoNuzkyZar":
                        o.SplnenoNuzkyZarC = activeColor;
                        break;
                    case "PohOhybackaZar":
                        o.PohOhybackaZarC = activeColor;
                        break;
                    case "NepohOhybackaZar":
                        o.NepohOhybackaZarC = activeColor;
                        break;
                    case "PoutecOhybackaZar":
                        o.PoutecOhybackaZarC = activeColor;
                        break;
                    case "PohPilaZar":
                        o.PohPilaZarC = activeColor;
                        break;
                    case "NepohPilaZar":
                        o.NepohPilaZarC = activeColor;
                        break;
                    case "SplnenoVyrazeckaZar":
                        o.SplnenoVyrazeckaZarC = activeColor;
                        break;
                    case "SplnenoBodovaniZar":
                        o.SplnenoBodovaniZarC = activeColor;
                        break;
                    case "SplnenoSvarovaniZar":
                        o.SplnenoSvarovaniZarC = activeColor;
                        break;
                    case "SplnenoBrouseniZar":
                        o.SplnenoBrouseniZarC = activeColor;
                        break;
                    case "SplnenoStrikarnaZar":
                        o.SplnenoStrikarnaZarC = activeColor;
                        break;
                    case "NarezRameckyZar":
                        o.NarezRameckyZarC = activeColor;
                        break;
                    case "FrezRameckyZar":
                        o.FrezRameckyZarC = activeColor;
                        break;
                    case "SvarRameckyZar":
                        o.SvarRameckyZarC = activeColor;
                        break;
                    case "StriRameckyZar":
                        o.StriRameckyZarC = activeColor;
                        break;
                    case "SplnenoKompletaceZar":
                        o.SplnenoKompletaceZarC = activeColor;
                        break;
                    case "PlechSkladZar":
                        o.PlechSkladZarC = activeColor;
                        break;
                    case "BarvaSkladZar":
                        o.BarvaSkladZarC = activeColor;
                        break;
                    case "KovaniSkladZar":
                        o.KovaniSkladZarC = activeColor;
                        break;
                    case "SkloSkladZar":
                        o.SkloSkladZarC = activeColor;
                        break;


                    case "DokumentPripravaKrid":
                        o.DokumentPripravaKridC = activeColor;
                        break;
                    case "IndocaPripravaKrid":
                        o.IndocaPripravaKridC = activeColor;
                        break;
                    case "PohLaserKrid":
                        o.PohLaserKridC = activeColor;
                        break;
                    case "NepohLaserKrid":
                        o.NepohLaserKridC = activeColor;
                        break;
                    case "KlapackaLaserKrid":
                        o.KlapackaLaserKridC = activeColor;
                        break;
                    case "OkopLaserKrid":
                        o.OkopLaserKridC = activeColor;
                        break;
                    case "PohOhybackaKrid":
                        o.PohOhybackaKridC = activeColor;
                        break;
                    case "NepohOhybackaKrid":
                        o.NepohOhybackaKridC = activeColor;
                        break;
                    case "KlapackaOhybackaKrid":
                        o.KlapackaOhybackaKridC = activeColor;
                        break;
                    case "SplnenoBodovaniKrid":
                        o.SplnenoBodovaniKridC = activeColor;
                        break;
                    case "NarezThermacolKrid":
                        o.NarezThermacolKridC = activeColor;
                        break;
                    case "LepeniThermacolKrid":
                        o.LepeniThermacolKridC = activeColor;
                        break;
                    case "CncThermacolKrid":
                        o.CncThermacolKridC = activeColor;
                        break;
                    case "SplnenoLepeniKrid":
                        o.SplnenoLepeniKridC = activeColor;
                        break;
                    case "SplnenoStrikarnaKrid":
                        o.SplnenoStrikarnaKridC = activeColor;
                        break;
                    case "NarezRameckyKrid":
                        o.NarezRameckyKridC = activeColor;
                        break;
                    case "FrezRameckyKrid":
                        o.FrezRameckyKridC = activeColor;
                        break;
                    case "SvarRameckyKrid":
                        o.SvarRameckyKridC = activeColor;
                        break;
                    case "StriRameckyKrid":
                        o.StriRameckyKridC = activeColor;
                        break;
                    case "SplnenoKompletaceKrid":
                        o.SplnenoKompletaceKridC = activeColor;
                        break;
                    case "PlechSkladKrid":
                        o.PlechSkladKridC = activeColor;
                        break;
                    case "KovaniSkladKrid":
                        o.KovaniSkladKridC = activeColor;
                        break;
                    case "BarvaSkladKrid":
                        o.BarvaSkladKridC = activeColor;
                        break;
                    case "SkloSkladKrid":
                        o.SkloSkladKridC = activeColor;
                        break;

                }
            }

            o.PripravaZarC = "Yellow";
            o.LaserZarC = "Yellow";
            o.NuzkyZarC = "Yellow";
            o.OhybackaZarC = "Yellow";
            o.PilaZarC = "Yellow";
            o.VyrazeckaZarC = "Yellow";
            o.BodovaniZarC = "Yellow";
            o.SvarovaniZarC = "Yellow";
            o.BrouseniZarC = "Yellow";
            o.StrikarnaZarC = "Yellow";
            o.RameckyZarC = "Yellow";
            o.KompletaceZarC = "Yellow";
            o.SkladZarC = "Yellow";
            o.PripravaKridC = "Yellow";
            o.LaserKridC = "Yellow";
            o.OhybackaKridC = "Yellow";
            o.BodovaniKridC = "Yellow";
            o.ThermacolKridC = "Yellow";
            o.LepeniKridC = "Yellow";
            o.StrikarnaKridC = "Yellow";
            o.RameckyKridC = "Yellow";
            o.KompletaceKridC = "Yellow";
            o.SkladKridC = "Yellow";


            if ((o.InDocaPripravaZar == "✓" || o.InDocaPripravaZar == "✗") && (o.DokumentPripravaZar == "✓" || o.DokumentPripravaZar == "✗"))
            {
                o.PripravaZarC = "Limegreen";
            }
            if ((o.NepohLaserZar == "✓" || o.NepohLaserZar == "✗") && (o.PohLaserZar == "✓" || o.PohLaserZar == "✗") && (o.PoutecLaserZar == "✓" || o.PoutecLaserZar == "✗"))
            {
                o.LaserZarC = "Limegreen";
            }
            if ((o.SplnenoNuzkyZar == "✓" || o.SplnenoNuzkyZar == "✗"))
            {
                o.NuzkyZarC = "Limegreen";
            }
            if ((o.PohOhybackaZar == "✓" || o.PohOhybackaZar == "✗") && (o.PoutecOhybackaZar == "✓" || o.PoutecOhybackaZar == "✗") &&
                (o.NepohOhybackaZar == "✓" || o.NepohOhybackaZar == "✗"))
            {
                o.OhybackaZarC = "Limegreen";
            }
            if ((o.NepohPilaZar == "✓" || o.NepohPilaZar == "✗") && (o.PohPilaZar == "✓" || o.PohPilaZar == "✗"))
            {
                o.PilaZarC = "Limegreen";
            }
            if ((o.SplnenoVyrazeckaZar == "✓" || o.SplnenoVyrazeckaZar == "✗"))
            {
                o.VyrazeckaZarC = "Limegreen";
            }
            if ((o.SplnenoBodovaniZar == "✓" || o.SplnenoBodovaniZar == "✗"))
            {
                o.BodovaniZarC = "Limegreen";
            }
            if ((o.SplnenoSvarovaniZar == "✓" || o.SplnenoSvarovaniZar == "✗"))
            {
                o.SkladC = "Limegreen";
            }
            if ((o.SplnenoBrouseniZar == "✓" || o.SplnenoBrouseniZar == "✗"))
            {
                o.BrouseniZarC = "Limegreen";
            }
            if ((o.SplnenoStrikarnaZar == "✓" || o.SplnenoStrikarnaZar == "✗"))
            {
                o.StrikarnaZarC = "Limegreen";
            }
            if ((o.FrezRameckyZar == "✓" || o.FrezRameckyZar == "✗") && (o.StriRameckyZar == "✓" || o.StriRameckyZar == "✗") &&
                (o.SvarRameckyZar == "✓" || o.SvarRameckyZar == "✗") && (o.NarezRameckyZar == "✓" || o.NarezRameckyZar == "✗"))
            {
                o.RameckyZarC = "Limegreen";
            }
            if ((o.SplnenoKompletaceZar == "✓" || o.SplnenoKompletaceZar == "✗"))
            {
                o.KompletaceZarC = "Limegreen";
            }
            if ((o.PlechSkladZar == "✓" || o.PlechSkladZar == "✗") && (o.BarvaSkladZar == "✓" || o.BarvaSkladZar == "✗") &&
                (o.KovaniSkladZar == "✓" || o.KovaniSkladZar == "✗") && (o.SkloSkladZar == "✓" || o.SkloSkladZar == "✗"))
            {
                o.SkladZarC = "Limegreen";
            }


            if ((o.IndocaPripravaKrid == "✓" || o.IndocaPripravaKrid == "✗") && (o.DokumentPripravaKrid == "✓" || o.DokumentPripravaKrid == "✗"))
            {
                o.PripravaKridC = "Limegreen";
            }
            if ((o.NepohLaserKrid == "✓" || o.NepohLaserKrid == "✗") && (o.KlapackaLaserKrid == "✓" || o.KlapackaLaserKrid == "✗") &&
                (o.PohLaserKrid == "✓" || o.PohLaserKrid == "✗") && (o.OkopLaserKrid == "✓" || o.OkopLaserKrid == "✗"))
            {
                o.LaserKridC = "Limegreen";
            }
            if ((o.NepohOhybackaKrid == "✓" || o.NepohOhybackaKrid == "✗") && (o.PohOhybackaKrid == "✓" || o.PohOhybackaKrid == "✗") &&
                (o.KlapackaOhybackaKrid == "✓" || o.KlapackaOhybackaKrid == "✗"))
            {
                o.OhybackaKridC = "Limegreen";
            }
            if ((o.SplnenoBodovaniKrid == "✓" || o.SplnenoBodovaniKrid == "✗"))
            {
                o.BodovaniKridC = "Limegreen";
            }
            if ((o.NarezThermacolKrid == "✓" || o.NarezThermacolKrid == "✗") && (o.LepeniThermacolKrid == "✓" || o.LepeniThermacolKrid == "✗") &&
                (o.CncThermacolKrid == "✓" || o.CncThermacolKrid == "✗"))
            {
                o.ThermacolKridC = "Limegreen";
            }
            if ((o.SplnenoLepeniKrid == "✓" || o.SplnenoLepeniKrid == "✗"))
            {
                o.LepeniKridC = "Limegreen";
            }
            if ((o.SplnenoStrikarnaKrid == "✓" || o.SplnenoStrikarnaKrid == "✗"))
            {
                o.StrikarnaKridC = "Limegreen";
            }
            if ((o.StriRameckyKrid == "✓" || o.StriRameckyKrid == "✗") && (o.NarezRameckyKrid == "✓" || o.NarezRameckyKrid == "✗") &&
                (o.FrezRameckyKrid == "✓" || o.FrezRameckyKrid == "✗") && (o.SvarRameckyKrid == "✓" || o.SvarRameckyKrid == "✗"))
            {
                o.RameckyKridC = "Limegreen";
            }
            if ((o.SplnenoKompletaceKrid == "✓" || o.SplnenoKompletaceKrid == "✗"))
            {
                o.KompletaceKridC = "Limegreen";
            }
            if ((o.PlechSkladKrid == "✓" || o.PlechSkladKrid == "✗") && (o.SkloSkladKrid == "✓" || o.SkloSkladKrid == "✗") &&
                (o.KovaniSkladKrid == "✓" || o.KovaniSkladKrid == "✗") && (o.BarvaSkladKrid == "✓" || o.BarvaSkladKrid == "✗"))
            {
                o.SkladKridC = "Limegreen";
            }

            db.SaveChanges();
            Popupek.IsOpen = false;
        }

        List<string> columns = new List<string> { "Zahajeni Datum", "Technik", "Zakazka", "Typ zarubne",
            "Poznamky", "Material", "Cislo Zakazky", "Vyrobni Cislo","Soubor", "Upravit soubor", "Kridla",
            "Zarubne", "Priprava", "Nuzky", "Ohybacka", "Pila","Vyrazecka","Bodovani","Svarovani",
            "Brouseni","Strikarna","Ramecky","Kompletace","Sklad","Priprava ","Laser ","Ohybacka ","Bodovani ",
            "Thermacol ","Lepeni ","Strikarna ","Ramecky ","Kompletace ","Sklad ", "Pozadovany Datum",
            "Datum Dokonceni", "Mesto", "Mapa", "Plosny material", "Hotovo bloky"};

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

                case "Typ zarubne":
                    TypZarubne.Visibility = TypZarubne.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;



                case "Nuzky":
                    NuzkyZar.Visibility = NuzkyZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Ohybacka":
                    OhybackaZar.Visibility = OhybackaZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Pila":
                    PilaZar.Visibility = PilaZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Vyrazecka":
                    VyrazeckaZar.Visibility = VyrazeckaZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Bodovani":
                    BodovaniZar.Visibility = BodovaniZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Svarovani":
                    SvarovaniZar.Visibility = SvarovaniZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Brouseni":
                    BrouseniZar.Visibility = SvarovaniZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Strikarna":
                    StrikarnaZar.Visibility = StrikarnaZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

                    break;
                case "Ramecky":
                    RameckyZar.Visibility = RameckyZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;

                case "Kompletace":
                    KompletaceZar.Visibility = KompletaceZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Sklad":
                    SkladZar.Visibility = KompletaceZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Priprava ":
                    PripravaKrid.Visibility = KompletaceZar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Laser ":
                    LaserKrid.Visibility = LaserKrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Ohybacka ":
                    OhybackaKrid.Visibility = LaserKrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Bodovani ":
                    BodovaniKrid.Visibility = BodovaniKrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Thermacol ":
                    ThermacolKrid.Visibility = ThermacolKrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Lepeni ":
                    LepeniKrid.Visibility = LepeniKrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Strikarna ":
                    StrikarnaKrid.Visibility = StrikarnaKrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Ramecky ":
                    RameckyKrid.Visibility = RameckyKrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Kompletace ":
                    KompletaceKrid.Visibility = KompletaceKrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                case "Sklad ":
                    SkladKrid.Visibility = SkladKrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
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
                    Adresa.Visibility = Adresa.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

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
                    List<DataGridLength> lists = new List<DataGridLength>();

                    SteelDatagrid.Columns.ToList().ForEach(x => lists.Add(x.ActualWidth));

                    if (lists.Count != 0)
                    {
                        string fileName = System.IO.Path.Combine(cesta, "steelWidths.json");

                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        var jsonString = JsonConvert.SerializeObject(lists);
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
