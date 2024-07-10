using ModManager.AddonInstallerSystem;
using ModManager.ExtractorSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ModManager.ModSystem
{
    public class ModRegisterer : ILoadableSingleton
    {
        public const string RegistryId = "Mod";

        private readonly ModInstaller _modInstaller;

        public ModRegisterer(ModInstaller modInstaller)
        {
            _modInstaller = modInstaller;
        }

        public void Load()
        {
            AddonInstallerRegistry.Instance.Add(RegistryId, _modInstaller);
            AddonExtractorRegistry.Instance.Add(RegistryId, new ModExtractor());
        }
    }
}