using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechFood : Tech {

    public void Start() {
        woodCost = new float[4] { 10, 15, 20, 25 };
        steelCost = new float[4] { 5, 10, 15, 20 };
        oilCost = new float[4] { 0, 5, 10, 10 };
        researchTime = new float[4] { 20, 25, 30, 40 };
    }

    /// <summary>
    /// Upgrades for Food
    /// </summary>
    public override void Research(bool successful) {
        base.Research(successful);
        if (success) {
            switch (level) {
                case 1:
                    // TODO: Hunters spend less time in the forest.
                    break;
                case 2:
                    foreach (GameObject building in buildingManager.GetComponent<BuildingManager>().GetBuildingsOfType("BuildingCanteen"))
                        building.GetComponent<BuildingProduction>().ManipulateResourceMultiplier(0.1f);
                    UpgradeManager.CanteenRate += 0.1f;
                    break;
                case 3:
                    foreach (GameObject building in buildingManager.GetComponent<BuildingManager>().GetBuildingsOfType("BuildingCanteen"))
                        if (GameObject.Find("Human").GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.COOKEDFOOD, -1))
                        {
                            GameObject.Find("Human").GetComponent<Human>().ManipulateDebuff(0.1f);
                        }
                        
                    break;
                default:
                    break;

            }
            success = false;
        }
    }
}
