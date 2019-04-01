using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Assets.Scripts.Humans.AIStates.HumanStates.Children;

public class FSM
{
    protected GameObject FSMOwner;
    protected string prevState;
    public string currentState { get; protected set; }

    //protected Action stateLogicToUpdate;
    protected StateCollection states;

    public FSM(GameObject gameObject)
    {
        FSMOwner = gameObject;
        states = new StateCollection();
        states.OnStateChangedEvent += HandleStateChanged;
    }

    protected virtual void HandleStateChanged(object sender, StateChangedEventArgs e)
    {
        ChangeState(e.OldState, e.NewState);
    }

    public void AddState(string stateName, AIState state)
    {
        states.AddState(stateName, state);
    }

    public void RemoveState(string humanStateName)
    {
        states.RemoveState(humanStateName);

        if (currentState == humanStateName)
        {
            ChangeState(currentState, HumanStateGlobals.IDLE);
        }
    }

    public void AddStartState(string stateName, AIState state)
    {
        states.AddState(stateName, state);
        currentState = stateName;
        ChangeState(stateName, stateName);
    }

    protected void ChangeState(string oldState, string newState)
    {
        prevState = oldState;
        currentState = newState;
        states[newState].Enter(FSMOwner, newState);
    }

    public void FSMUpdate()
    {
        if (states[currentState] != null)
        {
            states[currentState].ExecuteState();
        }
    }
}

#region 
public class StateCollection : Dictionary<string, AIState>
{
    public event EventHandler<StateChangedEventArgs> OnStateChangedEvent;

    public void AddState(string stateKey, AIState newState)
    {
        newState.OnExitState += HandleExitState;
        Add(stateKey, newState);
    }

    public void RemoveState(string state)
    {
        this[state].OnExitState -= HandleExitState;
    }

    void HandleExitState(object sender, StateChangedEventArgs e)
    {
        //Debug.Log("Sender:  " + sender.GetType() + " Changed State: " + e.OldState + " ---> " + e.NewState);
        OnStateChangedEvent.Invoke(this, e);
        //OnStateChangedEvent(this, e);
    }
}

public class StateChangedEventArgs : EventArgs
{
    public string OldState { get; private set; }
    public string NewState { get; private set; }

    /// <summary>
    /// Write "Previous" as newState if you wanna make a transition to previous state.
    /// </summary>
    /// <param name="oldState"></param>
    /// <param name="newState"></param>
    public StateChangedEventArgs(string oldState, string newState)
    {
        this.OldState = oldState;
        this.NewState = newState;
    }
}
#endregion

public static class HumanStateGlobals
{
    public static string IDLE = "IDLE";
    public static string WORKING = "WORKING";
    public static string WORKINGWORKSITE = "WORKINGWORKSITE";
    public static string RESTING = "RESTING";
    public static string SCHOOL = "SCHOOL";
    public static string RANDOMINTERACTION = "RANDOMINTERACTION";

    public static HumanAIState GetAIState(string humanState)
    {
        if (humanState == IDLE)
        {
            return new IdleState();
        }
        else if (humanState == WORKING)
        {
            return new WorkState();
        }
        else if (humanState == WORKINGWORKSITE)
        {
            return new WorkWorkSiteState();
        }
        else if (humanState == RESTING)
        {
            return new RestingState();
        }
        else if (humanState == SCHOOL)
        {
            return new SchoolState();
        }
        else if (humanState == RANDOMINTERACTION)
        {
            return new RandomInteractionState();
        }
        return null;
    }
}

public class HumanFSM : FSM
{
    readonly Human human;

    public HumanFSM(GameObject gameObject) : base(gameObject)
    {
        human = gameObject.GetComponent<Human>();
    }

    public HumanFSM(GameObject gameObject, string startState) : base(gameObject)
    {
        human = gameObject.GetComponent<Human>();
        human.SuddenNeedEvent += HandleSuddenHumanNeed;
    }

    protected override void HandleStateChanged(object sender, StateChangedEventArgs e)
    {
        //if the state before the Exit was a RandomInteraction (was at bar, knick-knack etc)
        if (e.OldState == HumanStateGlobals.RANDOMINTERACTION)
        {
            //Change from randomInteraction to do what the human was doing before.
            ChangeState(e.OldState, prevState);
        }
        else
            base.HandleStateChanged(sender, e);
    }

    private void HandleSuddenHumanNeed(object sender, EventArgs e)
    {
        if (currentState != HumanStateGlobals.RANDOMINTERACTION)
        {
            states[currentState].Exit(HumanStateGlobals.RANDOMINTERACTION);
        }
    }

    public void ChangeOccupationStatus(GameObject productionBuildingObj)
    {
        var workplace = productionBuildingObj;

        if (human.GetComfort() > 0 && workplace)
        {
            human.SetNewHumanLocation(LocationTarget.OccupationBuilding, workplace);
            if (human.GetAgeCategory() == HumanAgeService.AgeCategory.Adult)
            {
                states[currentState].Exit(HumanStateGlobals.WORKING);
                human.HasJob = true;
            }
            else if (GlobalConstants.ChildLabor)
            {
                states[currentState].Exit(HumanStateGlobals.WORKING);
                human.HasJob = true;
            }
            else if (!GlobalConstants.ChildLabor && human.GetAgeCategory() == HumanAgeService.AgeCategory.Child)
            {
                if (workplace.tag == ("BuildingProductionSchool"))
                {
                    states[currentState].Exit(HumanStateGlobals.SCHOOL);
                    human.HasJob = true;
                }
            }

        }
        else if (!workplace)
        {
            states[currentState].Exit(HumanStateGlobals.IDLE);
            human.HasJob = false;
        }
    }
}


