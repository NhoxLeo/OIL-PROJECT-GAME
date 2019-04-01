using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class RandomInteractionState : HumanAIState
{
  Timer interactionTimer;
  bool interActionTimerFinished;
  const string PreviousState = "Previous";

  public RandomInteractionState()
  {
    interactionTimer = new Timer();
  }

  public override void Enter(GameObject owner, string enteringState)
  {
    base.Enter(owner, enteringState);

    GameObject target = _humanScript.LocationService[LocationTarget.RandomInteractionBuilding];
    interActionTimerFinished = false;
    if (target)
    {
      PrepareMove(target.transform.position);
      ChangeTargetLocation(LocationTarget.RandomInteractionBuilding);
    }
    else
      Exit(PreviousState);
  }

  public override void ExecuteState()
  {
    base.ExecuteState();

    if (interActionTimerFinished)
    {
      _humanScript.LocationService[CurrentLocationTarget].GetComponent<IVisitable>().HandleVisitor(_owner);
      Exit(PreviousState);
    }

  }

  protected override void DoReachedTargetLogic()
  {

    Halt();

    interactionTimer.Interval = _humanScript.LocationService[CurrentLocationTarget].GetComponent<IVisitable>().VisitTime;

    interActionTimerFinished = false;
    interactionTimer.Start();

    interactionTimer.Elapsed += delegate
    {
      interActionTimerFinished = true;
      interactionTimer.Stop();
    };

  }
}