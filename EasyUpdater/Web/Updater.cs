using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using LabApi.Features.Console;

namespace EasyUpdater.Web
{
    public class Updater
    {
        HttpClient _client;
        
        public Updater()
        {
            _client = new HttpClient();
        }
        
        public void CheckForUpdates(Dictionary<LabApi.Loader.Features.Plugins.Plugin, List<WebPlugin>> plugins)
        {
            foreach (var kvp in plugins)
            {
                foreach (var candidate in kvp.Value)
                {
                    foreach (var asset in candidate.Releases[0].Assets)
                    {
                        if (kvp.Key.FilePath.EndsWith(asset.Name))
                        {
                            // Check the date of the release
                            if (File.GetLastWriteTimeUtc(kvp.Key.FilePath).CompareTo(candidate.Releases[0].UpdatedAt) < 0)
                            {
                                // Update available
                                Logger.Info($"Update available for {kvp.Key.Name}. Current version: {kvp.Key.Version}, New version: {candidate.Releases[0].TagName}");
                                string updateUrl = asset.DownloadUrl;
                                Logger.Info($"Download it from: {updateUrl}");
                                if (Plugin.Instance.Config.AutoUpdate)
                                {
                                    // Todo: Download and replace the file
                                }
                            }
                            else
                            {
                                Logger.Info("No updates available for " + kvp.Key.Name);
                            }
                        }
                    }
                }
            }
        }
        
    }
}