using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using LabApi.Features.Console;
using Newtonsoft.Json;
using Utf8Json;

namespace EasyUpdater.Web
{
    public class PluginFinder
    {
        HttpClient _client = new HttpClient();
        public Dictionary<LabApi.Loader.Features.Plugins.Plugin,List<WebPlugin>> FoundPlugins = new Dictionary<LabApi.Loader.Features.Plugins.Plugin, List<WebPlugin>>();
        
        public PluginFinder()
        {
            _client.BaseAddress = new Uri("https://plugins.scpslgame.com");
        }
        
        public void FindPlugins()
        {
            // Shallow Search for all enabled plugins
            if (Plugin.Instance.Config.DeepSearch)
            {
                DeepSearch();
            }
            else
            {
                foreach (var plugin in LabApi.Loader.PluginLoader.EnabledPlugins)
                {
                    if(Plugin.Instance.Config.IgnoredPlugins.Contains(plugin.Name)) continue;
                    if (!TryGetPlugin(plugin.Author, plugin.Name, out var pluginJson))
                    {
                        //Logger.Warn($"Failed to find plugin {plugin.Name} by {plugin.Author} via direct lookup. Attempting search...");
                        // Need to check another way
                        if (!TrySearchPlugin(plugin.Name, out pluginJson))
                        {
                            //Logger.Debug($"Failed to find plugin {plugin.Name} by {plugin.Author} via search.");
                            continue;
                        }
                    }
                    var rootObject = JsonConvert.DeserializeObject<RootObject>(pluginJson);
                    if (rootObject == null || rootObject.Data == null || rootObject.Data.Data.Count == 0)
                    {
                        Logger.Error($"Failed to parse plugin JSON for {plugin.Name} by {plugin.Author}.");
                        continue;
                    }
                    
                    FoundPlugins.Add(plugin, rootObject.Data.Data);
                }
            }
        }

        /**
         * Attempt to find a plugin by author and name
         * Looking at the API /plugin/{author}/{pluginName}
         */
        public bool TryGetPlugin(string author, string pluginName, out string pluginJson)
        {
            var response = _client.GetAsync($"/api/v1/plugin/{author}/{pluginName}").Result;
            if (response.IsSuccessStatusCode)
            {
                pluginJson = response.Content.ReadAsStringAsync().Result;
                //Logger.Debug(pluginJson);
                return true;
            }
            else
            {
                //Logger.Debug($"Failed to find plugin {pluginName} by {author}. Status code: {response.StatusCode}");
            }
            pluginJson = null;
            return false;
        }
        
        public bool TrySearchPlugin(string searchTerm, out string searchJson)
        {
            var response = _client.GetAsync($"/api/v1/plugin?search={searchTerm}").Result;
            //Logger.Debug(response.RequestMessage.RequestUri);
            if (response.IsSuccessStatusCode)
            {
                searchJson = response.Content.ReadAsStringAsync().Result;
                RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(searchJson);
                if (rootObject == null || rootObject.Data == null || rootObject.Data.Data.Count == 0)
                {
                    //Logger.Debug($"No plugins found for search term {searchTerm}");
                }
                else
                {
                    //Logger.Debug(searchJson);
                    return true;
                }
                
            }
            //Console.WriteLine($"Failed to search for plugin {searchTerm}. Status code: {response.StatusCode}");
            if (searchTerm.Contains("."))
            {
                // Remove the last segment after the dot and try again
                var newSearchTerm = searchTerm.Substring(0, searchTerm.LastIndexOf('.'));
                return TrySearchPlugin(newSearchTerm, out searchJson);
            }
            
            searchJson = null;
            return false;
        }

        public bool DeepSearch()
        {
            int page = 1;
            int pages = 1;
            do
            {
                var response = _client.GetAsync($"/api/v1/plugin?search&orderBy=createdAt&orderDirection=desc&tags&authorId&limit=12&page={page}").Result;
                
                if (response.IsSuccessStatusCode)
                {
                    var searchJson = response.Content.ReadAsStringAsync().Result;
                    var rootObject = JsonConvert.DeserializeObject<RootObject>(searchJson);
                    if (rootObject == null || rootObject.Data == null || rootObject.Data.Data.Count == 0)
                    {
                        //Logger.Debug("No more plugins found.");
                        return false;
                    }

                    pages = rootObject.Data.Meta.TotalPages;
                    foreach (var plugin in rootObject.Data.Data)
                    {
                        
                        Logger.Debug($"Found plugin: {plugin.Name} by {plugin.Author.Username} (v{plugin.Releases[0].UpdatedAt}) - {plugin.Downloads} downloads");
                        foreach (var enabledPlugin in LabApi.Loader.PluginLoader.EnabledPlugins)
                        {
                            if(Plugin.Instance.Config.IgnoredPlugins.Contains(enabledPlugin.Name)) continue;
                            bool found = false;
                            string dllName;
                            if(enabledPlugin.FilePath.Contains("\\"))
                                dllName = enabledPlugin.FilePath.Substring(enabledPlugin.FilePath.LastIndexOf('\\') + 1);
                            else
                                dllName = enabledPlugin.FilePath.Substring(enabledPlugin.FilePath.LastIndexOf('/') + 1);
                            // Check if any of the assets match the dll name
                            foreach (var asset in plugin.Releases[0].Assets)
                            {
                                if (asset.Name == dllName)
                                {
                                    found = true;
                                    FoundPlugins.Add(enabledPlugin, new List<WebPlugin> {plugin});
                                    break;
                                }
                            }

                            if (found)
                                break;
                        }
                    }
                }
                else
                {
                    Logger.Error($"Failed to search for plugins. Status code: {response.StatusCode}");
                    return false;
                }
                page++;
            } while (page <= pages);
            return true;
        }
    }
}