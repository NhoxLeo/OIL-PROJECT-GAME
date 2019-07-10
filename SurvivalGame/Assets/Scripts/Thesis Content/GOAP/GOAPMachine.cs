using System.Collections.Generic;
using UnityEngine;

public class GOAPMachine : MonoBehaviour
{
    private HumanAIState[] actionPool;
    private List<HumanAIState> path = ReturnNewList.GetEmptyPath();
    private readonly Human originalHuman;
    private bool foundPath;
    private string location;


    /// <summary>
    /// Searches the pool of available actions, iteratively changes the goal based on if it found an action that satisfies the previous goal.
    /// Then repeat the search until the found action contains the prerequisites applicable tot he current state of the human.
    /// Adds all actions chosen to a list in sequence as they are found and reverses it.
    /// </summary>
    public HumanAIState PlanActionSequence(Dictionary<string, bool> _humanStates, Dictionary<string, bool> _goals)
    {
        actionPool = gameObject.GetComponents<HumanAIState>();

        Dictionary<string, bool> goals = _goals;            

        path.Clear();
        if (SearchThroughActonPool(actionPool, _humanStates, goals))
        {
            foundPath = false;
            return path[0];
        }

        return null;
    }

    private bool SearchThroughActonPool(HumanAIState[] _actionPool, Dictionary<string, bool> _humanStates, Dictionary<string, bool> _goals)
    {
        for (int i = 0; i < _actionPool.Length; i++) // First check the available pool of actions
        {
            int goalChecker = 0;
            if (CheckOutcomes(_actionPool[i], _goals, goalChecker))
            {
                int preRequisiteChecker = 0;
                bool pathChecker = CheckPreReqs(_actionPool[i], _humanStates, preRequisiteChecker, _goals);
                if (pathChecker && foundPath)
                {
                    return true;
                }
                else if (pathChecker)
                {
                    // SearchThroughActonPool(_actionPool, _humanStates, ref _goals);
                    i = 0;
                }
            }
        }
        return false;
    }

    private bool CheckOutcomes(HumanAIState _action, Dictionary<string, bool> _goals, int _goalChecker)
    {
        foreach (KeyValuePair<string, bool> goal in _goals)
        {
            foreach (KeyValuePair<string, bool> outcome in _action.Outcomes())
            {
                if (goal.Key == outcome.Key && goal.Value == outcome.Value) // if the goals contains the current outcome of the current action if not, we'll break out of the loop and look at a new action                                                                          
                {
                    _goalChecker++;
                    if (_goalChecker == _action.Outcomes().Count) // if the action contains all the goals we'll look at teh prerequisites <-- this needs to look for a single goal and find a way to get the rest of teh goals through other actions.
                    {
                        //path.Add(_action);
                        //path.Reverse();
                        //foundPath = true;
                        return true;
                    }
                }
            }
        }
        //for (int j = 0; j < _action.Outcomes().Count; j++) // Second, iterate through all the outcomes of the current action
        //{
        //    if (_goals.Contains(_action.Outcomes()[j])) // if the goals contains the current outcome of the current action if not, we'll break out of the loop and look at a new action                                                                          
        //    {
        //        _goalChecker++;
        //        if (_goalChecker == _action.Outcomes().Count) // if the action contains all the goals we'll look at teh prerequisites <-- this needs to look for a single goal and find a way to get the rest of teh goals through other actions.
        //        {
        //            //path.Add(_action);
        //            //path.Reverse();
        //            //foundPath = true;
        //            return true;
        //        }
        //    }
        //}
        return false;
    }

    private bool CheckPreReqs(HumanAIState _action, Dictionary<string, bool> _humanStates, int _preReqChecker, Dictionary<string, bool> _goals)
    {
        if (_action.PreRequisites().Count == 0)
        {
            path.Add(_action);
            path.Reverse();
            foundPath = true;
            return true;
        }
        foreach (KeyValuePair<string, bool> humanState in _humanStates)
        {
            foreach (KeyValuePair<string, bool> prereq in _action.PreRequisites())
            {
                if (humanState.Key == prereq.Key && humanState.Value == prereq.Value) // if the prereq of teh current action correlate to the current humanState
                {
                    _preReqChecker++;
                    if (_preReqChecker == _action.PreRequisites().Count) // if the current human state correlate to all the prereq of teh action. //we have found the end of a viable path.
                    {
                        path.Add(_action);
                        path.Reverse();
                        foundPath = true;
                        return true;
                    }
                    else // else we set teh new goals to correlate to the prereq of teh action.  We can do this because we know the goals of the action is by extention leading to the original goal set by the goal manager.
                    {
                        path.Add(_action);
                        _goals.Clear();
                        _goals.Add(prereq.Key, prereq.Value);
                        return true;
                    }
                }
            }
        }
        //for (int k = 0; k < _humanStates.Count; k++) // thirdly we'll iterate through the state of teh human 
        //{
        //    if (_action.PreRequisites().Count == 0)
        //    {
        //        path.Add(_action);
        //        path.Reverse();
        //        foundPath = true;
        //        return true;
        //    }
        //    else if (_action.PreRequisites().Contains(_humanStates[k])) // if the prereq of teh current action correlate to the current humanState
        //    {
        //        _preReqChecker++;
        //        if (_preReqChecker == _action.PreRequisites().Count) // if the current human state correlate to all the prereq of teh action. //we have found the end of a viable path.
        //        {
        //            path.Add(_action);
        //            path.Reverse();
        //            foundPath = true;
        //            return true;
        //        }
        //        else // else we set teh new goals to correlate to the prereq of teh action.  We can do this because we know the goals of the action is by extention leading to the original goal set by the goal manager.
        //        {
        //            path.Add(_action);
        //            _goals.Clear();
        //            _goals.Add(_action.PreRequisites()[_preReqChecker]);
        //            return true;
        //        }
        //    }
        //}
        return false;
    }
}

public static class ReturnNewList
{   
    public static  List<HumanAIState> GetEmptyPath()
    {
        return new List<HumanAIState>();        
    }
}
