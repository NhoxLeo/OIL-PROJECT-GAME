using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowProductionEvent : AGameEvent{

    BuildingManager bm;
    string selectedResource;
    GameObject building;
    public override void Awake()
    {
        base.Awake();
        TargetType = "ValuePerMinute";
    }
    private void Start()
    {
        bm = GameObject.Find("Buildings").GetComponent<BuildingManager>();
        ResourceTargets = new string[6];
        //resourceTargets[0] = "CookedFood";
        ResourceTargets[1] = "Food";
        ResourceTargets[2] = "Water";
        //resourceTargets[3] = "CleanWater";
        ResourceTargets[3] = "Wood";
        ResourceTargets[4] = "Oil";
        ResourceTargets[5] = "Steel";
        selectedResource = ResourceTargets[Random.Range(0, ResourceTargets.Length)];

        building = GameObject.Find("Buildings").GetComponent<BuildingManager>().GetRndBuilding();

        DeltaValue = 2f;
        BoostValue = 1;
        Duration = 1;
        DeltaValue *= baseDeltaValue;
        Duration *= baseBuffDuration;
        BoostValue *= baseBoostValue;

        eventContext = $"After a series of cracks and rumbles the production of {selectedResource}!";

        buttonOneContext = "These things happen. Shut it down and repair it as fast as possible.";
        buttonOneTooltip = $"Will shut down building temporarily , expect the quickest repair but no production";

        buttonTwoContext = "We can't afford to completely shut down production, keep working with what we can and repair as we go along.";
        buttonTwoTooltip = $"Building will still produce {selectedResource} but at a 40% rate, it will also take longer ";

        buttonThreeContext = $"There is no way we can suspend production of {selectedResource}, work it to the best of out ability, creeks and cracks be damned!";
        buttonThreeTooltip = $"Production will average at 40%, but it will not repair and will eventually break entirely";

    }

    void SetTargetBuilding(string resource)
    {      
        if (resource == "Food")
            bm.GetBuildingsOfType("BuildingCanteen"); //TODO should be hunter/farmer not canteen but hunter not implemented yet
        else if (resource == "Water")
            bm.GetBuildingsOfType("BuildingWaterProduction");       
        else if (resource == "Wood")
            bm.GetBuildingsOfType("BuildingWoodProduction");
        else if (resource == "Oil")
            bm.GetBuildingsOfType("BuildingOilProduction");
        else
            bm.GetBuildingsOfType("BuildingSteelProduction");
    }

    public override bool PreRequisuites()
    {
        if (!CanFire)
            return false;
        if (resources[selectedResource] >= resourceMin)
        return true;
        return false;
    }

    public override void OutCome(int playerChoice)
    {
        if(playerChoice == 0)
        {
            Debug.Log("Not Possible to implement yet");
        }
        else if (playerChoice == 1)
        {
            Debug.Log("Not Possible to implement yet");
        }
        else
        {
            Debug.Log("Not Possible to implement yet");
        }
    }
    public override Dictionary<string, float> GetPlayerChoiceAResource()
    {      
        return null;
    }

    public override Dictionary<string, float> GetPlayerChoiceBResource()
    {      
        return null;
    }

    public override Dictionary<string, float> GetPlayerChoiceCResource()
    {
        return null;
    }
}
