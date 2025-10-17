using System;
using EasyUpdater.Web;
using LabApi.Features;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using MEC;

namespace EasyUpdater
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance { get; private set; }
        public PluginFinder Finder { get; private set; }
        public Updater Updater { get; private set; }
        
        public override void Enable()
        {
            Instance = this;
            Finder = new PluginFinder();
            Updater = new Updater();
            Timing.CallDelayed(Timing.WaitForOneFrame, () =>
            {
                Finder.FindPlugins();
                Updater.CheckForUpdates(Finder.FoundPlugins);
            });
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRoundRestarted;
        }

        public override void Disable()
        {
            if (Finder != null)
                Finder = null;
            if (Updater != null)
                Updater = null;
            Instance = null;
            
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRoundRestarted;
        }
        
        public void OnRoundRestarted()
        {
            Timing.CallDelayed(Timing.WaitForOneFrame, () =>
            {
                Finder.FindPlugins();
                Updater.CheckForUpdates(Finder.FoundPlugins);
            });
        }

        public override string Name { get; } = "EasyUpdater"; 
        public override string Description { get; } = "Uses the official plugins.scpslgame.com API to automatically update your plugins.";
        public override string Author { get; } = "tayjay";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);
        public override LoadPriority Priority { get; } = (LoadPriority)Byte.MaxValue;
    }
}