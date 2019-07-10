using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkAction : HumanAIState {
    Vector3 idlePos = new Vector3(42, 0, 47);
    //bool homeStatusChanged;
   // bool localHasHome;
    IdlingMovement idleMovement;
    //LocationTarget CurrentLocationTarget;
    bool hasDrunk;
    bool isWriggling = false;
    private float unStuckTimer;

    private void Start()
    {
        id = "DRINKING";
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();
        prereqs.Add("thirst", true);
        outcomes.Add("thirst", false);
    }

    public override void Enter(GameObject owner, string enteringState)
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        idlePos = _humanScript.idlePos;
        base.Enter(owner, enteringState);
        idleMovement = new IdlingMovement();
        _humanScript.IsResting = true;
        hasDrunk = false;

        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
        if (_humanScript.LocationService[LocationTarget.Well])
        {
            CurrentLocationTarget = LocationTarget.Well;          
            NewMovementOrder();       
        }
        else if (_humanScript.LocationService[LocationTarget.Home])
        {
            if (CurrentLocationTarget != LocationTarget.Home)
            {
                CurrentLocationTarget = LocationTarget.Home;
                NewMovementOrder();
             //   localHasHome = true;
            }
        }
        else
        {
            CurrentLocationTarget = LocationTarget.None;         
            NewMovementOrder();
          //  localHasHome = false;
        }
    }

    void NewMovementOrder()
    {
        move.NewDestination(CurrentLocationTarget);
        if (CurrentLocationTarget == LocationTarget.Idle)
        {
            move.TryMove(idlePos);
        }
        else
        {
            move.TryMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
        }
    }

    public override void ExecuteState()
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        base.ExecuteState();

        if (CheckIfStuck())
        {           
            DoOnStuck();
            isWriggling = true;
        }
        if (isWriggling == true)
        {
            unStuckTimer += Time.deltaTime;
            if (unStuckTimer > 1)
            {
                NewMovementOrder();
                unStuckTimer = 0;
                isWriggling = false;
            }
        }

        //if (localHasHome != HumanScriptHasHome(_humanScript))
        //{
        //    homeStatusChanged = true;
        //}

        //in case a house is removed/added while being idle 
        //if (homeStatusChanged)
        //{
        //    homeStatusChanged = false;

        //    if (HumanScriptHasHome(_humanScript))
        //    {
        //        localHasHome = true;
        //        CurrentLocationTarget = LocationTarget.Home;
             
        //        NewMovementOrder();
        //        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.
        
       // , true);
        //    }
        //    else
        //    {
        //        localHasHome = false;
        //        CurrentLocationTarget = LocationTarget.None;
              
        //        NewMovementOrder();
        //        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
        //    }
        //}    
    }

    public override void Exit(string newState)
    {
        base.Exit(newState);
    }

    bool HumanScriptHasHome(Human _humanScript)
    {
        if (_humanScript.LocationService[LocationTarget.Home])
        {
            return true;
        }
        return false;
    }

    protected override void DoReachedTargetLogic()
    {
        base.DoReachedTargetLogic();

        if (base.CurrentLocationTarget != LocationTarget.Idle)
        {
            move.TryHaltAndHide();
        }
        if (!hasDrunk)
        {
            hasDrunk = true;
            gameObject.GetComponent<Human>().AttemptToSatisfyThirst();
        }
    }

    protected override void DoOnStuck()
    {
      move.TryMove(idleMovement.GetNewDestination(_humanScript, CurrentLocationTarget));
    }


    //protected override void CheckIfNearEnterableTarget(LocationTarget locationTarget)
    //{
    //    base.CheckIfNearEnterableTarget(locationTarget);
    //}
}
