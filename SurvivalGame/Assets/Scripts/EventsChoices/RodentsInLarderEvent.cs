using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodentsInLarderEvent : AGameEvent
{
    int minimumResourceValue;

    public override void Awake()
    {
        base.Awake();
        TargetType = "FlatValue";
    }
    private void Start()
    {
        ResourceTargets = new string[2];
        ResourceTargets[0] = "Wood";
        ResourceTargets[1] = "Food";

        DeltaValue = 3f;
        BoostValue = 1;
        Duration = 2;

        DeltaValue *= baseDeltaValue;
        Duration *= baseBuffDuration;
        BoostValue *= baseBoostValue;

        resourceMin = DeltaValue;

        eventContext = "Rodents have infested a storage unit! We've found where these foul vermin gnawed their way in," +
            " but maybe we can still benefit from this incidient. We could either exterminate these rodents before they do further harm and patch the hole." +
            " Or we can trap them, rats are a great source of protein after all.";

        buttonOneContext = "Eeew, burn them all!";
        buttonOneTooltip = $"Lose {DeltaValue} {ResourceTargets[0]}";

        buttonTwoContext = "M'eh, I've had worse, more food for us!";
        buttonTwoTooltip = $"Gain {DeltaValue} {ResourceTargets[1]} but lose {BoostValue * 2} global happiness";

        buttonThreeContext = "Leave it be!";
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
        Start(); //TODO fix
        if (playerChoice == 0)
            resourceManager.ResourceConv(ResourceTargets[0], -DeltaValue);
        else if (playerChoice == 1)
        {
            resourceManager.ResourceConv(ResourceTargets[1], DeltaValue);
            popMan.GetComponent<PopulationManager>().AddNewMoralBoost(-(int)BoostValue * 2, (int)Duration);
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
        dic.Add(ResourceTargets[1], DeltaValue);
        return dic;
    }

    public override Dictionary<string, float> GetPlayerChoiceCResource()
    {
        return null;
    }
}
