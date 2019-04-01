using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UIScripts.Screens.UIExtensions
{
  public class ActionButton : Button
  {

    [SerializeField]
    string HoverSoundID;
    [SerializeField]
    string ClickSoundID;

    protected Action _action;
    protected bool execute;

    public void SetButtonAction(Action action)
    {
      _action = action;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
      base.OnPointerClick(eventData);

      execute = CanExecute();
  
      if(execute)
      {
        SuccessClickSound();
        Execute();
      }
      else
      {
        FailToClickSound();
        HandleFailToExecute();
       
      }
    }

    public void OnEnabled(bool interactable)
    {
      this.interactable = interactable;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
      HoverSound();
      base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
      base.OnPointerExit(eventData);
    }

    protected virtual bool CanExecute()
    {
      return true;
    }

    protected virtual void SuccessClickSound()
    {
      SoundManager.PlaySound(ClickSoundID);
    }

    protected virtual void FailToClickSound()
    {

    }

    protected virtual void HoverSound()
    {
      SoundManager.PlaySound(HoverSoundID);
    }

    protected virtual void Execute()
    {
      _action.Invoke();
    }

    protected virtual void HandleFailToExecute()
    {

    }
  }
}
