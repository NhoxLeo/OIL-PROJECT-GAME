using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// tempClass for stage evenets, 
/// </summary>
public class StageOneEvent : AGameEvent {

    
   public override void Awake() 
    {
       // base.Awake();
        TargetType = "Stage";
        Name = "StageOne";
    }

    // Use this for initialization
    void Start () {

        eventContext = "Your workers has stumbled upon an old ruin. Through the rubble an old structure was discovered." +
            "You should investigate as soon as you can!";
        buttonOneContext = "Ok";
    }

    public override bool PreRequisuites()
    {
        if (!CanFire)
            return false;
        return true;
    }

    public override void OutCome(int playerChoice)
    {
        Start();
        GlobalConstants.currentStage = GlobalConstants.Stage.STAGE1;
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
