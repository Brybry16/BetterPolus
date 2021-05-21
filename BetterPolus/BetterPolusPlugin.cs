﻿using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;

namespace BetterPolus {
    [BepInPlugin(Id, Name, Version)]
    [BepInProcess("Among Us.exe")]
    public class BetterPolusPlugin : BasePlugin {
        public const string Id = "ch.brybry.betterpolus";
        public const string Name = "BetterPolus";
        public const string Version = "1.2.1";
		static internal BepInEx.Logging.ManualLogSource Logger;
        public Harmony Harmony { get; } = new Harmony(Id);
        public override void Load(){
            Logger = Log;
            BetterPolusPlugin.Logger.LogInfo("Succesfully loaded BetterPolus");
            Harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerUpdate {
        public static void Postfix(VersionShower __instance){
            __instance.text.text += " - <color=#5E4CA6FF>BetterPolus v1.2.1-R</color>";
        }
    }
}
