using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;

namespace BetterPolus{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    public class BetterPolusPlugin : BasePlugin{
        public const string Id = "ch.brybry.betterpolus";
        public Harmony Harmony { get; } = new Harmony(Id);
        public static ManualLogSource log;
        public override void Load(){
            log = Log;
            log.LogMessage("Succesfully loaded BetterPolus");
            Harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerUpdate {
        public static void Postfix(VersionShower __instance){
            __instance.text.text += " - <color=#5E4CA6FF>BetterPolus v1.2.0-R</color>";
        }
    }
}
