using Bindito.Core;
using ModManager.ExtractorSystem;
using ModManager.LoggingSystem;
using Timberborn.GameExitSystem;
using Timberborn.SliderToggleSystem;

namespace ModManagerUI.UiSystem
{
    [Context("MainMenu")]
    public class ModManagerConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ModManagerRegisterer>().AsSingleton();
            containerDefinition.Bind<ModManagerPanel>().AsSingleton();
            containerDefinition.Bind<AddonExtractorService>().ToInstance(AddonExtractorService.Instance);
            containerDefinition.Bind<GoodbyeBoxFactory>().AsSingleton();
            containerDefinition.Bind<ModFullInfoController>().AsSingleton();
            containerDefinition.Bind<SliderToggleButtonFactory>().AsSingleton();
            containerDefinition.Bind<SliderToggleFactory>().AsSingleton();
            containerDefinition.Bind<UpdateableModRegistry>().AsSingleton();
            containerDefinition.Bind<IModManagerLogger>().To<ModManagerLogger>().AsSingleton();
        }
    }
}