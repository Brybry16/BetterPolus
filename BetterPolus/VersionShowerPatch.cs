using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;

namespace BetterPolus
{
    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public class VersionShowerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch]
        [HarmonyPriority(Priority.First)]
        public static void Postfix(VersionShower __instance)
        {
            var reactorVS = GameObject.Find("ReactorVersion");
            GameObject.Destroy(reactorVS);

            TextRenderer text = __instance.text;
            text.Text += "\nLoaded [5E4CA6FF]BetterPolus v1.1.1-R []by Brybry";
        }
    }
}