using HarmonyLib;

namespace BetterPolus.Patches;

[HarmonyPatch(typeof(ReactorSystemType))]
public static class ReactorSystemTypePatches
{
    [HarmonyPatch(nameof(ReactorSystemType.RepairDamage))]
    [HarmonyPrefix]
    private static bool RepairDamagePrefix(ReactorSystemType __instance, PlayerControl player, byte opCode)
    {
        if (ShipStatus.Instance.Type != ShipStatus.MapType.Pb || opCode != 128 || __instance.IsActive) return true;
        
        __instance.Countdown = BetterPolusPlugin.ReactorCountdown.Value;
        __instance.UserConsolePairs.Clear();
        __instance.IsDirty = true;

        return false;
    }
}