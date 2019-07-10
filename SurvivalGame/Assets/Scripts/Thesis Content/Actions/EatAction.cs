using System.Collections.Generic;
using UnityEngine;

public class EatAction : HumanAIState
{
    private Vector3 idlePos = new Vector3(42, 0, 47);
    private bool homeStatusChanged;
    private bool localHasHome;
   // private LocationTarget CurrentLocationTarget;
    private bool hasEaten = false;
    private IdlingMovement idleMovement;
    private float unStuckTimer; 
    bool isWriggling = false;

    private void Start()
    {
        id = "EATING";
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();
        prereqs.Add("hunger", true);
        outcomes.Add("hunger", false);
        unStuckTimer = 0;

    }

    public override void Enter(GameObject owner, string enteringState)
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        idlePos = _humanScript.idlePos;
        base.Enter(owner, enteringState);
        idleMovement = new IdlingMovement();

        _humanScript.IsResting = true;
        hasEaten = false;

        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
        if (_humanScript.LocationService[LocationTarget.Canteen])
        {
            CurrentLocationTarget = LocationTarget.Canteen;         
            NewMovementOrder();       
        }
        else if (_humanScript.LocationService[LocationTarget.Home])
        {
            if (CurrentLocationTarget != LocationTarget.Home)
            {
                CurrentLocationTarget = LocationTarget.Home;               
                NewMovementOrder();
                localHasHome = true;
            }
        }
        else
        {
            CurrentLocationTarget = LocationTarget.Idle;          
            NewMovementOrder();
            localHasHome = false;
        }
    }

    private void NewMovementOrder()
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
        if(isWriggling == true)
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

        ////in case a house is removed/added while being idle 
        //if (homeStatusChanged)
        //{
        //    homeStatusChanged = false;

        //    if (HumanScriptHasHome(_humanScript))
        //    {
        //        localHasHome = true;
        //        CurrentLocationTarget = LocationTarget.Home;              
        //        NewMovementOrder();
        //        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
        //    }
        //    else
        //    {
        //        localHasHome = false;
        //        CurrentLocationTarget = LocationTarget.Idle;             
        //        NewMovementOrder();
        //        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
        //    }
        //}      
    }

    public override void Exit(string newState)
    {
        base.Exit(newState);
    }

    private bool HumanScriptHasHome(Human _humanScript)
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
        if (!hasEaten)
        {
            hasEaten = true;
            gameObject.GetComponent<Human>().AttemptToSatisfyHunger();
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
