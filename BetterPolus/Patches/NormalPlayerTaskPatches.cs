using System.Collections.Generic;
using HarmonyLib;
using Il2CppSystem.Text;

namespace BetterPolus.Patches;

[HarmonyPatch(typeof(NormalPlayerTask))]
public static class NormalPlayerTaskPatches
{
    private static readonly List<TaskTypes> TaskTypesToPatch = new()
        { TaskTypes.RebootWifi, TaskTypes.RecordTemperature, TaskTypes.ChartCourse };

    [HarmonyPatch(nameof(NormalPlayerTask.AppendTaskText))]
    [HarmonyPrefix]
    private static bool AppendTaskTextPrefix(NormalPlayerTask __instance, StringBuilder sb)
    {
        if (!BetterPolusPlugin.Enabled.Value || !ShipStatus.Instance || ShipStatus.Instance.Type != ShipStatus.MapType.Pb) return true;
        if (!TaskTypesToPatch.Contains(__instance.TaskType)) return true;
        var flag = __instance.ShouldYellowText();
        if (flag)
        {
            sb.Append(__instance.IsComplete ? "<color=#00DD00FF>" : "<color=#FFFF00FF>");
        }

        var room = GetUpdatedRoom(__instance);

        sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(room));
        sb.Append(": ");
        sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(__instance.TaskType));
        if (__instance is { ShowTaskTimer: true, TimerStarted: NormalPlayerTask.TimerState.Started })
        {
            sb.Append(" (");
            sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SecondsAbbv,
                (int)__instance.TaskTimer));
            sb.Append(")");
        }
        else if (__instance.ShowTaskStep)
        {
            sb.Append(" (");
            sb.Append(__instance.taskStep);
            sb.Append("/");
            sb.Append(__instance.MaxStep);
            sb.Append(")");
        }

        if (flag)
        {
            sb.Append("</color>");
        }

        sb.AppendLine();

        return false;
    }

    private static SystemTypes GetUpdatedRoom(NormalPlayerTask task)
    {
        return task.TaskType switch
        {
            TaskTypes.RecordTemperature => SystemTypes.Outside,
            TaskTypes.RebootWifi => SystemTypes.Dropship,
            TaskTypes.ChartCourse => SystemTypes.Comms,
            _ => task.StartAt
        };
    }
}