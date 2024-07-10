using System;
using System.IO;
using System.Threading.Tasks;
using Modio.Models;
using ModManager;
using ModManager.AddonSystem;
using ModManager.MapSystem;
using ModManager.ModIoSystem;
using UnityEngine;
using File = Modio.Models.File;

namespace ModManagerUI.UiSystem
{
    public class InstallController : Singleton<InstallController>
    {
        private static readonly AddonService AddonService = AddonService.Instance;
        
        public static async Task DownloadAndExtract(Mod mod, File? file)
        {
            var modCard = ModCardRegistry.Get(mod);
            modCard?.ModActionStarted();
            try
            {
                file ??= mod.Modfile!;
                var downloadedMod = await AddonService.Download(mod, file);
                TryInstall(downloadedMod);
            }
            catch (MapException ex)
            {
                Debug.LogWarning(ex.Message);
            }
            catch (AddonException ex)
            {
                Debug.LogWarning(ex.Message);
            }
            catch (IOException ex)
            {
                Debug.LogError($"{ex.Message}");
            }
            modCard?.ModActionStopped();
        }
        
        public static async Task DownloadAndExtractWithDependencies(Mod mod)
        {
            var file = await AddonService.TryGetCompatibleVersion(mod.Id, ModManagerPanel.CheckForHighestInsteadOfLive);
            await DownloadAndExtract(mod, file);
            await foreach (var dependency in AddonService.GetDependencies(mod))
            {
                if (dependency.IsInstalled())
                    continue;
                var dependencyFile = await AddonService.TryGetCompatibleVersion(dependency.ModId, ModManagerPanel.CheckForHighestInsteadOfLive);
                var dependencyMod = ModIoModRegistry.Get(dependency.ModId);
                await DownloadAndExtract(dependencyMod, dependencyFile);
            }
        }
        
        public static void Uninstall(Mod mod)
        {
            var modCard = ModCardRegistry.Get(mod);
            modCard?.ModActionStarted();
            
            try
            {
                AddonService.Uninstall(mod.Id);
            }
            catch (IOException ex)
            {
                Debug.LogWarning(ex.Message);
            }
            catch (AddonException ex)
            {
                Debug.LogWarning(ex.Message);
            }

            modCard?.ModActionStopped();
        }
        
        private static void TryInstall((string location, Mod Mod) mod)
        {
            try
            {
                if (InstalledAddonRepository.Instance.TryGet(mod.Mod.Id, out var manifest) && manifest.Version != mod.Mod.Modfile.Version)
                {
                    AddonService.ChangeVersion(mod.Mod, mod.Mod.Modfile, mod.location);
                }
                else
                {
                    AddonService.Install(mod.Mod, mod.location);
                }
            }
            catch (MapException ex)
            {
                Debug.LogWarning(ex.Message);
                Debug.LogWarning(ex.StackTrace);
            }
            catch (AddonException ex)
            {
                Debug.LogWarning(ex.Message);
                Debug.LogWarning(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                Debug.LogError(ex.StackTrace);
                throw ex;
            }
        }
    }
}