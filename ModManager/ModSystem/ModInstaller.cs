using System;
using System.IO;
using System.Linq;
using ModManager.AddonInstallerSystem;
using ModManager.AddonSystem;
using ModManager.ExtractorSystem;
using ModManager.MapSystem;
using ModManager.PersistenceSystem;
using Mod = Modio.Models.Mod;

namespace ModManager.ModSystem
{
    public class ModInstaller : IAddonInstaller
    {
        private readonly InstalledAddonRepository _installedAddonRepository;
        private readonly AddonExtractorService _addonExtractorService;
        
        private readonly PersistenceService _persistenceService = PersistenceService.Instance;

        public ModInstaller(InstalledAddonRepository installedAddonRepository, AddonExtractorService addonExtractorService)
        {
            _installedAddonRepository = installedAddonRepository;
            _addonExtractorService = addonExtractorService;
        }

        public bool Install(Mod mod, string zipLocation)
        {
            if (!mod.Tags.Any(x => x.Name == "Mod"))
                return false;
            var installLocation = _addonExtractorService.Extract(mod, zipLocation);
            var manifest = new ModManagerManifest(installLocation, mod, mod.Modfile!);
            var modManifestPath = Path.Combine(installLocation, ModManagerManifest.FileName);
            _persistenceService.SaveObject(manifest, modManifestPath);
            _installedAddonRepository.Add(manifest);
            return true;
        }

        public bool Uninstall(ModManagerManifest modManagerManifest)
        {
            if (modManagerManifest is not ModManagerManifest && modManagerManifest is MapModManagerManifest)
                return false;
            _installedAddonRepository.Remove(modManagerManifest.ResourceId);

            var modDirInfo = new DirectoryInfo(Path.Combine(modManagerManifest.RootPath));
            var modSubFolders = modDirInfo.GetDirectories("*", SearchOption.AllDirectories);
            foreach (var subDirectory in modSubFolders.Reverse())
            {
                DeleteFilesFromFolder(subDirectory);
                TryDeleteFolder(subDirectory);
            }

            DeleteFilesFromFolder(modDirInfo);
            TryDeleteFolder(modDirInfo);
            return true;
        }

        public bool ChangeVersion(Mod mod, Modio.Models.File file, string zipLocation)
        {
            if (!mod.Tags.Any(x => x.Name == "Mod"))
                return false;
            mod.Modfile = file;
            var installLocation = _addonExtractorService.Extract(mod, zipLocation);
            var manifest = new ModManagerManifest(installLocation, mod, mod.Modfile);
            var modManifestPath = Path.Combine(installLocation, ModManagerManifest.FileName);
            _persistenceService.SaveObject(manifest, modManifestPath);

            _installedAddonRepository.Remove(mod.Id);
            _installedAddonRepository.Add(manifest);

            return true;
        }


        private void DeleteFilesFromFolder(DirectoryInfo directoryInfo)
        {
            foreach (var file in directoryInfo.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (UnauthorizedAccessException ex)
                {
                    file.MoveTo($"{file.FullName}{Names.Extensions.Remove}");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void TryDeleteFolder(DirectoryInfo directoryInfo)
        {
            if (directoryInfo.EnumerateDirectories().Any() == false && directoryInfo.EnumerateFiles().Any() == false)
            {
                directoryInfo.Delete();
            }
            else
            {
                directoryInfo.MoveTo($"{directoryInfo.FullName}{Names.Extensions.Remove}");
            }
        }
    }
}