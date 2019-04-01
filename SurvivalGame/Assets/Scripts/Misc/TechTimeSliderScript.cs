using Assets.Scripts.Misc.Techs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechTimeSliderScript : MonoBehaviour
{
  private bool isResearching;
  private float researchTimeProgress, researchTimeGoal;
  public GameObject slider;
  private GameObject currentTech;
  private GameObject currentTechButton;

  [SerializeField]
  Text currentResearch;

  /// <summary>
  /// An event that all buttons in tech tree will listen to
  /// to be able to have a countdown and not be clickable while
  /// something is being researched.
  /// </summary>
  public event EventHandler<GameObject> ResearchStartedEvent;

  public event EventHandler<GameObject> ResearchFinishedEvent;

  void Update()
  {
    if (isResearching)
    {
      if (researchTimeProgress < researchTimeGoal)
      {
        researchTimeProgress += Time.deltaTime;
        slider.GetComponent<Slider>().value += Time.deltaTime;
      }
      else
      {
        isResearching = false;
        slider.GetComponent<Slider>().value = 0;
        slider.transform.localScale = new Vector3(0, 0, 0);
        currentTech.GetComponent<Tech>().Research(true);
        currentResearch.text = "";
        ResearchFinishedEvent(this, currentTechButton);
      }
    }
  }

  public void StartResearch(float researchTimeGoal, GameObject currentTech, GameObject ClickedButton)
  {
    this.currentTech = currentTech;
    currentTechButton = ClickedButton;
    isResearching = true;
    researchTimeProgress = 0f;
    this.researchTimeGoal = researchTimeGoal;
    slider.GetComponent<Slider>().maxValue = researchTimeGoal;
    slider.transform.localScale = new Vector3(1, 1, 1);
    currentResearch.text = ClickedButton.gameObject.GetComponentInChildren<Text>().text;
    ResearchStartedEvent(this, ClickedButton);
  }

  public bool IsResearching()
  {
    return isResearching;
  }
}
