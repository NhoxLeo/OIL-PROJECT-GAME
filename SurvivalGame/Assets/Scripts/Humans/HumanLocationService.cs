using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationTarget
{
  None, // not set
  OccupationBuilding, // The main occupation building, Scool, WorkPlace
  WorkSite, // The WorkSite connected to a work
  Home,
  IdlePos, // Idle Shelter
  RandomInteractionBuilding //Bar, Knick-Knack, Hospital ///first Check if there is an existing one.
}

public class HumanLocationService : Dictionary<LocationTarget, GameObject>
{
  public event EventHandler RandomInteractionEvent;

  public HumanLocationService()
  {
    //Fills the dictionary with possible locations for a human.
    Add(LocationTarget.OccupationBuilding, null);
    Add(LocationTarget.Home, null);
    Add(LocationTarget.None, null);
    Add(LocationTarget.WorkSite, null);
    Add(LocationTarget.RandomInteractionBuilding, null);
  }

  public void SetBuildingOfInterest(LocationTarget locationType, GameObject locationObject)
  {
    this[locationType] = locationObject;
  }
}
