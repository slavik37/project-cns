using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;

namespace MemOrg.WinApp.MainView
{
    public class MainViewViewModel : ViewModelBase
    {
        private readonly ITmpXmlExportImportService _exportImportService;

        public MainViewViewModel(ITmpXmlExportImportService exportImportService)
        {
            _exportImportService = exportImportService;
            ExportCommand = new DelegateCommand(ExecuteExportCommand, () => true);
            ImportCommand = new DelegateCommand(ExecuteImportCommand, () => true);
            ClearCommand = new DelegateCommand(ExecuteClearCommand, () => true);
        }

        public DelegateCommand ExportCommand { get; set; }
        public DelegateCommand ImportCommand { get; set; }
        public DelegateCommand ClearCommand { get; set; }

        private void ExecuteExportCommand()
        {
           _exportImportService.SaveGraph(); 
        }

        private void ExecuteClearCommand()
        {
            var graphService = (IGraphService)ServiceLocator.Current.GetService(typeof(IGraphService));
            graphService.ClearGraph();
            GlobalCommands.RefreshGraphViewCompositeCommand.Execute(null);
        }
        
        private void ExecuteImportCommand()
        {
            _exportImportService.LoadGraph();
            GlobalCommands.RefreshGraphViewCompositeCommand.Execute(null);
        }
    }
}
