using Modio.Models;
using ModManager.AddonSystem;
using Timberborn.Modding;
using Mod = Modio.Models.Mod;

namespace ModManager.AddonInstallerSystem
{
    public class AddonInstallerService
    {
        private readonly AddonInstallerRegistry _addonInstallerRegistry = AddonInstallerRegistry.Instance;

        private readonly ModRepository _modRepository;

        public AddonInstallerService(ModRepository modRepository)
        {
            _modRepository = modRepository;
        }

        public void Install(Mod mod, string zipLocation)
        {
            foreach (var installer in _addonInstallerRegistry.GetAddonInstallers())
            {
                if (installer.Install(mod, zipLocation))
                {
                    _modRepository.Load();
                    
                    return;
                }
            }

            throw new AddonInstallerException($"{mod.Name} could not be installed by any installer");
        }

        public void Uninstall(ModManagerManifest modManagerManifest)
        {
            foreach (var installer in _addonInstallerRegistry.GetAddonInstallers())
            {
                if (installer.Uninstall(modManagerManifest))
                {
                    _modRepository.Load();
                    
                    return;
                }
            }

            throw new AddonInstallerException($"{modManagerManifest.ModName} could not be uninstalled by any installer");
        }

        public void ChangeVersion(Mod mod, File file, string zipLocation)
        {
            foreach (var installer in _addonInstallerRegistry.GetAddonInstallers())
            {
                if (installer.ChangeVersion(mod, file, zipLocation))
                {
                    _modRepository.Load();
                    
                    return;
                }
            }

            throw new AddonInstallerException($"The version of {mod.Name} could not be changed by any installer");
        }
    }
}