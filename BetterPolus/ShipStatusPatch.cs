using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace BetterPolus
{
    [HarmonyPatch(typeof(ShipStatus))]
    public static class ShipStatusPatch
    {
        // Positions
        public static readonly Vector3 DvdScreenNewPos = new Vector3(26.635f, -15.92f, 1f);
        public static readonly Vector3 VitalsNewPos = new Vector3(30.255f, -6.66f, 1f);
        public static readonly Vector3 WifiNewPos = new Vector3(15.975f, 0.084f, 1f);
        public static readonly Vector3 NavNewPos = new Vector3(11.07f, -15.298f, -0.015f);

        // Scales
        public const float DvdScreenNewScale = 0.75f;
        public const float VitalsNewScale = 1.05f;

        // Checks
        public static bool IsObjectsFetched;
        public static bool IsAdjustmentsDone;
        public static bool IsVentsFetched;

        // Tasks Tweak
        public static Console WifiConsole;
        public static Console NavConsole;

        // Vitals Tweak
        public static SystemConsole Vitals;
        public static GameObject WeatherMap;
        public static GameObject DvdScreenOffice;

        // Vents Tweak
        public static Vent ElectricBuildingVent;
        public static Vent ElectricalVent;
        public static Vent ScienceBuildingVent;
        public static Vent StorageVent;

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Begin))]
        public static class ShipStatusBeginPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch]       
            public static void Prefix(ShipStatus __instance)
            {
                ApplyChanges(__instance);
            }
        }

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Awake))]
        public static class ShipStatusAwakePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch]
            public static void Prefix(ShipStatus __instance)
            {
                ApplyChanges(__instance);
            }
        }

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.FixedUpdate))]
        public static class ShipStatusFixedUpdatePatch
        {
            [HarmonyPrefix]
            [HarmonyPatch]
            public static void Prefix(ShipStatus __instance)
            {
                if (!IsObjectsFetched || !IsAdjustmentsDone)
                {
                    ApplyChanges(__instance);
                }
            }
        }

        public static void ApplyChanges(ShipStatus instance)
        {
            if (instance.Type == ShipStatus.MapType.Pb)
            {
                FindPolusObjects();
                AdjustPolus();
            }
        }

        public static void AdjustPolus()
        {
            MoveVitals();
            SwitchNavWifi();
            AdjustVents();

            IsAdjustmentsDone = true;
        }
        
        // --------------------
        // - Objects Fetching -
        // --------------------

        public static void FindVents()
        {
            BetterPolusPlugin.Logger.LogMessage("Fetching Vent Objects");
            
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

        public static void FindPolusObjects()
        {

            FindVents();
            
            // if (Comms == null)
            // {
            //     Comms = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Comms");
            // }
            //
            // if (DropShip == null)
            // {
            //     DropShip = Object.FindObjectsOfType<GameObject>().ToList().Find(o => o.name == "Dropship");
            // }
            
            if (WifiConsole == null)
            {
                BetterPolusPlugin.Logger.LogMessage("Fetching WifiConsole Object");
                WifiConsole = Object.FindObjectsOfType<Console>().ToList()
                    .Find(console => console.name == "panel_wifi");
            }

            if (NavConsole == null)
            {
                BetterPolusPlugin.Logger.LogMessage("Fetching NavConsole Object");
                NavConsole = Object.FindObjectsOfType<Console>().ToList()
                    .Find(console => console.name == "panel_nav");
            }

            if (Vitals == null)
            {
                BetterPolusPlugin.Logger.LogMessage("Fetching Vitals Object");
                Vitals = Object.FindObjectsOfType<SystemConsole>().ToList()
                    .Find(console => console.name == "panel_vitals");
            }

            if (WeatherMap == null)
            {
                BetterPolusPlugin.Logger.LogMessage("Fetching WeatherMap Object");
                WeatherMap = Object.FindObjectsOfType<GameObject>().ToList()
                    .Find(o => o.name == "Weathermap0001");
            }
                
            if (DvdScreenOffice == null)
            {
                BetterPolusPlugin.Logger.LogMessage("Fetching DvdScreenAdmin Object");
                GameObject DvdScreenAdmin = Object.FindObjectsOfType<GameObject>().ToList()
                    .Find(o => o.name == "dvdscreen");

                if (DvdScreenAdmin != null)
                {
                    BetterPolusPlugin.Logger.LogMessage("Creating DvdScreenOffice Object");
                    DvdScreenOffice = Object.Instantiate(DvdScreenAdmin);
                }
            }

            IsObjectsFetched = WifiConsole != null && NavConsole != null && Vitals != null && WeatherMap != null &&
                                DvdScreenOffice != null;
        }

        // -------------------
        // - Map Adjustments -
        // -------------------
        
        public static void AdjustVents()
        {
            if (IsVentsFetched)
            {
                BetterPolusPlugin.Logger.LogMessage("Adjusting Vents");

                ElectricBuildingVent.Left = ElectricalVent;
                ElectricalVent.Center = ElectricBuildingVent;

                ScienceBuildingVent.Left = StorageVent;
                StorageVent.Center = ScienceBuildingVent;
            }
            else
            {
                BetterPolusPlugin.Logger.LogError("Couldn't adjust Vents as not all objects have been fetched.");
            }
        }
        
        public static void SwitchNavWifi()
        {
            if (IsObjectsFetched)
            {
                if (WifiConsole.transform.position != WifiNewPos)
                {
                    BetterPolusPlugin.Logger.LogMessage("Moving Wifi to Dropship");
                    Transform wifiTransform = WifiConsole.transform;
                    wifiTransform.position = WifiNewPos;
                    WifiConsole.Room = SystemTypes.Dropship;
                }

                if (NavConsole.transform.position != NavNewPos)
                {
                    BetterPolusPlugin.Logger.LogMessage("Moving Nav to Comms");
                    Transform navTransform = NavConsole.transform;
                    navTransform.position = NavNewPos;
                    NavConsole.Room = SystemTypes.Comms;
                }
            }
            else
            {
                BetterPolusPlugin.Logger.LogError("Couldn't Switch Nav & Wifi as not all objects have been fetched.");
            }
        }
        
        public static void MoveVitals()
        {
            if (IsObjectsFetched)
            {
                if (Vitals.transform.position != VitalsNewPos)
                {
                    // Vitals
                    BetterPolusPlugin.Logger.LogMessage("Moving Vitals to Laboratory");
                    Transform vitalsTransform = Vitals.gameObject.transform;
                    vitalsTransform.position = VitalsNewPos;
                    var localScale = vitalsTransform.localScale;
                    localScale =
                        new Vector3(VitalsNewScale, localScale.y, localScale.z);
                    vitalsTransform.localScale = localScale;
                }

                if (WeatherMap.active)
                {
                    // WeatherMap
                    BetterPolusPlugin.Logger.LogMessage("Deactivating WeatherMap");
                    WeatherMap.SetActive(false);
                }

                if (DvdScreenOffice.transform.position != DvdScreenNewPos)
                {
                    // DvdScreen
                    BetterPolusPlugin.Logger.LogMessage("Moving DvdScreenOffice");
                    Transform dvdScreenTransform = DvdScreenOffice.transform;
                    dvdScreenTransform.position = DvdScreenNewPos;
                    var localScale = dvdScreenTransform.localScale;
                    localScale =
                        new Vector3(DvdScreenNewScale, localScale.y,
                            localScale.z);
                    dvdScreenTransform.localScale = localScale;
                }
            }
            else
            {
                BetterPolusPlugin.Logger.LogError("Couldn't move Vitals as not all objects have been fetched.");
            }
        }
    }
}