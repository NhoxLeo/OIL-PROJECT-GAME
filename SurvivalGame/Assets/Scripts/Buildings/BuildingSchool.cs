using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSchool : BuildingProduction
{
  private class PupilEducationObj
  {
    public GameObject pupilGameObj { get; }
    public float EducationTime { get; set; }
    public PupilEducationObj(GameObject pupil, float educationTime)
    {
      pupilGameObj = pupil;
      EducationTime = educationTime;
    }
  }

  private List<PupilEducationObj> pupils;
  private int maxPupils;
  private float educationTimeLeft;
  float educationTime = 30; // Maybe go for 240?

  public new void Start()
  {
    name = "School";
    educationTimeLeft = educationTime;
    maxWorkers = 1;
    maxPupils = 5;
    pupils = new List<PupilEducationObj>();
    productionTime = 500f;
    productionTimeLeft = productionTime;
    base.Start();
  }

  /// <summary>
  /// Counts down the time a child spends in school. Changed dictionary because there would be multiple objects with the same key othwerwise.
  /// </summary>
  public override void Update()
  {
    if (workerList.Count > 0)
    {
      foreach (PupilEducationObj pupil in pupils)
      {
        pupil.EducationTime -= Time.deltaTime;
        if (pupil.EducationTime <= 0)
        {
          pupil.pupilGameObj.GetComponent<Human>().SetSkilled();
          //pupil.pupilGameObj.GetComponent<HumanStateMachine>().ChangeWorkState(null); // TEST
          pupils.Remove(pupil);
          if (UpgradeManager.SchoolPlayground)
          {
            pupil.pupilGameObj.GetComponent<Human>().AddBuff(new Buff("Playground", -10f, pupil.pupilGameObj));
          }
          try
          {
            SetInformationText(GameObject.Find("BuildingInfo").gameObject);
          }
          catch { }
          break;
        }
      }
    }
  }

  /// <summary>
  /// Assigns a child to this school.
  /// </summary>
  /// <param name="pupil">The transform of the assigned child.</param>
  public bool AssignPupil(GameObject pupil)
  {
    if (pupils.Count < maxPupils)
    {
      pupils.Add(new PupilEducationObj(pupil, educationTime));
      if (UpgradeManager.SchoolPlayground)
      {
        pupil.GetComponent<Human>().AddBuff(new Buff("Playground", 10f, pupil));
      }
      SetInformationText(GameObject.Find("BuildingInfo").gameObject);
      return true;
    }
    return false;
  }

  protected override bool CanUpgrade()
  {
    if (buildingLevel + 1 <= UpgradeManager.schoolAllowedUpgrade && buildingLevel + 1 <= UpgradeManager.schoolMaxUpgrades)
      return true;
    else
      return base.CanUpgrade();
  }

  /// <summary>
  /// Upgrades the level of this school.
  /// </summary>
  public override void Upgrade()
  {
    if (CanUpgrade())
    {
      maxPupils = 10;
      base.Upgrade();
    }
  }
  public void BuildPlayground()
  {
    foreach (PupilEducationObj pupil in pupils)
    {
      pupil.pupilGameObj.GetComponent<Human>().AddBuff(new Buff("Playground", 10f, pupil.pupilGameObj));
    }
  }

  public override void SetBuildingUIType(GameObject buildingTypes)
  {
    foreach (Transform item in buildingTypes.transform)
    {
      var UIbuilding = item.gameObject;
      if (UIbuilding.name != "SchoolBuilding")
        UIbuilding.SetActive(false);
      else
        UIbuilding.SetActive(true);
    }
  }

  /// <summary>
  /// Sets the information text.
  /// </summary>
  /// <param name="objectInfo">The objectInfo-transform.</param>
  public override void SetInformationText(GameObject objectInfo)
  {
    objectInfo.transform.Find("ClickedObjectInfo").GetComponent<Text>().text = name + "\nLevel: " + buildingLevel + "\nTeachers: " + workerList.Count + "/" + maxWorkers + "\nPupils: " + pupils.Count + "/" + maxPupils;
  }

  /// <summary>
  /// Returns the cost of this school as an int array where 0 = wood and 1 = steel.
  /// </summary>
  /// <returns></returns>
  public override int[] GetCost()
  {
    switch (buildingLevel)
    {
      case 1: return GlobalConstants.schoolCost;
      case 2: return GlobalConstants.school2Cost;
      default: return null;
    }
  }
}
