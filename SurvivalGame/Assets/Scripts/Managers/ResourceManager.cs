using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager class taking care of the resource management, UI and interactions.
/// </summary>
public class ResourceManager : MonoBehaviour
{
    Dictionary<string, float> resources = new Dictionary<string, float>();
    GameObject buildingManager;
    GameManager gm;
    public Text[] resourceUI;
    private int resourceCap;
    public int Oil { get { return (int)resources["Oil"]; } }
    public int Steel { get { return (int)resources["Steel"]; } }
    public int Wood { get { return (int)resources["Wood"]; } }
    public int Water { get { return (int)resources["Water"]; } }
    public int Food { get { return (int)resources["Food"]; } }
    public int Population { get { return (int)resources["Population"]; } }
    public int CleanWater { get { return (int)resources["CleanWater"]; } }
    public int CookedFood { get { return (int)resources["CookedFood"]; } }
    [SerializeField]
    private int oil, water, food, steel, wood, cleanWater, cookedFood, population = 0;

    //List<string> allResources = new List<string>();

    public void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        FillResourceTable();
        //FillResourceList();
        GameObject populationList = GameObject.Find("Population");
        buildingManager = GameObject.Find("Buildings");
        foreach (Transform human in populationList.transform)
        {
            resources["Population"]++;
        }
    }

    /// <summary>
    /// fills a lookup table for all resources (updating this and the index table below,
    /// will automaticly update the event system with new resources or state of current resources
    /// </summary>
    void FillResourceTable()
    {
        resources.Add("Oil", oil);
        resources.Add("Water", water);
        resources.Add("Food", food);
        resources.Add("Steel", steel);
        resources.Add("Wood", wood);
        resources.Add("CleanWater", cleanWater);
        resources.Add("CookedFood", cookedFood);
        resources.Add("Population", population);
    }

    ///// <summary>
    ///// View as an index table for the resource dictionary (add all resources in the game)
    ///// </summary>
    //void FillResourceList()
    //{
    //    allResources.Add("Oil");
    //    allResources.Add("Water");
    //    allResources.Add("Food");
    //    allResources.Add("Steel");
    //    allResources.Add("Wood");
    //    allResources.Add("CleanWater");
    //    allResources.Add("CookedFood");
    //    allResources.Add("Population");
    //}

    /// <summary>
    /// Gets a table with all resources. Default call -> resources["Resource"]
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, float> GetResourceValues()
    {
        return resources;
    }

    /// <summary>
    /// Sets new (updated) table 
    /// </summary>
    /// <param name="_resources"></param>
    public void SetResourceValues(Dictionary<string, float> _resources)
    {
        resources = _resources;
    }

    /// <summary>
    /// COnverts from resources dictionary to enum
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void ResourceConv(string key, float value)
    {
        Array enumRes = Enum.GetValues(typeof(GlobalConstants.Resources));
        foreach (GlobalConstants.Resources item in enumRes)
            if (key.ToUpper() == item.ToString())
                ManipulateResources(item, value);
    }
  

    //public List<string> GetAllResourceNames()
    //{
    //    return allResources;
    //}

    public float GetSingleResource(string resourceName)
    {
        //Array enumRes = Enum.GetValues(typeof(GlobalConstants.Resources));
        //foreach (GlobalConstants.Resources item in enumRes)
        //    if (resourceName.ToUpper() == item.ToString())
                return resources[$"{resourceName}"];
    }

    public void Update()
    {
        UpdateResourceUI();
    }

    public bool CheckIfViableResourceChange(GlobalConstants.Resources resource, float amount)
    {
        bool canChange = false;
        switch (resource)
        {
            case GlobalConstants.Resources.WATER:
                if ((resources["Water"] + amount >= 0 || amount >= 0) && resources["Water"] + amount <= resourceCap)
                    canChange = true;
                else if (amount < 0 && resources["Water"] + amount >= 0)
                    canChange = true;
                else if (resources["Water"] + amount > resourceCap)
                    canChange = true; // but will hit cap
                else
                    canChange = false;
                break;
            case GlobalConstants.Resources.FOOD:
                if ((resources["Food"] + amount >= 0 || amount >= 0) && resources["Food"] + amount <= resourceCap)
                    canChange = true;
                else if (amount < 0 && resources["Food"] + amount >= 0)
                    canChange = true;
                else if (resources["Food"] + amount > resourceCap)
                    canChange = true;
                else
                    canChange = false;
                break;
            case GlobalConstants.Resources.OIL:
                if ((resources["Oil"] + amount >= 0 || amount >= 0) && resources["Oil"] + amount <= resourceCap)                
                    canChange = true;                              
                else if (amount < 0 && resources["Oil"] + amount >= 0)           
                    canChange = true;              
                else if (resources["Oil"] + amount > resourceCap)
                    canChange = true;
                else
                    canChange = false;
                break;
            case GlobalConstants.Resources.STEEL:
                if ((resources["Steel"] + amount >= 0 || amount >= 0) && resources["Steel"] + amount <= resourceCap)
                    canChange = true;
                else if (amount < 0 && resources["Steel"] + amount >= 0)
                    canChange = true;
                else if (resources["Steel"] + amount > resourceCap)
                    canChange = true;
                else
                    canChange = false;
                break;
            case GlobalConstants.Resources.WOOD:
                if ((resources["Wood"] + amount >= 0 || amount >= 0) && resources["Wood"] + amount <= resourceCap)
                    canChange = true;
                else if (amount < 0 && resources["Wood"] + amount >= 0)
                    canChange = true;
                else if (resources["Wood"] + amount > resourceCap)
                    canChange = true;
                else
                    canChange = false;
                break;
            case GlobalConstants.Resources.CLEANWATER:
                if ((resources["CleanWater"] + amount >= 0 || amount >= 0) && resources["CleanWater"] + amount <= resourceCap)
                    canChange = true;
                else if (amount < 0 && resources["CleanWater"] + amount >= 0)
                    canChange = true;
                else if (resources["CleanWater"] + amount > resourceCap)
                    canChange = true;
                else
                    canChange = false;
                break;
            case GlobalConstants.Resources.COOKEDFOOD:
                if ((resources["CookedFood"] + amount >= 0 || amount >= 0) && resources["CookedFood"] + amount <= resourceCap)
                    canChange = true;
                else if (amount < 0 && resources["CookedFood"] + amount >= 0)
                    canChange = true;
                else if (resources["CookedFood"] + amount > resourceCap)
                    canChange = true;
                else
                    canChange = false;
                break;
            case GlobalConstants.Resources.POPULATION:
                canChange = true;
                break;
            default:
                canChange = false;
                break;
        }     
        return canChange;
    }

    /// <summary>
    /// Attempts to remove or add a resource.
    /// </summary>
    /// <param name="resource">The resource to add/remove.</param>
    /// <param name="amount">How much of that resource to add/remove.</param>
    /// <returns></returns>
    public bool ManipulateResources(GlobalConstants.Resources resource, float amount)
    {
        bool success = true;
        switch (resource)
        {
            case GlobalConstants.Resources.WATER:
                if ((resources["Water"] + amount >= 0 || amount >= 0) && resources["Water"] + amount <= resourceCap)
                    resources["Water"] += amount;
                else if (amount < 0 && resources["Water"] + amount >= 0)
                    resources["Water"] += amount;
                else if (resources["Water"] + amount > resourceCap)
                    resources["Water"] = resourceCap;
                else
                    success = false;
                break;
            case GlobalConstants.Resources.FOOD:
                if ((resources["Food"] + amount >= 0 || amount >= 0) && resources["Food"] + amount <= resourceCap)
                    resources["Food"] += amount;
                else if (amount < 0 && resources["Food"] + amount >= 0)
                    resources["Food"] += amount;
                else if (resources["Food"] + amount > resourceCap)
                    resources["Food"] = resourceCap;
                else
                    success = false;
                break;
            case GlobalConstants.Resources.OIL:
                if ((resources["Oil"] + amount >= 0 || amount >= 0) && resources["Oil"] + amount <= resourceCap)
                {
                    resources["Oil"] += amount;
                    //  GameObject.Find("OilText").GetComponent<Text>().GetComponent<OnValueResourceChangeText>().FireText((int)amount); //TODO timer/ only at purchase or event
                }
                else if (amount < 0 && resources["Oil"] + amount >= 0)
                {
                    resources["Oil"] += amount;
                    // GameObject.Find("OilText").GetComponent<Text>().GetComponent<OnValueResourceChangeText>().FireText((int)amount);
                }
                else if (resources["Oil"] + amount > resourceCap)
                    resources["Oil"] = resourceCap;
                else
                    success = false;
                break;
            case GlobalConstants.Resources.STEEL:
                if ((resources["Steel"] + amount >= 0 || amount >= 0) && resources["Steel"] + amount <= resourceCap)
                    resources["Steel"] += amount;
                else if (amount < 0 && resources["Steel"] + amount >= 0)
                    resources["Steel"] += amount;
                else if (resources["Steel"] + amount > resourceCap)
                    resources["Steel"] = resourceCap;
                else
                    success = false;
                break;
            case GlobalConstants.Resources.WOOD:
                if ((resources["Wood"] + amount >= 0 || amount >= 0) && resources["Wood"] + amount <= resourceCap)
                    resources["Wood"] += amount;
                else if (amount < 0 && resources["Wood"] + amount >= 0)
                    resources["Wood"] += amount;
                else if (resources["Wood"] + amount > resourceCap)
                    resources["Wood"] = resourceCap;
                else
                    success = false;
                break;
            case GlobalConstants.Resources.CLEANWATER:
                if ((resources["CleanWater"] + amount >= 0 || amount >= 0) && resources["CleanWater"] + amount <= resourceCap)
                    resources["CleanWater"] += amount;
                else if (amount < 0 && resources["CleanWater"] + amount >= 0)
                    resources["CleanWater"] += amount;
                else if (resources["CleanWater"] + amount > resourceCap)
                    resources["CleanWater"] = resourceCap;
                else
                    success = false;
                break;
            case GlobalConstants.Resources.COOKEDFOOD:
                if ((resources["CookedFood"] + amount >= 0 || amount >= 0) && resources["CookedFood"] + amount <= resourceCap)
                    resources["CookedFood"] += amount;
                else if (amount < 0 && resources["CookedFood"] + amount >= 0)
                    resources["CookedFood"] += amount;
                else if (resources["CookedFood"] + amount > resourceCap)
                    resources["CookedFood"] = resourceCap;
                else
                    success = false;
                break;
            case GlobalConstants.Resources.POPULATION:
                resources["Population"] += amount;
                break;
            default:
                success = false;
                break;
        }
        UpdateResourceUI();
        return success;
    }

    /// <summary>
    /// Updates the resource UI.
    /// </summary>
    private void UpdateResourceUI()
    {
        UpdateCaps();
        resourceUI[0].text = "Oil: " + (int)resources["Oil"];
        resourceUI[1].text = "Water (dirty): " + (int)resources["Water"];
        resourceUI[2].text = "Food (raw): " + (int)resources["Food"];
        resourceUI[3].text = "Steel: " + (int)resources["Steel"];
        resourceUI[4].text = "Wood: " + (int)resources["Wood"];
        resourceUI[5].text = "Water (clean): " + (int)resources["CleanWater"];
        resourceUI[6].text = "Food (cooked): " + (int)resources["CookedFood"];
        resourceUI[7].text = "Population: " + (int)resources["Population"];
        resourceUI[8].text = "Year: " + gm.Year;
        //GameObject selectedBuilding = buildingManager.GetComponent<BuildingManager>().GetSelectedBuilding();
        //if (selectedBuilding)
        //  if (selectedBuilding.tag.Contains("BuildingResourceDepot"))
        //    selectedBuilding.GetComponent<Building>().SetInformationText(GameObject.Find("ObjectInfo").transform.Find("BuildingInfo").gameObject);
    }

    private void UpdateCaps()
    {
        resourceCap = 0;
        foreach (GameObject depot in buildingManager.GetComponent<BuildingManager>().GetResourceDepotList())
            resourceCap += depot.GetComponent<BuildingResourceDepot>().GetLevel() * 100;
        if (resources["Oil"] >= resourceCap * 0.9f)
        {
            resourceUI[0].transform.GetChild(0).gameObject.SetActive(true);
            if (resources["Oil"] >= resourceCap - 1)
                resourceUI[0].transform.GetChild(0).GetComponentInChildren<Text>().text = "Capacity reached!";
            else
                resourceUI[0].transform.GetChild(0).GetComponentInChildren<Text>().text = "Near max capacity";
        }
        else
            resourceUI[0].transform.GetChild(0).gameObject.SetActive(false);

        if (resources["Water"] >= resourceCap * 0.9f)
        {
            resourceUI[1].transform.GetChild(0).gameObject.SetActive(true);
            if (resources["Water"] >= resourceCap - 1)
                resourceUI[1].transform.GetChild(0).GetComponentInChildren<Text>().text = "Capacity reached!";
            else
                resourceUI[1].transform.GetChild(0).GetComponentInChildren<Text>().text = "Near max capacity";
        }
        else
            resourceUI[1].transform.GetChild(0).gameObject.SetActive(false);

        if (resources["Food"] >= resourceCap * 0.9f)
        {
            resourceUI[2].transform.GetChild(0).gameObject.SetActive(true);
            if (resources["Food"] >= resourceCap - 1)
                resourceUI[2].transform.GetChild(0).GetComponentInChildren<Text>().text = "Capacity reached!";
            else
                resourceUI[2].transform.GetChild(0).GetComponentInChildren<Text>().text = "Near max capacity";
        }
        else
            resourceUI[2].transform.GetChild(0).gameObject.SetActive(false);

        if (resources["Steel"] >= resourceCap * 0.9f)
        {
            resourceUI[3].transform.GetChild(0).gameObject.SetActive(true);
            if (resources["Steel"] >= resourceCap - 1)
                resourceUI[3].transform.GetChild(0).GetComponentInChildren<Text>().text = "Capacity reached!";
            else
                resourceUI[3].transform.GetChild(0).GetComponentInChildren<Text>().text = "Near max capacity";
        }
        else
            resourceUI[3].transform.GetChild(0).gameObject.SetActive(false);

        if (resources["Wood"] >= resourceCap * 0.9f)
        {
            resourceUI[1].transform.GetChild(0).gameObject.SetActive(true);
            if (resources["Wood"] >= resourceCap - 1)
                resourceUI[4].transform.GetChild(0).GetComponentInChildren<Text>().text = "Capacity reached!";
            else
                resourceUI[4].transform.GetChild(0).GetComponentInChildren<Text>().text = "Near max capacity";
        }
        else
            resourceUI[4].transform.GetChild(0).gameObject.SetActive(false);

        if (resources["CleanWater"] >= resourceCap * 0.9f)
        {
            resourceUI[5].transform.GetChild(0).gameObject.SetActive(true);
            if (resources["CleanWater"] >= resourceCap - 1)
                resourceUI[5].transform.GetChild(0).GetComponentInChildren<Text>().text = "Capacity reached!";
            else
                resourceUI[5].transform.GetChild(0).GetComponentInChildren<Text>().text = "Near max capacity";
        }
        else
            resourceUI[5].transform.GetChild(0).gameObject.SetActive(false);

        if (resources["CookedFood"] >= resourceCap * 0.9f)
        {
            resourceUI[6].transform.GetChild(0).gameObject.SetActive(true);
            if (resources["CookedFood"] >= resourceCap - 1)
                resourceUI[6].transform.GetChild(0).GetComponentInChildren<Text>().text = "Capacity reached!";
            else
                resourceUI[6].transform.GetChild(0).GetComponentInChildren<Text>().text = "Near max capacity";
        }
        else
            resourceUI[6].transform.GetChild(0).gameObject.SetActive(false);
    }

    // The initial Resource Depot receives resources.
    public float[] GetStartingResources()
    {
        return new float[] { oil, water, food, steel, wood, cleanWater, CookedFood };
    }
}
