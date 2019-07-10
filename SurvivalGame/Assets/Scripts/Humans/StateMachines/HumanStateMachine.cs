using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The class that will be used on a human prefab.
/// </summary>
public class HumanStateMachine : MonoBehaviour
{
    private HumanFSM FSM;
    private GetAIComponents states;
    private GOAPMachine goap;
   // private GoalMachine goals;

    private void Start()
    {
        FSM = new HumanFSM(gameObject, GetAIComponents.IDLE);
        //   states = GameObject.Find("GoapAndActionPool").GetComponent<GetAIComponents>();
        //  goap = GameObject.Find("GoapAndActionPool").GetComponent<GOAPMachine>();
        states = gameObject.GetComponent<GetAIComponents>();
        goap = gameObject.GetComponent<GOAPMachine>();
       // goals = new GoalMachine(gameObject.GetComponent<Human>(), gameObject.GetComponent<NavMeshAgent>().speed);
        AddState(GetAIComponents.WORKING);
        AddState(GetAIComponents.WORKINGWORKSITE);
        //  AddState(GetAIComponents.RESTING);
        //  AddState(GetAIComponents.SCHOOL);
        //  AddState(GetAIComponents.RANDOMINTERACTION);
          AddState(GetAIComponents.EATING);
        // AddState(HumanStateGlobals.GOTOWELL);
        // AddState(GetAIComponents.GOTOCANTEEN);
        // AddState(HumanStateGlobals.GOTOWORK);
        // AddState(GetAIComponents.GOTOHOME);
         AddState(GetAIComponents.DRINKING);


        AddStartState(GetAIComponents.IDLE);
    }

    public string GetState()
    {
        return FSM.currentState;
    }

    /// <summary>
    /// Changes the state of a human, this will be called from player interactions.
    /// </summary>
    /// <param name="productionBuilding"></param>
    public void ChangeWorkState(GameObject productionBuilding)
    {
        FSM.ChangeOccupationStatus(productionBuilding);
    }

    //public void AddState(string humanState)
    //{
    //    AIState test = states.GetAIState(humanState);
    //    FSM.AddState(humanState, test );
    //}   
    //public void ReplaceState(string oldStateName, string newStateName)
    //{
    //    FSM.AddState(newStateName, states.GetAIState(newStateName));
    //    FSM.RemoveState(oldStateName);
    //}

    public void AddState(string humanState)
    {
        AIState test = GetAIState(humanState);
        FSM.AddState(humanState, test);
    }
    public void ReplaceState(string oldStateName, string newStateName)
    {
        FSM.AddState(newStateName, GetAIState(newStateName));
        FSM.RemoveState(oldStateName);
    }

    public void RemoveState(string humanState)
    {
        FSM.RemoveState(humanState);
    }

    public void ChangeState(string oldState, string newState)
    {
        FSM.ChangeState(oldState, newState);
    }

    private void Update()
    {
        if (FSM != null)
        {
            FSM.FSMUpdate();
        }
    }

    /// <summary>
    /// Required to be called in the constructor of this class.
    /// </summary>
    /// <param name="humanStartState"></param>
    private void AddStartState(string humanStartState)
    {
        FSM.AddStartState(humanStartState, GetAIState(humanStartState));
    }

    public static string IDLE = "IDLE";
    public static string WORKING = "WORKING";
    public static string WORKINGWORKSITE = "WORKINGWORKSITE";
    public static string RESTING = "RESTING";
    public static string SCHOOL = "SCHOOL";
    public static string RANDOMINTERACTION = "RANDOMINTERACTION";
    public static string EATING = "EATING";
    public static string GOTOWELL = "GOTOWELL";
    public static string GOTOCANTEEN = "GOTOCANTEEN";
    public static string GOTOWORK = "GOTOWORK";
    public static string GOTOHOME = "GOTOHOME";
    public static string DRINKING = "DRINKING";

    private AIState GetAIState(string humanState) // Thesis: TODO THIS NEEDS TO HAVE THEIR MOVETO LOGIC REMOVED
    {
        if (humanState == IDLE)
        {
            return gameObject.GetComponent<IdleState>();
        }
        else if (humanState == WORKING)
        {
            return gameObject.GetComponent<WorkState>();
        }
        else if (humanState == WORKINGWORKSITE)
        {
            return GetComponent<WorkWorkSiteState>();
        }
        //else if (humanState == RESTING)
        //{
        //    return gameObject.GetComponent<RestingState>();
        //}
        //else if (humanState == SCHOOL)
        //{
        //    return GetComponent<Assets.Scripts.Humans.AIStates.HumanStates.Children.SchoolState>();
        //}
        //else if (humanState == RANDOMINTERACTION)
        //{
        //    return gameObject.GetComponent<RandomInteractionState>();
        //}
        else if (humanState == EATING)
        {
            return GetComponent<EatAction>();
        }
        //else if (humanState == GOTOWELL)
        //{

        //}
        //else if (humanState == GOTOCANTEEN)
        //{
        //    return GameObject.Find("GoapAndActionPool").GetComponent<GoToCanteen>();
        //}
        ////else if (humanState == GOTOWORK)
        ////{

        ////}
        //else if (humanState == GOTOHOME)
        //{
        //    return GameObject.Find("GoapAndActionPool").GetComponent<GoToHome>();
        //}
        else if (humanState == DRINKING)
        {
            return GetComponent<DrinkAction>();
        }
        return null;
    }

}
