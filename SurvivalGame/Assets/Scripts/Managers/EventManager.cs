using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    [Header("Button Choice")]
    public Button choiceA, choiceB, choiceC;
    [Header("Button Panel")]
    public GameObject panelA, panelB, panelC;
    [Header("Player Notification Button")]
    public GameObject notificationButtonObj;
    [Header("Event Content Text")]
    public Text panelText, buttonAContext, buttonBContext, buttonCContext;
    [Header("MouseOver tooltip")]
    MouseOverListener buttonATooltip, buttonBTooltip, buttonCTooltip, notificationToolTip;
    [Header("Event Area Panel")]
    public GameObject panelObject;  //TODO this should be automatic // [Header("Time Between Events")]    public int eventInterval = 5;
    [Header("Floating text display")]
    public Text floatingText;

    public bool HasEventRunning { get; set; }
    public static bool canTriggerMoralChoices = false;
    public static bool canTriggerEvents = true;

    bool hasAlteredButtonContext = false;
    bool isForcingEvent = false;

    AGameEvent[] eventCandidates;
    GameObject eventObjects, popMan;

    public AGameEvent SelectedEvent { get; set; }

    AGameEvent[] gameEvents;
    AGameEvent randomEvent;
    ResourceManager resMan;
    Transform man;

   // public EventFSM fsm;
    Button notificationButton;

    int autoChoice;

    void Awake()
    {
       // eventObjects = GameObject.Find("GameEvents").gameObject;

       // fsm = new EventFSM(gameObject, EventStateGlobals.COOLDOWN);
       // AddState(EventStateGlobals.EVALUATE);
       // AddState(EventStateGlobals.SELECT);
       // AddState(EventStateGlobals.APPLY);
        //AddStartState(EventStateGlobals.COOLDOWN);
       // popMan = GameObject.Find("Population").gameObject;
      //  resMan = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        //buttonATooltip = choiceA.GetComponent<MouseOverListener>();
       // buttonBTooltip = choiceB.GetComponent<MouseOverListener>();
       // buttonCTooltip = choiceC.GetComponent<MouseOverListener>();
      //  notificationToolTip = notificationButtonObj.GetComponent<MouseOverListener>();
     //   notificationButton = notificationButtonObj.GetComponent<Button>();
     //   gameEvents = eventObjects.GetComponents<AGameEvent>();

    }
    //private void AddStartState(string eventStartState)
    //{
    //    fsm.AddStartState(eventStartState, EventStateGlobals.GetAIState(eventStartState));
    //}
    public void AddState(string eventState)
    {
      //  fsm.AddState(eventState, EventStateGlobals.GetAIState(eventState));
    }

    void Update()
    {
        UpdateEventFSM();
        UpdateEvents();
        CheckIfForce();
        if (HasEventRunning && notificationButtonObj.GetComponent<Animator>().isActiveAndEnabled)
        {
            UpdateEventAnim();
        }
    }
    void UpdateEventFSM()
    {
     //   if (fsm != null && !isForcingEvent)
        {
       //     fsm.FSMUpdate();
        }
    }
    void UpdateEvents()
    {
        foreach (AGameEvent item in gameEvents) // updates all event objects
        {
            item.Update();
        }
    }

    void CheckIfForce()
    {
        if (isForcingEvent && !HasEventRunning)
            PresentEvent();
    }

    void UpdateEventAnim()
    {
        if (notificationButtonObj.GetComponent<Animator>().GetBool("eventNotifier"))
        {
            notificationButtonObj.GetComponent<Animator>().SetTrigger("shakeCooldown");
        }
    }

    public void SetNewEventCandidates(AGameEvent[] events, AGameEvent rndEvent)
    {
        if (eventCandidates == null)
        {
            eventCandidates = new AGameEvent[events.Length+1];
            for (int i = 0; i < events.Length; i++)
            {
                eventCandidates[i] = events[i];
            }
            eventCandidates[eventCandidates.Length-1] = randomEvent = rndEvent;
            CheckPreReqs();
        }
    }

    void CheckPreReqs()
    {
        if (eventCandidates != null)
        {
            if (!HasEventRunning && !isForcingEvent)
                CheckPreRequisites();
        }
    }

    public void EvaluateMoralValue()
    {
        float totalMoral = popMan.GetComponent<PopulationManager>().TotalMoral();
        float outcome;
        // bool hasChosen = false;
        if (totalMoral < 75)
        {
            outcome = UnityEngine.Random.Range(0f, 100f);
            canTriggerMoralChoices = true;

            GetDispleasedHumans(75);
            if (outcome < 2)// TODO: timer needed, 
            {
                canTriggerEvents = true;
            }
            if (totalMoral < 25)
            {
                GetDispleasedHumans(25);
            }
        }
        canTriggerMoralChoices = false;
    }
    void GetDispleasedHumans(int moralThreshhold)
    {
        man = popMan.GetComponent<PopulationManager>().GetHuman();
    }

    /// <summary>
    /// Checks which choice is valid. Currently a weired bug with the comiler showing the choice instances as null values.
    /// It still compiles and does what it should (checks prereqs).. whilst saying its a null instance.. it works though
    /// </summary>
    void CheckPreRequisites()
    {
        List<AGameEvent> validEvents = new List<AGameEvent>();
             

        foreach (AGameEvent e in eventCandidates)
        {
            if (e.PreRequisuites())
            {
                validEvents.Add(e);
            }
        }
        if (validEvents.Count == 0)
        {
            validEvents.Add(randomEvent);
        }
        SelectEvent(validEvents);
    }

    /// <summary>
    /// more sophistcated logic to select a choice to go in here
    /// </summary>
    /// <param name="validChoices"></param>
    void SelectEvent(List<AGameEvent> validChoices)
    {
        if (validChoices.Count > 0)
        {
            SelectedEvent = validChoices[UnityEngine.Random.Range(0, validChoices.Count)];

            eventCandidates = null;

            PresentEvent();
        }
    }

    void PresentEvent()
    {
        HasEventRunning = true;
        bool isWorldEvent = false;

        if (SelectedEvent.TargetType == "WorldEvent" || SelectedEvent.TargetType == "Stage")
            isWorldEvent = true;
        else
            isWorldEvent = false;

        panelText.text = SelectedEvent.eventContext;

        if (!hasAlteredButtonContext)
            GetButtonContext(isWorldEvent);

        notificationButtonObj.SetActive(true);
        notificationButtonObj.GetComponent<Animator>().SetBool("eventNotifier", true);
        SoundManager.PlaySound("byym");
        panelObject.GetComponent<EventTimeSliderScript>().ResetDecisionTimer();
        panelObject.GetComponent<EventTimeSliderScript>().StartDecisionTimer();
        List<int> validChoices = new List<int>();

        if (CheckResourcesAvailable(SelectedEvent.GetPlayerChoiceAResource(), choiceA))
        {
            validChoices.Add(0);
            panelA.GetComponent<Image>().color = Color.black;
            buttonAContext.text = SelectedEvent.buttonOneContext;
            buttonATooltip.tooltipText = SelectedEvent.buttonOneTooltip;
        }
        else
        {
            panelA.GetComponent<Image>().color = Color.gray;
        }
        if (CheckResourcesAvailable(SelectedEvent.GetPlayerChoiceBResource(), choiceB))
        {
            validChoices.Add(1);
            panelB.GetComponent<Image>().color = Color.black;
            buttonBContext.text = SelectedEvent.buttonTwoContext;
            buttonBTooltip.tooltipText = SelectedEvent.buttonTwoTooltip;
        }
        else
        {
            panelB.GetComponent<Image>().color = Color.gray;
        }
        if (CheckResourcesAvailable(SelectedEvent.GetPlayerChoiceCResource(), choiceC))
        {
            validChoices.Add(2);
            panelC.GetComponent<Image>().color = Color.black;
            buttonCContext.text = SelectedEvent.buttonThreeContext;
            buttonCTooltip.tooltipText = SelectedEvent.buttonThreeTooltip;
        }
        else
        {
            panelC.GetComponent<Image>().color = Color.gray;
        }
        autoChoice = validChoices[new System.Random().Next(0, validChoices.Count)];
        SetChosenPanel();
    }

    bool CheckResourcesAvailable(Dictionary<string, float> eventRes, Button button)
    {
        Array enumRes = Enum.GetValues(typeof(GlobalConstants.Resources));
        int counter = 0;
        int target = 0;
        if (eventRes != null)
            foreach (KeyValuePair<string, float> dic in eventRes)
            {
                target = eventRes.Count;
                foreach (GlobalConstants.Resources item in enumRes)
                {
                    if (dic.Key.ToUpper() == item.ToString())
                    {
                        if (resMan.CheckIfViableResourceChange(item, dic.Value))                                           
                        {                            
                            counter++;                          
                            break;
                        }
                    }
                }
            }
        if (counter != target)
        {

            button.interactable = false;           
            return false;

        }
        else
        {
            button.interactable = true;
            return true;
        }

    }

    void SetChosenPanel()
    {
        if (autoChoice == 0) panelA.GetComponent<PanelColorToTimer>().SetChosen();
        else if (autoChoice == 1) panelB.GetComponent<PanelColorToTimer>().SetChosen();
        else panelC.GetComponent<PanelColorToTimer>().SetChosen();
    }

    void GetButtonContext(bool isWorldEvent)
    {
        if (isWorldEvent)
        {
            buttonAContext.text = SelectedEvent.buttonOneContext;
            buttonATooltip.tooltipText = SelectedEvent.buttonOneTooltip;
        }
        //if (!isWorldEvent)
        //{
        //    if (CheckButtonValidity(SelectedEvent.GetPlayerChoiceAResource()))
        //        buttonAContext.text = SelectedEvent.buttonOneContext;
        //    buttonATooltip.tooltipText = SelectedEvent.buttonOneTooltip;
        //}

        //if (!isWorldEvent)
        //{
        //    if (CheckButtonValidity(SelectedEvent.GetPlayerChoiceAResource()))
        //        buttonBContext.text = SelectedEvent.buttonTwoContext;
        //    buttonBTooltip.tooltipText = SelectedEvent.buttonTwoTooltip;
        //}

        //if (!isWorldEvent)
        //{
        //    if (CheckButtonValidity(SelectedEvent.GetPlayerChoiceAResource()))
        //        buttonCContext.text = SelectedEvent.buttonThreeContext;
        //    buttonCTooltip.tooltipText = SelectedEvent.buttonThreeTooltip;
        //}
        hasAlteredButtonContext = true;
    }
    /// <summary>
    /// If not enough resources, 
    /// </summary>
    /// <param name="dic"></param>
    /// <returns></returns>
    bool CheckButtonValidity(Dictionary<string, float> dic)
    {
        if (dic == null)
            return false;

        foreach (KeyValuePair<string, float> item in dic)
        {
            float temp = resMan.GetSingleResource(item.Key);
            if (temp >= item.Value)
                return true;
        }
        return false;
    }

    void GrayBadButtons() //TODO gray out button
    {

    }

    public void SetDecision(Button playerChoice)
    {
        if (playerChoice == choiceA)
        {
            SelectedEvent.OutCome(0);
            floatingText.text = SelectedEvent.buttonOneResource;
            CloseEvent();

        }
        else if (playerChoice == choiceB)
        {
            SelectedEvent.OutCome(1);
            CloseEvent();
        }
        else if (playerChoice == choiceC)
        {
            SelectedEvent.OutCome(2);
            CloseEvent();
        }
    }

    void CloseEvent()
    {
        buttonATooltip.ForceDisplayOff();
        buttonBTooltip.ForceDisplayOff();
        buttonCTooltip.ForceDisplayOff();
        buttonAContext.text = null;
        buttonBContext.text = null;
        buttonCContext.text = null;
        buttonATooltip.tooltipText = null;
        buttonBTooltip.tooltipText = null;
        buttonCTooltip.tooltipText = null;

        notificationButtonObj.SetActive(false);
        notificationButton.interactable = true;
        panelObject.SetActive(false);
        SelectedEvent.CanFire = false;
        SelectedEvent = null;
        eventCandidates = null;
        HasEventRunning = false;
        hasAlteredButtonContext = false;
        isForcingEvent = false;
        panelA.GetComponent<PanelColorToTimer>().Reset();
        panelB.GetComponent<PanelColorToTimer>().Reset();
        panelC.GetComponent<PanelColorToTimer>().Reset();      

        // eventInterval = Random.Range(30, 120);       
    }

    public void ForceEvent(AGameEvent selectedEvent)
    {
        eventCandidates = null;
        SelectedEvent = selectedEvent;
        isForcingEvent = true;
    }

    public void DecisionTimerRunDown()
    {
        SelectedEvent.OutCome(autoChoice);
        CloseEvent();
    }

    public void OpenedEvent()
    {
        notificationButtonObj.GetComponent<Animator>().SetBool("eventNotifier", false);
        notificationButtonObj.GetComponent<Animator>().SetBool("presentEvent", true);

        notificationToolTip.ForceDisplayOff();
        notificationButtonObj.SetActive(false);

        notificationButton.interactable = false;
    }
}