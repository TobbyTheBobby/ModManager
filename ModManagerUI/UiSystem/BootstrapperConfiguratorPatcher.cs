using Bindito.Core;
using HarmonyLib;
using Timberborn.Bootstrapper;

namespace ModManagerUI.UiSystem
{
    [HarmonyPatch]
    public class BootstrapperConfiguratorPatcher
    {
        [HarmonyPatch(typeof(BootstrapperConfigurator), "Configure")]
        [HarmonyPrefix]
        public static void Prefix(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ModManagerStarter>().AsSingleton();
        }
    }
}