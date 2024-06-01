using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TouchGame
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyAllPropertiesChanged()
        {
            NotifyPropertyChanged(null);
        }

        protected bool SetProperty<T>(
            ref T backingStore,
            T value,
            [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            NotifyPropertyChanged(propertyName);

            return true;
        }

        protected bool SetProperty<T>(
           ref T backingStore,
           T value,
           [CallerMemberName] string propertyName = "",
           params string[] dependencies)
        {
            if (!SetProperty(ref backingStore, value, propertyName))
            {
                return false;
            }

            if (dependencies != null && dependencies.Length > 0)
            {
                for (var i = 0; i < dependencies.Length; i++)
                {
                    NotifyPropertyChanged(dependencies[i]);
                }
            }

            return true;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}