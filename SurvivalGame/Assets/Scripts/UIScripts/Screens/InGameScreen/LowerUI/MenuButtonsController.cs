using Assets.Scripts.UIScripts.Screens.MainMenu;
using Assets.Scripts.UIScripts.Screens.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UIScripts.Screens.InGameScreen.LowerUI
{
  public class MenuButtonsController : MonoBehaviour
  {
    [SerializeField]
    ActionButton WorkerInfoButton;
    [SerializeField]
    ActionButton TechTreeButton;
    [SerializeField]
    ActionButton EmergencyButton;

    [SerializeField]
    List<SubMenuPanel> SubMenus = new List<SubMenuPanel>();
    [SerializeField]
    SubMenuPanel TechTreePanel;
    [SerializeField]
    SubMenuPanel EmergencyPanel;
    [SerializeField]
    SubMenuPanel WorkerInfoPanel;

    GameObject currentSubMenu = null;
    GameObject prevSubMenu = null;


    void Start()
    {
      TechTreeButton.SetButtonAction(() => { ChangeSubMenu(TechTreePanel.gameObject);});
      EmergencyButton.SetButtonAction(() => { ChangeSubMenu(EmergencyPanel.gameObject); });
      WorkerInfoButton.SetButtonAction(() => { ChangeSubMenu(WorkerInfoPanel.gameObject); });
    }

    void ChangeSubMenu(GameObject newSubMenu)
    {
      if (currentSubMenu == newSubMenu)
      {
        CloseSubMenu();
        //Debug.Log("Menu was same as before so close");
      }
      else
      {
        prevSubMenu = currentSubMenu;
        if (prevSubMenu)
        {
          prevSubMenu.GetComponent<SubMenuPanel>().Exit();
          currentSubMenu.gameObject.SetActive(false);
        }
        currentSubMenu = newSubMenu;
        currentSubMenu.gameObject.SetActive(true);
        currentSubMenu.GetComponent<SubMenuPanel>().Enter();
      }

    }

    void CloseSubMenu()
    {
      prevSubMenu = currentSubMenu;
      currentSubMenu.gameObject.SetActive(false);
      currentSubMenu.GetComponent<SubMenuPanel>().Exit();
      currentSubMenu = null;
    }
  }

}

