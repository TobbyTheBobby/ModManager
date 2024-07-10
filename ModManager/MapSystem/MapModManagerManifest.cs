using Modio.Models;
using ModManager.AddonSystem;
using System.Collections.Generic;

namespace ModManager.MapSystem
{
    public class MapModManagerManifest : ModManagerManifest
    {
        public new const string FileName = ModManagerManifest.FileName;

        public List<string> MapFileNames { get; }

        public MapModManagerManifest(string installLocation, Mod mod, Timberborn.Modding.Mod timberbornMod, File file, List<string> mapFleNames) 
            : base(installLocation, mod, timberbornMod, file)
        {
            MapFileNames = mapFleNames;
        }
    }
}