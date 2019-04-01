using Assets.Scripts.Misc;
using Assets.Scripts.Misc.Techs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Tech : MonoBehaviour
{
  protected int level = 0;    // ie the level that is to be upgraded when the user press the button. Starts at 0.
  protected bool success;
  GameObject resourceManager;
  public GameObject buildingManager;
  protected float[] woodCost;
  protected float[] steelCost;
  protected float[] oilCost;
  protected float[] researchTime;
  protected List<TechLevelObject> levels = new List<TechLevelObject>();
  [SerializeField]
  [TextArea]
  [Header("Info:")]
  protected string tooltip;

  [SerializeField]
  List<GameObject> RowButtons = new List<GameObject>();

  /// <summary>
  /// Research this tech. Actual logic happens in this class' children.
  /// </summary>
  public virtual void Research(bool successful)
  {
    success = successful;
    resourceManager = GameObject.FindGameObjectWithTag("ResourceManager").gameObject;
    if (!success && !GameObject.Find("CanvasAlwaysActive").GetComponent<TechTimeSliderScript>().IsResearching())
    {
      if (resourceManager.GetComponent<ResourceManager>().Wood >= woodCost[level] && resourceManager.GetComponent<ResourceManager>().Steel >= steelCost[level] && resourceManager.GetComponent<ResourceManager>().Oil >= oilCost[level])
      {
        resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, -woodCost[level]);
        resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, -steelCost[level]);
        resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.OIL, -oilCost[level]);
        GameObject.Find("CanvasAlwaysActive").GetComponent<TechTimeSliderScript>().StartResearch(researchTime[level], gameObject, transform.GetChild(level).gameObject);
      }
    }
    else if (success)
    {
      LogWindow.Singleton.AddText(transform.GetChild(level).transform.GetChild(0).GetComponent<Text>().text + " has been researched.");
      try
      {
        if (UpgradeManager.OilRigCurrentLevel > level + 1)
          transform.GetChild(level + 1).GetComponent<Button>().interactable = true;
        transform.GetChild(level).GetComponent<Button>().interactable = false;
        level++;

      }
      catch { transform.GetChild(level).GetComponent<Button>().interactable = false; level++; }
    }
  }

  public short GetLevel()
  {
    return (short)level;
  }

  public float[] GetCost(short level)
  {
    return new float[4] { woodCost[level], steelCost[level], oilCost[level], researchTime[level] };
  }
}
