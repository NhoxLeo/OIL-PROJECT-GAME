using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerInformationScrollView : MonoBehaviour
{
  private Transform pm;
  public GameObject content;
  private Human temp;
  public GameObject workerItemPrefab;
  private Text generalInfo;
  private float updateTimer;
  private short thirsty, hungry, homeless, unemployed;

  public void OnEnable()
  {
    generalInfo = transform.parent.Find("GeneralInfo").GetComponent<Text>();
    updateTimer = 1.0f;
    pm = GameObject.Find("Population").transform;
    PopulateList();
  }

  public void Update()
  {
    updateTimer -= Time.deltaTime;
    if (updateTimer < 0f)
    {
      updateTimer = 1.0f;
      PopulateList();
    }
  }

  private void PopulateList()
  {
    hungry = thirsty = homeless = unemployed = 0;
    foreach (Transform child in content.transform)
      Destroy(child.gameObject);
    foreach (Transform human in pm)
    {
      temp = human.GetComponent<Human>();
      GameObject item = Instantiate(workerItemPrefab, content.transform);
      item.transform.GetChild(0).GetComponent<Text>().text = temp.GetName();
      item.transform.GetChild(1).GetComponent<Text>().text = "" + temp.AgeService.Age;// GetAge();
      item.transform.GetChild(2).GetComponent<Text>().text = "" + (int)temp.GetMoral();
      if (temp.IsSkilledWorker)
        item.transform.GetChild(3).GetComponent<Text>().text = "Upper";
      else
        item.transform.GetChild(3).GetComponent<Text>().text = "Lower";
      item.transform.GetChild(4).GetComponent<Text>().text = "null";
      item.transform.GetChild(5).GetComponent<Text>().text = "" + temp.GetThirst();
      item.transform.GetChild(6).GetComponent<Text>().text = "" + temp.GetHunger();
      if (temp.GetHome() == 100)
        item.transform.GetChild(7).GetComponent<Text>().text = "Yes";
      else
        item.transform.GetChild(7).GetComponent<Text>().text = "No";
      item.transform.GetChild(8).GetComponent<Text>().text = "" + temp.GetComfort();
      // General info
      if (temp.GetHunger() < 0)
        hungry++;
      if (temp.GetThirst() < 0)
        thirsty++;
    }
    homeless = pm.GetComponent<PopulationManager>().GetHomelessCount();
    unemployed = pm.GetComponent<PopulationManager>().GetUnemployedCount();
    generalInfo.text = "<b>General Info:</b>\nHomeless: " + homeless + "\nUnemployed: " + unemployed + "\nThirsty: " + thirsty + "\nStarving: " + hungry + "\n";
  }
}
