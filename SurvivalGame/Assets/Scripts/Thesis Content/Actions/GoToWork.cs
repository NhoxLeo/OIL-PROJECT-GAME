using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWork : GoToAction {

    private void Start()
    {
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();       
    }

    public override Dictionary<string, bool> PreRequisites()
    {
        return prereqs;
    }

    public override Dictionary<string, bool> Outcomes()
    {       
        return outcomes;
    }

    public override void Enter(GameObject owner, string enteringState)
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        base.Enter(owner, enteringState);

        var workBuilding = _humanScript.LocationService[LocationTarget.OccupationBuilding];
        var workSite = _humanScript.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().workSite;

        if (workSite)
        {
            _humanScript.SetNewHumanLocation(
              LocationTarget.WorkSite, workSite);
       //     ChangeTargetLocation(LocationTarget.WorkSite);
            Exit(GetAIComponents.WORKINGWORKSITE);
        }
        else
        {
          //  PrepareMove(workBuilding.transform.position);
          //  ChangeTargetLocation(LocationTarget.OccupationBuilding);
        }
    }
}
