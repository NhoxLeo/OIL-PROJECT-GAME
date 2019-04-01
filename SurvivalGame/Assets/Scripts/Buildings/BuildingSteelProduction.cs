using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSteelProduction : BuildingProduction {

    public new void Start() {
        name = "Steel Refinery";
        currentResource = GlobalConstants.Resources.STEEL;
        maxWorkers = 15;
        productionTime = 6f;
        productionTimeLeft = productionTime;
        oilUsage = 0.25f;
        base.Start();
    }

  protected override bool CanUpgrade()
  {
    if (buildingLevel + 1 <= UpgradeManager.SteelRefineryAllowedUpgrade && buildingLevel + 1 <= UpgradeManager.steelRefineryMaxUpgrades)
      return true;
    else
      return base.CanUpgrade();
  }

  public override void Upgrade() {
        if (CanUpgrade())
            base.Upgrade();
    }

    public override int[] GetCost() {
        switch (buildingLevel) {
            case 1: return GlobalConstants.steelBuildingCost;
            case 2: return GlobalConstants.steelBuilding2Cost;
            case 3: return GlobalConstants.steelBuilding3Cost;
            case 4: return GlobalConstants.steelBuilding4Cost;
            default: return null;
        }
    }
}
