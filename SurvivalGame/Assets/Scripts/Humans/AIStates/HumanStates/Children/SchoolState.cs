using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Humans.AIStates.HumanStates.Children
{
    public class SchoolState : HumanAIState
    {
        private void Start()
        {
            prereqs = new Dictionary<string, bool>();
            outcomes = new Dictionary<string, bool>();
            id = "SCHOOL";
        }
        public override void Enter(GameObject owner, string enteringState)
        {
            Human _humanScript = gameObject.GetComponent<Human>();
            base.Enter(owner, enteringState);

            //  PrepareMove(_humanScript.LocationService[LocationTarget.OccupationBuilding].transform.position);
            // ChangeTargetLocation(LocationTarget.OccupationBuilding);
            move.NewDestination(LocationTarget.OccupationBuilding);
            move.TryMove(_humanScript.LocationService[LocationTarget.OccupationBuilding].transform.position);
        }

        public override void ExecuteState()
        {
            Human _humanScript = gameObject.GetComponent<Human>();
            base.ExecuteState();

            //Update knowledge status/school status.
            //Decrease food and other resources?

            //Exit conditions for school
            //1. Fully educated
            if (_humanScript.IsSkilledWorker)
            {
                Exit(GetAIComponents.IDLE);
            }
            if (!_humanScript.HasJob)
            {
                Exit(GetAIComponents.IDLE);
            }
            //2. Removed from player interraction by clicking minus button?
        }

        protected override void DoReachedTargetLogic()
        {
            if (CurrentLocationTarget == LocationTarget.OccupationBuilding)
            {
                move.TryHalt();
            }

        }

    }
}
