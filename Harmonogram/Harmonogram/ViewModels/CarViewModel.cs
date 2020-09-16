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
    class CarViewModel : INotifyPropertyChanged
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

        private ObservableCollection<Car> _cars;
        public ObservableCollection<Car> Cars
        {
            get { return _cars; }
            private set
            {
                if (value == _cars)
                    return;

                _cars = value;
                OnPropertyChanged();
            }
        }

        public CarViewModel()
        {
            _cars = new ObservableCollection<Car>();
            db.Cars.ToList().ForEach(p => _cars.Add(p));

        }
    }
}
