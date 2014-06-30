using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace OutputScreen.ModuleDefinition
{
    public class OutputScreenModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;

        public OutputScreenModule(IRegionViewRegistry regionViewRegistry)
        {
            _regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.OutputScreenRegion, typeof(RegionView));
        }
    }
}
