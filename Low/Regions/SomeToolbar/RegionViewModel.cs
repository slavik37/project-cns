using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using SomeToolbar.Annotations;

namespace SomeToolbar
{
    public class RegionViewModel : INotifyPropertyChanged
    {
        public RegionViewModel()
        {
            DoStepCommand = new DelegateCommand(() => World.DoStep());
        }
        
        public DelegateCommand DoStepCommand { get; set; }
        
        private IWorldService _world;
        private IWorldService World
        {
            get
            {
                if (_world != null)
                    return _world;

                _world =
                    (IWorldService)
                        ServiceLocator.Current.GetService(typeof(IWorldService));
                return _world;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
