using HarmonyLib;

namespace BetterPolus.Patches;

[HarmonyPatch(typeof(VersionShower))]
public static class VersionShowerPatches
{
    [HarmonyPatch(nameof(VersionShower.Start))]
    [HarmonyPostfix]
    private static void StartPostfix(VersionShower __instance)
    {
        __instance.text.text += $"<size=70%> + <color=#5E4CA6FF>BetterPolus v{BetterPolusPlugin.Version}</color> by Brybry</size>";
    }
}