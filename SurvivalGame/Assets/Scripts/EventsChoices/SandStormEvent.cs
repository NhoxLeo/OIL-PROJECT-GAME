using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStormEvent : AGameEvent
{

    Sandstorm script;
   
    public override void Awake()
    {
        base.Awake();
        TargetType = "WorldEvent";
    }
    void Start()
    {
      
        CanFire = true;
        Duration = 5;
        timerInterval = 120;
        //script = GameObject.Find("Sandstorm_Particles").GetComponent<Sandstorm>();
        script = GameObject.Find("Sandstorm").transform.Find("Sandstorm_Particles").GetComponent<Sandstorm>();

        eventContext = "Winds are roaoring and the sands are shifting";
        buttonOneContext = "Find shelter quick!";
        buttonOneTooltip = "There is nothing you can do."; // TODO implement sandstorm logic other than visuals

    }

    public override bool PreRequisuites()
    {
        if (!CanFire)
            return false;
        return true;
    }

    public override void OutCome(int playerChoice)
    {
        script.SetSandstorm();
        GameObject.Find("Sandstorm").transform.Find("Sandstorm_Particles").gameObject.SetActive(true);
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
