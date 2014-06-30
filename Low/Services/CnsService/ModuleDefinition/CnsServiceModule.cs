using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace CnsService.ModuleDefinition
{
    public class CnsServiceModule : IModule
    {
        private readonly IUnityContainer _container;

        public CnsServiceModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ICnsService, CnsService>();
        }
    }
}
