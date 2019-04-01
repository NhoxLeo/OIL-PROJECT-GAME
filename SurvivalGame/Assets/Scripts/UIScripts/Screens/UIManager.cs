using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UIScripts.Screens
{
  public enum DialogType { TechTreeDialog, EmergencyDialog, InformationDialog, ToolTipDialog,}

  public class UIManager : MonoBehaviour
  {

    [SerializeField]
    DialogController dialogController;


    static UIManager Instance;

    public static UIManager Singleton
    {
      get
      {
        return Instance;
      }
    }

    void Awake()
    {
      if (Instance != null)
      {
        Destroy(gameObject);
        return;
      }

      Instance = this;

    }

    public void OpenDialog(DialogType dialogType)
    {
      dialogController.ChangeDialog();
    }
  }

}
