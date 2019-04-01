using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : HumanAIState
{
  Vector3 idlePos = new Vector3(42, 0, 47);
  bool homeStatusChanged;
  bool localHasHome;

  public override void Enter(GameObject owner, string enteringState)
  {
    base.Enter(owner, enteringState);
    _humanScript.IsResting = true;

    _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
    if (_humanScript.LocationService[LocationTarget.Home])
    {
      ChangeTargetLocation(LocationTarget.Home);
      PrepareMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
      localHasHome = true;
    }
    else
    {
      ChangeTargetLocation(LocationTarget.None);
      PrepareMove(idlePos);
      localHasHome = false;
    }
  }

  public override void ExecuteState()
  {
    base.ExecuteState();

    if (localHasHome != HumanScriptHasHome())
    {
      homeStatusChanged = true;
    }

    //in case a house is removed/added while being idle 
    if (homeStatusChanged)
    {
      homeStatusChanged = false;

      if (HumanScriptHasHome())
      {
        localHasHome = true;
        ChangeTargetLocation(LocationTarget.Home);
        PrepareMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
      }
      else
      {
        localHasHome = false;
        ChangeTargetLocation(LocationTarget.None);
        PrepareMove(idlePos);
        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
      }
    }
  }

  bool HumanScriptHasHome()
  {
    if (_humanScript.LocationService[LocationTarget.Home])
    {
      return true;
    }
    return false;
  }

  protected override void DoReachedTargetLogic()
  {
    if (CurrentLocationTarget == LocationTarget.Home)
    {
      Halt();
    }
  }

}
