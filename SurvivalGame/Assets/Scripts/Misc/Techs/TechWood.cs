using Assets.Scripts.Misc.Techs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechWood : Tech
{
  public GameObject woodDropOffButton;

  public void Start()
  {
    woodCost = new float[4] { 10, 15, 20, 25 };
    steelCost = new float[4] { 5, 10, 15, 20 };
    oilCost = new float[4] { 0, 5, 10, 10 };
    researchTime = new float[4] { 20, 25, 30, 40 };
  }

  /// <summary>
  /// Upgrades for Wood
  /// </summary>
  public override void Research(bool successful)
  {
    base.Research(successful);
    if (success)
    {
      switch (level)
      {
        case 1: //this is the case where you get a bag to carry more wood, before this upgrade the wood per person was 0.25 and now increase to 0.35
          foreach (GameObject building in buildingManager.GetComponent<BuildingManager>().GetBuildingsOfType("BuildingWoodProduction"))
            building.GetComponent<BuildingProduction>().ManipulateResourceMultiplier(0.1f);
          UpgradeManager.WoodRate += 0.1f;
          break;
        case 2:
          woodDropOffButton.SetActive(true);
          break;
        case 3:
          UpgradeManager.SawmillAllowedUpgrade = 2;
          break;
        default:
          break;
      }
      success = false;
    }
  }
}
