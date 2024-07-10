using System.Collections.Generic;
using System.Linq;
using ModManager.ManifestLocationFinderSystem;
using Timberborn.Modding;
using Timberborn.SingletonSystem;

namespace ModManager.AddonSystem
{
    public class InstalledAddonRepository : ILoadableSingleton
    {
        public static InstalledAddonRepository Instance = null!;
        
        private readonly ManifestLocationFinderService _manifestLocationFinderService;
        
        private Dictionary<uint, ModManagerManifest> _installedMods = new();

        public InstalledAddonRepository(ManifestLocationFinderService manifestLocationFinderService)
        {
            Instance = this;
            _manifestLocationFinderService = manifestLocationFinderService;
        }

        public bool TryGet(uint modId, out ModManagerManifest modManagerManifest)
        {
            return _installedMods.TryGetValue(modId, out modManagerManifest);
        }

        public ModManagerManifest Get(uint modId)
        {
            return _installedMods[modId];
        }

        public bool Has(uint modId)
        {
            return _installedMods.ContainsKey(modId);
        }

        public void Remove(uint modId)
        {
            _installedMods.Remove(modId);
        }

        public void Add(ModManagerManifest modManagerManifest)
        {
            _installedMods.Add(modManagerManifest.ResourceId, modManagerManifest);
        }

        public IEnumerable<ModManagerManifest> All()
        {
            return _installedMods.Values;
        }

        public void Load()
        {
            _installedMods = _manifestLocationFinderService.FindAll().ToDictionary(manifest => manifest.ResourceId);
        }
    }
}