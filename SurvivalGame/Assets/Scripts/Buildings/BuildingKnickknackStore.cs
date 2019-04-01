using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingKnickknackStore : BuildingProduction, IVisitable
{
  private int knickknacks = 0;

  public float VisitTime { get; } = 2000;

  public new void Start()
  {
    name = "Knick-knack Store";
    currentResource = GlobalConstants.Resources.KNICKKNACK;
    maxWorkers = 2;
    productionTime = 40f;
    productionTimeLeft = productionTime;
    oilUsage = 0.1f;
    base.Start();
  }

  protected override void ProduceResource()
  {
    knickknacks += (int)(0.5f * activeWorkers);
    SetInformationText(Camera.main.transform.Find("ObjectInfo").gameObject);
  }

  /// <summary>
  /// Checks if the worker receives some knickknack, in which case this method returns true.
  /// </summary>
  /// <returns></returns>
  public void HandleVisitor(GameObject customer)
  {
    if (knickknacks > 0)
    {
      knickknacks--;
      //customer.GetComponent<Human>().BUFFMORALE;
    }
  }

  public override void SetInformationText(GameObject objectInfo)
  {
    objectInfo.transform.Find("ClickedObjectInfo").GetComponent<Text>().text = name + "\nGoods: " + knickknacks + "\nWorkers: " + workerList.Count + "/" + maxWorkers;
    try
    {
      GameObject.Find("ResidenceBuilding").gameObject.SetActive(false);
      GameObject.Find("ResourceDepotBuilding").gameObject.SetActive(false);
      GameObject.Find("UpgradeBuildingButton").gameObject.SetActive(false);
      GameObject.Find("SchoolBuilding").gameObject.SetActive(false);
      GameObject.Find("RocketPanel").gameObject.SetActive(false);
        }
    catch { }
  }

  public override int[] GetCost()
  {
    return GlobalConstants.knickknackStoreCost;
  }
}

