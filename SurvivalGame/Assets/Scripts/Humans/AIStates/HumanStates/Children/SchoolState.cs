using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Humans.AIStates.HumanStates.Children
{
  public class SchoolState : HumanAIState
  {
    public override void Enter(GameObject owner, string enteringState)
    {
      base.Enter(owner, enteringState);

      PrepareMove(_humanScript.LocationService[LocationTarget.OccupationBuilding].transform.position);
      ChangeTargetLocation(LocationTarget.OccupationBuilding);
    }

    public override void ExecuteState()
    {
      base.ExecuteState();

      //Update knowledge status/school status.
      //Decrease food and other resources?
      
      //Exit conditions for school
      //1. Fully educated
      if(_humanScript.IsSkilledWorker)
      {
        Exit(HumanStateGlobals.IDLE);
      }
      if (!_humanScript.HasJob)
      {
        Exit(HumanStateGlobals.IDLE);
      }
      //2. Removed from player interraction by clicking minus button?
    }

    protected override void DoReachedTargetLogic()
    {
      if (CurrentLocationTarget == LocationTarget.OccupationBuilding)
      {
        Halt();
      }

    }

  }
}
