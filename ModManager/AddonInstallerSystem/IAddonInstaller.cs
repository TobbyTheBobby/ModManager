using Modio.Models;
using ModManager.AddonSystem;

namespace ModManager.AddonInstallerSystem
{
    public interface IAddonInstaller
    {
        public bool Install(Mod mod, string zipLocation);

        public bool Uninstall(ModManagerManifest modManagerManifest);

        public bool ChangeVersion(Mod mod, File file, string zipLocation);
    }
}