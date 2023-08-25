﻿using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace BetterPolus.Patches;

[HarmonyPatch(typeof(ShipStatus))]
public static class ShipStatusPatches
{
    // Positions
    public static readonly Vector3 DvdScreenNewPos = new Vector3(26.635f, -15.92f, 1f);
    public static readonly Vector3 VitalsNewPos = new Vector3(31.275f, -6.45f, 1f);

    public static readonly Vector3 WifiNewPos = new Vector3(15.975f, 0.084f, 1f);
    public static readonly Vector3 NavNewPos = new Vector3(11.07f, -15.298f, -0.015f);

    public static readonly Vector3 TempColdNewPos = new Vector3(7.772f, -17.103f, -0.017f);

    // Scales
    public const float DvdScreenNewScale = 0.75f;

    // Checks
    public static bool IsAdjustmentsDone;
    public static bool IsObjectsFetched;
    public static bool IsRoomsFetched;
    public static bool IsVentsFetched;

    // Tasks Tweak
    public static Console WifiConsole;
    public static Console NavConsole;

    // Vitals Tweak
    public static SystemConsole Vitals;
    public static GameObject DvdScreenOffice;

    // Vents Tweak
    public static Vent ElectricBuildingVent;
    public static Vent ElectricalVent;
    public static Vent ScienceBuildingVent;
    public static Vent StorageVent;

    // TempCold Tweak
    public static Console TempCold;

    // Rooms
    public static GameObject Comms;
    public static GameObject DropShip;
    public static GameObject Outside;
    public static GameObject Science;

    private static void ApplyChanges(ShipStatus instance)
    {
        if (instance.Type == ShipStatus.MapType.Pb)
        {
            FindPolusObjects();
            AdjustPolus();
        }
    }

    public static void FindPolusObjects()
    {
        FindVents();
        FindRooms();
        FindObjects();
    }

    public static void AdjustPolus()
    {
        if (IsObjectsFetched && IsRoomsFetched)
        {
            MoveVitals();
            SwitchNavWifi();
            MoveTempCold();
        }
        else
        {
            BetterPolusPlugin.log.LogError("Couldn't move elements as not all of them have been fetched.");
        }

        AdjustVents();

        IsAdjustmentsDone = true;
    }

    // --------------------
    // - Objects Fetching -
    // --------------------

    public static void FindVents()
    {
        var ventsList = Object.FindObjectsOfType<Vent>().ToList();

        if (ElectricBuildingVent == null)
        {
            ElectricBuildingVent = ventsList.Find(vent => vent.gameObject.name == "ElectricBuildingVent");
        }

        if (ElectricalVent == null)
        {
            ElectricalVent = ventsList.Find(vent => vent.gameObject.name == "ElectricalVent");
        }

        if (ScienceBuildingVent == null)
        {
            ScienceBuildingVent = ventsList.Find(vent => vent.gameObject.name == "ScienceBuildingVent");
        }

        if (StorageVent == null)
        {
            StorageVent = ventsList.Find(vent => vent.gameObject.name == "StorageVent");
        }

        IsVentsFetched = ElectricBuildingVent != null && ElectricalVent != null && ScienceBuildingVent != null &&
                         StorageVent != null;
    }

    public static void FindRooms()
    {
        if (Comms == null)
        {
            Comms = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Comms");
        }

        if (DropShip == null)
        {
            DropShip = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Dropship");
        }

        if (Outside == null)
        {
            Outside = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Outside");
        }

        if (Science == null)
        {
            Science = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Science");
        }

        IsRoomsFetched = Comms != null && DropShip != null && Outside != null && Science != null;
    }

    public static void FindObjects()
    {
        if (WifiConsole == null)
        {
            WifiConsole = Object.FindObjectsOfType<Console>().ToList()
                .Find(console => console.name == "panel_wifi");
        }

        if (NavConsole == null)
        {
            NavConsole = Object.FindObjectsOfType<Console>().ToList()
                .Find(console => console.name == "panel_nav");
        }

        if (Vitals == null)
        {
            Vitals = Object.FindObjectsOfType<SystemConsole>().ToList()
                .Find(console => console.name == "panel_vitals");
        }

        if (DvdScreenOffice == null)
        {
            GameObject DvdScreenAdmin = Object.FindObjectsOfType<GameObject>().ToList()
                .Find(o => o.name == "dvdscreen");

            if (DvdScreenAdmin != null)
            {
                DvdScreenOffice = Object.Instantiate(DvdScreenAdmin);
            }
        }

        if (TempCold == null)
        {
            TempCold = Object.FindObjectsOfType<Console>().ToList()
                .Find(console => console.name == "panel_tempcold");
        }

        IsObjectsFetched = WifiConsole != null && NavConsole != null && Vitals != null &&
                           DvdScreenOffice != null && TempCold != null;
    }

    // -------------------
    // - Map Adjustments -
    // -------------------

    public static void AdjustVents()
    {
        if (IsVentsFetched)
        {
            ElectricBuildingVent.Left = ElectricalVent;
            ElectricalVent.Center = ElectricBuildingVent;

            ScienceBuildingVent.Left = StorageVent;
            StorageVent.Center = ScienceBuildingVent;
        }
        else
        {
            BetterPolusPlugin.log.LogError("Couldn't adjust Vents as not all objects have been fetched.");
        }
    }

    public static void MoveTempCold()
    {
        if (TempCold.transform.position != TempColdNewPos)
        {
            Transform tempColdTransform = TempCold.transform;
            tempColdTransform.parent = Outside.transform;
            tempColdTransform.position = TempColdNewPos;

            // Fixes collider being too high
            BoxCollider2D collider = TempCold.GetComponent<BoxCollider2D>();
            collider.isTrigger = false;
            collider.size += new Vector2(0f, -0.3f);
        }
    }

    public static void SwitchNavWifi()
    {
        if (WifiConsole.transform.position != WifiNewPos)
        {
            Transform wifiTransform = WifiConsole.transform;
            wifiTransform.parent = DropShip.transform;
            wifiTransform.position = WifiNewPos;
        }

        if (NavConsole.transform.position != NavNewPos)
        {
            Transform navTransform = NavConsole.transform;
            navTransform.parent = Comms.transform;
            navTransform.position = NavNewPos;

            // Prevents crewmate being able to do the task from outside
            NavConsole.checkWalls = true;
        }
    }

    public static void MoveVitals()
    {
        if (Vitals.transform.position != VitalsNewPos)
        {
            // Vitals
            Transform vitalsTransform = Vitals.gameObject.transform;
            vitalsTransform.parent = Science.transform;
            vitalsTransform.position = VitalsNewPos;
        }

        if (DvdScreenOffice.transform.position != DvdScreenNewPos)
        {
            // DvdScreen
            Transform dvdScreenTransform = DvdScreenOffice.transform;
            dvdScreenTransform.position = DvdScreenNewPos;

            var localScale = dvdScreenTransform.localScale;
            localScale =
                new Vector3(DvdScreenNewScale, localScale.y,
                    localScale.z);
            dvdScreenTransform.localScale = localScale;
        }
    }
    
    [HarmonyPatch(nameof(ShipStatus.Begin))]
    [HarmonyPrefix]
    private static void BeginPrefix(ShipStatus __instance)
    {
        ApplyChanges(__instance);
    }

    [HarmonyPatch(nameof(ShipStatus.Awake))]
    [HarmonyPrefix]
    private static void AwakePrefix(ShipStatus __instance)
    {
        ApplyChanges(__instance);
    }

    [HarmonyPatch(nameof(ShipStatus.FixedUpdate))]
    [HarmonyPrefix]
    private static void FixedUpdate(ShipStatus __instance)
    {
        if (IsObjectsFetched && IsAdjustmentsDone) return;
        ApplyChanges(__instance);
    }
}