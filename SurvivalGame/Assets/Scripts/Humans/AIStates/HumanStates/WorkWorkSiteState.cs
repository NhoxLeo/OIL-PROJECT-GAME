using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class WorkWorkSiteState : HumanAIState
{
  protected bool clockedInToWork;
  protected bool canWorkOverTime;
  protected bool workingOvertime;
  protected bool workTimeFinished;
  readonly Timer workTimer = new Timer();

  public override void Enter(GameObject owner, string enteringState)
  {
    base.Enter(owner, enteringState);

    //_humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort);

    ChangeTargetLocation(LocationTarget.WorkSite);
    PrepareMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
  }

  public override void ExecuteState()
  {
    base.ExecuteState();

    if (workTimeFinished)
    {//Work is now finished and the agent will move towards the delivery point.
      ChangeTargetLocation(LocationTarget.OccupationBuilding);
      PrepareMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
      workTimeFinished = false;
    }

    if (workingOvertime)
    {
      OverTimeExitConditions();
    }
    else
    {
      NormalExitConditions();
    }
  }

  protected override void DoReachedTargetLogic()
  {
    if (CurrentLocationTarget == LocationTarget.OccupationBuilding)
    {
      _humanScript.LocationService[CurrentLocationTarget].GetComponent<IVisitable>().HandleVisitor(_owner);
      ChangeTargetLocation(LocationTarget.WorkSite);
      PrepareMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
    }
    else if (CurrentLocationTarget == LocationTarget.WorkSite)
    {
      if (!clockedInToWork)
      {
        CheckInToWork();
      }

      Halt();

      workTimer.Interval = _humanScript.LocationService[CurrentLocationTarget].GetComponent<IVisitable>().VisitTime;
      workTimeFinished = false;
      workTimer.Start();

      workTimer.Elapsed += delegate
      {
        workTimeFinished = true;
        workTimer.Stop();
      };
    }
  }

  void NormalExitConditions()
  {
    //Exit conditions
    if (_humanScript.GetComfort() <= 0f)
    {
      if (canWorkOverTime)
      {
        workingOvertime = true;
      }
      else
      {
        CheckoutFromWork();
        Exit(HumanStateGlobals.RESTING);
      }
    }
  }

  void OverTimeExitConditions()
  {
    //Exit conditions
    if (_humanScript.GetComfort() < -75)
    {
      CheckoutFromWork();

      if (_humanScript.LocationService[LocationTarget.Home])
      {
        Exit(HumanStateGlobals.RESTING);
      }
      else
        Exit(HumanStateGlobals.IDLE);
    }
  }

  public override void Exit(string newState)
  {
    base.Exit(newState);
    CheckoutFromWork();
  }

  private void CheckoutFromWork()
  {
    if (clockedInToWork)
    {
      _humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().CheckOutWorker();
      clockedInToWork = false;
    }

  }

  private void CheckInToWork()
  {
    _humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().CheckInWorker();
    clockedInToWork = true;
  }


}
