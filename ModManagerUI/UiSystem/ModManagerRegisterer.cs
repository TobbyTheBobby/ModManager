using System.Linq;
using ModManager.PersistenceSystem;
using Timberborn.Modding;
using Timberborn.SingletonSystem;

namespace ModManagerUI.UiSystem
{
    public class ModManagerRegisterer : ILoadableSingleton
    {
        private readonly PersistenceService _persistenceService = PersistenceService.Instance;

        private readonly ModRepository _modRepository;
        
        private Mod ModInfo => _modRepository.Mods.First(mod => mod.Manifest.Id == "");
            
        public ModManagerRegisterer(ModRepository modRepository)
        {
            _modRepository = modRepository;
        }
        
        public void Load()
        {
            // foreach (var manifest in InstalledAddonRepository.Instance.All())
            // {
            //     if (manifest.ModId == ModHelper.ModManagerId)
            //     {
            //         return;
            //     }
            // }
            //
            // var modFiles = ModIoModFilesRegistry.Get(ModHelper.ModManagerId);
            // var file = modFiles.FirstOrDefault(file => file.Version == ModInfo.Manifest.Version.ToString());
            //
            // if (file == null)
            // {
            //     throw new NullReferenceException("The current Mod Manager plugin version is not the same as on Mod.io.");
            // }
            //
            // var mod = ModIoModRegistry.Get(ModHelper.ModManagerId);
            // var modManagerManifest = new Manifest(mod, file, ModInfo.ModDirectory.Path);
            // var modManifestPath = Path.Combine(ModInfo.ModDirectory.Path, Manifest.FileName);
            // _persistenceService.SaveObject(modManagerManifest, modManifestPath);
        }
    }
}