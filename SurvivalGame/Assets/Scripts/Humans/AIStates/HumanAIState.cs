using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState : MonoBehaviour
{
    public event EventHandler<FindNextState> OnExitState;

    protected GameObject _owner;
    protected string currentState;
    public string newState;
    protected bool targetLocationChanged;
    public bool IsComplete { get; set; }
   
   
    public string id;
    protected bool initiated;

   

    public virtual void Enter(GameObject owner, string enteringState)
    {
        currentState = enteringState;
        _owner = owner;
    }

    public virtual void ExecuteState() { }

    public virtual void Exit(string oldNewState)
    {           
        OnExitState.Invoke(this, new FindNextState(oldNewState, gameObject.GetComponent<Human>()));
    }    
}

public abstract class HumanAIState : AIState
{
    //protected Human _humanScript;
    protected LocationTarget CurrentLocationTarget { get; set; }
    protected GetAIComponents states;
    protected MovementHandler move;
    protected Human _humanScript;
   // public GOAPMachine goap;
   // public GoalMachine goalMachine;

    protected Dictionary<string, bool> prereqs, outcomes;
    private void Start()
    {
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();      
        //  states = GameObject.Find("GoapAndActionPool").GetComponent<GetAIComponents>();            
    }
    public virtual Dictionary<string, bool> PreRequisites()
    {
        return prereqs;
    }

    public virtual Dictionary<string, bool> Outcomes()
    {
        return outcomes;
    }

    public override void Enter(GameObject owner, string enteringState)
    {
        base.Enter(owner, enteringState);
        _humanScript = gameObject.GetComponent<Human>();
        move = gameObject.GetComponent<MovementHandler>();
        CurrentLocationTarget = move.Target;
        // _humanScript = _owner.GetComponent<Human>();
        //_humanScript = gameObject.GetComponent<Human>();
        // goap = GameObject.Find("GoapAndActionPool").GetComponent<GOAPMachine>();
        // goalMachine = GetGoalMAchine.GetNewGoalMachine();
        //  goalMachine = new GoalMachine();
        //  goalMachine.SetHumanOwner(_humanScript);
        // goalMachine.UpdateGoalsAndConditions();
    }

    public override void ExecuteState()
    {
      //  goalMachine.UpdateGoalsAndConditions(); // may want to lower update frequency
      //  newState = goap.PlanActionSequence(goalMachine.HumanNeeds, goalMachine.Goals).id;
        //if(currentState != newState)
        //{
        //    Exit("IDLE");
        //}
        //if (targetLocationChanged)
        //{
        //    CheckIfNearEnterableTarget(CurrentLocationTarget);
        //}
        move.CheckPoximity();
        if (move.DoOnTargetAction)
        {
            CurrentLocationTarget = move.Target;
            DoReachedTargetLogic();
        }
      
    }

    public override void Exit(string oldNewState)
    {       
       // newState = goap.PlanActionSequence(goalMachine.HumanNeeds, goalMachine.Goals).id;

        base.Exit(oldNewState);
    }

    protected bool CheckIfStuck()
    {
        if (move.CheckIfStuck())
        {
            return true;
        }
        return false;
    }

    protected virtual void DoOnStuck()
    {
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

    /// <summary>
    /// Handles what happens when an agent reaches a target.
    /// </summary>
    protected virtual void DoReachedTargetLogic() { }

    private LocationTarget CheckForNewTargetIfClose()
    {
        return LocationTarget.None;
    }

    ///// <summary>
    ///// Check whether an agent is within radius to its targetBuilding.
    ///// </summary>
    //protected virtual void CheckIfNearEnterableTarget(LocationTarget locationTarget)
    //{
    //    if (IsWithinRangeToTarget(gameObject.GetComponent<Human>().LocationService[locationTarget]))
    //    {
    //        targetLocationChanged = false;
    //        DoReachedTargetLogic();
    //    }
    //}

    //protected void ChangeTargetLocation(LocationTarget targetLocation)
    //{
    //    //Debug.Log("Changed TargetLocation to: " + targetLocation.ToString());
    //    CurrentLocationTarget = targetLocation;
    //    targetLocationChanged = true;
    //}

    ///// <summary>
    ///// The worker is inside a building and is not rendered or trying to move.
    ///// </summary>
    //protected void Halt()
    //{
    //    NavMeshAgent navAgent = _owner.GetComponent<NavMeshAgent>();
    //    //_owner.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    //    foreach (Transform part in _owner.transform)
    //        if (part.GetComponent<MeshRenderer>())
    //            part.GetComponent<MeshRenderer>().enabled = false;
    //        else if (part.GetComponent<SkinnedMeshRenderer>())
    //            part.GetComponent<SkinnedMeshRenderer>().enabled = false;
    //    navAgent.updatePosition = false;
    //    // Animation
    //    _owner.GetComponent<Animator>().SetBool("IsMoving", false);
    //}

    /// <summary>
    /// The worker is outside of a building and is now rendered and attempting to move to a target.
    /// </summary>
    //protected void PrepareMove(Vector3 newTargetPos)
    //{
    //    NavMeshAgent navAgent = _owner.GetComponent<NavMeshAgent>();
    //    //_owner.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    //    foreach (Transform part in _owner.transform)
    //        if (part.GetComponent<MeshRenderer>())
    //            part.GetComponent<MeshRenderer>().enabled = true;
    //        else if (part.GetComponent<SkinnedMeshRenderer>())
    //            part.GetComponent<SkinnedMeshRenderer>().enabled = true;
    //    navAgent.updatePosition = true;
    //    navAgent.SetDestination(newTargetPos);
    //    // Animation
    //    _owner.GetComponent<Animator>().SetBool("IsMoving", true);
    //}
}

//public static class InOut
//{
//    public static Dictionary<string, bool> GetDictionary()
//    {
//        return new Dictionary<string, bool>();
//    }
//}

//public static class GetGoalMAchine
//{
//    public static GoalMachine GetNewGoalMachine()
//    {
//        return new GoalMachine();
//    }
//}

