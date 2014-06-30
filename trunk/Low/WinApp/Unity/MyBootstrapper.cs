using System.ComponentModel;
using System.Windows;
using CnsService.ModuleDefinition;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using OutputScreen.ModuleDefinition;
using SomeToolbar.ModuleDefinition;
using WinApp.Avalon;
using WorldService.ModuleDefinition;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace WinApp.Unity
{
    public class MyBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Interfaces.RegionNames.MainViewRegion, typeof(MainView.MainView));
            
            return Container.Resolve<Shell.Shell>();
        }
        
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
        }

        RegionAdapterMappings _mappings;
        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            _mappings = base.ConfigureRegionAdapterMappings();

            _mappings.RegisterMapping(typeof(LayoutAnchorable), new AnchorableRegionAdapter(ServiceLocator.Current.GetInstance<RegionBehaviorFactory>()));
            _mappings.RegisterMapping(typeof(LayoutDocument), new DocumentRegionAdapter(ServiceLocator.Current.GetInstance<RegionBehaviorFactory>()));
            _mappings.RegisterMapping(typeof(DockingManager), new DockingManagerRegionAdapter(ServiceLocator.Current.GetInstance<RegionBehaviorFactory>()));

            return _mappings;
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var factory = base.ConfigureDefaultRegionBehaviors();
            return factory;
        }

        protected override void ConfigureModuleCatalog()
        {
            AddModule<CnsServiceModule>();
            AddModule<WorldServiceModule>();
            AddModule<SomeToolbarModule>();
            AddModule<OutputScreenModule>();
        }

        private void AddModule<T>() where T : IModule
        {
            var moduleType = typeof(T);
            ModuleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = moduleType.Name,
                ModuleType = moduleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });
        }
    }
}
