using Harmonogram.Helper;
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
    class ExpeditionViewModel : INotifyPropertyChanged
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

        private Expedition curr_expedition;
        public Expedition Current_expedition
        {
            get { return curr_expedition; }
            set
            {
                if (value == curr_expedition)
                    return;

                curr_expedition = value;
                if (curr_expedition != null)
                {
                    Console.WriteLine(curr_expedition.OrderId);

                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Expedition> _expeditions;
        public ObservableCollection<Expedition> Expeditions
        {
            get { return _expeditions; }
            private set
            {
                if (value == _expeditions)
                    return;

                _expeditions = value;
                OnPropertyChanged();
            }
        }

        public ExpeditionViewModel()
        {
            _expeditions = new ObservableCollection<Expedition>();
            _expeditions.CollectionChanged += expedition_CollectionChanged;
            var orders = db.Expeditions.OrderBy(x => x.ExpeditedSortNr).ToList();
            foreach ( Expedition e in orders )
            {
                Expedition expedition = e;
                _expeditions.Add(expedition);
            }
        }

        private void expedition_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (INotifyPropertyChanged item in e.NewItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged += expedition_PropertyChanged;
                }
            }
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (INotifyPropertyChanged item in e.OldItems.OfType<INotifyPropertyChanged>())
                {
                    item.PropertyChanged -= expedition_PropertyChanged;
                }
            }
        }
        //Property Changed will be called whenever a property of one of the 'Person'
        //objects is changed.
        private void expedition_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var row = sender as Expedition;
            switch (e.PropertyName)
            {
                case "KridlaKsExpedovanych":
                    handleKridelKs(row);
                    break;

                case "ZarubneKsExpedovanych":
                    handleZarubneKs(row);
                    break; 

                default:
                    SaveData(row);
                    break;
            }
        }

        private void SaveData(Expedition row)
        {
            Expedition e = db.Expeditions.Find(row.ExpeditionId);
            e = row;
            db.SaveChanges();

            //Save the row to the database here.
        }

        private void handleZarubneKs(Expedition row)
        {
            Expedition e = db.Expeditions.Where(p => p.ExpeditionId == row.ExpeditionId).FirstOrDefault();
            int kontrolanove;
            int.TryParse(row.ZarubneKsExpedovanych, out kontrolanove);
            int kontrolastare;
            int.TryParse(e.ZarubneKsZbyvaExpedovat, out kontrolastare);
            if (kontrolanove > kontrolastare)
            {
                //zobraz varovani ze jsi zadal spatnou hodnotu a vrat starou
                row.ZarubneKsExpedovanych = e.ZarubneKsExpedovanych;
            }
            else
            {
                int p;
                if (int.TryParse(row.ZarubneKsExpedovanych, out p))
                {
                    var expeds = db.Expeditions.Where(o => !o.Order.IsDeleted && o.OrderId == e.OrderId).ToList();
                    int i = 0;
                    foreach (Expedition expedition in expeds)
                    {
                        int ks = 0;
                        if (int.TryParse(expedition.ZarubneKsExpedovanych, out ks))
                        {
                            i = i + ks;
                        }
                    }
                    var updateOrder = db.Orders.Where(o => !o.IsDeleted && o.OrderId == e.OrderId).FirstOrDefault();
                    updateOrder.ZarubneKsExpedovanych = i.ToString();
                    int b = 0;
                    if (int.TryParse(updateOrder.ZarubneKs, out b))
                    {
                        updateOrder.ZarubneText = (b - i).ToString();
                    }
                    db.SaveChanges();
                }
                else
                {
                    row.ZarubneKsExpedovanych = e.ZarubneKsExpedovanych;
                    //zobraz ze nebylo zadano cislo
                }
            }
        }

        private void handleKridelKs(Expedition row)
        {
            Expedition e = db.Expeditions.Where(p => p.ExpeditionId == row.ExpeditionId).FirstOrDefault();
            int kontrolanove;
            int.TryParse(row.KridlaKsExpedovanych, out kontrolanove);
            int kontrolastare;
            int.TryParse(e.KridlaKsZbyvaExpedovat, out kontrolastare);
            if (kontrolanove > kontrolastare)
            {
                //zobraz varovani ze jsi zadal spatnou hodnotu a vrat starou
                row.KridlaKsExpedovanych = e.KridlaKsExpedovanych;
            }
            else
            {
                int p;
                if (int.TryParse(row.KridlaKsExpedovanych, out p))
                {
                    var expeds = db.Expeditions.Where(o => !o.Order.IsDeleted && o.OrderId == e.OrderId).ToList();
                    int i = 0;
                    foreach (Expedition expedition in expeds)
                    {
                        int ks = 0;
                        if (int.TryParse(expedition.KridlaKsExpedovanych, out ks))
                        {
                            i = i + ks;
                        }
                    }
                    var updateOrder = db.Orders.Where(o => !o.IsDeleted && o.OrderId == e.OrderId).FirstOrDefault();
                    updateOrder.KridlaKsExpedovanych = i.ToString();
                    int b = 0;
                    if (int.TryParse(updateOrder.KridlaKs, out b))
                    {
                        updateOrder.KridlaText = (b - i).ToString();
                    }
                    db.SaveChanges();
                }
                else
                {
                    row.KridlaKsExpedovanych = e.KridlaKsExpedovanych;
                    //zobraz ze nebylo zadano cislo
                }
            }
        }
    }
}
