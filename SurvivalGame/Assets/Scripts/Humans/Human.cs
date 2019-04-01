using Assets.Scripts.Humans;
using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//TODO 1. Fix dropoff from resourcedepot wood lvl 1, steel lvl 1. Idle ökar comfort med tex +2 when idle rest.
public static class HumanNeedMulitplierType
{
  public static string Hunger = "Hunger";
  public static string Comfort = "Comfort";
  public static string Thirst = "Thirst";
}

/// <summary>
/// General class of a human, can be a worker or a skilled worker.
/// </summary>
public class Human : PlayerInterActableBase
{
  protected GameObject resourceManager;
  private GameObject populationManager;
  private int oldAgeDeathRate = 3;
  protected GameObject environmentManager;
  public HumanAgeService AgeService { get; private set; }
  public HumanLocationService LocationService { get; private set; }

  [SerializeField]
  bool newBorn;


  /// <summary>
  /// Event to be fired when the human is need need of something extracurricular outside from work and daily life.
  /// Some nice knick-knack or maybe even a beer.
  /// </summary>
  public event EventHandler SuddenNeedEvent;

  //HumanScript
  protected float moral;
  //Hasjob is also used to check whether the human is in school or not.
  public bool HasJob, HasHome, IsSkilledWorker, IsSpecialized, IsResting, IsEaten;
  private List<Buff> activeBuffs;

  private float moralDebuff = 0f;
  private float moralMultiplier = 1;

  protected float hunger = 100f, comfort = 100f, thirst = 100f, home = 50f; // Comfort ~ heat
  private float deathThreshold = 120f, revoltThreshold = 200f, healthOddsFactor = 400f;

  private float hungerMultiplier, comfortMultiplier, thirstMultiplier, homeMultiplier;
  private float stdHungerMultiplier, stdComfortMultiplier, stdThirstMultiplier, stdHomeMultiplier, stdRestingMultiplier = 5f;

  public GameObject Residence { get; private set; }

  private float updateTimerLeft, updateTimerMax = 2f;

  public float Moral { get { return moral; } set { moral = value; } }

  public void Start()
  {
    resourceManager = GameObject.Find("ResourceManager");
    populationManager = GameObject.FindGameObjectWithTag("Population");
    environmentManager = Camera.main.transform.Find("Canvas").transform.Find("TemperaturePanel").gameObject;
    SetAttributeDeviations();

    activeBuffs = new List<Buff>();

    AgeService = new HumanAgeService(gameObject, newBorn);
    LocationService = new HumanLocationService();


  }

  private void UpdateInfo()
  {
    hoverInfoText = AgeService.Name + " " + AgeService.Age + "\n"
      + "Moral " + (int)moral + "\n"
      + "Hunger " + (int)hunger + "\n"
      + "Comfort " + (int)comfort + "\n"
      + "Thirst " + (int)thirst + "\n"
      + "Home " + (int)home + "\n";

  }

  public void SetNewHumanLocation(LocationTarget location, GameObject locationObject)
  {
    LocationService.SetBuildingOfInterest(location, locationObject);
  }

  /// <summary>
  /// Make each individual unique in his/her multipliers.
  /// </summary>
  private void SetAttributeDeviations()
  {
    float[] modifiers = populationManager.GetComponent<PopulationManager>().GetModifiers();
    hungerMultiplier = modifiers[0];
    comfortMultiplier = modifiers[1];
    thirstMultiplier = modifiers[2];
    homeMultiplier = modifiers[3];
    hungerMultiplier += UnityEngine.Random.Range(-hungerMultiplier / 2, hungerMultiplier / 2);
    comfortMultiplier += UnityEngine.Random.Range(-comfortMultiplier / 2, comfortMultiplier / 2);
    thirstMultiplier += UnityEngine.Random.Range(-thirstMultiplier / 2, thirstMultiplier / 2);
    homeMultiplier += UnityEngine.Random.Range(-homeMultiplier / 3, homeMultiplier / 3);
    stdComfortMultiplier = comfortMultiplier;
    stdHomeMultiplier = homeMultiplier;
    stdHungerMultiplier = hungerMultiplier;
    stdThirstMultiplier = thirstMultiplier;
  }

  /// <summary>
  /// Updates this human. Some parameters updates slower on a timer.
  /// </summary>
  public void Update()
  {
    updateTimerLeft -= Time.deltaTime;
    if (updateTimerLeft < 0)
    {
      updateTimerLeft = updateTimerMax;
      NeedDecay();
      AttemptToSatisfyNeeds();
      UpdateMoral();
      HealthCheck();
      UpdateInfo();
    }
    foreach (Buff buff in activeBuffs)
    {
      if (buff.IsTimedOut())
      {
        activeBuffs.Remove(buff);
        break;
      }
      buff.UpdateDuration();
    }
    if (Input.GetKeyDown(KeyCode.T))  // Debug sudden need (bar etc.)
    {
      // TriggerSuddenNeed(GlobalConstants.Needs.COMFORT);
      // ChangeMultiplierValue(HumanNeedMulitplierType.Comfort);
    }
  }

  /// <summary>
  /// Call this method when a need is low and the human should act upon that to go and do something quick to get the needs up
  /// from a building like knicknack or bar.
  /// </summary>
  /// <param name="need"></param>
  private void TriggerSuddenNeed(GlobalConstants.Needs need)
  {
    /// in each "case" set the the function below to whatever building it should go to.
    ///SetNewHumanLocation(LocationTarget.RandomInteractionBuilding, GameObject.Find("BuildingBar"));
    ///Then trigger the event.
    ///SuddenNeedEvent(this, new EventArgs());
    switch (need)
    {
      case GlobalConstants.Needs.THIRST:
        break;
      case GlobalConstants.Needs.HUNGER:
        break;
      case GlobalConstants.Needs.COMFORT:
        var tempBar = GameObject.Find("BuildingBar(Clone)");
        if (tempBar)
        {
          SetNewHumanLocation(LocationTarget.RandomInteractionBuilding, tempBar);
          SuddenNeedEvent(this, new EventArgs());
        }
        break;
      case GlobalConstants.Needs.HOME:
        break;
      default:
        break;
    }
  }

  /// <summary>
  /// The needs of this human decays over time.
  /// </summary>
  private void NeedDecay()
  {
    if (hunger >= -100f && hunger <= 100f)
      hunger += hungerMultiplier;
    if (comfort >= -100f && comfort <= 100f)
      comfort += comfortMultiplier;
    if (thirst >= -100f && thirst <= 100f)
      thirst += thirstMultiplier;
    if (home < 100f && home >= -100f)
      home += homeMultiplier;
    if (hunger > 100f)
      hunger = 100f;
    if (thirst > 100f)
      thirst = 100f;
    if (comfort > 100f)
      comfort = 100f;
  }

  /// <summary>
  /// This human attempts to satisfy a need that is in need of satisfaction.
  /// </summary>
  private void AttemptToSatisfyNeeds()
  {
    if (hunger < 25f)
    {
      if (resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.COOKEDFOOD, -1))
        hunger += 75;

      if (EmergencyBuffManager.Singleton.BuffActive(EmergencyBuffType.Eat_Shit))
      {
        if (resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.FOOD, -1))
          hunger += 50;
      }

    }
    if (thirst < 25f)
    {
      if (resourceManager.GetComponent<ResourceManager>().ManipulateResources(GlobalConstants.Resources.CLEANWATER, -1))
        thirst += 75;
    }
    //TODO tweak trigger parameters for when a a random need should happen.
    if (moral < 100)
    {
      int temp = UnityEngine.Random.Range(0, 1000);
      if (temp > 980 && AgeService.CurrentAgeCategory != HumanAgeService.AgeCategory.Child)
      {
        TriggerSuddenNeed(GlobalConstants.Needs.COMFORT);
      }

    }
  }

  public void ChangeMultiplierValue(string humanMultiplierType, bool increase)
  {
    float temp = environmentManager.GetComponent<EnvironmentManager>().Temperature;

    if (humanMultiplierType == HumanNeedMulitplierType.Thirst)
    {
      thirstMultiplier = stdThirstMultiplier - (temp * GlobalConstants.thirstDebuff);
    }
    else if (humanMultiplierType == HumanNeedMulitplierType.Comfort)
    {
      if (IsResting)
      {
        int homeMultiplier = 0;
        if (LocationService[LocationTarget.Home])
        {
          homeMultiplier = Residence.GetComponent<Building>().GetLevel();
          stdRestingMultiplier = 5;
        }
        else
        {
          stdRestingMultiplier = 3;
        }
        comfortMultiplier = stdRestingMultiplier + homeMultiplier;
      }
      else
      {
        comfortMultiplier = stdComfortMultiplier - (temp * GlobalConstants.comfortDebuff);
      }
    }
  }

  /// <summary>
  /// Checks if this human is about to die or revolt.
  /// </summary>
  private void HealthCheck()
  {
    float value = UnityEngine.Random.Range(0, deathThreshold);
    if (value > healthOddsFactor + hunger + (thirst * 1.2f) + (comfort * 0.9) || thirst <= -100f || hunger <= -100f)
      Die();
    value = UnityEngine.Random.Range(0, revoltThreshold);
    if (value > healthOddsFactor + hunger + (thirst * 1.2f) + (comfort * 0.9) || thirst <= -100f || hunger <= -100f)
      Revolt();
    if (AgeService.Age > 85 || (AgeService.Age > 65 && UnityEngine.Random.Range(0, 100) < oldAgeDeathRate))
    {
      Die();
    }
  }

  public void IncreaseOldAgeDeathProbability()
  {
    oldAgeDeathRate += 2;
  }

  /// <summary>
  /// Gives this human a home.
  /// </summary>
  public void SetHome(GameObject residence)
  {
    this.Residence = residence;
    //if null, then human is set to homeless
    if (residence == null)
    {
      home = 99f;
      HasHome = false;
    }
    else
    {
      home = 100f;
      HasHome = true;
    }
    SetNewHumanLocation(LocationTarget.Home, residence);
  }

  /// <summary>
  /// Calculate the summed up moral of all the needs.
  /// </summary>
  protected void UpdateMoral()
  {
    //moral = (hunger + comfort + thirst + home) / 4 + moralDebuff;
    var prevMoral = moral;
    moral = ((hunger + comfort + thirst + home) / 4) * moralMultiplier;
  }

  public float GetMoral()
  {
    return moral;
  }

  /// <summary>
  /// Better accessed via "AddBuff()"!
  /// </summary>
  /// <param name="_moralDebuff"></param>
  public void ManipulateDebuff(float _moralDebuff)
  {
    moralDebuff += _moralDebuff;
  }

  /// <summary>
  /// Better accessed via "AddBuff()"!
  /// </summary>
  /// <param name="_moralDebuff"></param>
  public void SetMoralMultiPlier(float _multiplier)
  {
    moralMultiplier = 1 - _multiplier;
    //moralChangeRate = 1 - _multiplier;
    //var s = 1;
    //Debug.Log("MoralMultiplier " + moralMultiplier);
    // moralDebuff += _moralDebuff;
  }

  /// <summary>
  /// Satisfies a certain need of a human.
  /// </summary>
  /// <param name="need">Type of need as enum</param>
  /// <param name="multiplier">How much satisfaction</param>
  public void UpdateNeed(GlobalConstants.Needs need, int multiplier)
  {
    switch (need)
    {
      case GlobalConstants.Needs.THIRST:
        thirst += multiplier;
        break;
      case GlobalConstants.Needs.HUNGER:
        hunger += multiplier;
        break;
      case GlobalConstants.Needs.COMFORT:
        comfort += multiplier;
        break;
      case GlobalConstants.Needs.HOME:
        home += multiplier;
        break;
      default:
        break;
    }
  }

  /// <summary>
  /// Adds a (de)buff to the worker.
  /// </summary>
  /// <param name="buff">The (de)buff.</param>
  public void AddBuff(Buff buff)
  {
    foreach (Buff b in activeBuffs)
    {
      if (b.Name == buff.Name)
      {
        return;
      }
    }

    activeBuffs.Add(buff);

  }

  /// <summary>
  /// Removes a (de)buff to the worker.
  /// </summary>
  /// <param name="buff">The (de)buff.</param>
  public void RemoveBuff(string buffName)
  {
    Buff buffToRemove = null;
    foreach (Buff b in activeBuffs)
    {
      if (b.Name == buffName)
      {
        buffToRemove = b;
        break;
      }
    }
    activeBuffs.Remove(buffToRemove);
  }

  /// <summary>
  /// Kills this human, that shit sad...
  /// </summary>
  private void Die()
  {
    if (Residence)
      Residence.GetComponent<BuildingHome>().MoveOut(this.gameObject);

    populationManager.GetComponent<PopulationManager>().NotifyDeath(gameObject, HasJob, HasHome);
    if (IsEaten)
      LogWindow.Singleton.AddText("<color=red>" + AgeService.Name + "</color>" + " was eaten by others.");
    else
      LogWindow.Singleton.AddText("<color=red>" + AgeService.Name + "</color>" + " has died.");
    Destroy(gameObject);
  }

  /// <summary>
  /// This human is fed up with your shit and is now revolting!
  /// </summary>
  private void Revolt()
  {

  }

  public void BreakDown()
  {
    //SetIdle();
  }

  /// <summary>
  /// Sets a worker to be a skilled worker.
  /// </summary>
  /// <param name="isSkilled">Generally true because why would you make a worker not skilled?</param>
  public void SetSkilled(bool isSkilledWorker = true)
  {
    this.IsSkilledWorker = isSkilledWorker;
  }

  public void SetSpecialized(bool isSpecialized = true)
  {
    this.IsSpecialized = isSpecialized;
  }

  /// <summary>
  /// Upgrades the clothing of the worker, ie increasing the comfort multiplier.
  /// </summary>
  public void UpgradeClothing(float amount)
  {
    stdComfortMultiplier += amount;
  }
  /// <summary>
  /// Human eat trash food, stills hunger 
  /// </summary>
  /// <param name="amount"></param>
  public void EatTrashFood(int amount)
  {
    //AddBuff(new Buff("Eats trashfood", -amount, 10, gameObject));
   // hunger -= amount;
    //stdComfortMultiplier -= amount;
  }

  public void EatPeople()
  {
    IsEaten = true;
    Die();
  }

  public HumanAgeService.AgeCategory GetAgeCategory()
  {
    return AgeService.CurrentAgeCategory;
  }

  public void SetThirst(float amount)
  {
    thirst = amount;
  }

  public string GetName()
  {
    return AgeService.Name;
  }

  public int GetHunger()
  {
    return (int)hunger;
  }

  public void DecreaseHunger(float amount)
  {
    hunger -= amount;
  }

  public int GetThirst()
  {
    return (int)thirst;
  }

  public int GetComfort()
  {
    return (int)comfort;
  }

  public int GetHome()
  {
    return (int)home;
  }

  public float GetHomeStatus()
  {
    return home;
  }
}
