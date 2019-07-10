using System.Collections.Generic;
using UnityEngine;

public class GoToHome : GoToAction
{
    private Vector3 idlePos = new Vector3(42, 0, 47);
    private float backToWorkValue = 0;
    private bool homeStatusChanged;
    private bool localHasHome;    
    LocationTarget currentTarget;

    private void Start()
    {
        prereqs = new Dictionary<string, bool>();
        outcomes = new Dictionary<string, bool>();       
        initiated = false;
    }

    public override void Enter(GameObject owner, string enteringState)
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        base.Enter(owner, enteringState);

    }
    public override void ExecuteState()
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        base.ExecuteState();
       

        if (!initiated)
        {
            initiated = true;
            GameObject target = _humanScript.LocationService[LocationTarget.Home];

            if (_humanScript.Residence)
            {
              //  ChangeTargetLocation(LocationTarget.Home);
               // PrepareMove(_humanScript.Residence.transform.position);
                localHasHome = true;
            }
            else
            {
               // ChangeTargetLocation(LocationTarget.None);
              //  PrepareMove(idlePos);
                localHasHome = false;
            }
        }
       

        if (localHasHome != HumanScriptHasHome())
        {
            homeStatusChanged = true;
        }

        //in case a house is removed/added while being idle 
        if (homeStatusChanged)
        {
            homeStatusChanged = false;

            if (HumanScriptHasHome())
            {
                localHasHome = true;
              //  ChangeTargetLocation(LocationTarget.Home);
              //  PrepareMove(_humanScript.LocationService[CurrentLocationTarget].transform.position);
                _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);                
            }
            else
            {
                localHasHome = false;
            //    ChangeTargetLocation(LocationTarget.None);
             //   PrepareMove(idlePos);
                _humanScript.ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, true);              
            }
        }
        //if (IsWithinRangeToTarget(_humanScript.LocationService[CurrentLocationTarget]))
        //{
        //    Exit("");
        //}
      
    }
    bool HumanScriptHasHome()
    {
        Human _humanScript = gameObject.GetComponent<Human>();
        if (_humanScript.LocationService[LocationTarget.Home])
        {
            return true;
        }
        return false;
    }  
}
