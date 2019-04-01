using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingState : HumanAIState
{
  Vector3 idlePos = new Vector3(42, 0, 47);
  float backToWorkValue = 0;
  bool homeStatusChanged;
  bool localHasHome;

  public override void Enter(GameObject owner, string enteringState)
  {
    backToWorkValue = UnityEngine.Random.Range(75, 100);
    base.Enter(owner, enteringState);

    _humanScript.IsResting = true;
    _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
    //If the player has a house he will go there.
    if (_humanScript.Residence)
    {
      ChangeTargetLocation(LocationTarget.Home);
      PrepareMove(_humanScript.Residence.transform.position);
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


    //Exit conditions
    if (_humanScript.GetComfort() >= backToWorkValue)
      Exit(HumanStateGlobals.WORKING);
  }

  protected override void DoReachedTargetLogic()
  {
    if (CurrentLocationTarget == LocationTarget.Home)
    {      Halt();

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
}
