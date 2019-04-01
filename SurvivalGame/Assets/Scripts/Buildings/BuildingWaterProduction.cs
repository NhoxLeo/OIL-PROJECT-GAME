using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWaterProduction : BuildingProduction
{

  public new void Start()
  {
    name = "Water well";
    currentResource = GlobalConstants.Resources.WATER;
    maxWorkers = 15;
    productionTime = 4f;
    resourceMultiplier = UpgradeManager.waterRate;
    productionTimeLeft = productionTime;
    interactionRadius = 2.4f;
    workSite = GameObject.Find("ForestWorkSite");
    base.Start();
  }

  protected override bool CanUpgrade()
  {
    if (buildingLevel + 1 <= UpgradeManager.WaterWellAllowedUpgrade && buildingLevel + 1 <= UpgradeManager.waterWellMaxUpgrades)
    {
      switch (buildingLevel)
      {
        case 1:
          if (resourceManager.GetComponent<ResourceManager>().Wood >= GlobalConstants.waterBuilding2Cost[0] && resourceManager.GetComponent<ResourceManager>().Steel >= GlobalConstants.waterBuilding2Cost[1])
          {
            return true;
          }
          break;
        case 2:
          if (resourceManager.GetComponent<ResourceManager>().Wood >= GlobalConstants.waterBuilding3Cost[0] && resourceManager.GetComponent<ResourceManager>().Steel >= GlobalConstants.waterBuilding3Cost[1])
          {
            return true;
          }
          break;
        case 3:
          if (resourceManager.GetComponent<ResourceManager>().Wood >= GlobalConstants.waterBuilding4Cost[0] && resourceManager.GetComponent<ResourceManager>().Steel >= GlobalConstants.waterBuilding4Cost[1])
          {
            return true;
          }
          break;
        default:

          return false;
      }
    }

    return false;

  }


public override void Upgrade()
{
  if (CanUpgrade())
  {
    switch (buildingLevel)
    {
      case 1:
        if (CanUpgrade())
        {
          resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, -GlobalConstants.waterBuilding2Cost[0]);
          resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, -GlobalConstants.waterBuilding2Cost[1]);
          GameObject upgradedVersion = Instantiate(upgradeVersions[0]);
          upgradedVersion.transform.position = transform.position;
          upgradedVersion.transform.rotation = transform.rotation;
          upgradedVersion.GetComponent<Building>().SetLevel(2);
          upgradedVersion.transform.parent = buildings.transform;
          upgradedVersion.GetComponent<Building>().SetInteractionRadius(1.9f);
          oilUsage += 0.2f;

          DestroyOnUpgrade();
        }
        break;
      case 2:
        if (CanUpgrade())
        {
          resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, -GlobalConstants.waterBuilding3Cost[0]);
          resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, -GlobalConstants.waterBuilding3Cost[1]);
          oilUsage += 0.1f;
          base.Upgrade();
        }
        break;
      case 3:
        if (CanUpgrade())
        {
          resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, -GlobalConstants.waterBuilding4Cost[0]);
          resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, -GlobalConstants.waterBuilding4Cost[1]);
          oilUsage += 0.1f;
          base.Upgrade();
        }
        break;
      default:
        break;
    }
  }
}

public override int[] GetCost()
{
  switch (buildingLevel)
  {
    case 1: return GlobalConstants.waterBuildingCost;
    case 2: return GlobalConstants.waterBuilding2Cost;
    case 3: return GlobalConstants.waterBuilding3Cost;
    case 4: return GlobalConstants.waterBuilding4Cost;
    default: return null;
  }
}
}
