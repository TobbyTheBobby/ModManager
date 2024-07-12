using Bindito.Core;
using HarmonyLib;
using ModManager.ManifestValidatorSystem;
using ModManager.MapSystem;
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
            containerDefinition.MultiBind<IManifestValidator>().To<MapManifestValidator>().AsSingleton();
            containerDefinition.Bind<ManifestValidatorService>().AsSingleton();
            containerDefinition.Bind<ModManagerStarter>().AsSingleton();
        }
    }
}