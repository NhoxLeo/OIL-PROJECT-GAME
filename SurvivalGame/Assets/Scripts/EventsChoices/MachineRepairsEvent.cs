using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineRepairsEvent : AGameEvent
{

    int minimumResourceValue;
    int maxValue;
    public override void Awake()
    {
        base.Awake();
        TargetType = "FlatValue";
    }
    private void Start()
    {
        ResourceTargets = new string[3];
        ResourceTargets[0] = "Oil";
        ResourceTargets[1] = "CookedFood";
        ResourceTargets[2] = "Wood";

        DeltaValue = .1f;
        BoostValue = 1;
        Duration = 1;

        DeltaValue *= baseDeltaValue;
        Duration *= baseBuffDuration;
        BoostValue *= baseBoostValue;

        maxValue = (int)(DeltaValue * 2);
        resourceMin = DeltaValue;

        eventContext = "Blasted machinery! Due to oilpump malfunctions we've lost power to a refridgerator bank. We're forced to use our oil reserves to gas our stoves until the malfunctions cease or the food will spoil.";

        buttonOneContext = "Extra gas for the stoves!";
        buttonOneTooltip = $"Lose {DeltaValue} {ResourceTargets[0]}";

        buttonTwoContext = "We'll not waste our black gold on petty things such as food, use a fire!";
        buttonTwoTooltip = $"Use {maxValue} of {ResourceTargets[2]} to gain {DeltaValue} of {ResourceTargets[1]} however the charred cabbage also lowers morale, lose {BoostValue / 2} global happiness";

        buttonThreeContext = "It can't be helped, eat it raw, or leave it to rot!";
        buttonThreeTooltip = $"Lose {BoostValue} global happiness";
    }

    public override bool PreRequisuites()
    {
        Start();
        if (!CanFire)
            return false;
        if (resources[ResourceTargets[0]] >= resourceMin)
            return true;
        if (resources[ResourceTargets[2]] >= maxValue)
            return true;
        return false;
    }

    public override void OutCome(int playerChoice)
    {
        Start(); //TODO fix
        if (playerChoice == 0)
            resourceManager.ResourceConv(ResourceTargets[playerChoice], -DeltaValue);
        else if (playerChoice == 1)
        {

            resourceManager.ResourceConv(ResourceTargets[2], -DeltaValue * 2);
            resourceManager.ResourceConv(ResourceTargets[1], DeltaValue);
            popMan.GetComponent<PopulationManager>().AddNewMoralBoost(-(int)BoostValue / 2, (int)Duration);
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
        dic.Add(ResourceTargets[2], -DeltaValue * 2);
        dic.Add(ResourceTargets[1], DeltaValue);
        return dic;
    }

    public override Dictionary<string, float> GetPlayerChoiceCResource()
    {
        return null;
    }


}
