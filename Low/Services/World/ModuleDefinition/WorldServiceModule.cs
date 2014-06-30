using Interfaces;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace WorldService.ModuleDefinition
{
    public class WorldServiceModule : IModule
    {
        private readonly IUnityContainer _container;

        public WorldServiceModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterInstance(typeof(IWorldService), new WorldService());
        }
    }
}
