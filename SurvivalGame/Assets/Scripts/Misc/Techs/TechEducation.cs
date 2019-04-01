using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechEducation : Tech {
    public GameObject schoolButton, academyButton;

    public void Start() {
        woodCost = new float[4] { 10, 15, 20, 25 };
        steelCost = new float[4] { 5, 10, 15, 20 };
        oilCost = new float[4] { 0, 5, 10, 10 };
        researchTime = new float[4] { 20, 25, 30, 40 };
    }

    public override void Research(bool successful) {
        base.Research(successful);
        if (success) {
            switch (level) {
                case 1:
                    schoolButton.SetActive(true);
                    break;
                case 2:
                    UpgradeManager.SchoolAllowedUpgrade = 2;
                    // TODO: Larger classes
                    break;
                case 3:
                    UpgradeManager.schoolPlayground = true;
                    foreach (GameObject building in buildingManager.GetComponent<BuildingManager>().GetBuildingsOfType("BuildingSchool(Clone)"))
                    {
                        building.GetComponent<BuildingSchool>().BuildPlayground();
                    }
                    // TODO: Playground +moral for kids in school
                    break;
                case 4:
                    academyButton.SetActive(true);
                    // TODO: Specializations (nurse, rocket scientist etc.)
                    break;
                default:
                    break;

            }
            success = false;
        }
    }
}
