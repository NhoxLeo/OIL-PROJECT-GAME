using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using UnityEngine.Analytics;
using System.Runtime.CompilerServices;
using System;


public enum WorkSiteType { Forest, MetalScraps, Water, Hunting }

public class WorkSite : Building, IVisitable
{
  [SerializeField]
  public WorkSiteType SiteType;

  [SerializeField]
  private float interactionRadiusSetter = 1.75f;

  private float visitTime = 3000;

  public float VisitTime { get { return visitTime; } }


  public override void Start()
  {
    base.Start();
    interactionRadius = interactionRadiusSetter;
  }


  public double GetWorkingTime()
  {
    return 3;
  }

  public void HandleVisitor(GameObject visitor)
  {
    throw new NotImplementedException();
  }
}
