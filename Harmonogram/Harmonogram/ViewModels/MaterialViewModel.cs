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
    class MaterialViewModel : INotifyPropertyChanged
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

        private ObservableCollection<Material> _materials;
        public ObservableCollection<Material> Materials
        {
            get { return _materials; }
            private set
            {
                if (value == _materials)
                    return;

                _materials = value;
                OnPropertyChanged();
            }
        }

        public MaterialViewModel()
        {
            _materials = new ObservableCollection<Material>();
            db.Materials.ToList().ForEach(p => _materials.Add(p));

        }
    }
}
