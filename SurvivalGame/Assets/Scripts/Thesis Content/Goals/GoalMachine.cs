using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// evalutes and fuzzifies human traits -> sets goal state
/// </summary>
public class GoalMachine
{
    private Human human;
    private Dictionary<string, float> humanStateOfCondition;
    public Dictionary<string, bool> HumanNeeds { get; private set; }
    public Dictionary<string, bool> Goals { get; private set; }
    private readonly bool isHungry, isThirsty, isSleepy;
    private float lowestValueTrait;
    private readonly int threshhold;
    private readonly float speed;
    private bool variableNeed;
    private bool isResting;
    private readonly int maxComfort = 100;
    private bool priorityChanged;   
    public bool ForceEval;
    public GoalMachine(Human _human, float _speed)
    {
        human = _human;
        threshhold = 25;
        speed = _speed;
    }

    public Dictionary<string, bool> UpdateGoalsAndConditions()
    {
        lowestValueTrait = 100;
        variableNeed = false;

        GetHumanData();
        //if (humanStateOfCondition["occupation"] <= 0)
        //{
        //    hasEvaluated = false;
        //}
        EvaluateHuamnTraits();
        SelectGoal();
        return Goals;
    }

    private void EvaluateHuamnTraits()
    {
        string currentKey = string.Empty;
        foreach (KeyValuePair<string, float> item in humanStateOfCondition)
        {
            if ((item.Key == "hunger" || item.Key == "thirst" || item.Key == "comfort") && item.Value < lowestValueTrait)
            {
                lowestValueTrait = item.Value;
                currentKey = item.Key;
                if (lowestValueTrait < threshhold)
                {
                    SetVariableGoal(item.Key);
                }
            }
        }
    }
        //if (humanStateOfCondition["hunger"] < threshhold && !isResting)
        //{
        //    SetGoalHunger();
        //}
        //else if (humanStateOfCondition["thirst"] < threshhold && !isResting)
        //{
        //    SetGoalThirst();
        //}
        //else if (humanStateOfCondition["comfort"] < threshhold && !isResting)
        //{
        //    SetGoalComfort();
        //}
        //if (isResting && humanStateOfCondition["comfort"] >= maxComfort)
        //{
        //    isResting = false;
        //}
    //}

    void SetVariableGoal(string _variable)
    {        
        if (_variable == "hunger")
        {
            SetGoalHunger();
        }
        else if (_variable == "thirst")
        {
            SetGoalThirst();
        }
        else
        {
            SetGoalComfort();
        }
        if (isResting && humanStateOfCondition["comfort"] >= maxComfort)
        {
            isResting = false;
        }
    }

    private void SetGoalHunger()
    {
        lowestValueTrait = humanStateOfCondition["hunger"];
        HumanNeeds["hunger"] = true;
        Goals.Add("hunger", false);
        variableNeed = true;
        ForceEval = true;
    }

    private void SetGoalThirst()
    {
        HumanNeeds["hunger"] = false;
        lowestValueTrait = humanStateOfCondition["thirst"];
        HumanNeeds["thirst"] = true;
        Goals.Add("thirst", false);
        variableNeed = true;
        ForceEval = true;
    }

    private void SetGoalComfort()
    {
        HumanNeeds["hunger"] = false;
        HumanNeeds["thirst"] = false;
        lowestValueTrait = humanStateOfCondition["comfort"];
        HumanNeeds["comfort"] = true;
        Goals.Add("isIdling", true);
        variableNeed = true;
        isResting = true;
        ForceEval = true;
    }

    private void SelectGoal()
    {
        if (!variableNeed && !isResting) // if there is no other need
        {
            if (humanStateOfCondition["occupation"] > 0)
            {
                if (GlobalConstants.advGoalEnabled)
                {
                    if (ForceEval == true)
                    {                       
                        ForceEval = false;
                        HumanNeeds["occupation"] = false;                                           
                        priorityChanged = EvaluateDistanceToTarget();                                         
                    }
                    else if(!priorityChanged)
                    {                        
                        HumanNeeds["occupation"] = true;

                        if (human.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().workSite)
                        {
                            Goals.Add("isGathering", true);
                        }
                        else
                        {
                            Goals.Add("isWorking", true);
                        }
                    }                   
                }
                else
                {
                    HumanNeeds["occupation"] = true;

                    if (human.LocationService[LocationTarget.OccupationBuilding].GetComponent<BuildingProduction>().workSite)
                    {
                        Goals.Add("isGathering", true);
                    }
                    else
                    {
                        Goals.Add("isWorking", true);
                    }
                }
            }
            else
            {
                Goals.Add("isIdling", true);
            }
        }
    }

    public bool EvaluateDistanceToTarget()
    {
        GetHumanData();        
        // float dist = GetDistance(human.transform.position, human.OccupationBuilding.transform.position);
        float timeWalkToOccupationBuilding = (GetDistance(human.transform.position, human.OccupationBuilding.transform.position) / speed);        
        float timeWalkToCanteen = (GetDistance(human.OccupationBuilding.transform.position, ValidateTarget(human.ClosestCanteen)) / speed);
        float timeWalkToWell = (GetDistance(human.OccupationBuilding.transform.position, ValidateTarget(human.ClosestWaterWell)) / speed);
        float timeWalkToHome = (GetDistance(human.OccupationBuilding.transform.position, ValidateTarget(human.Residence)) / speed);

        float predictedHunger = (human.RateOfNeddsDecay["hungerrate"] * (timeWalkToOccupationBuilding + timeWalkToCanteen)) + humanStateOfCondition["hunger"];
        float predtictedThirst = (human.RateOfNeddsDecay["thirstrate"] * (timeWalkToOccupationBuilding + timeWalkToWell)) + humanStateOfCondition["thirst"];
        float prefictedComfort = (human.RateOfNeddsDecay["comfortrate"] * (timeWalkToOccupationBuilding + timeWalkToHome)) + humanStateOfCondition["comfort"];

        return AdjustTarget(predictedHunger, predtictedThirst, prefictedComfort);
    }

    private float GetDistance(Vector3 value1, Vector3 value2)
    {
        return Vector3.Distance(value1, value2);
    }

    private Vector3 ValidateTarget(GameObject target)
    {
        if(target == null)
        {
            return human.idlePos;
        }
        else
        {
            return target.transform.position;
        }
    }

    private void WorksiteMultiplier(float value) => value *= 2;

    private bool AdjustTarget(float _predictedHunger, float _predictedThirst, float _predictedComfort)
    {
        if (_predictedHunger < threshhold)
        {
            SetGoalHunger();        
            return true;
        }
        else if (_predictedThirst < threshhold)
        {
            SetGoalThirst();          
            return true;
        }
        else if (_predictedComfort < threshhold)
        {
            SetGoalComfort();          
            return true;
        }
        else
        {
            return false;
        }
    }

    private void InitCollections()
    {
        if (HumanNeeds == null)
        {
            HumanNeeds = new Dictionary<string, bool>();
        }
        else
        {
            HumanNeeds.Clear();
        }

        if (Goals == null)
        {
            Goals = new Dictionary<string, bool>();
        }
        else
        {
            Goals.Clear();
        }
    }
    private void GetHumanValues()
    {
        humanStateOfCondition = human.GetHumanValues();
    }

    private void GetHumanData()
    {
        InitCollections();

        GetHumanValues();
        foreach (KeyValuePair<string, float> item in humanStateOfCondition)
        {
            HumanNeeds.Add(item.Key, false);
        }
    }
}
