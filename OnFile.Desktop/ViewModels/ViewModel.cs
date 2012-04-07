using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OnFile.Desktop.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetField<T>(ref T field, T value, params string[] propertyNames)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            propertyNames.ToList().ForEach(OnPropertyChanged);
            return true;
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }
    }
}