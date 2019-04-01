using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// choiceless events are the more powerful, but ultimately rarer events. Seperated from the normal ones for simplicity codewise, but also because the layout 
/// </summary>
public class EclipseEvent : AGameEvent
{   
    Eclipse script;
    int length;

    public override void Awake()
    {
        base.Awake();
        TargetType = "WorldEvent";
    }
    void Start()
    {
      
        Duration = 5;
        script = GameObject.Find("Sun").GetComponent<Eclipse>();
        timerInterval = 120;
        eventContext = "Old scars of cosmic disarray... take shelter as the the notorious sun goes to sleep. For the night is cold and dark.";
        buttonOneContext = "Ok";
        buttonOneTooltip = "Without the suns rays the temperature will fall and Farms won't work.";
    }

    public override bool PreRequisuites()
    {
        if (!CanFire)
            return false;
        return true;
    }

    public override void OutCome(int playerChoice)
    {
        script.SetEclipse();
        script.enabled = true;
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
