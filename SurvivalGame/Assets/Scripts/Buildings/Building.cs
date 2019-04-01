using Assets.Scripts.Humans;
using Assets.Scripts.UIScripts.Screens.InGameScreen;
using Assets.Scripts.UIScripts.Screens.InGameScreen.LowerUI;
using Assets.Scripts.UIScripts.Screens.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// General parent class of all buildings.
/// </summary>
public class Building : MonoBehaviour /*PlayerInterActableBase*/
{
  protected GameObject resourceManager;
  private GameObject upgradeButton;
  protected GameObject buildings;
  private GameObject playerController;
  public GameObject[] upgradeVersions;
  private GlobalConstants.WorkReqs workplaceReqs;
  protected float constructionTime = 5f;
  protected GameObject informationBox;
  protected string name;
  protected bool isOperational, isBroken, isUpgradable, isConstructed = true;
  protected int buildingLevel = 1;
  protected float interactionRadius;
  private bool showingInfo;
  protected float oilUsage;
  private float oilUsageTimeLeft, oilUsageTimer = 3f;
  [SerializeField]
  private Texture uiImage;


  public virtual void Start()
  {
    workplaceReqs = GlobalConstants.WorkReqs.NONE;
    informationBox = GameObject.Find("InformationBox");
    buildings = GameObject.Find("Buildings");
    resourceManager = GameObject.Find("ResourceManager");
    if (interactionRadius < 0.5f)
      interactionRadius = 2.4f;
    isOperational = true;
    isBroken = false;
    isUpgradable = false;
  }

  public virtual void Update()
  {
    if (oilUsage > 0 && isConstructed)
    {
      oilUsageTimeLeft -= Time.deltaTime;
      if (oilUsageTimeLeft < 0)
      {
        resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.OIL, -oilUsage);
        oilUsageTimeLeft = oilUsageTimer;
      }
    }
  }

  /// <summary>
  /// Attempts to buy the building or makes sure it's affordable.
  /// </summary>
  /// <param name="buy">buy = true if we try to purchase it.</param>
  /// <returns></returns>
  public bool AttemptPurchase(GameObject building, bool buy)
  {
    GameObject rm = GameObject.Find("ResourceManager").gameObject;
    bool success = false;
    int[] cost = GlobalConstants.GetBuildingCost(building.gameObject);
    if (!buy)
    { // ie Just checking if we have the resources.
      if (rm.GetComponent<ResourceManager>().Wood >= cost[0] && rm.GetComponent<ResourceManager>().Steel >= cost[1])
        success = true;
    }
    else
    {  // ie actually buying it.
      if (rm.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, -cost[0])
          && rm.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, -cost[1]))
        success = true;
    }
    return success;
  }


  protected virtual bool CanUpgrade()
  {
    return false;
  }
  /// <summary>
  /// Upgrades the building.
  /// </summary>
  public virtual void Upgrade()
  {
    buildingLevel++;
    SetInformationText(GameObject.Find("ObjectInfo").transform.Find("BuildingInfo").gameObject);
  }

  /// <summary>
  /// Sets the level of this building.
  /// </summary>
  /// <param name="level"></param>
  public void SetLevel(int level)
  {
    buildingLevel = level;
  }

  /// <summary>
  /// Modifies the interaction radius.
  /// </summary>
  /// <param name="radius"></param>
  public void SetInteractionRadius(float radius)
  {
    interactionRadius = radius;
  }

  /// <summary>
  /// Destroys the building and clears selection.
  /// </summary>
  public virtual void DestroyBuilding()
  {
    Destroy(Camera.main.GetComponent<PlayerInteraction>().GetSelectedBuilding().gameObject);
    Camera.main.GetComponent<PlayerInteraction>().DeselectCurrentSelection();
  }

  public virtual void SetInformationText(GameObject infoText) { }
  public virtual void SetBuildingUIType(GameObject buildingTypes) { }

  /// <summary>
  /// Displays information about the building in a GUI-box.
  /// </summary>
  /// <param name="showInfo">Show info</param>
  public void ShowInformation(bool showInfo)
  {
    if (isConstructed)
    {
      GameObject objectInfo = informationBox.transform.Find("ObjectInfo").gameObject;
      GameObject buildingInfo = objectInfo.transform.Find("BuildingInfo").gameObject;
      if (!showInfo)
      {
        // GameObject buildingInfo = objectInfo.transform.Find("BuildingInfo").gameObject;
        int childCount = buildingInfo.transform.childCount;
        for (int i = 0; i < childCount; i++)
          buildingInfo.transform.GetChild(i).gameObject.SetActive(true);
        showingInfo = false;
      }
      objectInfo.SetActive(showInfo);
      if (showInfo)
      {
        informationBox.GetComponent<BuildingInfoController>().SetEnabledButtons(CanUpgrade(), isBroken);

        //var UpgradeDestroyPanelLowerUI = GameObject.Find("UpgradeRepairDestroyPanel").gameObject;
        //UpgradeDestroyPanelLowerUI.transform.GetChild(0).GetComponent<CostButton>().OnEnabled(CanUpgrade());
        //var repairButton = UpgradeDestroyPanelLowerUI.transform.GetChild(1).gameObject.GetComponent<Button>(); //.IsInteractable = isBroken; //SetActive(isBroken);

        //GameObject.Find("RepairBuildingButton").gameObject.SetActive(isBroken);

        var buildingTypes = objectInfo.transform.Find("BuildingTypes").gameObject;
        //Set all other buildings to false.
        //foreach (Transform buildingUIObject in objectInfo.transform.Find("BuildingTypes").transform)
        //{
        //  buildingUIObject.gameObject.SetActive(false);
        //}

        SetBuildingUIType(buildingTypes);
        SetInformationText(buildingInfo);

        transform.parent.GetComponent<BuildingManager>().SetCurrentlySelectedBuilding(this.gameObject);
        if (uiImage)    // If all text somehow disappears, comment this line and the one below!
            GameObject.Find("ClickedObjectImage").GetComponent<RawImage>().texture = uiImage;
        showingInfo = true;
      }
      else
        transform.parent.GetComponent<BuildingManager>().SetCurrentlySelectedBuilding(null);
    }
  }

  /// <summary>
  /// Makes this building broken. It will need to be fixed.
  /// </summary>
  /// <param name="isBroken">Is this building broken or fixed?</param>
  public virtual void SetBrokenStatus(bool isBroken)
  {
    this.isBroken = isBroken;
    LogWindow.Singleton.AddText("A " + name + " has broken down - it needs repairs.");
  }

  public bool IsBroken()
  {
    return isBroken;
  }

  /// <summary>
  /// Attempts to repair this building.
  /// </summary>
  public void RepairBuilding()
  {
    int[] repairCost = GetCost();
    if (repairCost[0] / 2 <= resourceManager.GetComponent<ResourceManager>().Wood && repairCost[1] / 2 <= resourceManager.GetComponent<ResourceManager>().Steel)
    {
      isBroken = false;
      SetInformationText(GameObject.Find("ObjectInfo").transform.Find("BuildingInfo").gameObject);
      GameObject.Find("RepairBuildingButton").gameObject.SetActive(isBroken);
      resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, repairCost[0] / 2);
      resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, repairCost[1] / 2);
      LogWindow.Singleton.AddText("Building repaired.");
    }
  }

  /// <summary>
  /// Changes the state of this building's operational state.
  /// </summary>
  /// <param name="isOperational">Is this building now operational or not?</param>
  public void SetOperational(bool isOperational)
  {
    this.isOperational = isOperational;
  }

  public GlobalConstants.WorkReqs GetWorkReqs()
  {
    return workplaceReqs;
  }

  protected virtual void AssignOccupantsToUppGradedBuilding(LocationTarget buildingType, List<GameObject> occupants, GameObject upgradedBuilding)
  {
    foreach (GameObject worker in occupants)
    {
      worker.GetComponent<Human>().SetNewHumanLocation(buildingType, upgradedBuilding);
    }
  }

  /// <summary>
  /// Turns on spotlights for the buildings.
  /// </summary>
  /// <param name="on"></param>
  public void TurnOnOffSpotlights(bool on)
  {
    transform.Find("Spot Light").gameObject.SetActive(on);
  }

  public float GetConstructionTime()
  {
    return constructionTime;
  }

  public float GetInteractionRadius()
  {
    return interactionRadius;
  }

  public bool IsDisplayingInfo()
  {
    return showingInfo;
  }

  public int GetLevel()
  {
    return buildingLevel;
  }

  public virtual int[] GetCost()
  {
    return null;
  }

  public string GetName()
  {
    return name;
  }

  public void SetConstructed(bool isConstructed)
  {
    this.isConstructed = isConstructed;
  }
}
