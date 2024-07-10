using ModManager.AddonInstallerSystem;
using ModManager.ExtractorSystem;
using ModManager.ManifestValidatorSystem;
using Timberborn.SingletonSystem;

namespace ModManager.MapSystem
{
    public class MapRegisterer : ILoadableSingleton
    {
        public const string RegistryId = "Map";

        private readonly MapInstaller _mapInstaller;

        public MapRegisterer(MapInstaller mapInstaller)
        {
            _mapInstaller = mapInstaller;
        }
        
        public void Load()
        {
            AddonInstallerRegistry.Instance.Add(RegistryId, _mapInstaller);
            AddonExtractorRegistry.Instance.Add(RegistryId, new MapExtractor());
            ManifestValidatorRegistry.Instance.Add(RegistryId, new MapManifestValidator());
        }
    }
}