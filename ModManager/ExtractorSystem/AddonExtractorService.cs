using System.Collections.Generic;
using Modio.Models;

namespace ModManager.ExtractorSystem
{
    public class AddonExtractorService
    {
        private readonly IEnumerable<IAddonExtractor> _addonInstallers;

        public AddonExtractorService(IEnumerable<IAddonExtractor> addonInstallers)
        {
            _addonInstallers = addonInstallers;
        }

        public string Extract(Mod addonInfo, string addonZipLocation, bool overwrite = true)
        {
            foreach (var extractor in _addonInstallers)
            {
                if (extractor.Extract(addonZipLocation, addonInfo, out var extractLocation))
                {
                    return extractLocation;
                }
            }

            throw new AddonExtractorException($"{addonInfo.Name} could not be installed by any extractor.");
        }
    }
}