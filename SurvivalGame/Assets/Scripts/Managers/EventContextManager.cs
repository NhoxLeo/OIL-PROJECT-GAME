using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EventContextManager : MonoBehaviour {

    StreamReader eventReader, buttonReader;
    string eventPath, buttonPath;
    Dictionary<AGameEvent, string> eventContexts;
    Dictionary<AGameEvent, string> eventChoiceA;
    Dictionary<AGameEvent, string> eventChoiceB;

    string[] currentEvent;
    List<AGameEvent> events;
    public AGameEvent rodentsInSuppliesEvent, rottingFoodDueToRepairsNeeded;

    GameObject obj;
    AGameEvent[] gameEvents;

    void Start()
    {
        
      //  eventPath = "Assets/Assets/EventContext.txt";
      //  buttonPath = "Assets/Assets/ButtonContext.txt";
        //eventReader = new StreamReader(eventPath);
      //  buttonReader = new StreamReader(buttonPath);

        //rodentsInSuppliesEvent = GetComponent<RemoveResourceManager>();

        eventContexts = new Dictionary<AGameEvent, string>();
        eventChoiceA = new Dictionary<AGameEvent, string>();
        eventChoiceB = new Dictionary<AGameEvent, string>();

        Initialize();
    }

    void Initialize()
    {
        obj = GameObject.Find("GameEvents").gameObject;
        gameEvents = obj.GetComponents<AGameEvent>();

        for (int i = 0; i < gameEvents.Length; i++)
        {
            //eventContexts.Add(gameEvents[i], eventReader.ReadLine());      // Causes crash, commented out on 22 october by Martin      
        }
        for (int i = 0; i < gameEvents.Length; i++) // TODO: find better way to do this
        {           
            //eventChoiceA.Add(gameEvents[i], buttonReader.ReadLine());     // This too

            //eventChoiceB.Add(gameEvents[i], buttonReader.ReadLine());     // And this
        }

        //for (int i = 0; i < 10; i++)
        //{
        //    current
        //}
        //foreach (KeyValuePair<AGameEvent,string> key in eventContexts)
        //{

        //}

    }

    public string GetEventContext(AGameEvent key)
    {
        foreach (KeyValuePair<AGameEvent,string> item in eventContexts)
        {
            if(key == item.Key)
            {
                string context = item.Value;
                return context;
            }
        }
        return null;
    }

    public string[] GetButtonContext(AGameEvent key)
    {
        string[] tempArray = new string[2];
        int it = 0;
        foreach (KeyValuePair<AGameEvent, string> item in eventChoiceA)
        {
            if (key == item.Key)
            {
                tempArray[it++] = item.Value;
            }
        }
        foreach (KeyValuePair<AGameEvent, string> item in eventChoiceB)
        {
            if (key == item.Key)
            {
                tempArray[it++] = item.Value;
            }
        }
        return tempArray;
    }

    //public KeyValuePair<string,float> GetButtonValuesToChange()
    //{
                
    //}
}
