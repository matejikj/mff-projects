using Harmonogram.UserControls;
using Harmonogram.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace Harmonogram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int actualDep;
        private bool printVisible;

        public MainWindow()
        {
            actualDep = 0;
            printVisible = false;
            MaterialViewModel mvm = new MaterialViewModel();
            StaticResources.Materials = new List<string>();
            mvm.Materials.ToList().ForEach(p => StaticResources.Materials.Add(p.Name));
            CarViewModel cvm = new CarViewModel();
            StaticResources.Cars = new List<string>();
            cvm.Cars.ToList().ForEach(p => StaticResources.Cars.Add(p.Name));
            DepartmentViewModel dvm = new DepartmentViewModel();
            StaticResources.Departments = new List<string>();
            StaticResources.Departments.Add("Drevo");
            StaticResources.Departments.Add("Ocel");
            StaticResources.Departments.Add("Hlinik");


            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        
        private void ExpediceMenuItem_click(object sender, RoutedEventArgs e)
        {
            printVisible = true;
            actualDep = 4;


            contentCtrl.Content = new ExpeditionUserControl();

        }

        private void ArchiveMenuItem_click(object sender, RoutedEventArgs e)
        {
            printVisible = true;
            contentCtrl.Content = new ArchiveUserControl();

        }

        private void HistoryMenuItem_click(object sender, RoutedEventArgs e)
        {
            printVisible = false;
            contentCtrl.Content = new HistoryUserControl();

        }

        private void AluminiumMenuItem_click(object sender, RoutedEventArgs e)
        {
            printVisible = true;
            actualDep = 2;
            contentCtrl.Content = new AluminiumUserControl();
        }

        private void SteelMenuItem_click(object sender, RoutedEventArgs e)
        {
            printVisible = true;
            actualDep = 3;
            contentCtrl.Content = new SteelUserControl();
        }

        private void WoodMenuItem_click(object sender, RoutedEventArgs e)
        {
            printVisible = true;
            actualDep = 1;
            contentCtrl.Content = new WoodUserControl();

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string cesta = "";
            try
            {
                cesta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "harmonogramlignis");
                if (!Directory.Exists(cesta))
                {
                    Directory.CreateDirectory(cesta);
                }
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku {0}, zkontrolujte prosím svá oprávnění.", cesta);
            }


            switch (actualDep)
            {
                case 1:
                    var a = contentCtrl.Content as WoodUserControl;
                    List<DataGridLength> listw = new List<DataGridLength>();

                    a.WoodDatagrid.Columns.ToList().ForEach(x => listw.Add(x.ActualWidth));

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

                    break;
                case 2:
                    var b = contentCtrl.Content as AluminiumUserControl;
                    List<DataGridLength> lista = new List<DataGridLength>();

                    b.AluminiumDatagrid.Columns.ToList().ForEach(x => lista.Add(x.ActualWidth));

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

                    break;
                case 3:
                    var c = contentCtrl.Content as SteelUserControl;
                    List<DataGridLength> lists = new List<DataGridLength>();

                    c.SteelDatagrid.Columns.ToList().ForEach(x => lists.Add(x.ActualWidth) );

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


                    break;
            }
        }
    }
}
