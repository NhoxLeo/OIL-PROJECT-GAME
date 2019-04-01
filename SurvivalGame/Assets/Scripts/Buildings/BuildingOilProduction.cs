using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingOilProduction : BuildingProduction {

    public new void Start() {
        name = "The Oil Pump";
        currentResource = GlobalConstants.Resources.OIL;
        maxWorkers = 0;
        productionTime = 2f;
        productionTimeLeft = productionTime;
        produceWithoutWorkers = true;
        base.Start();
    }

    public override void SetInformationText(GameObject objectInfo) {
        objectInfo.transform.Find("ClickedObjectInfo").GetComponent<Text>().text = name + "\nLevel: " + buildingLevel;
        try {
            GameObject.Find("ResidenceBuilding").gameObject.SetActive(false);
            GameObject.Find("ResourceDepotBuilding").gameObject.SetActive(false);
            GameObject.Find("ProductionBuilding").gameObject.SetActive(false);
            GameObject.Find("DestroyButton").gameObject.SetActive(false);
            GameObject.Find("SchoolBuilding").gameObject.SetActive(false);
            GameObject.Find("RocketPanel").gameObject.SetActive(false);
        }
        catch { }
    }

  public override void SetBuildingUIType(GameObject buildingTypes)
  {
    foreach (Transform item in buildingTypes.transform)
    {
      var UIbuilding = item.gameObject;
        UIbuilding.SetActive(false);

    }
  }

  protected override void ProduceResource() {
        switch (buildingLevel) {
            case 1:
                resourceManager.GetComponent<ResourceManager>().ManipulateResources(currentResource, (int)GlobalConstants.oilProduction1);
                break;
            case 2:
                resourceManager.GetComponent<ResourceManager>().ManipulateResources(currentResource, (int)GlobalConstants.oilProduction2);
                break;
            case 3:
                resourceManager.GetComponent<ResourceManager>().ManipulateResources(currentResource, (int)GlobalConstants.oilProduction3);
                break;
            case 4:
                resourceManager.GetComponent<ResourceManager>().ManipulateResources(currentResource, (int)GlobalConstants.oilProduction4);
                break;
        }
    }

  protected override bool CanUpgrade()
  {
    if (buildingLevel + 1 <= UpgradeManager.OilrigAllowedUpgrade && buildingLevel + 1 <= UpgradeManager.oilrigMaxUpgrades)
      return true;
    else
      return base.CanUpgrade();
  }

  public override void Upgrade() {
        if (CanUpgrade()) {
            UpgradeManager.OilRigCurrentLevel++;
            base.Upgrade();
        }           
    }

    public override int[] GetCost() {
        switch (buildingLevel) {
            case 1: return GlobalConstants.oilBuilding2Cost;
            case 2: return GlobalConstants.oilBuilding2Cost;
            case 3: return GlobalConstants.oilBuilding3Cost;
            case 4: return GlobalConstants.oilBuilding4Cost;
            default: return null;
        }
    }

    public override void DestroyBuilding() {
        MessageDisplay.DisplayTooltip("You may not destroy your only life-line in this world.", true, false, true);
    }
}
