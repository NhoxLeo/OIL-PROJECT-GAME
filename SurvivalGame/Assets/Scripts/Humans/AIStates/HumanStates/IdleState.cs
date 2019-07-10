using System.Collections.Generic;
using UnityEngine;

public class IdleState : HumanAIState
{
    private Vector3 idlePos;
    private bool homeStatusChanged;
    private bool localHasHome;
    private IdlingMovement idleMovement;
    private float timer;    
    float idlePosInterval;
    bool hasHalted;

    private void Start()
    {
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();
        prereqs.Add("occupation", false);
        outcomes.Add("isIdling", true);
        id = "IDLE";
        timer = 0;
        idlePosInterval = 3;
        hasHalted = false;
    }


    public override void Enter(GameObject owner, string enteringState)
    {
        base.Enter(owner, enteringState);
        idlePos = _humanScript.idlePos;
        idleMovement = new IdlingMovement();
        // Human _humanScript = gameObject.GetComponent<Human>();
        _humanScript.IsResting = true;

        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
        if (_humanScript.LocationService[LocationTarget.Home])
        {
            // ChangeTargetLocation(LocationTarget.Home);
            //  PrepareMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
            move.NewDestination(LocationTarget.Home);
            move.TryMove(_humanScript.LocationService[move.Target].transform.position);

            localHasHome = true;
        }
        else
        {
            //  ChangeTargetLocation(LocationTarget.None);
            // PrepareMove(idlePos);
            move.NewDestination(LocationTarget.Idle);
            SetNewIdlePos();
            move.TryMove(idlePos);
            localHasHome = false;
        }
    }

    private void SetNewIdlePos()
    {
        idlePos = idleMovement.GetNewDestination(_humanScript, move.Target);
        move.SetNewIdlePos(idlePos);
    }

    public override void ExecuteState()
    {
        // Human _humanScript = gameObject.GetComponent<Human>();
        base.ExecuteState();
        if(idleMovement.CheckCollisions(idlePos, _humanScript))
        {
            DoReachedTargetLogic();
        }
        RandomMovement();      

        if (localHasHome != HumanScriptHasHome(_humanScript))
        {
            homeStatusChanged = true;
        }

        //in case a house is removed/added while being idle 
        CheckHomeStatus();
    }

    private void RandomMovement()
    {
        if (/*!localHasHome &&*/ hasHalted)
        {
            timer += Time.deltaTime;
            if (timer > idlePosInterval )
            {
                SetNewIdlePos();
                move.TryMove(idlePos);
                timer = 0;
                idlePosInterval = Random.Range(2, 5);
                hasHalted = false;
            }
        }
        else
        {
            if (CheckIfStuck())
            {
                DoOnStuck();
            }
        }
    }

    void CheckHomeStatus()
    {
        if (homeStatusChanged)
        {
            HomeStatusChanged();
        }
    }

    void HomeStatusChanged()
    {
        homeStatusChanged = false;

        if (HumanScriptHasHome(_humanScript))
        {
            localHasHome = true;
            //  ChangeTargetLocation(LocationTarget.Home);
            //  PrepareMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
            move.NewDestination(LocationTarget.Home);
            //SetNewIdlePos();
            move.TryMove(_humanScript.LocationService[move.Target].transform.position);
            _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
        }
        else
        {
            localHasHome = false;
            //  ChangeTargetLocation(LocationTarget.None);
            //   PrepareMove(idlePos);
            move.NewDestination(LocationTarget.Idle);
            SetNewIdlePos();
            move.TryMove(idlePos);
            _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);
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

    protected override void DoReachedTargetLogic()
    {
      //  if (CurrentLocationTarget == LocationTarget.Home)
      //  {
            //  move.TryHaltAndHide();
    //        move.TryHalt();
     //       hasHalted = true;
      //  }
      //  if (CurrentLocationTarget == LocationTarget.Idle)
      //  {
            move.TryHalt();
            hasHalted = true;
     //   }
    }

    protected override void DoOnStuck()
    {        
        DoReachedTargetLogic();
    }

}
