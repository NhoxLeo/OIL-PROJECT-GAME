using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

public abstract class AIState
{
  public event EventHandler<StateChangedEventArgs> OnExitState;

  protected GameObject _owner;
  protected string currentState;
  protected bool targetLocationChanged;

  public virtual void Enter(GameObject owner, string enteringState)
  {
    currentState = enteringState;
    _owner = owner;
  }

  public virtual void ExecuteState() { }

  public virtual void Exit(string newState)
  {
    OnExitState.Invoke(this, new StateChangedEventArgs(currentState, newState));
  }
}

public abstract class HumanAIState : AIState
{
  protected Human _humanScript;
  protected LocationTarget CurrentLocationTarget { get; private set; }

  public override void Enter(GameObject owner, string enteringState)
  {
    base.Enter(owner, enteringState);
    _humanScript = _owner.GetComponent<Human>();
  }

  public override void ExecuteState()
  {
    if (targetLocationChanged)
    {
      CheckIfNearEnterableTarget(CurrentLocationTarget);
    }
  }

  protected bool IsWithinRangeToTarget(GameObject agentTarget)
  {
    if (agentTarget != null)
    {
      float distance = Vector3.Distance(_owner.transform.position, agentTarget.transform.position);
      if (distance < agentTarget.GetComponent<Building>().GetInteractionRadius())
      {
        return true;
      }
    }
    return false;
  }

  /// <summary>
  /// Handles what happens when an agent reaches a target.
  /// </summary>
  protected virtual void DoReachedTargetLogic() { }

  LocationTarget CheckForNewTargetIfClose()
  {
    return LocationTarget.None;
  }

  /// <summary>
  /// Check whether an agent is within radius to its targetBuilding.
  /// </summary>
  protected virtual void CheckIfNearEnterableTarget(LocationTarget locationTarget)
  {
    if (IsWithinRangeToTarget(_humanScript.LocationService[locationTarget]))
    {
      targetLocationChanged = false;
      DoReachedTargetLogic();
    }
  }

  protected void ChangeTargetLocation(LocationTarget targetLocation)
  {
    //Debug.Log("Changed TargetLocation to: " + targetLocation.ToString());
    CurrentLocationTarget = targetLocation;
    targetLocationChanged = true;
  }

  /// <summary>
  /// The worker is inside a building and is not rendered or trying to move.
  /// </summary>
  protected void Halt()
  {
    NavMeshAgent navAgent = _owner.GetComponent<NavMeshAgent>();
        //_owner.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        foreach (Transform part in _owner.transform)
            if (part.GetComponent<MeshRenderer>())
                part.GetComponent<MeshRenderer>().enabled = false;
            else if (part.GetComponent<SkinnedMeshRenderer>())
                part.GetComponent<SkinnedMeshRenderer>().enabled = false;
    navAgent.updatePosition = false;
        // Animation
        _owner.GetComponent<Animator>().SetBool("IsMoving", false);
    }

  /// <summary>
  /// The worker is outside of a building and is now rendered and attempting to move to a target.
  /// </summary>
  protected void PrepareMove(Vector3 newTargetPos)
  {
    NavMeshAgent navAgent = _owner.GetComponent<NavMeshAgent>();
        //_owner.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        foreach (Transform part in _owner.transform)
            if (part.GetComponent<MeshRenderer>())
                part.GetComponent<MeshRenderer>().enabled = true;
            else if (part.GetComponent<SkinnedMeshRenderer>())
                part.GetComponent<SkinnedMeshRenderer>().enabled = true;
        navAgent.updatePosition = true;
        navAgent.SetDestination(newTargetPos);
        // Animation
        _owner.GetComponent<Animator>().SetBool("IsMoving", true);
    }
}

