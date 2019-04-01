using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UIScripts.Screens.InGameScreen.Emergency
{
  public class EmergencyController : MonoBehaviour
  {
    [SerializeField]
    Sprite OnSprite;

    [SerializeField]
    Sprite OffSprite;

    [SerializeField]
    GameObject ShareBedsButton;
    [SerializeField]
    GameObject CanibalismButton;
    [SerializeField]
    GameObject EatTrashButton;
    [SerializeField]
    GameObject ChildLaborButton;

    [SerializeField]
    BuffPanelController BuffPanelController;

    private void Start()
    {
      EmergencyBuffManager.Singleton.OnEmergencyBuffChangedEvent += EmergencyBuffManager_OnEmergencyBuffChangedEvent;

      ShareBedsButton.GetComponent<Button>().onClick.AddListener(() =>
        EmergencyBuffManager.Singleton.TriggerBuffChange(gameObject, new EmergencyBuffArgs(EmergencyBuffType.Share_Beds, OnOff(ShareBedsButton))));

      CanibalismButton.GetComponent<Button>().onClick.AddListener(() =>
      EmergencyBuffManager.Singleton.TriggerBuffChange(gameObject, new EmergencyBuffArgs(EmergencyBuffType.Canibalism, OnOff(CanibalismButton))));
      EatTrashButton.GetComponent<Button>().onClick.AddListener(() =>
      EmergencyBuffManager.Singleton.TriggerBuffChange(gameObject, new EmergencyBuffArgs(EmergencyBuffType.Eat_Shit, OnOff(EatTrashButton))));
      ChildLaborButton.GetComponent<Button>().onClick.AddListener(() =>
      EmergencyBuffManager.Singleton.TriggerBuffChange(gameObject, new EmergencyBuffArgs(EmergencyBuffType.Child_Labor, OnOff(ChildLaborButton))));
    }

    private void OnEnable()
    {
   
    }

    private void EmergencyBuffManager_OnEmergencyBuffChangedEvent(object sender, EmergencyBuffArgs e)
    {
      if ((GameObject)sender == gameObject)
        return;

      if (!e.On)
        DeActivateBuff(e.Type);
     
        
    }

    bool OnOff(GameObject b)
    {
      if (b.GetComponent<Image>().sprite == OnSprite)
      {
        b.GetComponent<Image>().sprite = OffSprite;
        return false;
      }
      b.GetComponent<Image>().sprite = OnSprite;
      return true;
    }

    public void DeActivateBuff(EmergencyBuffType _buff)
    {
      switch (_buff)
      {
        case EmergencyBuffType.Child_Labor:
          //ChildLaborButton.interactable = false;
          ChildLaborButton.GetComponent<Image>().sprite = OffSprite;
          break;
        case EmergencyBuffType.Canibalism:
          //CanibalismButton.interactable = false;
          CanibalismButton.GetComponent<Image>().sprite = OffSprite;
          break;
        case EmergencyBuffType.Share_Beds:
          // ShareBedsButton.interactable = false;
          ShareBedsButton.GetComponent<Image>().sprite = OffSprite;
          break;
        case EmergencyBuffType.Eat_Shit:
          //  EatTrashButton.interactable = false;
          EatTrashButton.GetComponent<Image>().sprite = OffSprite;
          break;
        case EmergencyBuffType.Empty:
          //.interactable = true;
          break;
        default:
          break;
      }


    }
  }
}
