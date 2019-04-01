using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffPanelController : MonoBehaviour
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

  private void EmergencyBuffManager_OnEmergencyBuffChangedEvent(object sender, EmergencyBuffArgs e)
  {
    if ((GameObject)sender == gameObject)
      return;

      if (e.On)
        ActivateBuff(e.Type);
      else
        DeActivateBuff(e.Type);
  }


  bool OnOff(GameObject b)
  {
    b.GetComponent<Button>().interactable = false;
    b.SetActive(false);
    return false;
  }

  public void ActivateBuff(EmergencyBuffType _buff)
  {
    var s = 1;
    switch (_buff)
    {
      case EmergencyBuffType.Child_Labor:
        ChildLaborButton.GetComponent<Button>().interactable = true;
        ChildLaborButton.SetActive(true);
        break;
      case EmergencyBuffType.Canibalism:
        CanibalismButton.GetComponent<Button>().interactable = true;
        CanibalismButton.SetActive(true);
        break;
      case EmergencyBuffType.Share_Beds:
        ShareBedsButton.GetComponent<Button>().interactable = true;
        ShareBedsButton.SetActive(true);
        break;
      case EmergencyBuffType.Eat_Shit:
        EatTrashButton.GetComponent<Button>().interactable = true;
        EatTrashButton.SetActive(true);
        break;
      case EmergencyBuffType.Empty:
        //.interactable = true;
        break;
      default:
        break;
    }

  }

  public void DeActivateBuff(EmergencyBuffType _buff)
  {
    var s = 1;
    switch (_buff)
    {
      case EmergencyBuffType.Child_Labor:
        ChildLaborButton.GetComponent<Button>().interactable = false;
        ChildLaborButton.SetActive(false);
        break;
      case EmergencyBuffType.Canibalism:
        CanibalismButton.GetComponent<Button>().interactable = false;
        CanibalismButton.SetActive(false);
        break;
      case EmergencyBuffType.Share_Beds:
        ShareBedsButton.GetComponent<Button>().interactable = false;
        ShareBedsButton.SetActive(false);
        break;
      case EmergencyBuffType.Eat_Shit:
        EatTrashButton.GetComponent<Button>().interactable = false;
        EatTrashButton.SetActive(false);
        break;
      case EmergencyBuffType.Empty:
        break;
      default:
        break;
    }

  }
}
