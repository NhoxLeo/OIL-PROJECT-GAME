using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechClothing : Tech {
    public GameObject knickknackStoreButton;

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
                    GameObject.Find("Population").GetComponent<PopulationManager>().UpgradeClothing(0.5f);
                    break;
                case 2:
                    GameObject.Find("Population").GetComponent<PopulationManager>().UpgradeShoes(0.3f);
                    break;
                case 3:
                    knickknackStoreButton.SetActive(true);
                    break;
                default:
                    break;
            }
            success = false;
        }
    }
}
