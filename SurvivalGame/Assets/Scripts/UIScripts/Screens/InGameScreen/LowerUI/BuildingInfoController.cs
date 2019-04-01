using Assets.Scripts.UIScripts.Screens.UIExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UIScripts.Screens.InGameScreen.LowerUI
{
  public class BuildingInfoController : MonoBehaviour
  {

    [SerializeField]
    CostButton UpgradeButton;
    [SerializeField]
    CostButton RepairButton;
    [SerializeField]
    GameObject BuildingManager;


    public void Start()
    {
      UpgradeButton.SetButtonAction(BuildingManager.GetComponent<BuildingManager>().UpgradeBuilding);
      RepairButton.SetButtonAction(BuildingManager.GetComponent<BuildingManager>().RepairBuilding);
    }

    public void SetEnabledButtons(bool canUpgrade, bool needRepair)
    {
      UpgradeButton.OnEnabled(canUpgrade);
      RepairButton.OnEnabled(needRepair);
    }
  }
}
