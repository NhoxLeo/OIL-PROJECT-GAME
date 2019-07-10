using Assets.Scripts.Humans.AIStates.HumanStates.Children;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    protected GameObject FSMOwner;
    protected string prevState;
    public string currentState { get; protected set; }
    protected GOAPMachine goap;  

    //protected Action stateLogicToUpdate;
    public StateCollection states;
    

    public FSM(GameObject gameObject)
    {
        FSMOwner = gameObject;

        states = new StateCollection();
        states.OnStateChangedEvent += HandleStateChanged;
    }

    protected virtual void HandleStateChanged(object sender, FindNextState e)
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
            ChangeState(currentState, GetAIComponents.IDLE);
        }
    }

    public void AddStartState(string stateName, AIState state)
    {
        states.AddState(stateName, state);
        currentState = stateName;
        ChangeState(stateName, stateName);
    }

    public void ChangeState(string oldState, string newState)
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
            if (states[currentState].IsComplete)
            {
                ChangeState(currentState, states[currentState].newState);
            }
        }
    }    
}

#region 
public class StateCollection : Dictionary<string, AIState>
{
    public event EventHandler<FindNextState> OnStateChangedEvent;

    public void AddState(string stateKey, AIState newState)
    {
        //   if (newState != null) // Thesis added null check
        newState.OnExitState += HandleExitState;
        Add(stateKey, newState);
    }

    public void RemoveState(string state)
    {
        this[state].OnExitState -= HandleExitState;
    }

    private void HandleExitState(object sender, FindNextState e)
    {
        //Debug.Log("Sender:  " + sender.GetType() + " Changed State: " + e.OldState + " ---> " + e.NewState);
        OnStateChangedEvent.Invoke(this, e);
        //OnStateChangedEvent(this, e);

    }
}

public class FindNextState : EventArgs
{
    public string OldState { get; private set; }
    public string NewState { get; private set; }
    GOAPMachine goap; 

    /// <summary>
    /// Write "Previous" as newState if you wanna make a transition to previous state.
    /// </summary>
    /// <param name="oldState"></param>
    /// <param name="newState"></param>
    public FindNextState(string _oldState, Human _owner)
    {
        goap = _owner.GetComponent<GOAPMachine>();
        OldState = _oldState;
        NewState = goap.PlanActionSequence(_owner.GoalMachine.HumanNeeds, _owner.GoalMachine.Goals).id;                
    }
}
#endregion

public class HumanFSM : FSM
{
    private Human human;

    public HumanFSM(GameObject gameObject) : base(gameObject)
    {
        human = gameObject.GetComponent<Human>();
    }

    public HumanFSM(GameObject gameObject, string startState) : base(gameObject)
    {
        human = gameObject.GetComponent<Human>();
        human.SuddenNeedEvent += HandleSuddenHumanNeed;      
        goap = human.GetComponent<GOAPMachine>();          
      //  goap.PlanActionSequence(human.GoalMachine.HumanNeeds, human.GoalMachine.Goals);
    }   

    protected override void HandleStateChanged(object sender, FindNextState e)
    {
        //if the state before the Exit was a RandomInteraction (was at bar, knick-knack etc)
        if (e.OldState == GetAIComponents.RANDOMINTERACTION)
        {
            //Change from randomInteraction to do what the human was doing before.
            ChangeState(e.OldState, prevState);
        }
        else
            base.HandleStateChanged(sender, e);
    }

    private void HandleSuddenHumanNeed(object sender, EventArgs e)
    {
        if (currentState != GetAIComponents.RANDOMINTERACTION)
        {
            states[currentState].Exit(GetAIComponents.RANDOMINTERACTION);
        }
    }

    public void ChangeOccupationStatus(GameObject productionBuildingObj)
    {
        GameObject workplace = productionBuildingObj;

        if (human.GetComfort() > 0 && workplace)
        {
            human.SetNewHumanLocation(LocationTarget.OccupationBuilding, workplace);
            if (human.GetAgeCategory() == HumanAgeService.AgeCategory.Adult)
            {
                states[currentState].Exit(GetAIComponents.WORKING);
                human.HasJob = true;
            }
            else if (GlobalConstants.ChildLabor)
            {
                states[currentState].Exit(GetAIComponents.WORKING);
                human.HasJob = true;
            }
            else if (!GlobalConstants.ChildLabor && human.GetAgeCategory() == HumanAgeService.AgeCategory.Child)
            {
                if (workplace.tag == ("BuildingProductionSchool"))
                {
                    states[currentState].Exit(GetAIComponents.SCHOOL);
                    human.HasJob = true;
                }
            }

        }
        else if (!workplace)
        {
            states[currentState].Exit(GetAIComponents.IDLE);
            human.HasJob = false;
        }
    }
}



