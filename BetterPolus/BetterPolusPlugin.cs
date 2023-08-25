using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx.Unity.IL2CPP;

namespace BetterPolus;

[BepInPlugin(Id, Name, Version)]
[BepInProcess("Among Us.exe")]
public class BetterPolusPlugin : BasePlugin
{
    private const string Id = "ch.brybry.betterpolus";
    private const string Name = "BetterPolus Mod";
    public const string Version = "1.2.1";

    public Harmony Harmony { get; } = new Harmony(Id);
    public static ManualLogSource log;
    public static ConfigEntry<bool> Enabled { get; private set; }
    public static ConfigEntry<float> ReactorCountdown { get; private set; }

    public override void Load()
    {
        log = Log;

        Enabled = Config.Bind("Polus", "Enable Better Polus", true, "Enable Polus map modifications");
        ReactorCountdown = Config.Bind("Polus", "Reactor Countdown", 40f, "Reactor sabotage countdown in Polus map");
            
        log.LogMessage($"{Name} loaded");

        Harmony.PatchAll();
    }
}