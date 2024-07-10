using System;
using System.Collections.Generic;
using ModManager.ModIoSystem;
using ModManager.ModManagerSystem;

namespace ModManager.StartupSystem
{
    public class ModManagerStartup : Singleton<ModManagerStartup>
    {
        public static bool IsLoaded;

        private readonly IEnumerable<ILoadable> _loadableClasses = new List<ILoadable>
        {
            Paths.Instance,
            ModIoGameInfo.Instance,
            ModManagerRegisterer.Instance,
            
            ModManagerExtractor.Instance
        };

        public static void Run(string apiKey, Action<ModManagerStartupOptions> options)
        {
            var modManagerOptions = new ModManagerStartupOptions();

            options(modManagerOptions);

            ModIo.InitializeClient(apiKey);

            IsLoaded = true;

            Instance.LoadClasses(modManagerOptions);
        }

        private void LoadClasses(ModManagerStartupOptions startupOptions)
        {
            foreach (var loadableClass in _loadableClasses)
            {
                loadableClass.Load(startupOptions);
            }
        }
    }
}