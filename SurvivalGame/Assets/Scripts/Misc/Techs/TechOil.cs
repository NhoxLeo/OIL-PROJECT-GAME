using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechOil : Tech {

    public void Start() {
        woodCost = new float[4] { 10, 15, 20, 25 };
        steelCost = new float[4] { 5, 10, 15, 20 };
        oilCost = new float[4] { 0, 5, 10, 10 };
        researchTime = new float[4] { 20, 25, 30, 40 };
    }

    /// <summary>
    /// Upgrades for Oil
    /// </summary>
    public override void Research(bool successful) {
        base.Research(successful);
        if (success) {
            switch (level) {
                case 1:
                    UpgradeManager.OilrigAllowedUpgrade = 2;
                    break;
                case 2:
                    UpgradeManager.OilrigAllowedUpgrade = 3;
                    break;
                case 3:
                    UpgradeManager.OilrigAllowedUpgrade = 4;
                    break;
                default:
                    break;
            }
            success = false;
        }
    }
}
