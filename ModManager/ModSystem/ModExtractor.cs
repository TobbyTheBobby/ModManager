using Modio.Models;
using ModManager.ExtractorSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using File = System.IO.File;

namespace ModManager.ModSystem
{
    public class ModExtractor : IAddonExtractor
    {
        private List<string> _foldersToIgnore = new() { "configs" };

        public bool Extract(string addonZipLocation, Mod modInfo, out string extractLocation, bool overWrite = true)
        {
            extractLocation = "";
            if (!modInfo.Tags.Any(x => x.Name == "Mod"))
                return false;

            var modFolderName = $"{modInfo.NameId}_{modInfo.Id}";
            ClearOldModFiles(modInfo, modFolderName);
            extractLocation = Path.Combine(Paths.Mods, modFolderName);
            ExtractContent(addonZipLocation, extractLocation, overWrite);
            File.Delete(addonZipLocation);
            return true;
        }

        private void ClearOldModFiles(Mod modInfo, string modFolderName)
        {
            if (TryGetExistingModFolder(modInfo, out var dirs))
            {
                var directoryInfo = new DirectoryInfo(dirs);
                if (directoryInfo.Name.Equals(modFolderName))
                {
                    return;
                }

                directoryInfo.MoveTo(Path.Combine(Paths.Mods, modFolderName));
                DeleteModFiles(modFolderName);
            }
        }

        private bool TryGetExistingModFolder(Mod modInfo, out string dirs)
        {
            dirs = null;
            try
            {
                dirs = Directory.GetDirectories(Paths.Mods, $"{modInfo.NameId}_{modInfo.Id}*").SingleOrDefault();
            }
            catch (InvalidOperationException ex)
            {
                throw new AddonExtractorException($"Found multiple folders for \"{modInfo.Name}\"");
            }

            if (dirs != null)
            {
                return true;
            }

            return false;
        }

        private void DeleteModFiles(string modFolderName)
        {
            var modDirInfo = new DirectoryInfo(Path.Combine(Paths.Mods, modFolderName));
            var modSubFolders = modDirInfo
                .GetDirectories("*", SearchOption.AllDirectories)
                .Where(folder => !_foldersToIgnore.Contains(folder.FullName.Split(Path.DirectorySeparatorChar).Last()));
            foreach (var subDirectory in modSubFolders.Reverse())
            {
                DeleteFilesFromFolder(subDirectory);
                TryDeleteFolder(subDirectory);
            }

            DeleteFilesFromFolder(modDirInfo);
            TryDeleteFolder(modDirInfo);
        }

        private void DeleteFilesFromFolder(DirectoryInfo dir)
        {
            foreach (var file in dir.GetFiles().Where(file => !file.Name.EndsWith(Names.Extensions.Remove)))
            {
                try
                {
                    file.Delete();
                }
                catch (UnauthorizedAccessException ex)
                {
                    file.MoveTo($"{file.FullName}{Names.Extensions.Remove}");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void TryDeleteFolder(DirectoryInfo dir)
        {
            if (dir.EnumerateDirectories().Any() == false && dir.EnumerateFiles().Any() == false)
            {
                dir.Delete();
            }
        }

        private static void ExtractContent(string zipFilePath, string extractPath, bool overWrite)
        {
            using (var archive = ZipFile.OpenRead(zipFilePath))
            {
                var entry = archive.Entries.FirstOrDefault(entry => entry.FullName.EndsWith("manifest.json", StringComparison.OrdinalIgnoreCase));

                if (entry != null)
                {
                    string folderPath;
                    if (string.IsNullOrEmpty(Path.GetDirectoryName(entry.FullName)))
                        folderPath = "";
                    else
                        folderPath = Path.GetDirectoryName(entry.FullName) + "/";

                    foreach (var e in archive.Entries)
                    {
                        if (!e.FullName.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase)) 
                            continue;
                        var relativePath = e.FullName.Substring(folderPath.Length);
                        var destinationPath = Path.GetFullPath(Path.Combine(extractPath, relativePath));

                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                        if (e.Name == "")
                        {
                            Directory.CreateDirectory(destinationPath);
                        }
                        else
                        {
                            e.ExtractToFile(destinationPath, overwrite: true);
                        }
                    }
                }
                else
                {
                    ZipFile.ExtractToDirectory(zipFilePath, extractPath, overWrite);
                }
            }
            
            // using (var archive = ZipFile.OpenRead(zipFilePath))
            // {
            //     var rootEntries = archive.Entries.Where(entry => entry.FullName.EndsWith('/')).Where(e => e.FullName.Count(c => c == '/') <= 1).ToList();
            //     if (rootEntries.Count != 1)
            //     {
            //         ZipFile.ExtractToDirectory(zipFilePath, extractPath, overWrite);
            //         return;
            //     }
            //     
            //     var rootEntry = rootEntries[0];
            //     if (!rootEntry.FullName.EndsWith("/")) 
            //     {
            //         ZipFile.ExtractToDirectory(zipFilePath, extractPath, overWrite);
            //         return;
            //     }
            //
            //     var rootFolderName = rootEntry.FullName;
            //
            //     Directory.CreateDirectory(extractPath);
            //
            //     foreach (var entry in archive.Entries)
            //     {
            //         if (!entry.FullName.StartsWith(rootFolderName) || entry.FullName == rootFolderName) 
            //             continue;
            //         var destinationPath = Path.Combine(extractPath, entry.FullName.Substring(rootFolderName.Length));
            //         if (entry.FullName.EndsWith("/"))
            //         {
            //             Directory.CreateDirectory(destinationPath);
            //         }
            //         else
            //         {
            //             Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            //             entry.ExtractToFile(destinationPath, overwrite: true);
            //         }
            //     }
            // }
        }
    }
}