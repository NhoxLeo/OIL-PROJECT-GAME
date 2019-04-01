using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Misc
{
  public class EmergencyBuff : MonoBehaviour
  {
    public enum EmergencyBuffType { Child_Labor, Canibalism, Share_Beds, Eat_Shit, Empty }

    [SerializeField]
    EmergencyBuffType type;

    [SerializeField]
    Sprite OnSprite;

    [SerializeField]
    Sprite OffSprite;

    public EmergencyBuffType Type { get { return type; } }

    bool active;
    public bool Active;

    //public bool Activate()
    //{
    //  if (Active)
    //    return false;

    //  Active = true;
    //  GetComponent<Button>().interactable = true;
    //  return true;
    //}

    //public bool DeActivate()
    //{
    //  if (!Active)
    //    return false;
    //  Active = false;
    //  GetComponent<Button>().interactable = false;

    //  return true;
    //}

    public void Off()
    {
      //GetComponent<Button>().interactable = false;
      GetComponent<Image>().sprite = OnSprite;
    }

    public void On()
    {
     // GetComponent<Button>().interactable = true;
      GetComponent<Image>().sprite = OnSprite;
    }

  }
}
