using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCanteen : BuildingProduction
{
  private GlobalConstants.Resources currentResource2;

  public new void Start()
  {
    name = "Canteen";
    currentResource = GlobalConstants.Resources.CLEANWATER;
    currentResource2 = GlobalConstants.Resources.COOKEDFOOD;
    maxWorkers = 5;
    productionTime = 7f;
    productionTimeLeft = productionTime;
    oilUsage = 0.2f;
    resourceMultiplier = UpgradeManager.CanteenRate;
    base.Start();
    interactionRadius = 2.4f;
  }

  /// <summary>
  /// Produces cooked food and clean water.
  /// </summary>
  protected override void ProduceResource()
  {
    float amount = resourceMultiplier * activeWorkers * buildingLevel;
    if (resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WATER, -(int)amount))
      resourceManager.GetComponent<ResourceManager>().ManipulateResources(currentResource, (int)amount);
    if (resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.FOOD, -(int)amount))
      resourceManager.GetComponent<ResourceManager>().ManipulateResources(currentResource2, (int)amount);
  }

  protected override bool CanUpgrade()
  {
    if (buildingLevel + 1 <= UpgradeManager.CanteenAllowedUpgrade && buildingLevel + 1 <= UpgradeManager.canteenMaxUpgrades)
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
      case 1: return GlobalConstants.canteenBuildingCost;
      case 2: return GlobalConstants.canteenBuilding2Cost;
      case 3: return GlobalConstants.canteenBuilding3Cost;
      case 4: return GlobalConstants.canteenBuilding4Cost;
      default: return null;
    }
  }
}
