using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  public static AudioClip buildingHover, buildingSelect, techHover, techSelect, beepOpen, construct, byym;
  private static AudioSource audioSrc;

  void Start()
  {
    buildingHover = Resources.Load<AudioClip>("Audio/UI/Click04");
    buildingSelect = Resources.Load<AudioClip>("Audio/UI/slack");
    techHover = Resources.Load<AudioClip>("Audio/UI/click02");
    beepOpen = Resources.Load<AudioClip>("Audio/UI/actionStart");
    construct = Resources.Load<AudioClip>("Audio/UI/construct");
    byym = Resources.Load<AudioClip>("Audio/UI/byym");
    audioSrc = GetComponent<AudioSource>();
  }

  public static void PlaySound(string sound)
  {
    switch (sound)
    {
      case "buildingHover":
        audioSrc.PlayOneShot(buildingHover);
        break;
      case "buildingSelect":
        audioSrc.PlayOneShot(buildingSelect);
        break;
      case "techHover":
        audioSrc.PlayOneShot(techHover);
        break;
      case "beepOpen":
        audioSrc.PlayOneShot(beepOpen);
        break;
      case "construct":
        audioSrc.PlayOneShot(construct);
        break;
      case "byym":
        audioSrc.PlayOneShot(byym, .3f);
        break;
      default:
        //Debug.Log("No sound was set for that action");
        break;
    }
  }

  public void PlaySoundNonStatic(string sound)
  {
    PlaySound(sound);
  }
}
