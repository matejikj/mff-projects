using Harmonogram.Helper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Harmonogram.ViewModels
{
    class UserViewModel : INotifyPropertyChanged
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

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            private set
            {
                if (value == _users)
                    return;

                _users = value;
                OnPropertyChanged();
            }
        }

        public UserViewModel()
        {
            _users = new ObservableCollection<User>();
            db.Users.ToList().ForEach(p => _users.Add(p)); 
        }
    }
}
