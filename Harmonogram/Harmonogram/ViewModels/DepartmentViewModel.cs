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
    class DepartmentViewModel : INotifyPropertyChanged
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

        private ObservableCollection<Department> _departments;
        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            private set
            {
                if (value == _departments)
                    return;

                _departments = value;
                OnPropertyChanged();
            }
        }

        public DepartmentViewModel()
        {
            _departments = new ObservableCollection<Department>();
            db.Departments.ToList().ForEach(p => _departments.Add(p));

        }
    }
}
