using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.Unity.IL2CPP;

namespace BetterPolus
{
    [BepInPlugin(Id, Name, Version)]
    [BepInProcess("Among Us.exe")]
    public class BetterPolusPlugin : BasePlugin
    {
        public const string Id = "ch.brybry.betterpolus";
        public const string Name = "BetterPolus Mod";
        public const string Version = "1.2.0";

        public Harmony Harmony { get; } = new Harmony(Id);
        public static ManualLogSource log;

        public override void Load()
        {
            log = Log;
            
            log.LogMessage($"{Name} loaded");

            Harmony.PatchAll();
        }

        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        public static class VersionShowerPatch
        {
            public static void Postfix(VersionShower __instance)
            {
                __instance.text.text += $"<size=70%> + <color=#5E4CA6FF>BetterPolus v{Version}</color> by Brybry</size>";
            }
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
        public static class ModStampPatch
        {
            [HarmonyPrefix]
            public static void Prefix()
            {
                DestroyableSingleton<ModManager>.Instance.ShowModStamp();
            }
        }
    }
}
