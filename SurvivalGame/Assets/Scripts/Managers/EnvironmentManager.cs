using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{
  public int Temperature { get { return temperature; } set { temperature = value; } }
  private int temperature = 30, oldTemperature;
  private int deviation = 3;
  private Transform population;
  // Change over time
  private bool periodicallyAffectTemperature;
  private int targetTemp, fromTemp;
  private float timePeriod, timePassed;
  private Eclipse eclipseScript;
  // Global temperature flucturations
  private float fluctureTimer, fluctureTimerStart = 30f;// / 2; test
  private float overallTempTimer, overallTempTimerStart = 120f;// / 5; test

  private void Start()
  {
    fluctureTimer = fluctureTimerStart;
    overallTempTimer = overallTempTimerStart;
    eclipseScript = GameObject.Find("Sun").GetComponent<Eclipse>();
    periodicallyAffectTemperature = false;
    population = GameObject.Find("Population").transform;
  }

  void Update()
  {
    transform.GetChild(0).GetComponent<Text>().text = temperature.ToString() + "°C";
    oldTemperature = temperature;
    DebugTemperature();
    if (oldTemperature != temperature)
      CheckThresholds();
    if (periodicallyAffectTemperature)
      PeriodicallyAffectTemperature();
    FluctureTemperature();
    RaiseOverallTemperature();
  }

  private void FluctureTemperature()
  {
    fluctureTimer -= Time.deltaTime;
    if (fluctureTimer < 0f)
    {
      temperature += UnityEngine.Random.Range(-deviation / 2, deviation);
      fluctureTimer = fluctureTimerStart;
    }
  }

  private void RaiseOverallTemperature()
  {
    overallTempTimer -= Time.deltaTime;
    if (overallTempTimer < 0f)
    {
      temperature++;
      overallTempTimer = overallTempTimerStart;
    }
  }

  private void CheckThresholds()
  {
    if (temperature >= GlobalConstants.comfortThreshold && temperature >= GlobalConstants.thirstThreshold)
    {
      foreach (Transform human in population.transform)
      {
        // human.GetComponent<Human>().ChangeMultiplierValue(HumanNeedMulitplierType.Both);
        human.GetComponent<Human>().ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, false);
        human.GetComponent<Human>().ChangeMultiplierValue(HumanNeedMulitplierType.Thirst, false);
      }
    }
    else if (temperature >= GlobalConstants.comfortThreshold)
    {
      foreach (Transform human in population.transform)
      {
        human.GetComponent<Human>().ChangeMultiplierValue(HumanNeedMulitplierType.Comfort, false);
      }
    }
    else if (temperature >= GlobalConstants.thirstThreshold)
    {
      foreach (Transform human in population.transform)
      {
        human.GetComponent<Human>().ChangeMultiplierValue(HumanNeedMulitplierType.Thirst, false);
      }
    }
  }

  /// <summary>
  /// Changes the temperature.
  /// </summary>
  /// <param name="newTemperature">The new temperature.</param>
  /// <param name="timePeriod">The time until the new temperature is reached, leave blank if it happens immediatly.</param>
  public void SetTemperature(int newTemperature, float timePeriod = 0f, bool eclipse = false)
  {   // TODO: Make into a coroutine!
    if (!eclipseScript.enabled || eclipse)
    {
      this.timePeriod = timePeriod;
      if (timePeriod == 0f)
      {
        temperature = newTemperature;
      }
      else
      {
        periodicallyAffectTemperature = true;
        targetTemp = newTemperature;
        fromTemp = temperature;
        timePassed = 0f;
      }
    }
  }

  private void PeriodicallyAffectTemperature()
  {
    timePassed += Time.deltaTime / timePeriod;
    temperature = (int)Mathf.SmoothStep(fromTemp, targetTemp, timePassed);
    if (temperature == targetTemp)
      periodicallyAffectTemperature = false;
  }

  /// <summary>
  /// Returns the current temperature.
  /// </summary>
  /// <returns></returns>
  public int GetTemperature()
  {
    return temperature;
  }

  void DebugTemperature()
  {
    if (Input.GetKeyDown(KeyCode.H))
    {
      //temperature++;
      temperature += 500;
    }
    else if (Input.GetKeyDown(KeyCode.J))
    {
      temperature--;
    }
  }
}
