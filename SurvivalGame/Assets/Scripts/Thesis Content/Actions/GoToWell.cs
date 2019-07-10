using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWell : GoToAction {

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

        GameObject target = _humanScript.LocationService[LocationTarget.Well];

        if (target)
        {
          //  PrepareMove(target.transform.position);
          //  ChangeTargetLocation(LocationTarget.Canteen);
        }
    }
}
