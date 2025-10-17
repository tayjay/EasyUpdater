using System.Collections.Generic;
using System.ComponentModel;

namespace EasyUpdater
{
    public class Config
    {
        [Description("Enable this if the updater is having trouble finding your plugins. Checks all available plugins rather than fine-tuning the search.")]
        public bool DeepSearch { get; set; } = false;
        
        [Description("Automatically update plugins when a new version is found. If false, you will be notified in the console and have to update manually.")]
        public bool AutoUpdate { get; set; } = false;

        public List<string> IgnoredPlugins { get; set; } = new List<string>
        {
            "Exiled Loader",
        };
    }
}