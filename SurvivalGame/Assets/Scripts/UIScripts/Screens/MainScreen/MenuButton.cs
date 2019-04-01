using Assets.Scripts.UIScripts.Screens.UIExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UIScripts.Screens.MainScreen
{
  public class MenuButton : ActionButton
  {
    [SerializeField]
    Animator _animator;

    public override void OnPointerEnter(PointerEventData eventData)
    {
      base.OnPointerEnter(eventData);
      animator.SetTrigger("MouseEnter");
    }
  }
}
