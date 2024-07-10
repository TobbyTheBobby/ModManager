using System;
using System.Collections.Generic;
using System.IO;
using ModManager.AddonSystem;
using ModManager.ManifestLocationFinderSystem;
using ModManager.PersistenceSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace ModManager.MapSystem
{
    public class MapManifestFinder : IManifestLocationFinder
    {
        private readonly PersistenceService _persistenceService = PersistenceService.Instance;

        public IEnumerable<ModManagerManifest> Find()
        {
            var manifestPath = Path.Combine(Paths.Maps, MapModManagerManifest.FileName);

            if (!File.Exists(manifestPath))
            {
                return new List<MapModManagerManifest>();
            }

            try
            {
                var manifests = _persistenceService.LoadObject<List<MapModManagerManifest>>(manifestPath, false);
                return manifests;
            }
            catch (JsonSerializationException ex)
            {
                Debug.LogWarning($"Failed to serialize JSON in file {manifestPath}. It is advised to delete the file.");
                return new List<MapModManagerManifest>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}