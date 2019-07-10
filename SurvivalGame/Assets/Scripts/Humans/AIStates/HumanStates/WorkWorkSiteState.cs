using System.Collections.Generic;
using UnityEngine;

public class WorkWorkSiteState : HumanAIState
{
    protected bool clockedInToWork;
    protected bool canWorkOverTime;
    protected bool workingOvertime;
    protected bool workTimeFinished;

    // private readonly Timer workTimer = new Timer();
    private readonly float interval = 3;
    private readonly int reset = 0;
    private float timer;
    bool seqStarted;

    private void Start()
    {
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();
        id = "WORKINGWORKSITE";
        prereqs.Add("occupation", true);
        outcomes.Add("isGathering", true);
    }

    public override void Enter(GameObject owner, string enteringState)
    {      
        base.Enter(owner, enteringState);

        _humanScript.IsResting = false;
        _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, false);              

        move.NewDestination(LocationTarget.WorkSite);
        move.TryMove(_humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().workSite.transform.position);
     
    }

    public override void ExecuteState()
    {      
        base.ExecuteState();      

        //Work is now finished and the agent will move towards the delivery point.   
        if (workTimeFinished)
        {     
            CurrentLocationTarget = LocationTarget.OccupationBuilding;
            move.NewDestination(LocationTarget.OccupationBuilding);
            move.TryMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);

            workTimeFinished = false;
        }      
    }

    protected override void DoReachedTargetLogic()
    {
        seqStarted = true;
        //    Human _humanScript = gameObject.GetComponent<Human>();
        if (CurrentLocationTarget == LocationTarget.OccupationBuilding)
        {
            _humanScript.LocationService[CurrentLocationTarget].GetComponent<IVisitable>().HandleVisitor(_owner);           
            CurrentLocationTarget = LocationTarget.WorkSite;
            move.NewDestination(LocationTarget.WorkSite);
            move.TryMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
            if (seqStarted)
            {
                seqStarted = false;
                _humanScript.ForceGoalEvalTarget();
            }         
        }
        else if (CurrentLocationTarget == LocationTarget.WorkSite)
        {
            if (!clockedInToWork)
            {
                CheckInToWork(_humanScript);
            }

            move.TryHaltAndHide();

          
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = reset;
                workTimeFinished = true;
            }
        }
    }  
   

    public override void Exit(string newState)
    {
        //     Human _humanScript = gameObject.GetComponent<Human>();
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

    private void CheckInToWork(Human _humanScript)
    {
        _humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().CheckInWorker();
        clockedInToWork = true;
    }


}
