using UnityEngine;
using UnityEngine.AI;

public class Movement
{
    private GameObject gameObject;
    private readonly bool targetLocationChanged;
    public Vector3 idlePos { get; set; }
    private LocationTarget locationTarget;
    public NavMeshAgent navAgent;
    Vector3 oldPos, currentPos;
    //   protected LocationTarget target { get; private set; }

    public Vector3 TargetPos { get; set; }

    public Movement(GameObject _gameObject, LocationTarget _target)
    {
        gameObject = _gameObject;
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        oldPos = new Vector3(0,0,0);
        //   target = _target;
    }


    /// <summary>
    /// The worker is outside of a building and is now rendered and attempting to move to a target.
    /// </summary>
    public void PrepareMove(Vector3 newTargetPos)
    {       
        //_owner.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        foreach (Transform part in gameObject.transform)
            if (part.GetComponent<MeshRenderer>())
                part.GetComponent<MeshRenderer>().enabled = true;
            else if (part.GetComponent<SkinnedMeshRenderer>())
                part.GetComponent<SkinnedMeshRenderer>().enabled = true;
        navAgent.updatePosition = true;
        navAgent.SetDestination(newTargetPos);
        // Animation
        gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
    }

    /// <summary>
    /// Check whether an agent is within radius to its targetBuilding.
    /// </summary>
    public bool CheckIfNearEnterableTarget(LocationTarget locationTarget)
    {
        this.locationTarget = locationTarget;
        if (IsWithinRangeToTarget(gameObject.GetComponent<Human>().LocationService[locationTarget]))
        {
            gameObject.GetComponent<Human>().TargetLocationChanged = false;
            return true;
            //  DoReachedTargetLogic();
        }
        return false;
    }

    private bool IsWithinRangeToTarget(GameObject agentTarget)
    {
        Vector3 targetPos;
        float radius;
        if (locationTarget == LocationTarget.Idle)
        {
            targetPos = idlePos;
            radius = 0.3f;
        }
        else
        {
            targetPos = agentTarget.transform.position;
            radius = agentTarget.GetComponent<Building>().GetInteractionRadius();
        }
        // if (agentTarget != null)
        //  {
        float distance = Vector3.Distance(gameObject.transform.position, targetPos);
        if (distance < radius)
        {
            return true;
        }
        //  }
        return false;
    }

    /// <summary>
    /// Handles what happens when an agent reaches a target. NEeds to be converted into a flag
    /// </summary>
    // void DoReachedTargetLogic() { }

    //private LocationTarget CheckForNewTargetIfClose()
    //{
    //    return LocationTarget.None;
    //}       

    //public void ChangeTargetLocation(LocationTarget targetLocation)
    //{
    //    //Debug.Log("Changed TargetLocation to: " + targetLocation.ToString());

    //}

    /// <summary>
    /// The worker is inside a building and is not rendered or trying to move.
    /// </summary>
    public void HaltAndHide()
    {       
        //_owner.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        foreach (Transform part in gameObject.transform)
            if (part.GetComponent<MeshRenderer>())
                part.GetComponent<MeshRenderer>().enabled = false;
            else if (part.GetComponent<SkinnedMeshRenderer>())
                part.GetComponent<SkinnedMeshRenderer>().enabled = false;
        navAgent.updatePosition = false;
        // Animation
        gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
    }

    public void Halt()
    {
        gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
    }

    public bool IsStuck()
    {
        currentPos = gameObject.transform.position;
        if(oldPos == currentPos)
        {
            return true;
        }
        oldPos = currentPos;
        return false;
    }
}
