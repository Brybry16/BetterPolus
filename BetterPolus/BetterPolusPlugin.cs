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
                int index = text.text.LastIndexOf('\n');
                text.text = text.text.Insert(index == -1 ? text.text.Length - 1 : index, 
                    "\nLoaded <color=#5E4CA6FF>BetterPolus v1.1.3-R</color> by Brybry");
            };
            
            Harmony.PatchAll();
        }
    }
}
