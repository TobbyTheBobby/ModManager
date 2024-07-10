using System;
using System.Collections.Generic;
using System.IO;
using ModManager.AddonSystem;
using ModManager.ManifestLocationFinderSystem;
using ModManager.PersistenceSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace ModManager.ModSystem
{
    public class ModManifestFinder : IManifestLocationFinder
    {
        private readonly PersistenceService _persistenceService = PersistenceService.Instance;

        public IEnumerable<ModManagerManifest> Find()
        {
            foreach (var enabledManifest in Directory.GetFiles(Paths.Mods, ModManagerManifest.FileName, SearchOption.AllDirectories))
            {
                var manifest = LoadManifest(enabledManifest);
                if (manifest == null)
                { 
                    continue; 
                }
                yield return manifest;

            }

            foreach (var disabledManifest in Directory.GetFiles(Paths.Mods, ModManagerManifest.FileName + Names.Extensions.Disabled, SearchOption.AllDirectories))
            {
                var manifest = LoadManifest(disabledManifest);
                if (manifest == null)
                {
                    continue;
                }
                yield return manifest;
            }

            foreach (var enabledManifest in Directory.GetFiles(Paths.Mods, ModManagerManifest.FileName + Names.Extensions.Remove, SearchOption.AllDirectories))
            {
                var manifest = LoadManifest(enabledManifest);
                if (manifest == null)
                {
                    continue;
                }
                yield return manifest;
            }
        }

        public IEnumerable<ModManagerManifest> FindRemovable()
        {
            return new List<ModManagerManifest>();
        }

        private ModManagerManifest? LoadManifest(string manifestPath)
        {
            try
            {
                var manifest = _persistenceService.LoadObject<ModManagerManifest>(manifestPath, false);
                manifest.RootPath = Path.GetDirectoryName(manifestPath)!;
                return manifest;
            }
            catch (JsonSerializationException ex)
            {
                Debug.LogWarning($"Failed to serialize JSON in file {manifestPath} It is advised to delete the file.");
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}