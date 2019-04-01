using Assets.Scripts.UIScripts.Screens.UIExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Humans
{

  public class PlayerInterActableBase : MonoBehaviour
  {
    private bool isActive;
    protected string hoverInfoText = "";

    public virtual string OnMouseHoverSetInfoText(bool IsOver)
    {
      return "";
    }

    private void OnMouseEnter()
    {
      if (!isActive)
      {
        isActive = true;
        HoverObjectInfo.Singleton.ShowInfo(transform, hoverInfoText);
      }
    }

    private void OnMouseExit()
    {
      isActive = false;
    }

  }
}
