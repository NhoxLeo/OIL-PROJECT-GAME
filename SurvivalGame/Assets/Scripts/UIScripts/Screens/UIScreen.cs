using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UIScripts.Screens
{
  public class UIScreen : MonoBehaviour
  {
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    [SerializeField]
    protected UIScreenType _screenType;

    public UIScreenType ScreenType { get { return _screenType; } }
    public bool IsOpen { get; set; }

    public virtual void UpdateScreenStatus(bool open)
    {
      _animator.SetBool("Open", open);
      _canvasGroup.interactable = open;
      SetScreenAlpha(open);
      _canvasGroup.blocksRaycasts = open;
      IsOpen = open;
    }

    private void SetScreenAlpha(bool open)
    {
      if (open)
      {
        _canvasGroup.alpha = 1;
      }
      else
        _canvasGroup.alpha = 0;
    }


  }
}
