using Assets.Scripts.UIScripts.Screens.MainScreen;
using Assets.Scripts.UIScripts.Screens.UIExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UIScripts.Screens.MainMenu
{
  public class MainMenuScreen : UIScreen
  {
    [SerializeField]
    protected MenuButton PlayButton = null;
    [SerializeField]
    protected MenuButton LoadButton = null;
    [SerializeField]
    protected MenuButton HighScoresButton = null;
    [SerializeField]
    protected MenuButton CreditsButton = null;
    [SerializeField]
    protected MenuButton ExitButton = null;

    [SerializeField]
    List<SubMenuPanel> SubMenus = new List<SubMenuPanel>();

    GameObject currentSubMenu = null;
    GameObject prevSubMenu = null;

    private void Start()
    {
      IsOpen = true;
            
      PlayButton.SetButtonAction(() =>
      {
        Debug.Log("Play!");
        ScreenController.Singleton.ChangeScreen(UIScreenType.Loading);
      });

      CreditsButton.SetButtonAction(() =>
      {
        Debug.Log("Show credits");
        ChangeSubMenu(SubMenus[0].gameObject);
      });

      LoadButton.SetButtonAction(() =>
      {
        Debug.Log("Show LoadMenu");
        ChangeSubMenu(SubMenus[1].gameObject);
      });

      HighScoresButton.SetButtonAction(() =>
      {
        Debug.Log("Show LoadMenu");
        ChangeSubMenu(SubMenus[2].gameObject);
      });

      ExitButton.SetButtonAction(() =>
      {
        Debug.Log("Leave game");
        Application.Quit();
      });
    }

    void ChangeSubMenu(GameObject newSubMenu)
    {
      if (currentSubMenu == newSubMenu)
      {
        CloseSubMenu();
        Debug.Log("Menu was same as before so close");
      }
      else
      {
        prevSubMenu = currentSubMenu;
        if (prevSubMenu)
        {
          prevSubMenu.GetComponent<SubMenuPanel>().Exit();
        }
        currentSubMenu = newSubMenu;
        currentSubMenu.GetComponent<SubMenuPanel>().Enter();
      }

    }

    void CloseSubMenu()
    {
      prevSubMenu = currentSubMenu;
      currentSubMenu.GetComponent<SubMenuPanel>().Exit();
    }

    

  }
}
