using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPopulationEvent : AGameEvent
{
    int upperValue;
    int lowerValue;
    public override void Awake()
    {
        base.Awake();
        TargetType = "FlatValue";
    }
    private void Start()
    {
        ResourceTargets = new string[4];
        ResourceTargets[0] = "Population";
        ResourceTargets[1] = "CookedFood";
        ResourceTargets[2] = "Food";
        ResourceTargets[3] = "CleanWater";


        DeltaValue = Random.Range(5, 20);
        upperValue = (int)(DeltaValue * 1.5f);
        lowerValue = (int)(DeltaValue * .5f);
        resourceMin = 0;
        timerInterval *= 2;
        

        eventContext = "You are approached by a group of travelers. They have been wandering the sands alone until now. They wish to join your colony.";
        buttonOneContext = "Welcome friends! You must be hungry and thirsty!";
        buttonOneTooltip = $"Gain {upperValue} {ResourceTargets[0]}. But lose {upperValue} {ResourceTargets[1]} and {ResourceTargets[3]}";
        buttonTwoContext = "Welcome, but due to problems of our own, we can't accept all of you.";
        buttonTwoTooltip = $"gain {(int)DeltaValue} of population. But lose {(int)DeltaValue} of {ResourceTargets[1]} and {ResourceTargets[3]}";
        buttonThreeContext = "We'll only accept the strongest ones of you, fight to the death! And we'll eat the rest!";
        buttonThreeTooltip = $"Gain {lowerValue} of population. Gain {(int)(DeltaValue)} of food but lose {DeltaValue} of global happiness ";
        buttonOneResource = $@"+ {upperValue} {ResourceTargets[0]}
                               - {upperValue} {ResourceTargets[1]}
                               - {ResourceTargets[3]} ";
    }

    public override bool PreRequisuites()
    {
        if (!CanFire)
            return false;
        if (resources[ResourceTargets[1]] >= upperValue &&
            resources[ResourceTargets[3]] >= upperValue)
            return true;
        else if (resources[ResourceTargets[1]] >= DeltaValue &&
            resources[ResourceTargets[3]] >= DeltaValue)
            return true;       
        return true;
    }

    public override void OutCome(int playerChoice)
    {
      //  Start();
        if (playerChoice == 0)
        {
            resourceManager.ResourceConv(ResourceTargets[0], upperValue);
            resourceManager.ResourceConv(ResourceTargets[1], -upperValue);
            resourceManager.ResourceConv(ResourceTargets[3], -upperValue);
            GameObject.Find("Population").GetComponent<PopulationManager>().AddPopulationRange(upperValue);
        }
        else if (playerChoice == 1)
        {
            resourceManager.ResourceConv(ResourceTargets[0], DeltaValue);
            resourceManager.ResourceConv(ResourceTargets[1], -DeltaValue);
            resourceManager.ResourceConv(ResourceTargets[3], -DeltaValue);
            GameObject.Find("Population").GetComponent<PopulationManager>().AddPopulationRange((int)DeltaValue);
        }
        else
        {
            resourceManager.ResourceConv(ResourceTargets[0], lowerValue);
            resourceManager.ResourceConv(ResourceTargets[2], (int)(DeltaValue));           
            popMan.GetComponent<PopulationManager>().AddNewMoralBoost(-(int)DeltaValue, (int)Duration);
            GameObject.Find("Population").GetComponent<PopulationManager>().AddPopulationRange(lowerValue);
        }
    }

    public override Dictionary<string, float> GetPlayerChoiceAResource()
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        dic.Add(ResourceTargets[0], DeltaValue);
        dic.Add(ResourceTargets[1], -DeltaValue);
        dic.Add(ResourceTargets[3], -DeltaValue);

        return dic;
    }

    public override Dictionary<string, float> GetPlayerChoiceBResource()
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        dic.Add(ResourceTargets[0], upperValue);
        dic.Add(ResourceTargets[1], -upperValue);
        dic.Add(ResourceTargets[3], -upperValue);

        return dic;
    }

    public override Dictionary<string, float> GetPlayerChoiceCResource()
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        dic.Add(ResourceTargets[0], lowerValue);
        dic.Add(ResourceTargets[2], (int)(DeltaValue));      

        return dic;
    }
}
