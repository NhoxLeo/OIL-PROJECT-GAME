using System.Collections.Generic;
using UnityEngine;


public class WorkState : HumanAIState
{
    protected bool clockedInToWork;
    protected bool canWorkOverTime;
    protected bool workingOvertime;

    private void Start()
    {
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();
        // prereqs.Add("work");
        prereqs.Add("occupation", true);
        outcomes.Add("isWorking", true);
        id = "WORKING";       
    }

    public override void Enter(GameObject owner, string enteringState)
    {
       // Human _humanScript = gameObject.GetComponent<Human>();
      //  move = gameObject.GetComponent<MovementHandler>();
      //  bool stop = false;
        base.Enter(owner, enteringState);
     

        _humanScript.IsResting = false;
        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, false);

        GameObject workBuilding = _humanScript.OccupationBuilding;      
        _humanScript.SetNewHumanLocation(LocationTarget.OccupationBuilding, workBuilding);
     //   GameObject workBuilding = _humanScript.LocationService[LocationTarget.OccupationBuilding];
        GameObject workSite = _humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().workSite;

        if (workSite)
        {
            _humanScript.SetNewHumanLocation(
              LocationTarget.WorkSite, workSite);
            //ChangeTargetLocation(LocationTarget.WorkSite);  
            move.NewDestination(LocationTarget.WorkSite);
        }
        else
        {            
          //  PrepareMove(workBuilding.transform.position);
            //  ChangeTargetLocation(LocationTarget.OccupationBuilding);
            move.NewDestination(LocationTarget.OccupationBuilding);
            move.TryMove(workBuilding.transform.position);
        }
    }


    public override void ExecuteState()
    {
    //    Human _humanScript = gameObject.GetComponent<Human>();
       

        if (workingOvertime)
        {
            OverTimeExitConditions(_humanScript);
        }
        else
        {
            NormalExitConditions(_humanScript);
        }
        base.ExecuteState();
        //move.CheckPoximity();
        //if (move.DoOnTargetAction)
        //{
        //    DoReachedTargetLogic();
        //}
    }

    private void NormalExitConditions(Human _humanScript)
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
                CheckoutFromWork(_humanScript);
                Exit(GetAIComponents.RESTING);
            }
        }
    }

    private void OverTimeExitConditions(Human _humanScript)
    {
        //Exit conditions
        if (_humanScript.GetComfort() < -75)
        {
            CheckoutFromWork(_humanScript);
            Exit(GetAIComponents.RESTING);
        }
    }

    protected override void DoReachedTargetLogic()
    {
        //Human _humanScript = gameObject.GetComponent<Human>();
        //if (CurrentLocationTarget == LocationTarget.OccupationBuilding)
        //{
        //    if (!clockedInToWork)
        //    {
        //        CheckInToWork(_humanScript);
        //    }

        //    Halt();
        //}
        if (move.Target == LocationTarget.OccupationBuilding)
        {
            if (!clockedInToWork)
            {
                CheckInToWork(_humanScript);
            }

            move.TryHaltAndHide();
        }
    }

    public override void Exit(string newState)
    {
//        Human _humanScript = gameObject.GetComponent<Human>();
        base.Exit(newState);
        CheckoutFromWork(_humanScript);
    }

    private void CheckoutFromWork(Human _humanScript)
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
    private void CheckInToWork(Human _humanScript)
    {
        _humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().CheckInWorker();
        clockedInToWork = true;
    }

    public string GetWorkplace()
    {
        return "null";  // TODO: Return a name of the workplace
    }
}

