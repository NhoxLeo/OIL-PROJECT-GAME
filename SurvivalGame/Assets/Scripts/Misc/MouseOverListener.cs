using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOverListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  [Header("New building button")]
  public GameObject buildingPrefab;
  [SerializeField]
  private string buildingName;
  [SerializeField]
  [Header("Tech tooltip")]
  [TextArea]
  public string tooltipText;
  [SerializeField]
  private bool isTechtip = true;
  [SerializeField]
  private bool isSmallBox = false;
  //private short tier = 1;

  public void OnPointerEnter(PointerEventData eventData)
  {

    var s = eventData.selectedObject;
    var resourceManager = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>();

        if (!isSmallBox) {
            // New building button
            if (buildingPrefab) {
                int[] cost = GlobalConstants.GetBuildingCost(buildingPrefab);
                string woodString = "", steelString = "";
                if (cost[0] > 0)
                    if (resourceManager.Wood < cost[0]) {
                        woodString = "<color=red>" + cost[0] + " wood" + "</color>";
                    }
                    else
                        woodString = cost[0] + " wood";
                if (cost[1] > 0)
                    if (resourceManager.Steel < cost[1]) {
                        steelString = "<color=red>" + cost[1] + " steel" + "</color>";
                    }
                    else
                        steelString = cost[1] + " steel";
                tooltipText = "<i>" + tooltipText + "</i>";
                MessageDisplay.DisplayTooltip("<b>" + buildingName + "\nCost: </b>" + woodString + " " + steelString + "\n" + tooltipText, true, true);
                SoundManager.PlaySound("buildingHover");
            }
            // Tech button
            else if (tooltipText != null && isTechtip) {
                tooltipText = "<i>" + tooltipText + "</i>";
                float[] cost = GetComponentInParent<Tech>().GetCost((short)transform.GetSiblingIndex());
                string woodString = "", steelString = "", oilString = "", researchTimeString = "";
                if (cost[0] > 0)
                    if (resourceManager.Wood < cost[0]) {
                        woodString = "<color=red>" + cost[0] + " wood " + "</color>";
                    }
                    else
                        woodString = cost[0] + " wood ";
                if (cost[1] > 0)
                    if (resourceManager.Steel < cost[1]) {
                        steelString = "<color=red>" + cost[1] + " steel " + "</color>";
                    }
                    else
                        steelString = cost[1] + " steel ";
                if (cost[2] > 0)
                    if (resourceManager.Oil < cost[2]) {
                        steelString = "<color=red>" + cost[2] + " oil " + "</color>";
                    }
                    else
                        oilString = cost[2] + " oil ";
                researchTimeString = "\n<b>Research time: </b>" + cost[3] + " s";
                MessageDisplay.DisplayTooltip("<b>" + GetComponentInChildren<Text>().text + "\nCost: </b>" + woodString + steelString + oilString + researchTimeString + "\n" + tooltipText, true, true);
                SoundManager.PlaySound("techHover");
            }
            else if (tooltipText != null && !isTechtip) {
                MessageDisplay.DisplayTooltip(tooltipText, false, true);
                SoundManager.PlaySound("techHover");
            }
        }
        else {
            MessageDisplay.DisplayTooltip(tooltipText, true, true, true);
        }
    }


  public void OnPointerExit(PointerEventData eventData)
  {
    MessageDisplay.DisplayTooltip(false);
  }

  public void ForceDisplayOff()
  {
    MessageDisplay.DisplayTooltip(false);
  }
}
