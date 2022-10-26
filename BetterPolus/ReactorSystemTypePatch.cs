//*
using HarmonyLib;

namespace BetterPolus
{
    [HarmonyPatch(typeof(ReactorSystemType), nameof(ReactorSystemType.RepairDamage))]
    public static class ReactorSystemType_RepairDamagePatch
    {
        public static bool Prefix(ReactorSystemType __instance, PlayerControl player, byte opCode)
        {
            if (ShipStatus.Instance.Type == ShipStatus.MapType.Pb && opCode == 128 && !__instance.IsActive)
            {
                __instance.Countdown = 40f;
                __instance.UserConsolePairs.Clear();
                __instance.IsDirty = true;

                return false;
            }

            return true;
        }
    }
}
//*/