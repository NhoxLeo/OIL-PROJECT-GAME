using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarshDesertWinds : AGameEvent
{
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

        DeltaValue = 2f;
        BoostValue = 1;
        Duration = 1.5f;

      

        DeltaValue *= baseDeltaValue;
        Duration *= baseBuffDuration;
        BoostValue *= baseBoostValue;

        resourceMin = DeltaValue;

        eventContext = "Harsh desert winds have forced our poeple to reenforce their homes. They leave which material to use up to you!";

        buttonOneContext = "Use wood!";
        buttonOneTooltip = $"Lose {DeltaValue} {ResourceTargets[0]}";
        buttonOneResource = $@"- {DeltaValue} {ResourceTargets[0]}";

        buttonTwoContext = "Use Steel!";
        buttonTwoTooltip = $"Lose {DeltaValue} {ResourceTargets[1]}";

        buttonThreeContext = "Let the sand fill the holes!";
        buttonThreeTooltip = $"Lose {BoostValue} global happiness";

    }

    public override bool PreRequisuites()
    {
        Start();
        if (!CanFire)
            return false;
        if (resources[ResourceTargets[0]] >= resourceMin ||
            resources[ResourceTargets[1]] >= resourceMin)
            return true;
        return false;
    }

    public override void OutCome(int playerChoice)
    {
        Start();
        if (playerChoice == 0)
        {
            resourceManager.ResourceConv(ResourceTargets[0], -DeltaValue);
        }
        else if (playerChoice == 1)
        {
            resourceManager.ResourceConv(ResourceTargets[1], -DeltaValue);
        }
        else
        {
            popMan.GetComponent<PopulationManager>().AddNewMoralBoost(-(int)BoostValue, (int)Duration);
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
