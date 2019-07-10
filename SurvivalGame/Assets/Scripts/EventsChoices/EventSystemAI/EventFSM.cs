using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class EventFSM : FSM
//{
//    readonly EventManager eventMan;
//    public EventFSM(GameObject gameObject, string startState) : base(gameObject)
//    {
//        eventMan = gameObject.GetComponent<EventManager>();
//    }
//}


//public static class EventStateGlobals
//{
//    public static string COOLDOWN = "COOLDOWN";
//    public static string EVALUATE = "EVALUATE";
//    public static string SELECT = "SELECT";
//    public static string APPLY = "APPLY";

//    public static EventAIState GetAIState(string eventState)
//    {
//        if (eventState == COOLDOWN)
//        {
//            return new CooldownState();
//        }
//        else if (eventState == EVALUATE)
//        {
//            return new EvaluateEventState();
//        }
//        else if (eventState == SELECT)
//        {
//            return new SelectEventState();
//        }
//        else if (eventState == APPLY)
//        {
//            return new ApplyEventState();
//        }
//        return null;
//    }
//}

//public class EventStateMachine : MonoBehaviour // in i eventManager
//{
//    EventFSM FSM;

//    void Start()
//    {
//        FSM = new EventFSM(gameObject, EventStateGlobals.COOLDOWN);

//        //FSM.AddState(EventStateGlobals.EVALUATE, new EvaluateEventState());
//        //FSM.AddState(EventStateGlobals.SELECT, new SelectEventState());
//        //FSM.AddState(EventStateGlobals.APPLY, new ApplyEventState());
//        //FSM.AddStartState(EventStateGlobals.COOLDOWN, new CooldownState());

//    }

//    public string GetState()
//    {
//        if (FSM != null)
//        {
//            return FSM.currentState;
//        }
//        return null;
//    }

//    void Update()
//    {
//        if (FSM != null)
//        {
//            FSM.FSMUpdate();
//        }
//    }
//}
//public abstract class EventAIState : AIState
//{
//    protected GameObject eventObjects;
//    protected AGameEvent[] gameEvents;
//    protected ResourceManager resMan;
//    protected TechtreeManager techTree;
//    protected PopulationManager popMan;
//    protected EventManager eventMan;

//    protected int populationAmount, homelessAmount, timesRepeated;
//    protected float duration, boostValue, deltaValue;

//    protected static Dictionary<string, float> resources, oldResources;
//    protected static Dictionary<string, int> deltaRes;
//    protected static string highestFlatRes, lowestFlatRes, rndRes, highestResPerMin, lowestResPerMin;
//    protected static float highesFlatVal, lowestFlatVal, rndVal, highestValuePerMin, lowestValuePerMin;
//    protected static int EventsTriggered;
//    protected static List<AGameEvent> selectedEvents;
//    protected static List<Human> humans;
//    protected static AGameEvent selectedEvent;
//    protected static AGameEvent randomEvent;
//    protected int baseEventInterval = 2;


//    public static int WorldEventDifficultyLevel { get; set; }
//    public static int WorldStageLevel { get; set; }
//    public static double CDTimer;
//    public static AGameEvent ForceThisEvent;

//    protected const int maxWorldEvents = 2;
//    protected Tuple<int, int> baseEventIntervalOffset = new Tuple<int, int>(180, 600); // secodns between events    

//    public override void Enter(GameObject owner, string enteringState)
//    {
//        base.Enter(owner, enteringState);
//        RetreiveManagers();

//        duration = WorldEventDifficultyLevel * .5f;
//        boostValue = WorldEventDifficultyLevel * .5f;
//    }

//    void RetreiveManagers()
//    {
//        resMan = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
//        popMan = GameObject.Find("Population").GetComponent<PopulationManager>();
//        eventMan = GameObject.Find("EventManager").GetComponent<EventManager>();
//        eventObjects = GameObject.Find("GameEvents").gameObject;
//        gameEvents = eventObjects.GetComponents<AGameEvent>();
//    }
//}

//public class CooldownState : EventAIState
//{
//    double eventTimer;

//    public override void Enter(GameObject owner, string enteringState)
//    {
//        base.Enter(owner, enteringState);
//        double timeSinceStart = Time.realtimeSinceStartup;
//        UpdateDifficulty(timeSinceStart);

//        if (selectedEvent != null && selectedEvent.TargetType == "WorldEvent")
//            eventTimer = selectedEvent.Duration;
//        else
//            eventTimer = 0;
//        InitVariables();
//    }

//    void InitVariables()
//    {
//        CDTimer = 0;
//        CDTimer -= eventTimer;
//        // baseEventInterval = UnityEngine.Random.Range(baseEventIntervalOffset.Item1, baseEventIntervalOffset.Item2);       
//        baseEventInterval = 20000;
//    }

//    void UpdateDifficulty(double timeSinceStart)
//    {
//        int time = (int)timeSinceStart;
//        WorldEventDifficultyLevel = 1;
//        if (time >= 300 && WorldStageLevel < 1) // DEBUG time set to under 5 mins
//        {
//            WorldEventDifficultyLevel = 2;
//            for (int i = 0; i < gameEvents.Length; i++)
//            {
//                if (gameEvents[i].Name == "StageOne")
//                {
//                    eventMan.ForceEvent(gameEvents[i]);
//                    WorldStageLevel = 1;
//                }
//            }
//        }

//        if (time > 1200) // DEBUG time set to over 5 mins 
//            WorldEventDifficultyLevel = 3;

//        if (time > 2400) //DEBUG time set to 10mins
//            WorldEventDifficultyLevel = 4;
//    }

//    public override void ExecuteState()
//    {
//        base.ExecuteState();
//        CDTimer += Time.deltaTime;
//        if (CDTimer > baseEventInterval / WorldEventDifficultyLevel) // higher the difficulty the more frequent events are triggered
//        {
//            Exit(EventStateGlobals.EVALUATE);
//        }
//    }
//}

//public class EvaluateEventState : EventAIState
//{
//    public override void Enter(GameObject owner, string enteringState)
//    {
//        base.Enter(owner, enteringState);


//        highesFlatVal = 0;
//        lowestFlatVal = 100;

//        lowestValuePerMin = 100;
//        highestValuePerMin = 0;

//        deltaRes = new Dictionary<string, int>();
//        oldResources = new Dictionary<string, float>();
//        if (ForceThisEvent == null)
//        {
//            if (resources == null)
//            {
//                resources = new Dictionary<string, float>();
//            }
//            foreach (KeyValuePair<string, float> oldItem in resources)
//            {
//                oldResources.Add(oldItem.Key, oldItem.Value);
//            }
//            resources.Clear();
//            foreach (KeyValuePair<string, float> item in resMan.GetResourceValues())
//            {
//                resources.Add(item.Key, item.Value);
//            }
//        }
//    }

//    public override void ExecuteState()
//    {
//        base.ExecuteState();
//        if (ForceThisEvent == null)
//        {
//            GetLowestAndHighestCurrentResource();
//            //  GetRandomResource();
//            GetResourcesGainedSinceLastEvent();
//            GetHighAndLowResourceGained();
//            GetPopAmount();
//        }




//        Exit(EventStateGlobals.SELECT);

//    }

//    void GetLowestAndHighestCurrentResource()
//    {
//        foreach (KeyValuePair<string, float> item in resources)
//        {
//            if (item.Key == "Population")
//                continue;

//            if (item.Value > highesFlatVal)
//            {
//                highesFlatVal = (int)item.Value;
//                highestFlatRes = item.Key;
//            }

//            if (item.Value < lowestFlatVal && item.Value != 0)
//            {
//                lowestFlatVal = (int)item.Value;
//                lowestFlatRes = item.Key;
//            }
//        }
//    }
//    ///// <summary>
//    ///// to mkae ai unpredictable
//    ///// </summary>
//    //void GetRandomResource()
//    //{
//    //    List<string> names = resMan.GetAllResourceNames();
//    //    names.Remove("Population");
//    //    rndVal = UnityEngine.Random.Range(0, names.Count - 1);
//    //    rndRes = names[(int)rndVal];
//    //}

//    void GetResourcesGainedSinceLastEvent()
//    {
//        if (oldResources != null)
//        {
//            foreach (KeyValuePair<string, float> oldItem in oldResources)
//            {
//                foreach (KeyValuePair<string, float> item in resources)
//                {
//                    if (oldItem.Key == item.Key)
//                    {
//                        deltaRes.Add(oldItem.Key, (int)(item.Value - oldItem.Value));
//                        break; // should only happen once (O=n^2/2) 
//                    }
//                }
//            }
//        }
//    }

//    void GetHighAndLowResourceGained()
//    {
//        foreach (KeyValuePair<string, int> item in deltaRes)
//        {
//            if (item.Key == "Population")
//                continue;

//            if (item.Value > highestValuePerMin)
//            {
//                highestValuePerMin = (int)item.Value;
//                highestResPerMin = item.Key;
//            }

//            if (item.Value < lowestValuePerMin)
//            {
//                lowestValuePerMin = (int)item.Value;
//                lowestResPerMin = item.Key;
//            }
//        }
//        System.Console.WriteLine($"Higest p Min = {highestResPerMin}, {highestValuePerMin}");
//        System.Console.WriteLine($"Lowest p Min = {lowestResPerMin}, {lowestValuePerMin}");
//    }

//    void GetPopAmount()
//    {
//        humans = popMan.GetHumans(100); // gets all humans at 100 <= moral 
//        populationAmount = humans.Count;
//    }
//}

//public class SelectEventState : EventAIState
//{
//    public override void Enter(GameObject owner, string enteringState)
//    {
//        base.Enter(owner, enteringState);
//        selectedEvents = new List<AGameEvent>();
//    }

//    public override void ExecuteState()
//    {
//        base.ExecuteState();
//        if (ForceThisEvent == null)
//        {
//            int selector = UnityEngine.Random.Range(0, 100);
//            if (selector < 50)
//            {
//                if (selector < 25)
//                    SelectEvents(lowestFlatRes); //FlatVAl
//                else
//                    SelectEvents(highestFlatRes);
//            }
//            else
//            {
//                if (selector < 75)
//                    SelectEvents(lowestResPerMin);
//                else
//                    SelectEvents(highestResPerMin);
//            }

//            SelectWorldEvent();// TODO worldevent prereq logic     
//            if (selectedEvents.Count == 0)
//            {
//                System.Console.WriteLine("FLAG");
//            }
//            SetRandomEvent();
//        }


//        Exit(EventStateGlobals.APPLY);
//    }
//    void SelectEvents(string onTargetMethod)
//    {
//        for (int i = 0; i < gameEvents.Length; i++)
//        {
//            if (gameEvents[i].TargetType != "WorldEvent" && gameEvents[i].TargetType != "Stage")
//                for (int j = 0; j < gameEvents[i].ResourceTargets.Length; j++)
//                {
//                    if (gameEvents[i].ResourceTargets[j] == onTargetMethod)
//                    {
//                        selectedEvents.Add(gameEvents[i]);
//                        break;
//                    }
//                }
//        }
//    }

//    void SelectPopulation() { }

//    void SelectWorldEvent() //sandstorms, eclips etc
//    {
//        if (EventsTriggered < maxWorldEvents && WorldEventDifficultyLevel > 1)
//            for (int i = 0; i < gameEvents.Length; i++)
//            {
//                if (gameEvents[i].TargetType == "WorldEvent")
//                {
//                    selectedEvents.Add(gameEvents[i]);
//                }
//            }
//    }

//    /// <summary>
//    /// sets a random event incase the selected events are all on cd
//    /// </summary>
//    void SetRandomEvent()
//    {
//        randomEvent = gameEvents[UnityEngine.Random.Range(0, gameEvents.Length)];
//        if (randomEvent.TargetType == "Stage" || randomEvent.TargetType == "WorldEvent")
//        {
//            SetRandomEvent();
//        }
//    }
//}

//public class ApplyEventState : EventAIState
//{

//    public override void Enter(GameObject owner, string enteringState)
//    {
//        base.Enter(owner, enteringState);
//        if (ForceThisEvent == null)
//        {
//            eventMan.SelectedEvent = selectedEvent;
//            eventMan.SetNewEventCandidates(selectedEvents.ToArray(), randomEvent);
//            selectedEvents.Clear();
//        }
//        else
//        {
//            eventMan.ForceEvent(ForceThisEvent);
//            ForceThisEvent = null;
//        }

//    }

//    public override void ExecuteState()
//    {
//        base.ExecuteState();
//        if (!eventMan.HasEventRunning)
//            Exit(EventStateGlobals.COOLDOWN);
//    }
//}