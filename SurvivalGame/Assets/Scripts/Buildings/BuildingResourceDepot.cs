using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingResourceDepot : BuildingProduction, IVisitable
{
    private float maxWood, maxWater, maxSteel, maxFood, maxCookedFood, maxCleanWater, maxOil;
    private float wood, water, steel, food, cookedFood, cleanWater, oil;

    public float VisitTime { get { return 2000; } }

    //private int metalWorkerCount = 0, woodWorkerCount = 0, huntingWorkerCount = 0;
    //protected WorkSiteType currentSelectedSiteType;

    public new void Start()
    {
        maxWood = maxWater = maxSteel = maxFood = maxCookedFood = maxCleanWater = maxOil = 100f;
        name = "Resource Depot";
        maxWorkers = 200;
        float[] tempArr = GameObject.Find("ResourceManager").GetComponent<ResourceManager>().GetStartingResources();
        oil = tempArr[0];
        water = tempArr[1];
        food = tempArr[2];
        ;
        steel = tempArr[3];
        wood = tempArr[4];
        cleanWater = tempArr[5];
        cookedFood = tempArr[6];
        base.Start();
    }

    public void HandleVisitor(GameObject deliveryHuman)
    {
        WorkSiteType siteType =
          deliveryHuman.GetComponent<Human>().LocationService[LocationTarget.WorkSite].GetComponent<WorkSite>().SiteType;

        ///The constant value (4) to deliver is hardcoded for now.
        switch (siteType)
        {
            case WorkSiteType.Forest:
                resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.WOOD, UpgradeManager.WoodRate);
                break;
            case WorkSiteType.MetalScraps:
                resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.STEEL, UpgradeManager.SteelRate);
                break;
            case WorkSiteType.Hunting:
                resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.FOOD, UpgradeManager.HuntingRate);
                break;
            default:
                break;
        }
    }

    protected override void ProduceResource() { }

    protected override bool CanUpgrade()
    {
        if (buildingLevel + 1 <= UpgradeManager.SawmillAllowedUpgrade && buildingLevel + 1 <= UpgradeManager.sawmillMaxUpgrades)
            return true;
        else
            return base.CanUpgrade();
    }

    public override void Upgrade()
    {
        if (CanUpgrade())
        {
            maxWood = maxWater = maxSteel = maxFood = maxCookedFood = maxCleanWater = maxOil += 100;
            base.Upgrade();
        }
        base.Upgrade();
    }


    public override void SetBuildingUIType(GameObject buildingTypes)
    {
        foreach (Transform item in buildingTypes.transform)
        {
            GameObject UIbuilding = item.gameObject;
            if (UIbuilding.name != "ResourceDepotBuilding")
                UIbuilding.SetActive(false);
            else
                UIbuilding.SetActive(true);
        }
    }

    public override void SetInformationText(GameObject objectInfo)
    {
        objectInfo.transform.Find("ClickedObjectInfo").GetComponent<Text>().text = name
            + "\nLevel: " + buildingLevel
            + "\nCapacity: " + maxWood
            + "\nWorkers"
            + "\nMetal: " + metalWorkerCount
            + "\nWood: " + woodWorkerCount
            + "\nHunting: " + huntingWorkerCount;
    }

    /// <summary>
    /// Only destroys this building if it isn't the last resource depot left.
    /// </summary>
    public override void DestroyBuilding()
    {
        if (buildings.GetComponent<BuildingManager>().GetResourceDepotList().Count > 1)
        {
            buildings.GetComponent<BuildingManager>().RemoveResourceDepot(gameObject);
            base.DestroyBuilding();
        }
        else
            MessageDisplay.DisplayTooltip("You may not destroy your last Resoruce Depot!", true, false, true);
    }

    /// <summary>
    /// Get this building's cost to build/upgrade.
    /// </summary>
    /// <returns></returns>
    public override int[] GetCost()
    {
        switch (buildingLevel)
        {
            case 1:
                return GlobalConstants.resourceDepotCost;
            case 2:
                return GlobalConstants.resourceDepot2Cost;
            case 3:
                return GlobalConstants.resourceDepot3Cost;
            case 4:
                return GlobalConstants.resourceDepot4Cost;
            default:
                return null;
        }
    }


    ///// <summary>
    ///// Finds the closest relevant worksite and set it as its worksite.
    ///// </summary>
    ///// <param name="workSiteToFind">Work site to find.</param>
    //public void SetCurrentWorkSiteType(WorkSiteType workSiteToFind)
    //{
    //    GameObject workAreas = GameObject.FindWithTag("WorkSiteAreas");
    //    List<WorkSite> tempWorkSiteList = new List<WorkSite>();

    //    float shortestDistance = float.PositiveInfinity;
    //    currentSelectedSiteType = workSiteToFind;
    //    foreach (Transform area in workAreas.transform)
    //    {
    //        WorkSite tempWorkSite = area.gameObject.GetComponent<WorkSite>();

    //        if (tempWorkSite.SiteType == workSiteToFind)
    //        {
    //            tempWorkSiteList.Add(tempWorkSite);
    //            if (workSite)
    //            {
    //                float tempDistance = Vector3.Distance(transform.position, workSite.gameObject.transform.position);
    //                if (tempDistance < shortestDistance)
    //                {
    //                    shortestDistance = tempDistance;
    //                    workSite = tempWorkSite.gameObject;
    //                }
    //            }
    //            else
    //                workSite = tempWorkSite.gameObject;


    //        }
    //    }
    //}
    /// <summary>
    /// Remove worker from the building.
    /// </summary>
    /// <returns></returns>
    //public override void RemoveWorkers(string workSite = "")
    //{
    //    GameObject tempWorker = null;
    //    //Just for default.
       
    //    //For the case when there the workplace on has on worksite
    //    if (workSite == string.Empty || workSite == "")
    //    {
    //        tempWorker = workerList[0];
    //        workerList.Remove(workerList[0]);
    //    }
    //    else
    //    {
    //        if (workSite == "MetalSite")
    //        {
    //            currentSelectedSiteType = WorkSiteType.MetalScraps;

    //        }
    //        else if (workSite == "ForestSite")
    //        {
    //            currentSelectedSiteType = WorkSiteType.Forest;
    //        }
    //        else if (workSite == "HuntingSite")
    //        {
    //            currentSelectedSiteType = WorkSiteType.Hunting;
    //        }

    //        foreach (GameObject worker in workerList)
    //        {
    //            if (currentSelectedSiteType == worker.GetComponent<Human>().LocationService[LocationTarget.WorkSite].GetComponent<WorkSite>().SiteType)
    //            {
    //                tempWorker = worker;
    //                workerList.Remove(worker);
    //                ManageWorkerCount(currentSelectedSiteType, -1);
    //                tempWorker.GetComponent<Human>().HasJob = false;
    //                Debug.Log("removed worker: " + currentSelectedSiteType +" - current Count = " + ManageWorkerCount(currentSelectedSiteType, 0));
    //                Debug.Log("Worker site info: " + worker.GetComponent<Human>().LocationService[LocationTarget.WorkSite].GetComponent<WorkSite>().SiteType.ToString());
    //                break;
    //            }
    //        }
    //    }
    //    //Sets the removed worker to have no job.
    //    // tempWorker.GetComponent<HumanStateMachine>().ChangeWorkState(null); //TEST
    //}

    ///// <summary>
    ///// Add a worker to the building.
    ///// </summary>
    ///// <param name = "amount" ></ param >
    //public override bool AddWorker(GameObject worker)
    //{
    //    if (!GlobalConstants.ChildLabor)
    //    {
    //        if (worker.GetComponent<Human>().GetAgeCategory() == HumanAgeService.AgeCategory.Child)
    //            return false;
    //    }
    //    if (workerList.Count < maxWorkers && worker != null && worker.GetComponent<Human>().HasJob == false)
    //    {
    //        workerList.Add(worker.gameObject);
    //        ManageWorkerCount(currentSelectedSiteType, 1);
    //        Debug.Log("added worker: " + currentSelectedSiteType + " - current Count = " + ManageWorkerCount(currentSelectedSiteType, 0));
    //   //     Debug.Log("Worker site info: " + worker.GetComponent<Human>().LocationService[LocationTarget.WorkSite].GetComponent<WorkSite>().SiteType.ToString());
    //        return true;
    //    }
    //    return false;
    //}    
}
