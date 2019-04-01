using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBuildingEvent : AGameEvent
{

    int deathChanceWood, deathChanceSteel;
    public override void Awake()
    {
        base.Awake();
        TargetType = "FlatValue";
    }

    void Start()
    {
        ResourceTargets = new string[2];
        ResourceTargets[0] = "Wood";
        ResourceTargets[1] = "Steel";
        DeltaValue = 1f;
        BoostValue = 2;
        Duration = 2;

        DeltaValue *= baseDeltaValue;
        Duration *= baseBuffDuration;
        BoostValue *= baseBoostValue;

        resourceMin = DeltaValue;

        deathChanceWood = 70; // /100
        deathChanceSteel = 30; // 100
        eventContext = "Horrible news! A building is about to collapse!"; //ToDO % to dmg people
        buttonOneContext = "Oh no!";
        buttonOneTooltip = "A building has collapsed";
        buttonOneResource = "-1 Building";

        buttonTwoContext = "Quick people, reenforce that building!";
        buttonTwoTooltip = $"The building might be saved if we reenforce it with {DeltaValue}, but there is a {deathChanceWood}% chance it still collapses";

        buttonThreeContext = "Again?! Fix it properly this time!";
        buttonThreeTooltip = $"Using {DeltaValue} steel however might just do the trick, there is a {deathChanceSteel}% chance it still breaks";
    }

    public override bool PreRequisuites()
    {
        Start();
        if (GameObject.Find("Buildings").transform.childCount > 3)
        {
            if (resources[ResourceTargets[0]] >= resourceMin)
                return true;
            else if (resources[ResourceTargets[1]] >= resourceMin)
                return true;
        }
        else
            return false;
        return false;

    }

    public override void OutCome(int playerChoice)
    {

        Start();
        if (playerChoice == 0)
        {
            GameObject.Find("Buildings").GetComponent<BuildingManager>().BreakDownBuilding();
        }
        else if (playerChoice == 1)
        {
            resourceManager.ResourceConv(ResourceTargets[0], -DeltaValue);
            if (Random.Range(0, 100) < deathChanceWood)
                GameObject.Find("Buildings").GetComponent<BuildingManager>().BreakDownBuilding();

        }
        else
        {
            if (Random.Range(0, 100) < deathChanceSteel)
                GameObject.Find("Buildings").GetComponent<BuildingManager>().BreakDownBuilding();
            resourceManager.ResourceConv(ResourceTargets[1], -DeltaValue);
            //  popMan.GetComponent<PopulationManager>().AddNewMoralBoost(-(int)BoostValue, (int)Duration);
        }
    }

    public override Dictionary<string, float> GetPlayerChoiceAResource()
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        dic.Add(ResourceTargets[0], -DeltaValue);
        return dic;
    }

    public override Dictionary<string, float> GetPlayerChoiceBResource()
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        dic.Add(ResourceTargets[1], -DeltaValue);
        return dic;
    }

    public override Dictionary<string, float> GetPlayerChoiceCResource()
    {
        return null;
    }
}
