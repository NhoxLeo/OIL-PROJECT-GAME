using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWoodProduction : BuildingProduction
{

  public new void Start()
  {
    name = "Sawmill";
    currentResource = GlobalConstants.Resources.WOOD;
    maxWorkers = 15;
    productionTime = 6f;
    productionTimeLeft = productionTime;
    oilUsage = 0.25f;
    SpawnTrucks(1, GameObject.Find("ForestInteractionArea").transform, transform);
    base.Start();
  }

  protected override bool CanUpgrade()
  {
    if (buildingLevel + 1 <= UpgradeManager.SawmillAllowedUpgrade && buildingLevel + 1 <= UpgradeManager.sawmillMaxUpgrades)
      return true;
    else
      return base.CanUpgrade();
  }

  public override void Upgrade()
  {
    if (CanUpgrade())
      base.Upgrade();
  }

  public override int[] GetCost()
  {
    switch (buildingLevel)
    {
      case 1: return GlobalConstants.woodBuildingCost;
      case 2: return GlobalConstants.woodBuilding2Cost;
      case 3: return GlobalConstants.woodBuilding3Cost;
      case 4: return GlobalConstants.woodBuilding4Cost;
      default: return null;
    }
  }
}
