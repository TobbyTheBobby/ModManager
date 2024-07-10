using Bindito.Core;
using ModManager.AddonInstallerSystem;
using ModManager.AddonSystem;
using ModManager.ManifestLocationFinderSystem;
using ModManager.MapSystem;
using ModManager.ModSystem;

namespace ModManager.StartupSystem
{
    [Context("MainMenu")]
    public class ModManagerConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<RemoveFilesOnStartup>().AsSingleton();
            
            containerDefinition.Bind<InstalledAddonRepository>().AsSingleton();
            containerDefinition.Bind<AddonInstallerService>().AsSingleton();
            containerDefinition.Bind<AddonService>().AsSingleton();
            
            containerDefinition.MultiBind<IManifestLocationFinder>().To<MapManifestFinder>().AsSingleton();
            containerDefinition.Bind<MapInstaller>().AsSingleton();
            containerDefinition.Bind<MapRegisterer>().AsSingleton();
            
            containerDefinition.MultiBind<IManifestLocationFinder>().To<ModManifestFinder>().AsSingleton();
            containerDefinition.Bind<ModInstaller>().AsSingleton();
            containerDefinition.Bind<ModRegisterer>().AsSingleton();
            
            containerDefinition.Bind<ManifestLocationFinderService>().AsSingleton();
            containerDefinition.Bind<ManifestLocationFinderRegistry>().AsSingleton();
        }
    }
}