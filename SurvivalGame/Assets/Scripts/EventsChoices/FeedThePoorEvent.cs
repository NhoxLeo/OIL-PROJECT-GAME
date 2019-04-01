using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedThePoorEvent : AGameEvent
{

    public override void Awake()
    {
        base.Awake();
        TargetType = "FlatValue";
    }
    private void Start()
    {
        ResourceTargets = new string[2];
        ResourceTargets[0] = "CookedFood";
        ResourceTargets[1] = "Food";

        DeltaValue = 1f;
        BoostValue = 1;
        Duration = 1;

        DeltaValue *= baseDeltaValue;
        Duration *= baseBuffDuration;
        BoostValue *= baseBoostValue;

        resourceMin = DeltaValue;

        eventContext = "Not everyone can work, and most workers know someone who can't. The morale of your workers might get boosted if you gave those poor souls some food";

        buttonOneContext = "Arms for the poor!";
        buttonOneTooltip = $"Lose {DeltaValue} Cooked Food, gain a {BoostValue}% boost to morale";
        buttonOneResource = $@"+ {BoostValue}% morale
                               - {DeltaValue} Cooked Food";

        buttonTwoContext = "They'll have to make due with raw food.";
        buttonTwoTooltip = $"Lose {DeltaValue} Food";

        buttonThreeContext = "Work for bread, or there shall be no bread";
        buttonThreeTooltip = $"Lose {BoostValue * 2} global happiness";

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

            popMan.GetComponent<PopulationManager>().AddNewMoralBoost((int)BoostValue, (int)Duration);
        }
        else if (playerChoice == 1)
        {
            resourceManager.ResourceConv(ResourceTargets[1], -DeltaValue);
        }
        else
        {
            popMan.GetComponent<PopulationManager>().AddNewMoralBoost((-(int)BoostValue * 2), (int)Duration);
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
