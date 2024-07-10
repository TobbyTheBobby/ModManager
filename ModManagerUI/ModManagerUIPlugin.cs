using HarmonyLib;
using Timberborn.ModManagerScene;

namespace ModManagerUI
{
    public class ModManagerUIPlugin : IModStarter
    {
        public void StartMod()
        {
            var harmony = new Harmony("com.modmanagerui");
            harmony.PatchAll();
        }
    }
}
