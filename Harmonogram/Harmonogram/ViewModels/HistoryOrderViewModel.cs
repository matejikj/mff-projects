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
    class HistoryOrderViewModel : INotifyPropertyChanged
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

        private Order curr_order;
        public Order Current_order
        {
            get { return curr_order; }
            set
            {
                if (value == curr_order)
                    return;

                curr_order = value;
                if (curr_order != null)
                {
                    Console.WriteLine(curr_order.OrderId);

                }
                OnPropertyChanged();
            }
        }

        public HistoryOrderViewModel()
        {
            _orders = new ObservableCollection<Order>();
            db.Orders.Where(p => p.IsDeleted == true).ToList().ForEach(p => _orders.Add(p));
        }
    }
}
