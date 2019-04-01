using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContaminatedWaterEvent : AGameEvent
{

    int minimumResourceValue;
    int minValue;
    public override void Awake()
    {
        base.Awake();
        TargetType = "FlatValue";
    }
    private void Start()
    {
        ResourceTargets = new string[4];
        ResourceTargets[0] = "Water";
        ResourceTargets[1] = "CleanWater";
        ResourceTargets[2] = "Wood";
        ResourceTargets[3] = "Oil";


        DeltaValue = 2f;
        BoostValue = 5;
        Duration = 1;
        DeltaValue *= baseDeltaValue;
        Duration *= baseBuffDuration;
        BoostValue *= baseBoostValue;

        minValue = (int)(DeltaValue / 2);
        resourceMin = DeltaValue;

        eventContext = "Debris has spoiled some of our water reservoirs. Carried by the wasteland winds";

        buttonOneContext = "Clean the resecoir and reboil the water";
        buttonOneTooltip = $"Lose {DeltaValue} {ResourceTargets[2]} and {minValue} {ResourceTargets[0]} and gain {minValue} of {ResourceTargets[1]}";
        buttonOneResource = $@"+ {minValue} {ResourceTargets[1]}
        - {DeltaValue} {ResourceTargets[2]}
        - {minValue} {ResourceTargets[0]}";

        buttonTwoContext = "It's fine, clean the resevoir and we'll deal with the water later.";
        buttonTwoTooltip = $"Gain {minValue} {ResourceTargets[0]} Lose {minValue} {ResourceTargets[1]} and {(int)BoostValue} global happiness";

        buttonThreeContext = "We can't spare any extra wood for the boiling, burn some oil instead!";
        buttonThreeTooltip = $"Lose {(int)BoostValue / 3} global happiness for spending the black gold, but gain {minValue} of {ResourceTargets[1]} and get rid of {minValue} {ResourceTargets[0]} and spend {DeltaValue} {ResourceTargets[3]}";
    }

    public override bool PreRequisuites()
    {
        Start();
        if (!CanFire)
            return false;
        //  resources = resourceManager.GetResourceValues();
        if (resources[ResourceTargets[2]] >= resourceMin &&
            resources[ResourceTargets[0]] >= minValue)
            return true;
        else if (resources[ResourceTargets[0]] >= resourceMin &&
            resources[ResourceTargets[1]] >= minValue)
            return true;
        else if (resources[ResourceTargets[0]] >= minValue &&
            resources[ResourceTargets[3]] >= resourceMin)
            return true;
        return false;
    }

    public override void OutCome(int playerChoice)
    {
        Start(); //TODO fix
        if (playerChoice == 0)
        {           
            resourceManager.ResourceConv(ResourceTargets[0], -minValue);
            resourceManager.ResourceConv(ResourceTargets[1], minValue);
            resourceManager.ResourceConv(ResourceTargets[2], -DeltaValue);

        }
        //     resourceManager.SetResourceValues(resourceTargets[playerChoice], DeltaValue);
        else if (playerChoice == 1)
        {
            resourceManager.ResourceConv(ResourceTargets[0], minValue);
            resourceManager.ResourceConv(ResourceTargets[1], -minValue);
            popMan.GetComponent<PopulationManager>().AddNewMoralBoost(-(int)BoostValue, (int)Duration);
        }
        else
        {            
            popMan.GetComponent<PopulationManager>().AddNewMoralBoost(-(int)BoostValue, (int)Duration);
            resourceManager.ResourceConv(ResourceTargets[0], -minValue);
            resourceManager.ResourceConv(ResourceTargets[1], minValue);
            resourceManager.ResourceConv(ResourceTargets[3], -DeltaValue);
        }
    }

    public override Dictionary<string, float> GetPlayerChoiceAResource()
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        dic.Add(ResourceTargets[0], -minValue);
        dic.Add(ResourceTargets[1], minValue);
        dic.Add(ResourceTargets[2], -DeltaValue);

        return dic;
    }

    public override Dictionary<string, float> GetPlayerChoiceBResource()
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        dic.Add(ResourceTargets[0], minValue);
        dic.Add(ResourceTargets[1], -minValue);
        return dic;
    }

    public override Dictionary<string, float> GetPlayerChoiceCResource()
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        dic.Add(ResourceTargets[0], -minValue);
        dic.Add(ResourceTargets[1], minValue);
        dic.Add(ResourceTargets[3], -DeltaValue);
        return dic;
    }
}
