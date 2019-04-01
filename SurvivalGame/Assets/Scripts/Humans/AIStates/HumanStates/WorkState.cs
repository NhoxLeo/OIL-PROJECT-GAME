using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;


public class WorkState : HumanAIState
{
  protected bool clockedInToWork;
  protected bool canWorkOverTime;
  protected bool workingOvertime;

  public override void Enter(GameObject owner, string enteringState)
  {
    base.Enter(owner, enteringState);

    // _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort);
    _humanScript.IsResting = false;
    _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, false);

    var workBuilding = _humanScript.LocationService[LocationTarget.OccupationBuilding];
    var workSite = _humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().workSite;

    if (workSite)
    {
      _humanScript.SetNewHumanLocation(
        LocationTarget.WorkSite, workSite);
      ChangeTargetLocation(LocationTarget.WorkSite);
      Exit(HumanStateGlobals.WORKINGWORKSITE);
    }
    else
    {
      PrepareMove(workBuilding.transform.position);
      ChangeTargetLocation(LocationTarget.OccupationBuilding);
    }
  }


  public override void ExecuteState()
  {
    base.ExecuteState();

    if (workingOvertime)
    {
      OverTimeExitConditions();
    }
    else
    {
      NormalExitConditions();
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
      Exit(HumanStateGlobals.RESTING);
    }
  }

  protected override void DoReachedTargetLogic()
  {
    if (CurrentLocationTarget == LocationTarget.OccupationBuilding)
    {
      if (!clockedInToWork)
      {
        CheckInToWork();
      }

      Halt();
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

  /// <summary>
  /// Checks the agent in to work. Is done when they are close to their workplace.
  /// </summary>
  private void CheckInToWork()
  {
    _humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().CheckInWorker();
    clockedInToWork = true;
  }

  public string GetWorkplace()
  {
    return "null";  // TODO: Return a name of the workplace
  }
}

