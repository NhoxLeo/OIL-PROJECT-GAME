using Assets.Scripts.Humans.AIStates.HumanStates.Children;
using UnityEngine;

public class GetAIComponents : MonoBehaviour
{
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

    public AIState GetAIState(string humanState) // Thesis: TODO THIS NEEDS TO HAVE THEIR MOVETO LOGIC REMOVED
    {
        if (humanState == IDLE)
        {
            return GameObject.Find("GoapAndActionPool").GetComponent<IdleState>();
        }
        else if (humanState == WORKING)
        {
            return GameObject.Find("GoapAndActionPool").GetComponent<WorkState>();
        }
        else if (humanState == WORKINGWORKSITE)
        {
            return GameObject.Find("GoapAndActionPool").GetComponent<WorkWorkSiteState>();
        }
        else if (humanState == RESTING)
        {
            return GameObject.Find("GoapAndActionPool").GetComponent<RestingState>();
        }
        else if (humanState == SCHOOL)
        {
            return GameObject.Find("GoapAndActionPool").GetComponent<SchoolState>();
        }
        else if (humanState == RANDOMINTERACTION)
        {
            return GameObject.Find("GoapAndActionPool").GetComponent<RandomInteractionState>();
        }
        //else if (humanState == EATING)
        //{

        //}
        //else if (humanState == GOTOWELL)
        //{

        //}
        else if (humanState == GOTOCANTEEN)
        {
            return GameObject.Find("GoapAndActionPool").GetComponent<GoToCanteen>();
        }
        //else if (humanState == GOTOWORK)
        //{

        //}
        else if (humanState == GOTOHOME)
        {
            return GameObject.Find("GoapAndActionPool").GetComponent<GoToHome>();
        }
        else if (humanState == DRINKING)
        {
            return GameObject.Find("GoapAndActonPool").GetComponent<DrinkAction>();
        }
        return null;
    }
}
