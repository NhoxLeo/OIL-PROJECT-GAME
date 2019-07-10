using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeManager {
    // Max upgrades allowed
    public static int sawmillMaxUpgrades = 2;
    public static int steelRefineryMaxUpgrades = 2;
    public static int canteenMaxUpgrades = 3;
    public static int oilrigMaxUpgrades = 4;
    public static int waterWellMaxUpgrades = 3;
    public static int residenceMaxUpgrades = 3;
    public static int resourceDepotMaxUpgrades = 3;
    public static int schoolMaxUpgrades = 2;

    // Current allowed upgrades
    public static int sawmillAllowedUpgrade = 1;
    public static int steelRefineryAllowedUpgrade = 1;
    public static int canteenAllowedUpgrade = 1;
    public static int oilrigAllowedUpgrade = 1;
    public static int waterWellAllowedUpgrade = 1;
    public static int residenceAllowedUpgrade = 1;
    public static int resourceDepotAllowedUpgrade = 1;
    public static int schoolAllowedUpgrade = 1;

    // Oilrig
    public static short oilRigCurrentLevel = 1;

    // Technology buffs
    public static bool schoolPlayground = false;
    public static float waterRate = 0.25f;
    public static float woodRate = 1f;
    public static float steelRate = 1f;
    public static float canteenRate = 0.33f;
    public static float huntingRate = 2f;

    // Properties
    public static int SawmillAllowedUpgrade { get { return sawmillAllowedUpgrade; } set { sawmillAllowedUpgrade = value; } }
    public static int SteelRefineryAllowedUpgrade { get { return steelRefineryAllowedUpgrade; } set { steelRefineryAllowedUpgrade = value; } }
    public static int CanteenAllowedUpgrade { get { return canteenAllowedUpgrade; } set { canteenAllowedUpgrade = value; } }
    public static int OilrigAllowedUpgrade { get { return oilrigAllowedUpgrade; } set { oilrigAllowedUpgrade = value; } }
    public static int WaterWellAllowedUpgrade { get { return waterWellAllowedUpgrade; } set { waterWellAllowedUpgrade = value; } }
    public static int ResidenceAllowedUpgrade { get { return residenceAllowedUpgrade; } set { residenceAllowedUpgrade = value; } }
    public static int ResourceDepotAllowedUpgrade { get { return resourceDepotAllowedUpgrade; } set { resourceDepotAllowedUpgrade = value; } }
    public static int SchoolAllowedUpgrade { get { return schoolAllowedUpgrade; } set { schoolAllowedUpgrade = value; } }
    public static bool SchoolPlayground { get { return schoolPlayground; } set { schoolPlayground = value; } }
    public static float WaterRate { get { return waterRate; } set { waterRate = value; } }
    public static float WoodRate { get { return woodRate; } set { woodRate = value; } }
    public static float SteelRate { get { return steelRate; } set { steelRate = value; } }
    public static float CanteenRate { get { return canteenRate; } set { canteenRate = value; } }
    public static float HuntingRate { get { return huntingRate; } set { huntingRate = value; } }
    public static short OilRigCurrentLevel { get { return oilRigCurrentLevel; } set { oilRigCurrentLevel = value; } }
}
