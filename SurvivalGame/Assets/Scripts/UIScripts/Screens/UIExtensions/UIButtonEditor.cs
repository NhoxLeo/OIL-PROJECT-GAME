using Assets.Scripts.UIScripts.Screens.InGameScreen;
using Assets.Scripts.UIScripts.Screens.MainScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Scripts.UIScripts.Screens.UIExtensions
{
#if UNITY_EDITOR

  [CustomEditor(typeof(CostButton))]
  public class UIButtonEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      CostButton b = (CostButton)target;
    }
  }

  [CustomEditor(typeof(ActionButton))]
  public class UIActionButtonEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      ActionButton b = (ActionButton)target;
    }
  }

  [CustomEditor(typeof(MenuButton))]
  public class UIMenuButtonEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();
      MenuButton b = (MenuButton)target;
    }
  }

#endif
}
