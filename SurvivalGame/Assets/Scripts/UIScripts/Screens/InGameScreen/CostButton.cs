using Assets.Scripts.UIScripts.Screens.UIExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UIScripts.Screens.InGameScreen
{
  public class CostButton : ActionButton
  {
    [SerializeField]
    GameObject ResourceManager;
    [SerializeField]
    GameObject PlayerInteraction;
    [SerializeField]
    GameObject BuildingManager;

    [SerializeField]
    string DisabledToolTip;

    private GameObject selectedBuilding;
    private ResourceManager resourceManager;

    protected override void Start()
    {
      //SetButtonAction(BuildingManager.GetComponent<BuildingManager>().UpgradeBuilding);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
      base.OnPointerClick(eventData);
    }

    protected override bool CanExecute()
    {
      if (!selectedBuilding)
        return false;

      return execute;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
      base.OnPointerEnter(eventData);

      resourceManager = ResourceManager.GetComponent<ResourceManager>();
      selectedBuilding = PlayerInteraction.GetComponent<PlayerInteraction>().GetSelectedBuilding();


      if (selectedBuilding)
      {

        if (interactable)
        {
          int successCount = 0;
          int[] cost = GlobalConstants.GetBuildingCost(selectedBuilding);
          if (cost == null)
          {
            Debug.LogWarning("A cost has not been set for the next upgrade needs to be done...");
            execute = false;
            return;
          }

          string woodString = "", steelString = "";
          if (cost[0] > 0)
            if (resourceManager.Wood < cost[0])
            {
              woodString = "<color=red>" + cost[0] + " wood" + "</color>";
            }
            else
            {
              woodString = cost[0] + " wood";
              successCount++;
            }

          if (cost[1] > 0)
            if (resourceManager.Steel < cost[1])
            {
              steelString = "<color=red>" + cost[1] + " steel" + "</color>";
            }
            else
            {
              steelString = cost[1] + " steel";
              successCount++;
            }

          if (successCount == 2)
            execute = true;
          else
            execute = false;

          MessageDisplay.DisplayTooltip("<b> Cost: </b>" + woodString + " " + steelString, true, true);
        }

        else
        {
          if (!string.IsNullOrEmpty(DisabledToolTip))
            MessageDisplay.DisplayTooltip("<b>" + DisabledToolTip + "</b>", true, true);
        }


      }
    }

    protected override void Execute()
    {

    }

    protected override void OnDisable()
    {
      MessageDisplay.DisplayTooltip(false);
    }
    protected override void FailToClickSound()
    {
      SoundManager.PlaySound("byym");
    }

    protected override void HoverSound()
    {
      SoundManager.PlaySound("buildingHover");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
      MessageDisplay.DisplayTooltip(false);
    }

    public void ForceDisplayOff()
    {
      MessageDisplay.DisplayTooltip(false);
    }


  }
}
