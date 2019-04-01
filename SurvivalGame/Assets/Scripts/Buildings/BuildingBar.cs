using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBar : Building, IVisitable
{
  private GameObject[] workers;
  private int maxCustomers;
  public float VisitTime { get; } = 2000;

  public new void Start()
  {
    base.Start();
    name = "Bar";
    maxCustomers = 10;
    interactionRadius = 3;
  }

  public void HandleVisitor(GameObject visitor)
  {
    visitor.GetComponent<Human>().AddBuff(new Buff("Visited bar", 10f, 60f, visitor.gameObject));
    visitor.GetComponent<Human>().SetThirst(100f);
  }

  public override void SetInformationText(GameObject objectInfo)
  {
    objectInfo.transform.Find("ClickedObjectInfo").GetComponent<Text>().text = name + "\nWorkers: N/A";
    try
    {
      GameObject.Find("ResidenceBuilding").gameObject.SetActive(false);
      GameObject.Find("ResourceDepotBuilding").gameObject.SetActive(false);
      GameObject.Find("UpgradeBuildingButton").gameObject.SetActive(false);
      GameObject.Find("SchoolBuilding").gameObject.SetActive(false);
    }
    catch { }
  }

  public override int[] GetCost()
  {
    return GlobalConstants.barCost;
  }
}
