using System.Collections.Generic;
using UnityEngine;

public class RestingState : HumanAIState
{
    private Vector3 idlePos = new Vector3(42, 0, 47);
    private float backToWorkValue = 0;
    private bool homeStatusChanged;
    private bool localHasHome;

    private void Start()
    {
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();
        id = "RESTING";
    }

    public override void Enter(GameObject owner, string enteringState)
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        backToWorkValue = UnityEngine.Random.Range(75, 100);
        base.Enter(owner, enteringState);

        _humanScript.IsResting = true;
        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
        //If the player has a house he will go there.
        if (_humanScript.Residence)
        {
            //    ChangeTargetLocation(LocationTarget.Home);
            //    PrepareMove(_humanScript.Residence.transform.position);
            move.NewDestination(LocationTarget.Idle);
            move.TryMove(_humanScript.Residence.transform.position);
            localHasHome = true;
        }
        else
        {
       //     ChangeTargetLocation(LocationTarget.Idle);
        //    PrepareMove(idlePos);
            localHasHome = false;
        }
    }

    public override void ExecuteState()
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        base.ExecuteState();
        if (localHasHome != HumanScriptHasHome(_humanScript))
        {
            homeStatusChanged = true;
        }

        //in case a house is removed/added while being idle 
        if (homeStatusChanged)
        {
            homeStatusChanged = false;

            if (HumanScriptHasHome(_humanScript))
            {
                localHasHome = true;
                //   ChangeTargetLocation(LocationTarget.Home);
                //  PrepareMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
                move.NewDestination(LocationTarget.Home);
                move.TryMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
                _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
            }
            else
            {
                localHasHome = false;
                // ChangeTargetLocation(LocationTarget.None);
                //  PrepareMove(idlePos);
                move.NewDestination(LocationTarget.Idle);
                move.TryMove(idlePos);
                _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
            }
        }


        //Exit conditions
        if (_humanScript.GetComfort() >= backToWorkValue)
            Exit(GetAIComponents.WORKING);
    }

    protected override void DoReachedTargetLogic()
    {
        if (CurrentLocationTarget == LocationTarget.Home)
        {
            move.TryHalt();

        }
    }

    private bool HumanScriptHasHome(Human _humanScript)
    {
        if (_humanScript.LocationService[LocationTarget.Home])
        {
            return true;
        }
        return false;
    }
}
