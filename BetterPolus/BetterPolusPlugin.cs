using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using Reactor;
using Reactor.Patches;

namespace BetterPolus
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class BetterPolusPlugin : BasePlugin
    {
        public const string Id = "ch.brybry.betterpolus";

        public Harmony Harmony { get; } = new Harmony(Id);
        public static ManualLogSource log;


        public override void Load()
        {
            log = Log;
            
            log.LogMessage("BetterPolus Mod loaded");

            ReactorVersionShower.TextUpdated += (text) =>
            {
                int index = text.Text.LastIndexOf('\n');
                text.Text = text.Text.Insert(index == -1 ? text.Text.Length - 1 : index, 
                    "\nLoaded [5E4CA6FF]BetterPolus v1.1.2-R []by Brybry");
            };
            
            Harmony.PatchAll();
        }
    }
}
