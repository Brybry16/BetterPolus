using HarmonyLib;

namespace BetterPolus.Patches;

[HarmonyPatch(typeof(AmongUsClient))]
public static class AmongUsClientPatches
{
    [HarmonyPatch(nameof(AmongUsClient.Awake))]
    [HarmonyPrefix]
    private static void AwakePrefix()
    {
        DestroyableSingleton<ModManager>.Instance.ShowModStamp();
    }
}