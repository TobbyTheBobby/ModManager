using Bindito.Core;
using ModManager.AddonInstallerSystem;
using ModManager.AddonSystem;
using ModManager.ExtractorSystem;
using ModManager.ManifestLocationFinderSystem;
using ModManager.MapSystem;
using ModManager.ModManagerSystem;
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
            containerDefinition.Bind<AddonExtractorService>().AsSingleton();
            containerDefinition.Bind<AddonService>().AsSingleton();
            
            containerDefinition.MultiBind<IManifestLocationFinder>().To<MapManifestFinder>().AsSingleton();
            containerDefinition.MultiBind<IAddonInstaller>().To<MapInstaller>().AsSingleton();
            containerDefinition.MultiBind<IAddonExtractor>().To<MapExtractor>().AsSingleton();
            
            containerDefinition.MultiBind<IManifestLocationFinder>().To<ModManifestFinder>().AsSingleton();
            containerDefinition.MultiBind<IAddonInstaller>().To<ModInstaller>().AsSingleton();
            containerDefinition.MultiBind<IAddonExtractor>().To<ModExtractor>().AsSingleton();
            
            containerDefinition.MultiBind<IAddonExtractor>().To<ModManagerExtractor>().AsSingleton();
            
            containerDefinition.Bind<ManifestLocationFinderService>().AsSingleton();
            containerDefinition.Bind<ManifestLocationFinderRegistry>().AsSingleton();
        }
    }
}