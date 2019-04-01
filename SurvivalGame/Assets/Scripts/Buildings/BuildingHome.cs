using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Home of a worker.
/// </summary>
public class BuildingHome : Building
{
  private List<GameObject> occupantList;
  private int maxOccupants = 10;
  private GameObject population;

  public new void Start()
  {
    name = "House";
    occupantList = new List<GameObject>();
    population = GameObject.Find("Population");
    base.Start();
    interactionRadius = 2.2f;
  }

  public void ShareBeds(bool on)
  {
    if (on)
      maxOccupants = 20;
    else
    {
      maxOccupants = 10;
      if(occupantList.Count > maxOccupants)
      {
        List<GameObject> moveOutList = new List<GameObject>();
        for (int i = 0; i< occupantList.Count; i++)
        {
          if (i >= maxOccupants)
          {
            moveOutList.Add(occupantList[i]);
          }
        }
        foreach(var human in moveOutList)
        {
          MoveOut(human);
        }
        population.GetComponent<PopulationManager>().MakeHomeless(moveOutList);
      }
    }
  }

  /// <summary>
  /// Fetches the occupant list of this building.
  /// </summary>
  /// <returns></returns>
  public List<GameObject> GetOccupantList()
  {
    if (occupantList.Count < maxOccupants)
      return occupantList;
    else
      return null;
  }

  /// <summary>
  /// Returns the max amount of occupants allowed here.
  /// </summary>
  /// <returns></returns>
  public int GetMaxOccupants()
  {
    return maxOccupants;
  }

  /// <summary>
  /// Sets the occupant list of this building.
  /// </summary>
  /// <param name="newOccupantList">The new occupant list.</param>
  public void SetOccupantList(List<GameObject> newOccupantList)
  {
    occupantList = newOccupantList;
  }

  /// <summary>
  /// Adds occupants to the building.
  /// </summary>
  /// <param name="occupant"></param>
  public void AddOccupants(GameObject occupant)
  {
    occupantList.Add(occupant);
  }

  /// <summary>
  /// Shows the occupants of this building.
  /// </summary>
  public void ShowOccupants()
  {
    foreach (GameObject occupant in occupantList)
    {
      //TODO: Send info to GUI
    }
  }

  public override void SetBuildingUIType(GameObject buildingTypes)
  {
    foreach (Transform item in buildingTypes.transform)
    {
      var UIbuilding = item.gameObject;
      if (UIbuilding.name != "ResidenceBuilding")
        UIbuilding.SetActive(false);
      else
        UIbuilding.SetActive(true);
    }
  }

  /// <summary>
  /// Sets the information box text to its relevant parameters.
  /// </summary>
  /// <param name="buildingInfo">The buldingInfo-panel.</param>
  public override void SetInformationText(GameObject buildingInfo)
  {
    buildingInfo.transform.Find("ClickedObjectInfo").GetComponent<Text>().text = name + "\nLevel: " + buildingLevel + "\nOccupants: " + occupantList.Count + "/" + maxOccupants;
  }

  /// <summary>
  /// Clears the occupant list and destroys the building.
  /// </summary>
  public override void DestroyBuilding()
  {
    population.GetComponent<PopulationManager>().MakeHomeless(occupantList);
    occupantList.Clear();
    base.DestroyBuilding();
  }

  /// <summary>
  /// An occupant moves out.
  /// </summary>
  /// <param name="occupant"></param>
  public void MoveOut(GameObject occupant)
  {
    occupantList.Remove(occupant);
  }

  protected override bool CanUpgrade()
  {
    if (buildingLevel + 1 <= UpgradeManager.ResidenceAllowedUpgrade && buildingLevel + 1 <= UpgradeManager.residenceMaxUpgrades)
      return true;
    else
      return base.CanUpgrade();
  }

  public override void Upgrade()
  {
    if (CanUpgrade())
      switch (buildingLevel)
      {
        case 1:
          if (resourceManager.GetComponent<ResourceManager>().Wood >= GlobalConstants.home2Cost[0] && resourceManager.GetComponent<ResourceManager>().Steel >= GlobalConstants.home2Cost[1])
          {
            resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, -GlobalConstants.home2Cost[0]);
            resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, -GlobalConstants.home2Cost[1]);
            GameObject upgradedVersion = Instantiate(upgradeVersions[0]);
            upgradedVersion.transform.position = transform.position;
            upgradedVersion.transform.rotation = transform.rotation;
            upgradedVersion.GetComponent<Building>().SetLevel(2);
            upgradedVersion.transform.parent = buildings.transform;
            oilUsage += 0.1f;
            AssignOccupantsToUppGradedBuilding(LocationTarget.Home, occupantList, upgradedVersion);
            upgradedVersion.GetComponent<BuildingHome>().SetOccupantList(occupantList);

            base.DestroyBuilding();
          }
          break;
        case 2:
          if (resourceManager.GetComponent<ResourceManager>().Wood >= GlobalConstants.home3Cost[0] && resourceManager.GetComponent<ResourceManager>().Steel >= GlobalConstants.home3Cost[1])
          {
            resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, -GlobalConstants.home3Cost[0]);
            resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, -GlobalConstants.home3Cost[1]);
            oilUsage += 0.05f;
            base.Upgrade();
          }
          break;
      }
  }

  public override int[] GetCost()
  {
    switch (buildingLevel)
    {
      case 1: return GlobalConstants.homeCost;
      case 2: return GlobalConstants.home2Cost;
      case 3: return GlobalConstants.home3Cost;
      case 4: return GlobalConstants.home4Cost;
      default: return null;
    }
  }
}
