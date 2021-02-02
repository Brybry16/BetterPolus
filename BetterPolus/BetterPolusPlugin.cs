using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using Reactor;

namespace BetterPolus
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class BetterPolusPlugin : BasePlugin
    {
        public const string Id = "ch.brybry.betterpolus";

        public Harmony Harmony { get; } = new Harmony(Id);
        public static ManualLogSource Logger;


        public override void Load()
        {
            Logger = Log;
            
            Harmony.PatchAll();
        }
    }
}
