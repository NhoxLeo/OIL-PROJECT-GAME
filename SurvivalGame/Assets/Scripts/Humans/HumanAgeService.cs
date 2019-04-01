using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAgeService
{
  public enum AgeCategory { Adult, Pensioner, Child, NotSet, }

  private AgeCategory currentAgeCategory = AgeCategory.NotSet;
  public AgeCategory CurrentAgeCategory { get { return currentAgeCategory; } }

  private int age;
  public int Age { get { return age; } }
  bool isMale;
  public bool IsMale { get { return isMale; } }
  string name;
  public string Name { get { return name; } }

  private GameObject humanGameObject;

  public HumanAgeService(GameObject humanGameObject, bool newBorn)
  {
    this.humanGameObject = humanGameObject;
    if (!newBorn)
    {
      age = UnityEngine.Random.Range(GlobalConstants.initialAdultAge[0], GlobalConstants.initialAdultAge[1]);
    }
    else
      age = 0;

    if (UnityEngine.Random.Range((int)0, (int)2) == 0)
      isMale = true;
    else
      isMale = false;
    name = GlobalConstants.Baptize(isMale);

    currentAgeCategory = GetAgeCategory(age);
    Grow();
  }

  public void UpdateAge()
  {
    age++;

    humanGameObject.GetComponent<Human>().IncreaseOldAgeDeathProbability();
    if (currentAgeCategory != GetAgeCategory(age))
    {
      Grow();
    }

    TryGetChild();
  }

  public AgeCategory GetAgeCategory(int newAge)
  {
    if (newAge < 18)
    {
      currentAgeCategory = AgeCategory.Child;
      return currentAgeCategory;
    }
    else if (newAge >= 18 && newAge < 65)
    {
      currentAgeCategory = AgeCategory.Adult;
      return currentAgeCategory;
    }
    else
    {
      currentAgeCategory = AgeCategory.Pensioner;
      return currentAgeCategory;
    }
  }

  private void Grow()
  {
    Vector3 currentScale = humanGameObject.GetComponent<Transform>().localScale;

    switch (currentAgeCategory)
    {
      case AgeCategory.Child:
        currentScale = new Vector3(8, 8, 8);
        break;
      case AgeCategory.Adult:
        currentScale = new Vector3(16.44657f, 16.44657f, 16.44657f);
        break;
      case AgeCategory.Pensioner:
        currentScale = new Vector3(20.44657f, 12.44657f, 25.44657f);
        break;
      default:
        break;
    }
    humanGameObject.GetComponent<Transform>().localScale = currentScale;
  }

  public void TryGetChild()
  {
    if (currentAgeCategory != AgeCategory.Adult || isMale)
      return;

    var s = UnityEngine.Random.Range(0, 20);
    if (s == 15)
    {
      GameObject.FindGameObjectWithTag("Population").GetComponent<PopulationManager>().CreateChild(humanGameObject);
    }
  }
}