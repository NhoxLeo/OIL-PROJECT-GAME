using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
  public enum EmergencyBuffType { Child_Labor, Canibalism, Share_Beds, Eat_Shit, Empty }

  /// <summary>
  /// The buffs that this manager controls are global.
  /// </summary>
  public class EmergencyBuffManager : MonoBehaviour
  {

    [SerializeField]
    GameObject PopulationManager;

    //float CombinedMoralMultiplierValue = 1;
    int _activeBuffs = 0;

    public event EventHandler<EmergencyBuffArgs> OnEmergencyBuffChangedEvent;

    Dictionary<EmergencyBuffType, EmergencyBuff> Buffs;

    static EmergencyBuffManager Instance;

    public static EmergencyBuffManager Singleton
    {
      get
      {
        return Instance;
      }
    }

    void Awake()
    {
      if (Instance != null)
      {
        Destroy(gameObject);
        return;
      }
      Buffs = new Dictionary<EmergencyBuffType, EmergencyBuff>
      {
        { EmergencyBuffType.Child_Labor, new ChildLaborBuff(PopulationManager) },
        { EmergencyBuffType.Eat_Shit, new EatTrashBuff(PopulationManager) },
        { EmergencyBuffType.Share_Beds, new ShareBedsBuff(PopulationManager) },
      };

      Instance = this;
    }

    public bool BuffActive(EmergencyBuffType type)
    {
      return Buffs[type].Active;
    }

    /// <summary>
    /// Only to be triggered when a button (de)activates a buff.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="emergencyBuffArgs"></param>
    public void TriggerBuffChange(object sender, EmergencyBuffArgs emergencyBuffArgs)
    {

      _activeBuffs = Buffs[emergencyBuffArgs.Type].Activate(emergencyBuffArgs.On, _activeBuffs);
      //Debug.Log(emergencyBuffArgs.Type + " On: " + emergencyBuffArgs.On);
      //Debug.Log("Active buffs " + _activeBuffs);
      OnEmergencyBuffChangedEvent.Invoke(sender, emergencyBuffArgs);
    }

  }

  public abstract class EmergencyBuff
  {
    protected bool _active;
    public bool Active { get { return _active; } }

    public int Activate(bool on, int prevActiveBuffCount)
    {
      _active = on;
      return OnActivate(prevActiveBuffCount);
    }

    protected float TotalBuffValue(int activeBuffCount)
    {
      return activeBuffCount * 0.1f;
    }

    protected abstract int OnActivate(int prevActiveBuffCount);

  }

  public class ChildLaborBuff : EmergencyBuff
  {
    private GameObject populationManager;

    public ChildLaborBuff(GameObject populationManager)
    {
      this.populationManager = populationManager;
    }

    protected override int OnActivate(int prevActiveBuffCount)
    {
      //Deactivated
      if (!Active)
      {
        populationManager.GetComponent<PopulationManager>().ChildLabor(Active, TotalBuffValue(prevActiveBuffCount - 1));
        return prevActiveBuffCount - 1;
      }
      else
      {
        populationManager.GetComponent<PopulationManager>().ChildLabor(Active, TotalBuffValue(prevActiveBuffCount + 1));
        return prevActiveBuffCount + 1;
      }
        
    }
  }

  public class ShareBedsBuff : EmergencyBuff
  {
    private GameObject populationManager;

    public ShareBedsBuff(GameObject populationManager)
    {
      this.populationManager = populationManager;
    }

    protected override int OnActivate(int prevActiveBuffCount)
    {
      //Deactivated
      if (!Active)
      {
        populationManager.GetComponent<PopulationManager>().ShareBeds(Active, TotalBuffValue(prevActiveBuffCount - 1));
        return prevActiveBuffCount - 1;
      }
      else
      {
        populationManager.GetComponent<PopulationManager>().ShareBeds(Active, TotalBuffValue(prevActiveBuffCount + 1));
        return prevActiveBuffCount + 1;
      }

    }
  }

  public class EatTrashBuff : EmergencyBuff
  {
    private GameObject populationManager;

    public EatTrashBuff(GameObject populationManager)
    {
      this.populationManager = populationManager;
    }

    protected override int OnActivate(int prevActiveBuffCount)
    {
      //Deactivated
      if (!Active)
      {
        populationManager.GetComponent<PopulationManager>().EatTrashFood(Active, TotalBuffValue(prevActiveBuffCount - 1));
        return prevActiveBuffCount - 1;
      }
      else
      {
        populationManager.GetComponent<PopulationManager>().EatTrashFood(Active, TotalBuffValue(prevActiveBuffCount + 1));
        return prevActiveBuffCount + 1;
      }
    }
  }


  public class EmergencyBuffArgs : EventArgs
  {
    public EmergencyBuffType Type { get; }
    public bool On { get; }

    public EmergencyBuffArgs(EmergencyBuffType type, bool on)
    {
      Type = type;
      On = on;
    }
  }
}