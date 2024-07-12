using ModManager.AddonInstallerSystem;
using ModManager.AddonSystem;
using ModManager.ExtractorSystem;
using ModManager.PersistenceSystem;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Timberborn.Modding;
using File = Modio.Models.File;
using Mod = Modio.Models.Mod;

namespace ModManager.MapSystem
{
    public class MapInstaller : IAddonInstaller
    {
        private readonly InstalledAddonRepository _installedAddonRepository;
        private readonly AddonExtractorService _addonExtractorService;
        private readonly ModLoader _modLoader;

        private readonly PersistenceService _persistenceService = PersistenceService.Instance;
        private readonly MapManifestFinder _mapManifestFinder = new();

        public MapInstaller(InstalledAddonRepository installedAddonRepository, AddonExtractorService addonExtractorService, ModLoader modLoader)
        {
            _installedAddonRepository = installedAddonRepository;
            _addonExtractorService = addonExtractorService;
            _modLoader = modLoader;
        }

        public bool Install(Mod mod, string zipLocation)
        {
            if (!mod.Tags.Any(x => x.Name == "Map"))
            {
                return false;
            }

            List<string> timberFileNames = new();
            using (var zipFile = ZipFile.OpenRead(zipLocation))
            {
                timberFileNames = zipFile.Entries
                    .Where(x => x.Name.Contains(".timber"))
                    .Select(x => x.Name.Replace(Names.Extensions.TimberbornMap, ""))
                    .ToList();
            }

            for (var i = 0; i < timberFileNames.Count(); i++)
            {
                var files = Directory.GetFiles(Paths.Maps, timberFileNames[i]);
                if (files.Length > 0)
                {
                    timberFileNames[i] += $"_{files.Length + 1}";
                }
            }

            var installLocation = _addonExtractorService.Extract(mod, zipLocation);
            
            var manifest = new MapModManagerManifest(installLocation, mod, mod.Modfile, timberFileNames);
            var manifests = _mapManifestFinder.Find()
                .Select(a => (MapModManagerManifest)a)
                .ToList();
            manifests.Add(manifest);

            var mapManifestPath = Path.Combine(installLocation, MapModManagerManifest.FileName);
            _persistenceService.SaveObject(manifests, mapManifestPath);
            _installedAddonRepository.Add(manifest);

            return true;
        }

        public bool Uninstall(ModManagerManifest modManagerManifest)
        {
            if (modManagerManifest is not MapModManagerManifest)
            {
                return false;
            }

            var manifestPath = Path.Combine(Paths.Maps, MapModManagerManifest.FileName);
            var manifests = _mapManifestFinder.Find().Select(a => (MapModManagerManifest)a).ToList();
            manifests.Remove(manifests.SingleOrDefault(x => x.ResourceId == modManagerManifest.ResourceId));
            _persistenceService.SaveObject(manifests, manifestPath);

            _installedAddonRepository.Remove(modManagerManifest.ResourceId);

            foreach (var mapFileName in ((MapModManagerManifest)modManagerManifest).MapFileNames)
            {
                var files = Directory.GetFiles(Paths.Maps, $"{mapFileName}{Names.Extensions.TimberbornMap}*");
                foreach (var file in files)
                {
                    System.IO.File.Delete(file);
                }
            }

            return true;
        }

        public bool ChangeVersion(Mod mod, File file, string zipLocation)
        {
            if (!mod.Tags.Any(x => x.Name == "Map"))
            {
                return false;
            }

            List<string> timberFileNames = new();
            using (var zipFile = ZipFile.OpenRead(zipLocation))
            {
                timberFileNames = zipFile.Entries
                    .Where(x => x.Name.Contains(".timber"))
                    .Select(x => x.Name.Replace(Names.Extensions.TimberbornMap, ""))
                    .ToList();
            }

            for (var i = 0; i < timberFileNames.Count(); i++)
            {
                var files = Directory.GetFiles(Paths.Maps, timberFileNames[i]);
                if (files.Length > 0)
                {
                    timberFileNames[i] += $"_{files.Length + 1}";
                }
            }

            var installLocation = _addonExtractorService.Extract(mod, zipLocation);
         
            var manifest = new MapModManagerManifest(installLocation, mod, mod.Modfile, timberFileNames);
            var manifests = _mapManifestFinder.Find()
                .Where(a => a.ResourceId != mod.Id)
                .Select(a => (MapModManagerManifest)a)
                .ToList();
            manifests.Add(manifest);

            var mapManifestPath = Path.Combine(installLocation, MapModManagerManifest.FileName);
            _persistenceService.SaveObject(manifests, mapManifestPath);

            _installedAddonRepository.Remove(manifest.ResourceId);
            _installedAddonRepository.Add(manifest);

            return true;
        }
    }
}