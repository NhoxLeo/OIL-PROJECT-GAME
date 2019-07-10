using System.Collections.Generic;
using UnityEngine;

public class GoToCanteen : GoToAction
{    
    private void Start()
    {                
        id = "GoToCanteen";
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();      
        initiated = false;
    }

    public override Dictionary<string, bool> PreRequisites()
    {
        return prereqs;
    }

    public virtual Dictionary<string, bool> Outcomes()
    {
     
        return outcomes;
    }

    public override void Enter(GameObject owner, string enteringState)
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        base.Enter(owner, enteringState);

        GameObject target = _humanScript.LocationService[LocationTarget.Canteen];

        // PrepareMove(target.transform.position);
        //  ChangeTargetLocation(LocationTarget.Canteen);
        move.NewDestination(LocationTarget.Canteen);
        move.TryMove(target.transform.position);
    }

    public override void ExecuteState()
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        base.ExecuteState();

        if (!initiated)
        {
            initiated = true;
            _humanScript.IsResting = false;
            _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, false);

            var workBuilding = _humanScript.LocationService[LocationTarget.OccupationBuilding];
            var workSite = _humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().workSite;

            if (workSite)
            {
                _humanScript.SetNewHumanLocation(
                  LocationTarget.WorkSite, workSite);
                //   ChangeTargetLocation(LocationTarget.WorkSite);
                move.NewDestination(LocationTarget.WorkSite);
                Exit(GetAIComponents.WORKINGWORKSITE);
            }
            else
            {
                //  PrepareMove(workBuilding.transform.position);
                //    ChangeTargetLocation(LocationTarget.OccupationBuilding);
                move.NewDestination(LocationTarget.OccupationBuilding);
                move.TryMove(workBuilding.transform.position);
            }
        }
     
        int test = 0;
    }
}
