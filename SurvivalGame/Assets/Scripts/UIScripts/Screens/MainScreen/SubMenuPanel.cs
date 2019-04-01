using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UIScripts.Screens.MainMenu
{
  public class SubMenuPanel : MonoBehaviour
  {
    [SerializeField]
    Animator _animator;

    bool _open;
    public bool IsOpen { get { return _open; } }

    private void Start()
    {

    }

    public void Enter()
    {
      _open = true;
      if (_animator)
        _animator.SetTrigger("Enter");
    }

    public void Exit()
    {
      _open = false;
      if (_animator)
        _animator.SetTrigger("Exit");
    }
  }
}
