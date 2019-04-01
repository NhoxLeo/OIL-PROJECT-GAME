using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// General class that is parent to all production buildings.
/// </summary>
public class BuildingProduction : Building
{
    protected GameObject buildingManager;
    protected GlobalConstants.Resources currentResource;
    protected float resourceMultiplier = 0.25f;
    protected int maxWorkers, activeWorkers;
    protected float productionTime, productionTimeLeft;
    protected List<GameObject> workerList;
    protected List<GameObject> truckList;
    private GameObject population;
    protected bool produceWithoutWorkers = false;
    [SerializeField]
    public GameObject workSite = null;
    double timer;
    [SerializeField]
    int interval = 0;
    int reset = 0 ;

    public new void Start()
    {
        activeWorkers = 0;
        workerList = new List<GameObject>();
        population = GameObject.Find("Population");
        buildingManager = GameObject.Find("Buildings");
        if (GameObject.Find("Sun").GetComponent<Eclipse>().enabled || GameObject.Find("Sandstorm").transform.GetChild(0).gameObject.activeSelf == true)
        {
            if (truckList != null)
            {
                foreach (GameObject truck in truckList)
                {
                    truck.GetComponent<Truck>().TurnOnOffSpotlight(true);
                }
            }
            foreach (Transform light in transform)
            {
                if (light.tag == "Spotlight")
                    light.gameObject.SetActive(true);
            }
        }
        if (interval == 0)
        {
            interval = 1;
        }
        base.Start();
    }

    public override void Update()
    {
        if (isOperational && !isBroken && isConstructed)
        {
            if (workerList.Count > 0 || produceWithoutWorkers)
            {
                productionTimeLeft -= Time.deltaTime;
                if (productionTimeLeft < 0)
                {
                    ProduceResource();
                    productionTimeLeft = productionTime;
                }
            }

            timer += Time.deltaTime;
            if(timer > interval)
            {
                GameObject.Find("Population").GetComponent<PopulationManager>().ValidateWorkerList(ref workerList);
                timer = reset;
            }
            
            base.Update();
        }
    }

    /// <summary>
    /// Spawns trucks to the building.
    /// </summary>
    /// <param name="amount">Amount of trucks to spawn.</param>
    /// <param name="workSite">Where the truck picks up resources (forest, ruins etc.).</param>
    /// <param name="deliverySite">Where the truck delivers the resources (sawmill, steelRefinery etc.)</param>
    protected void SpawnTrucks(int amount, Transform workSite, Transform deliverySite)
    {
        if (truckList == null)
            truckList = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            truckList.Add(Instantiate(Resources.Load("Prefabs/Humans/TruckPrefab"), deliverySite.position, Quaternion.identity, deliverySite) as GameObject);
        }
        foreach (GameObject truck in truckList)
            truck.GetComponent<Truck>().Initialize(workSite, deliverySite);
    }

    /// <summary>
    /// This worker begins his/her shift.
    /// </summary>
    public void CheckInWorker()
    {
        activeWorkers++;
        //Debug.Log("[CheckInWorker] " + " checked in workers: " +  activeWorkers +  " TotalWorkers " + workerList.Count);
    }

    /// <summary>
    /// This worker ends his/her shift.
    /// </summary>
    public void CheckOutWorker()
    {
        activeWorkers--;
        //Debug.Log("[CheckOutWorker] " + " checked in workers: " + activeWorkers + " TotalWorkers " + workerList.Count);
    }

    /// <summary>
    /// Produces resources related to the building.
    /// </summary>
    protected virtual void ProduceResource()
    {
        float amount = resourceMultiplier * activeWorkers * buildingLevel;
        if (amount > 0)
            resourceManager.GetComponent<ResourceManager>().ManipulateResources(currentResource, (int)amount);
    }

    /// <summary>
    /// Add a worker to the building.
    /// </summary>
    /// <param name="amount"></param>
    public virtual bool AddWorker(GameObject worker)
    {
        if (!GlobalConstants.ChildLabor)
        {
            if (worker.GetComponent<Human>().GetAgeCategory() == HumanAgeService.AgeCategory.Child)
                return false;
        }
        if (workerList.Count < maxWorkers && worker != null)
        {
            workerList.Add(worker.gameObject);
            return true;
        }
        return false;
    }


    /// <summary>
    /// Remove worker from the building.
    /// </summary>
    /// <returns></returns>
    public virtual void RemoveWorkers(string workSite = "")
    {
        GameObject tempWorker = null;
        //Just for default.
        WorkSiteType workSiteType = WorkSiteType.Forest;
        //For the case when there the workplace on has on worksite
        if (workSite == string.Empty)
        {
            tempWorker = workerList[0];
            workerList.Remove(workerList[0]);
        }
        else
        {
            if (workSite == "MetalSite")
            {
                workSiteType = WorkSiteType.MetalScraps;
            }
            else if (workSite == "Forest")
            {
                workSiteType = WorkSiteType.Forest;
            }
            else if (workSite == "HuntingSite")
            {
                workSiteType = WorkSiteType.Hunting;
            }
            foreach (GameObject worker in workerList)
            {
                if (workSiteType == worker.GetComponent<Human>().LocationService[LocationTarget.WorkSite].GetComponent<WorkSite>().SiteType)
                {
                    tempWorker = worker;
                    workerList.Remove(worker);
                    break;
                }
            }
        }
        //Sets the removed worker to have no job.
        tempWorker.GetComponent<HumanStateMachine>().ChangeWorkState(null);
    }

    /// <summary>
    /// Returns this building's resource.
    /// </summary>
    /// <returns></returns>
    public GlobalConstants.Resources GetResType()
    {
        return currentResource;
    }


    public override void SetBuildingUIType(GameObject buildingTypes)
    {
        foreach (Transform item in buildingTypes.transform)
        {
            var UIbuilding = item.gameObject;
            if (UIbuilding.name != "ProductionBuilding")
                UIbuilding.SetActive(false);
            else
                UIbuilding.SetActive(true);
        }
    }
    /// <summary>
    /// Displays info about the building to the GUI.
    /// </summary>
    /// <param name="objectInfo">Panel of GUI</param>
    public override void SetInformationText(GameObject objectInfo)
    {
        objectInfo.transform.Find("ClickedObjectInfo").GetComponent<Text>().text = name + "\nLevel: " + buildingLevel + "\nWorkers: " + workerList.Count + "/" + maxWorkers;
    }

    /// <summary>
    /// Clears worker list and destroys the building.
    /// </summary>
    public override void DestroyBuilding()
    {
        population.GetComponent<PopulationManager>().MakeUnemployed(workerList);
        foreach (GameObject human in workerList.ToList())
        {
            human.GetComponent<HumanStateMachine>().ChangeWorkState(null);
        }
        base.DestroyBuilding();
    }

    protected void DestroyOnUpgrade()
    {
        base.DestroyBuilding();
    }

    /// <summary>
    /// A worker quits the job.
    /// </summary>
    /// <param name="worker"></param>
    public void QuitJob(GameObject worker)
    {
        workerList.Remove(worker);
    }

    public void ManipulateResourceMultiplier(float amount)
    {
        resourceMultiplier += amount;
    }
}
