using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine.SceneManagement;

namespace BetterPolus
{
    [BepInPlugin(Id, Name, Version)]
    [BepInProcess("Among Us.exe")]
    public class BetterPolusPlugin : BasePlugin
    {
        public const string Id = "ch.brybry.betterpolus";
        public const string Name = "BetterPolus Mod";
        public const string Version = "1.1.4";

        public Harmony Harmony { get; } = new Harmony(Id);
        public static ManualLogSource log;

        public override void Load()
        {
            log = Log;
            
            log.LogMessage("BetterPolus Mod loaded");
            
            SceneManager.add_sceneLoaded((Action<Scene, LoadSceneMode>) ((scene, loadSceneMode) =>
            {
                ModManager.Instance.ShowModStamp();
            }));

            Harmony.PatchAll();
        }

        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        public static class VersionShowerPatch
        {
            public static void Postfix(VersionShower __instance)
            {
                __instance.text.text += " + <color=#5E4CA6FF>BetterPolus v1.1.4</color> by Brybry";
            }
        }
    }
}
