using System.ComponentModel;
using System.Windows;
using MemOrg.WinApp.Avalon;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using WinApp.Avalon;

namespace WinApp.Unity
{
    public class MyBootstrapper : Microsoft.Practices.Prism.Bootstrapper
    {
        public override void Run(bool runWithDefaultConfiguration)
        {
            throw new System.NotImplementedException();
        }

        protected override DependencyObject CreateShell()
        {
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Interfaces.RegionNames.MainViewRegion, typeof(MainView.MainView));
            regionManager.RegisterViewWithRegion(Interfaces.RegionNames.TempToolbarRegion, typeof(TempToolbar.ContentView));
            
            Container.RegisterType<ITmpXmlExportImportService, TmpXmlExportImportService.TmpXmlExportImportService>();

            return Container.Resolve<Shell.Shell>();
        }

        protected override void ConfigureServiceLocator()
        {
            throw new System.NotImplementedException();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            
            Application.Current.MainWindow = (Window) Shell.Shell;
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
            //AddModule<GraphServiceModule>();
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
