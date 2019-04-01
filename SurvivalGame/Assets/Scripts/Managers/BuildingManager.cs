using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages buildings, currently selected buildings etc.
/// </summary>
public class BuildingManager : MonoBehaviour {
    private GameObject currentlySelectedBuilding;
    private List<GameObject> resourceDepotList = new List<GameObject>();
    private GameObject resourceManager;
   // public GameObject upgradeButton;
    
    private void Awake() {
        //resourceDepotList.Add(GameObject.Find("BuildingResourceDepot").gameObject);
        resourceManager = GameObject.Find("ResourceManager");
    }

    public void Update() {  // TODO: Fix or remove this one.
        //if (currentlySelectedBuilding) {
        //    int[] cost = currentlySelectedBuilding.GetComponent<Building>().GetCost();
        //    if (!currentlySelectedBuilding.tag.Contains("SpaceShip")) {
        //        if (!currentlySelectedBuilding.tag.Contains("BuildingAcademy"))
        //        {
        //            //if (cost[0] <= resourceManager.GetComponent<ResourceManager>().Wood && cost[1] <= resourceManager.GetComponent<ResourceManager>().Steel)
        //                //upgradeButton.GetComponent<Button>().interactable = true;
        //           // else
        //               // upgradeButton.GetComponent<Button>().interactable = false;
        //        }
        //    }            
        //}           //more errors
    }

    /// <summary>
    /// Sets the currently selected building, letting other scripts reach that feature.
    /// </summary>
    /// <param name="selectedBuilding">The selected building to be set.</param>
    public void SetCurrentlySelectedBuilding(GameObject selectedBuilding) {
        currentlySelectedBuilding = selectedBuilding;
    }

    /// <summary>
    /// Override to deselect building.
    /// </summary>
    /// <param name="none"></param>
    public void SetCurrentlySelectedBuilding(bool none) {
        if (none)
            currentlySelectedBuilding = null;
    }

    public List<GameObject> GetBuildingsOfType(string buildingName) {
        List<GameObject> buildingTypeList = new List<GameObject>();
        foreach (Transform building in transform) {
            if (building.gameObject.name == buildingName) {
                buildingTypeList.Add(building.gameObject);
            }
        }
        return buildingTypeList;
    }

    /// <summary>
    /// Returns the list of resource depots available.
    /// </summary>
    /// <returns></returns>
    public List<GameObject> GetResourceDepotList() {
        return resourceDepotList;
    }

    /// <summary>
    /// Notifies the Building Manager of a new Resource Depot being built.
    /// </summary>
    /// <param name="resourceDepot">The new Resource Depot</param>
    public void AddResourceDepot(GameObject resourceDepot) {
        resourceDepotList.Add(resourceDepot);
    }

    public void RemoveResourceDepot(GameObject resourceDepot) {
        resourceDepotList.Remove(resourceDepot);
    }

    /// <summary>
    /// Returns the currently selected building.
    /// </summary>
    /// <returns></returns>
    public GameObject GetSelectedBuilding() {
        return currentlySelectedBuilding;
    }

    /// <summary>
    /// Destroys the currently selected building.
    /// </summary>
    public void DestroyBuilding() {
        currentlySelectedBuilding = Camera.main.GetComponent<PlayerInteraction>().GetSelectedBuilding().gameObject;
        switch (currentlySelectedBuilding.tag) {
            case "BuildingResidence":
                currentlySelectedBuilding.GetComponent<BuildingHome>().DestroyBuilding();
                break;
            case "BuildingProduction":
                currentlySelectedBuilding.GetComponent<BuildingProduction>().DestroyBuilding();
                break;
            default:
                currentlySelectedBuilding.GetComponent<Building>().DestroyBuilding();
                break;
        }
        currentlySelectedBuilding = null;
    }

    /// <summary>
    /// Upgrades the currently selected building.
    /// </summary>
    public void UpgradeBuilding() {
        currentlySelectedBuilding = Camera.main.GetComponent<PlayerInteraction>().GetSelectedBuilding().gameObject;
        currentlySelectedBuilding.GetComponent<Building>().Upgrade();
    }

    /// <summary>
    /// Repairs the currently selected building.
    /// </summary>
    public void RepairBuilding() {
        currentlySelectedBuilding = Camera.main.GetComponent<PlayerInteraction>().GetSelectedBuilding().gameObject;
        currentlySelectedBuilding.GetComponent<Building>().RepairBuilding();
    }

    /// <summary>
    /// Turns on lights in case of eclipse or sandstorm.
    /// </summary>
    /// <param name="on">Lights on or off.</param>
    public void TurnOnOffLights(bool on) {
        foreach (Transform building in transform) {
            if (building.childCount > 0) {
                foreach (Transform child in building.transform) {
                    if (child.tag == "Truck")
                        child.GetComponent<Truck>().TurnOnOffSpotlight(on);
                    else if (child.tag == "Spotlight")
                        child.gameObject.SetActive(on);
                }
            }           
        }
    }

    /// <summary>
    /// Destroys a building.
    /// </summary>
    /// <param name="randomBuilding">Will the script pick a random building to destroy?</param>
    public void BreakDownBuilding(bool randomBuilding = true) {
        GameObject chosenBuilding;
        for (int tries = 0; tries < 10; tries++) {
            chosenBuilding = transform.GetChild(Random.Range(0, transform.childCount)).gameObject;
            if (chosenBuilding.tag != "BuildingResourceDepot" && chosenBuilding.tag != "BuildingAlwaysWorking" && !chosenBuilding.GetComponent<Building>().IsBroken() && (GlobalConstants.currentStage != GlobalConstants.Stage.RUINS || chosenBuilding.tag != "BuildingSpaceShip")){
                chosenBuilding.GetComponent<Building>().SetBrokenStatus(true);
                break;
            }
        }
    }

    public GameObject GetRndBuilding()
    {
        GameObject chosenBuilding;
        for (int tries = 0; tries < 10; tries++)
        {
            chosenBuilding = transform.GetChild(Random.Range(0, transform.childCount)).gameObject;
            if (chosenBuilding.tag != "BuildingResourceDepot" && chosenBuilding.tag != "BuildingAlwaysWorking" && !chosenBuilding.GetComponent<Building>().IsBroken() && (GlobalConstants.currentStage != GlobalConstants.Stage.RUINS || chosenBuilding.tag != "BuildingSpaceShip"))
            {
                return chosenBuilding;
            }
        }
        return null;
    }

    /// <summary>
    /// Switches on/off raycasting for buildings.
    /// </summary>
    /// <param name="enabled">Raycasting on if enabled == true.</param>
    public void SwitchRaycast(bool enabled) {
        if (!enabled)
            foreach (Transform building in transform)
                building.gameObject.layer = 2;
        else
            foreach (Transform building in transform)
                building.gameObject.layer = 0;
    }

    /// <summary>
    /// Checks if the buildings collides with any other buildings.
    /// </summary>
    /// <param name="placeableBuildingBounds"></param>
    /// <returns>The bounds of the collider.</returns>
    public bool CheckCollisions(Bounds placeableBuildingBounds) {
        foreach (Transform building in transform) {
            if (building.GetComponent<BoxCollider>().bounds.Intersects(placeableBuildingBounds))
                return false;
        }
        return true;
    }
}
