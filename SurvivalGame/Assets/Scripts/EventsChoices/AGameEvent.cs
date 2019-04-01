using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AGameEvent : MonoBehaviour
{
    protected ResourceManager resourceManager;
    protected GameObject popMan;
    protected Dictionary<string, float> resources = new Dictionary<string, float>();
    public string Name { get; set; }
    public string TargetType { get; set; }
    public string eventContext, buttonOneContext, buttonTwoContext, buttonThreeContext, buttonOneTooltip, buttonTwoTooltip, buttonThreeTooltip;
    public string buttonOneResource, buttonTwoResource, buttonThreeResource;
    public string[] ResourceTargets { get; set; }
    public float DeltaValue { get; set; }
    public float BoostValue { get; set; }
    public float Duration { get; set; }
    public float CooldownTimer { get; set; }
    public float EventShowDuration { get; set; }
    protected float resourceMin, baseDeltaValue, baseBoostValue, baseBuffDuration, timerInterval;
    public bool CanFire = true;   
  

    public virtual void Awake()
    {
        resourceManager = GameObject.Find("ResourceManager").gameObject.GetComponent<ResourceManager>();
        resources = resourceManager.GetResourceValues();
        popMan = GameObject.Find("Population").gameObject;
        baseBoostValue = 10 * EventAIState.WorldEventDifficultyLevel;
        baseDeltaValue = 10 * EventAIState.WorldEventDifficultyLevel;
        baseBuffDuration = 90; 
       // EventShowDuration = 10 / EventAIState.WorldEventDifficultyLevel;
        resources = resourceManager.GetResourceValues();
        timerInterval = 60;
    }

    public AGameEvent()
    {
       
    }

    public virtual void Update()
    {     
        if (!CanFire) //shouldn't update while event is firing
            CooldownTimer += Time.deltaTime;

        if (CooldownTimer >= timerInterval)
        {
            CanFire = true;
            CooldownTimer = 0;
        }
    }   

    public abstract bool PreRequisuites();

    public abstract void OutCome(int playerChoice);

    public abstract Dictionary<string, float> GetPlayerChoiceAResource();

    public abstract Dictionary<string, float> GetPlayerChoiceBResource();

    public abstract Dictionary<string, float> GetPlayerChoiceCResource();

}
