using System.Collections.Generic;

namespace ModManager.ManifestLocationFinderSystem
{
    public class ManifestLocationFinderRegistry
    {
        private readonly IEnumerable<IManifestLocationFinder> _manifestLocationFinders;

        public ManifestLocationFinderRegistry(IEnumerable<IManifestLocationFinder> manifestLocationFinders)
        {
            _manifestLocationFinders = manifestLocationFinders;
        }

        public IEnumerable<IManifestLocationFinder> GetManifestLocationFinders()
        {
            return _manifestLocationFinders;
        }
    }
}