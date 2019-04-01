using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UIScripts.Screens
{
  public class UIDialog : MonoBehaviour
  {


    [SerializeField]
    protected GameObject _DialogPanel;
    [SerializeField]
    protected DialogType _dialogType;

    public DialogType ScreenType { get { return _dialogType; } }
    public bool IsOpen { get; set; }

    public virtual void UpdateScreenStatus(bool open)
    {
      _DialogPanel.SetActive(open);
      //_canvasGroup.interactable = open;
      SetScreenAlpha(open);
      //  _canvasGroup.blocksRaycasts = open;
      IsOpen = open;
    }

    private void SetScreenAlpha(bool open)
    {
      if (open)
      {
        //_canvasGroup.alpha = 1;
      }

      // _canvasGroup.alpha = 0;

    }
  }
}
