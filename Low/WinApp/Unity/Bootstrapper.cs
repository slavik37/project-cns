using System.ComponentModel;
using System.Windows;
using MemOrg.WinApp.Avalon;

namespace WinApp.Unity
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(Interfaces.RegionNames.MainViewRegion, typeof(MemOrg.WinApp.MainView.MainView));
            regionManager.RegisterViewWithRegion(Interfaces.RegionNames.TempToolbarRegion, typeof(TempToolbar.ContentView));
            
            Container.RegisterType<ITmpXmlExportImportService, TmpXmlExportImportService.TmpXmlExportImportService>();

            return Container.Resolve<Shell.Shell>();
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
            AddModule<GraphServiceModule>();
            AddModule<GraphDrawServiceModule>();
            AddModule<GraphOrganizeServiceModule>();
            AddModule<GraphVisualizeServiceModule>();
            AddModule<GraphViewerModule>();
            AddModule<GraphManagementServiceModule>();
            AddModule<TempToolbarModule>();
            AddModule<ChapterViewerModule>();
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
