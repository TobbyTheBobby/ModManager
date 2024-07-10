using System.IO;
using System.Linq;
using ModManager.ManifestValidatorSystem;
using ModManager.StartupSystem;
using Timberborn.Modding;
using Timberborn.PlatformUtilities;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace ModManagerUI.UiSystem
{
    public class ModManagerStarter : ILoadableSingleton
    {
        private readonly ModRepository _modRepository;

        public ModManagerStarter(ModRepository modRepository)
        {
            _modRepository = modRepository;
        }

        public void Load()
        {
            ManifestValidatorService.Instance.ValidateManifests();

            var modManifest = _modRepository.Mods.First(mod => mod.Manifest.Id == "ModManager");
            
            ModManagerStartup.Run("MOD_IO_APIKEY", options =>
            {
                options.GameId = 3659;
                options.GamePath = Application.dataPath;
                options.IsGameRunning = true;
                options.ModInstallationPath = Path.Combine(UserDataFolder.Folder, UserFolderModsProvider.ModsDirectoryName);
                options.ModIoGameUrl = "https://mod.io/g/timberborn";
                options.ModManagerPath = modManifest.ModDirectory.Path;
                options.Logger = new ModManagerLogger();
            });
        }
    }
}