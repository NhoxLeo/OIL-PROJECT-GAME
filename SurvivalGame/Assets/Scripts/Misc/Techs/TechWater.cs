using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechWater : Tech {

    public void Start() {
        woodCost = new float[4] { 10, 15, 20, 25 };
        steelCost = new float[4] { 5, 10, 15, 20 };
        oilCost = new float[4] { 0, 5, 10, 10 };
        researchTime = new float[4] { 20, 25, 30, 40 };
    }

    /// <summary>
    /// Upgrades for Wood
    /// </summary>
    public override void Research(bool successful) {
        base.Research(successful);
        if (success) {
            switch (level) {
                case 1:
                    foreach (GameObject building in buildingManager.GetComponent<BuildingManager>().GetBuildingsOfType("BuildingWaterProduction"))
                        building.GetComponent<BuildingProduction>().ManipulateResourceMultiplier(0.1f);
                    UpgradeManager.WaterRate += 0.1f;
                    break;
                case 2:
                    UpgradeManager.WaterWellAllowedUpgrade = 2;
                    break;
                case 3:
                    UpgradeManager.WaterWellAllowedUpgrade = 3;
                    break;
                default:
                    break;
            }
            success = false;
        }       
    }
}
