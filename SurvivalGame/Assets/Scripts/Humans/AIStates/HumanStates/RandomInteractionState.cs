using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class RandomInteractionState : HumanAIState
{
    private Timer interactionTimer;
    private bool interActionTimerFinished;
    private const string PreviousState = "Previous";

    private void Start()
    {
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();
        id = "RANDOMINTERACTION";
    }

    public RandomInteractionState()
    {
        interactionTimer = new Timer();
    }

    public override void Enter(GameObject owner, string enteringState)
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        base.Enter(owner, enteringState);

        GameObject target = _humanScript.LocationService[LocationTarget.RandomInteractionBuilding];
        interActionTimerFinished = false;
        if (target)
        {
            //  PrepareMove(target.transform.position);
            //  ChangeTargetLocation(LocationTarget.RandomInteractionBuilding);
            move.NewDestination(LocationTarget.RandomInteractionBuilding);
            move.TryMove(target.transform.position);
        }
        else
            Exit(PreviousState);
    }

    public override void ExecuteState()
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        base.ExecuteState();

        if (interActionTimerFinished)
        {
            _humanScript.LocationService[CurrentLocationTarget].GetComponent<IVisitable>().HandleVisitor(_owner);
            Exit(PreviousState);
        }

    }

    protected override void DoReachedTargetLogic()
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        move.TryHalt();

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