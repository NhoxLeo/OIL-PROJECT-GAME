using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechtreeManager : MonoBehaviour {
    private enum TechGroup { Resources, Living, Emergencies, }
    public GameObject techPanelResources, techPanelLiving;

  /// <summary>
  /// Displays the correct tech group.
  /// </summary>
  /// <param name="groupIndex">The index of the group, 1 = resources, 2 = living, 3 = null.</param>
  public void ShowTechGroup(int groupIndex) {
        switch (groupIndex) {
            case 1:
                try {
                    techPanelLiving.SetActive(false);
                }
                catch {}
                techPanelResources.SetActive(true);
                break;
            case 2:
                try {
                    techPanelResources.SetActive(false);
                }
                catch { }
                techPanelLiving.SetActive(true);
                break;
            case 3:
                try
                {
                    
                }
                catch { }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Unlocks a techtree tier.
    /// </summary>
    public void OnEnable() {
        if (GameObject.Find("OilRow").transform.GetComponent<Tech>().GetLevel() == UpgradeManager.OilRigCurrentLevel - 1)
            GameObject.Find("OilRow").transform.GetChild(UpgradeManager.OilRigCurrentLevel - 1).transform.GetComponent<Button>().interactable = true;
        foreach (Transform row in techPanelLiving.transform) {
            if (row.GetComponent<Tech>().GetLevel() == UpgradeManager.OilRigCurrentLevel - 1)
                row.GetChild(UpgradeManager.OilRigCurrentLevel - 1).transform.GetComponent<Button>().interactable = true;
        }
        foreach (Transform row in techPanelResources.transform) {
            if (row.GetComponent<Tech>().GetLevel() == UpgradeManager.OilRigCurrentLevel - 1 && UpgradeManager.OilRigCurrentLevel != 4)
                row.GetChild(UpgradeManager.OilRigCurrentLevel - 1).transform.GetComponent<Button>().interactable = true;
        }
        
    }
}
