using System.Collections.Generic;
using System.Linq;
using ModManager.AddonSystem;
using UnityEngine;

namespace ModManager.ManifestLocationFinderSystem
{
    public class ManifestLocationFinderService
    {
        private readonly ManifestLocationFinderRegistry _manifestLocationFinderRegistry;

        public ManifestLocationFinderService(ManifestLocationFinderRegistry manifestLocationFinderRegistry)
        {
            _manifestLocationFinderRegistry = manifestLocationFinderRegistry;
        }

        public IEnumerable<ModManagerManifest> FindAll()
        {
            return _manifestLocationFinderRegistry.GetManifestLocationFinders().SelectMany(manifestLocationFinder => manifestLocationFinder.Find());
        }
    }
}