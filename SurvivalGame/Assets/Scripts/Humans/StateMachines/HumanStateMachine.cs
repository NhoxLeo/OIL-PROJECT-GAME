using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class that will be used on a human prefab.
/// </summary>
public class HumanStateMachine : MonoBehaviour
{
  HumanFSM FSM;

  void Start()
  {
    FSM = new HumanFSM(gameObject, HumanStateGlobals.IDLE);

    AddState(HumanStateGlobals.WORKING);
    AddState(HumanStateGlobals.WORKINGWORKSITE);
    AddState(HumanStateGlobals.RESTING);
    AddState(HumanStateGlobals.SCHOOL);
    AddState(HumanStateGlobals.RANDOMINTERACTION);
    AddStartState(HumanStateGlobals.IDLE);
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

  public void AddState(string humanState)
  {
    FSM.AddState(humanState, HumanStateGlobals.GetAIState(humanState));
  }

  public void ReplaceState(string oldStateName, string newStateName)
  {
    FSM.AddState(newStateName, HumanStateGlobals.GetAIState(newStateName));
    FSM.RemoveState(oldStateName);
  }

  public void RemoveState(string humanState)
  {
    FSM.RemoveState(humanState);
  }

  void Update()
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
    FSM.AddStartState(humanStartState, HumanStateGlobals.GetAIState(humanStartState));
  }

}
