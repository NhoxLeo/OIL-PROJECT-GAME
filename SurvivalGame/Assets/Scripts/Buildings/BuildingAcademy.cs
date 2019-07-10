using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingAcademy : BuildingProduction
{
    private class Students
    {
        public GameObject studentGameObj { get; }
        public float EducationTime { get; set; }
        public Students(GameObject student, float educationTime)
        {
            studentGameObj = student;
            EducationTime = educationTime;
        }
    }

    private List<Students> students;
    private int maxStudents;
    private float educationTimeLeft;
    float educationTime = 300;

    public new void Start()
    {
        name = "Academy";
        educationTimeLeft = educationTime;
        maxWorkers = 1;
        maxStudents = 15;
        students = new List<Students>();
        productionTime = 1500f;
        productionTimeLeft = productionTime;
        base.Start();
    }

    public override void Update()
    {
        if (workerList.Count > 0)
        {
            foreach (Students student in students)
            {
                student.EducationTime -= Time.deltaTime;
                if (student.EducationTime <= 0)
                {
                    student.studentGameObj.GetComponent<Human>().SetSpecialized();
                  //  student.studentGameObj.GetComponent<HumanStateMachine>().ChangeWorkState(null); //TEST
                    students.Remove(student);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Assigns a student to the academy
    /// </summary>
    /// <param name="student">The transform of the assigned child.</param>
    public bool AssignStudent(GameObject student)
    {
        if (!student.GetComponent<Human>().IsSpecialized)
        {
            if (students.Count < maxStudents)
            {
                students.Add(new Students(student, educationTime));
            }
            SetInformationText(GameObject.Find("BuildingInfo").gameObject);
            return true;
        }
        return false;
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
        objectInfo.transform.Find("ClickedObjectInfo").GetComponent<Text>().text = name + "\nLevel: " + buildingLevel + "\nTeachers: " + workerList.Count + "/" + maxWorkers + "\nStudents: " + students.Count + "/" + maxStudents;
    }

    /// <summary>
    /// Returns the cost of this academy as an int array where 0 = wood and 1 = steel.
    /// </summary>
    /// <returns></returns>
    public override int[] GetCost()
    {
        switch (buildingLevel)
        {
            case 1: return GlobalConstants.academyCost;
            default: return null;
        }
    }
}
