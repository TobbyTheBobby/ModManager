using System;
using System.IO;
using System.Linq;
using ModManager.AddonInstallerSystem;
using ModManager.AddonSystem;
using ModManager.ExtractorSystem;
using ModManager.MapSystem;
using ModManager.PersistenceSystem;
using Timberborn.Modding;
using UnityEngine;
using Mod = Modio.Models.Mod;

namespace ModManager.ModSystem
{
    public class ModInstaller : IAddonInstaller
    {
        private readonly InstalledAddonRepository _installedAddonRepository;
        private readonly ModLoader _modLoader;

        private readonly AddonExtractorService _addonExtractorService = AddonExtractorService.Instance;
        private readonly PersistenceService _persistenceService = PersistenceService.Instance;

        public ModInstaller(InstalledAddonRepository installedAddonRepository, ModLoader modLoader)
        {
            _installedAddonRepository = installedAddonRepository;
            _modLoader = modLoader;
        }

        public bool Install(Mod mod, string zipLocation)
        {
            if (!mod.Tags.Any(x => x.Name == "Mod"))
                return false;
            var installLocation = _addonExtractorService.Extract(mod, zipLocation);
            // _modLoader.TryLoadMod(new ModDirectory(new DirectoryInfo(installLocation), true, "Local"), out var timberbornMod);
            // var manifest = new ModManagerManifest(installLocation, mod, timberbornMod, mod.Modfile!);
            // var modManifestPath = Path.Combine(installLocation, ModManagerManifest.FileName);
            // _persistenceService.SaveObject(manifest, modManifestPath);
            // _installedAddonRepository.Add(manifest);
            //
            // return true;
            if (_modLoader.TryLoadMod(new ModDirectory(new DirectoryInfo(installLocation), true, "Local"), out var timberbornMod))
            {
                var manifest = new ModManagerManifest(installLocation, mod, timberbornMod, mod.Modfile!);
                var modManifestPath = Path.Combine(installLocation, ModManagerManifest.FileName);
                _persistenceService.SaveObject(manifest, modManifestPath);
                _installedAddonRepository.Add(manifest);
                return true;
            }
            
            Debug.LogWarning($"Mod '{mod.Name}' does not contain valid manifest.json.");
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
            _modLoader.TryLoadMod(new ModDirectory(new DirectoryInfo(installLocation), true, "Local"), out var timberbornMod);
            var manifest = new ModManagerManifest(installLocation, mod, timberbornMod, mod.Modfile);
            var modManifestPath = Path.Combine(installLocation, ModManagerManifest.FileName);
            _persistenceService.SaveObject(manifest, modManifestPath);

            _installedAddonRepository.Remove(mod.Id);
            _installedAddonRepository.Add(manifest);

            return true;
        }


        private void DeleteFilesFromFolder(DirectoryInfo dir)
        {
            foreach (var file in dir.GetFiles())
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

        private void TryDeleteFolder(DirectoryInfo dir)
        {
            if (dir.EnumerateDirectories().Any() == false && dir.EnumerateFiles().Any() == false)
            {
                dir.Delete();
            }
            else
            {
                dir.MoveTo($"{dir.FullName}{Names.Extensions.Remove}");
            }
        }
    }
}