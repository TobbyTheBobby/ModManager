using Modio.Models;
using Newtonsoft.Json;

namespace ModManager.AddonSystem
{
    public class ModManagerManifest
    {
        public const string FileName = "mod_manager_manifest.json";

        public ModManagerManifest()
        {
            
        }
        
        public ModManagerManifest(string installLocation, Mod mod, Timberborn.Modding.Mod? timberbornMod, File file)
        {
            RootPath = installLocation;
            FileId = file.Id;
            ModId = timberbornMod?.Manifest.Id;
            ResourceId = mod.Id;
            Version = file.Version!;
        }

        [JsonIgnore]
        public string RootPath { get; set; } = null!;

        public uint FileId { get; set; }

        public string? ModId { get; set; }
        
        public uint ResourceId { get; set; }
        
        [JsonProperty(Required = Required.AllowNull)]
        public string? ModName { get; set; } = null!;

        [JsonProperty(Required = Required.AllowNull)]
        public string? Version { get; set; }
    }
}
