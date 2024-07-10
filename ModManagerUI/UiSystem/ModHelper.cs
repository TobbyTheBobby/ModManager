using Modio.Models;
using ModManager.AddonSystem;

namespace ModManagerUI.UiSystem
{
    public abstract class ModHelper
    {
        public static uint ModManagerId => 2541476;

        public static bool IsModManager(Mod mod)
        {
            return mod.Id == ModManagerId;
        }
        
        public static bool IsModManager(ModManagerManifest modManagerManifest)
        {
            return modManagerManifest.ResourceId == ModManagerId;
        }
    }
}