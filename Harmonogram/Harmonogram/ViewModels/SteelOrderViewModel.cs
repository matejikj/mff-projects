using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.ViewModels
{
    class SteelOrderViewModel
    {
        HarmonogramDBEntities db = new HarmonogramDBEntities();

        #region INotifyPropertyChanged Impl
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            private set
            {
                if (value == _orders)
                    return;

                _orders = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Order> _new_orders;
        public ObservableCollection<Order> NewOrders
        {
            get { return _new_orders; }
            private set
            {
                if (value == _new_orders)
                    return;

                _new_orders = value;
                OnPropertyChanged();
            }
        }

        private Order curr_order;
        public Order Current_order
        {
            get { return curr_order; }
            set
            {
                if (value == curr_order)
                    return;

                Current_new_order = null;

                curr_order = value;
                if (curr_order != null)
                {
                    Console.WriteLine(curr_order.OrderId);

                }
                OnPropertyChanged();
            }
        }

        private Order curr_new_order;
        public Order Current_new_order
        {
            get { return curr_new_order; }
            set
            {
                if (value == curr_new_order)
                    return;

                curr_order = null;

                curr_new_order = value;
                if (curr_new_order != null)
                {
                    Console.WriteLine(curr_new_order.OrderId);

                }
                OnPropertyChanged();
            }
        }

        public SteelOrderViewModel()
        {
            _orders = new ObservableCollection<Order>();
            _orders.CollectionChanged += order_CollectionChanged;
            db.Orders.Where(p => p.IsInProccess == true && p.IsDeleted == false && p.IsArchived == false && p.Department == 3).OrderBy(p => p.SortNr).ToList().ForEach(p => _orders.Add(p));

            _new_orders = new ObservableCollection<Order>();
            _new_orders.CollectionChanged += order_CollectionChanged;
            db.Orders.Where(p => p.IsInProccess == false && p.IsDeleted == false && p.IsArchived == false && p.Department == 3).ToList().ForEach(p => _new_orders.Add(p));

        }

        private void order_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (INotifyPropertyChanged item in e.NewItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged += order_PropertyChanged;
                }
            }
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (INotifyPropertyChanged item in e.OldItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged -= order_PropertyChanged;
                }
            }
        }
        //Property Changed will be called whenever a property of one of the 'Person'
        //objects is changed.
        private void order_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SortNr")
            {

            }
            else
            {
                Order o = sender as Order;
                Order order = db.Orders.Find(o.OrderId);
                switch (e.PropertyName)
                {
                    case "Poznamky":
                        if (order.Poznamky != o.Poznamky)
                        {
                            order.Poznamky = o.Poznamky;
                        }
                        break;
                    case "Technik":
                        if (order.Technik != o.Technik)
                        {
                            order.Technik = o.Technik;
                        }
                        break;
                    case "TypZarubne":
                        if (order.TypZarubne != o.TypZarubne)
                        {
                            order.TypZarubne = o.TypZarubne;
                        }
                        break;
                    case "Zakazka":
                        if (order.Zakazka != o.Zakazka)
                        {
                            order.Zakazka = o.Zakazka;
                        }
                        break;
                    case "Material":
                        if (order.Material != o.Material)
                        {
                            order.Material = o.Material;
                        }
                        break;
                    case "ZakazkaNr":
                        if (order.ZakazkaNr != o.ZakazkaNr)
                        {
                            order.ZakazkaNr = o.ZakazkaNr;
                        }
                        break;
                    case "VyrobniNr":
                        if (order.VyrobniNr != o.VyrobniNr)
                        {
                            order.VyrobniNr = o.VyrobniNr;
                        }
                        break;
                    case "DocUrl":
                        if (order.DocUrl != o.DocUrl)
                        {
                            order.DocUrl = o.DocUrl;
                        }
                        break;
                    case "KridlaKs":
                        if (order.KridlaKs != o.KridlaKs)
                        {
                            order.KridlaKs = o.KridlaKs;
                        }
                        break;
                    case "KridlaKsExpedovanych":
                        if (order.KridlaKsExpedovanych != o.KridlaKsExpedovanych)
                        {
                            order.KridlaKsExpedovanych = o.KridlaKsExpedovanych;
                        }
                        break;
                    case "KridlaText":
                        if (order.KridlaText != o.KridlaText)
                        {
                            order.KridlaText = o.KridlaText;
                        }
                        break;
                    case "ZarubneKs":
                        if (order.ZarubneKs != o.ZarubneKs)
                        {
                            order.ZarubneKs = o.ZarubneKs;
                        }
                        break;
                    case "ZarubneKsExpedovanych":
                        if (order.ZarubneKsExpedovanych != o.ZarubneKsExpedovanych)
                        {
                            order.ZarubneKsExpedovanych = o.ZarubneKsExpedovanych;
                        }
                        break;
                    case "ZarubneText":
                        if (order.ZarubneText != o.ZarubneText)
                        {
                            order.ZarubneText = o.ZarubneText;
                        }
                        break;
                    case "Mesto":
                        if (order.Mesto != o.Mesto)
                        {
                            order.Mesto = o.Mesto;
                        }
                        break;
                    case "PlosnyMaterial":
                        if (order.PlosnyMaterial != o.PlosnyMaterial)
                        {
                            order.PlosnyMaterial = o.PlosnyMaterial;
                        }
                        break;
                    case "HotovoBloky":
                        if (order.HotovoBloky != o.HotovoBloky)
                        {
                            order.HotovoBloky = o.HotovoBloky;
                        }
                        break;
                    case "Ulice":
                        if (order.Ulice != o.Ulice)
                        {
                            order.Ulice = o.Ulice;
                        }
                        break;
                    case "PSC":
                        if (order.PSC != o.PSC)
                        {
                            order.PSC = o.PSC;
                        }
                        break;
                    case "Cislo":
                        if (order.Cislo != o.Cislo)
                        {
                            order.Cislo = o.Cislo;
                        }
                        break;

                }
                db.SaveChanges();
            }
        }
    }
}
