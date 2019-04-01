using Assets.Scripts.Misc;
using Assets.Scripts.UIScripts.Screens.MainMenu;
using Assets.Scripts.UIScripts.Screens.UIExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UIScripts.Screens.UITechTree
{
  public class TechTreeCategoryController : MonoBehaviour
  {
    [SerializeField]
    ActionButton LivingButton;
    [SerializeField]
    ActionButton ResourcesButton;
    
    [SerializeField]
    GameObject CombinedTechCategories;
    [SerializeField]
    GameObject LivingTechPanel;
    [SerializeField]
    GameObject ResourceTechPanel;
    [SerializeField]
    GameObject OilTechPanel;

    GameObject currentSubMenu = null;
    GameObject prevSubMenu = null;


    void Start()
    {

      currentSubMenu = ResourceTechPanel;

      GameObject.Find("CanvasAlwaysActive").GetComponent<TechTimeSliderScript>().ResearchStartedEvent += HandleResearchStarted;
      GameObject.Find("CanvasAlwaysActive").GetComponent<TechTimeSliderScript>().ResearchFinishedEvent += HandleResearchFinished;

      LivingButton.SetButtonAction(() => { ChangeSubMenu(LivingTechPanel.gameObject); });
      ResourcesButton.SetButtonAction(() => { ChangeSubMenu(ResourceTechPanel.gameObject); });

      LivingTechPanel.SetActive(true);
      ResourceTechPanel.SetActive(true);
      OilTechPanel.SetActive(true);

      ChangeSubMenu(LivingTechPanel);
      ChangeSubMenu(currentSubMenu);
    }

    void PassSliderStartedToChild(GameObject allCategoryPanels)
    {
      foreach (CoolDown child in ResourceTechPanel.GetComponentsInChildren<CoolDown>(true))
      {
          child.OnResearchStarted(child.gameObject);
      }
      foreach (CoolDown child in LivingTechPanel.GetComponentsInChildren<CoolDown>(true))
      {
        child.OnResearchStarted(child.gameObject);
      }
      foreach (CoolDown child in OilTechPanel.GetComponentsInChildren<CoolDown>(true))
      {
        child.OnResearchStarted(child.gameObject);
      }
    }

    void PassSliderFinishedToChild(GameObject allCategoryPanels)
    {
      foreach (CoolDown child in ResourceTechPanel.GetComponentsInChildren<CoolDown>(true))
      {
        child.OnResearchFinished();
      }
      foreach (CoolDown child in LivingTechPanel.GetComponentsInChildren<CoolDown>(true))
      {
        child.OnResearchFinished();
      }
      foreach (CoolDown child in OilTechPanel.GetComponentsInChildren<CoolDown>(true))
      {
        child.OnResearchFinished();
      }
    }

    private void HandleResearchFinished(object sender, GameObject finishedTech)
    {
      PassSliderFinishedToChild(CombinedTechCategories);
    }

    private void HandleResearchStarted(object sender, GameObject e)
    {
      PassSliderStartedToChild(CombinedTechCategories);
     
    }

    void ChangeSubMenu(GameObject newSubMenu)
    {
      if (currentSubMenu == newSubMenu)
      {
        //CloseSubMenu();
        //Debug.Log("Menu was same as before so close");
      }
      else
      {
        prevSubMenu = currentSubMenu;
        if (prevSubMenu)
        {
          //prevSubMenu.GetComponent<SubMenuPanel>().Exit();
          currentSubMenu.gameObject.SetActive(false);
        }
        currentSubMenu = newSubMenu;
        currentSubMenu.gameObject.SetActive(true);
       // currentSubMenu.GetComponent<SubMenuPanel>().Enter();
      }

    }

    void CloseSubMenu()
    {
      prevSubMenu = currentSubMenu;
      currentSubMenu.gameObject.SetActive(false);
     // currentSubMenu.GetComponent<SubMenuPanel>().Exit();
      currentSubMenu = null;
    }
  }
}

