using System.Collections.Generic;
using UnityEngine;

public class HumanInteractableBuildings
{
    private List<GameObject> Canteens { get; set; }
    private List<GameObject> WaterWells { get; set; }

    private readonly float range;

    public HumanInteractableBuildings()
    {          
    }

    public GameObject GetClosestCanteen(Human _human)
    {
        float distance = 0;
        float closestDist = float.MaxValue;
        GameObject selectedInstance = null;
        Canteens = new List<GameObject>();
        WaterWells = new List<GameObject>();


        foreach (GameObject building in GameObject.Find("Buildings").GetComponent<BuildingManager>().GetBuildingsOfType("BuildingCanteen(Clone)"))
        {
            Canteens.Add(building);
        }

        foreach (GameObject canteen in Canteens)
        {
            distance = Vector3.Distance(_human.transform.position, canteen.transform.position);
            if (distance < closestDist)
            {
                closestDist = distance;
                selectedInstance = canteen;
            }
        }
        return selectedInstance;
    }

    public GameObject GetClosestWaterWell(Human _human)
    {
        float distance = 0;
        float closestDist = float.MaxValue;
        GameObject selectedInstance = null;

        foreach (GameObject building in GameObject.Find("Buildings").GetComponent<BuildingManager>().GetBuildingsOfType("BuildingWaterProduction(Clone)"))
        {
            WaterWells.Add(building);
        }

        foreach (GameObject waterwell in WaterWells)
        {
            distance = Vector3.Distance(_human.transform.position, waterwell.transform.position);
            if (distance < closestDist)
            {
                closestDist = distance;
                selectedInstance = waterwell;
            }
        }
        return selectedInstance;
    }

}
