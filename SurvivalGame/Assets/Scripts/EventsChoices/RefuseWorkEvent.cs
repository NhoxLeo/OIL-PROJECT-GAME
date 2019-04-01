using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuseWorkEvent : AGameEvent
{

    List<Human> workers;

    public override void Awake()
    {
        base.Awake();
        TargetType = "FlatValue";
    }
    void Start()
    {
        
        ResourceTargets = new string[1];
        ResourceTargets[0] = "CookedFood";
        workers = GameObject.Find("Population").GetComponent<PopulationManager>().GetHumans(100); //TODO better to use lower moral

        //DeltaValue = -10;
        //BoostValue = -15;
        //Duration = 10;
        DeltaValue = 4f;
        BoostValue = 2;
        Duration = 1.5f;
        DeltaValue *= baseDeltaValue;
        Duration *= baseBuffDuration;
        BoostValue *= baseBoostValue;

        resourceMin = DeltaValue;

        eventContext = "All work work work, NO MORE - The Union";

        buttonOneContext = "It's ok, let them take a break.";
        buttonOneTooltip = $"Some workers go home";

        buttonTwoContext = "Although we are fighting for our survival, its important that the population enjoys itself. Provide them some extra food for them tonight!";
        buttonTwoTooltip = $"Use {DeltaValue} of {ResourceTargets[0]} and gain {BoostValue} global happiness";

        buttonThreeContext = "Hmpf...Fetch the blackjacks...";
        buttonThreeTooltip = $"Lose {BoostValue} global happiness";
    }

    public override bool PreRequisuites()
    {
        if (!CanFire)
            return false;
        if (workers.Count > 0 || resources[ResourceTargets[0]] >= resourceMin)
            return true;
        return false;
    }

    public override void OutCome(int playerChoice)
    {
        if (playerChoice == 0)
        {
            foreach (Human human in workers)
            {
                if (human.GetMoral() <= 80)
                {
                    human.UpdateNeed(GlobalConstants.Needs.HOME, 100); // TODO verify production is hit and verify human count
                }
            }
        }
        else if (playerChoice == 1)
        {
            resourceManager.ResourceConv(ResourceTargets[0], -DeltaValue);

            popMan.GetComponent<PopulationManager>().AddNewMoralBoost((int)BoostValue, (int)Duration);
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

        return null;
    }

    public override Dictionary<string, float> GetPlayerChoiceBResource()
    {
        Dictionary<string, float> dic = new Dictionary<string, float>();
        dic.Add(ResourceTargets[1], DeltaValue);
        return null;
    }

    public override Dictionary<string, float> GetPlayerChoiceCResource()
    {
        return null;
    }
}
