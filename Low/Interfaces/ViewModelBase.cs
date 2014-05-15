using System.ComponentModel;

namespace Interfaces
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public virtual void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged == null) return;
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
