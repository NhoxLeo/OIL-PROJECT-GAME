using System.Collections.Generic;
using UnityEngine;

public class GoToAction : HumanAIState
{    
   
    public virtual void Enter(GameObject owner, string enteringState)
    {
        base.Enter(owner, enteringState);        
    }

    public override void ExecuteState()
    {
        base.ExecuteState();
    }

    //protected bool IsWithinRangeToTarget(GameObject agentTarget)
    //{
    //    if (agentTarget != null)
    //    {
    //        float distance = Vector3.Distance(_owner.transform.position, agentTarget.transform.position);
    //        if (distance < agentTarget.GetComponent<Building>().GetInteractionRadius())
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    private void NormalExitConditions(Human _humanScript)
    {
        //Exit conditions
        if (_humanScript.GetComfort() <= 0f)
        {
        }
    }

    private void OverTimeExitConditions(Human _humanScript)
    {
        //Exit conditions
        if (_humanScript.GetComfort() < -75)
        {           
            Exit(GetAIComponents.RESTING);
        }
    }

    protected override void DoReachedTargetLogic()
    {
        if (CurrentLocationTarget == LocationTarget.OccupationBuilding)
        {
            move.TryHaltAndHide();
        }
    }

    public override void Exit(string newState)
    {
        base.Exit(newState);

    }

}
