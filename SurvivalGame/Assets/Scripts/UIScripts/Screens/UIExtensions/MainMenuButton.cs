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
  class MainMenuButton : ActionButton
  {
    Animator animator;
    Text text;
    private Color texthighlightColor;
    public Color textDefaultColor;

    protected override void Start()
    {
      animator = GetComponent<Animator>();
      text = transform.GetChild(0).GetComponent<Text>();
      textDefaultColor = text.color;
    }

     public override void OnPointerClick(PointerEventData eventData)
    {
      base.OnPointerClick(eventData);
      _action.Invoke();
      SoundManager.PlaySound("buildingSelect");
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
      base.OnPointerEnter(eventData);
      animator.SetTrigger("MouseEnter");
      text.color = Color.white;
      SoundManager.PlaySound("techHover");
    }


    public override void OnPointerExit(PointerEventData eventData)
    {
      base.OnPointerExit(eventData);
      text.color = textDefaultColor;
    }
  }
}
