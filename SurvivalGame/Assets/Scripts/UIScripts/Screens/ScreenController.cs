using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UIScripts.Screens
{
  public enum UIScreenType { Loading, Main };

  public class ScreenController : MonoBehaviour
  {
    [SerializeField]
    private UIScreenType _startingScreen;

    [SerializeField]
    private UIScreen _mainScreen;
    [SerializeField]
    private UIScreen _loadingScreen;

    UIScreenType _currentScreen;
    Dictionary<UIScreenType, UIScreen> _screens;
  
    static ScreenController Instance;
    public static ScreenController Singleton
    {
      get
      {
        return Instance;
      }
    }

    private void Start()
    {
      if (Instance != null)
      {
        Destroy(gameObject);
        return;
      }

      Instance = this;

      _screens = new Dictionary<UIScreenType, UIScreen>()
      {
        {UIScreenType.Main,  _mainScreen},
        {UIScreenType.Loading,  _loadingScreen},
      };
      _currentScreen = _startingScreen;
      ChangeScreen(_startingScreen);
    }


    public void ChangeScreen(UIScreenType newScreen)
    {
      //var closingScreen = _screens[_currentScreen];
      //closingScreen.UpdateScreenStatus(false);

      var openingScreen = _screens[newScreen];
      openingScreen.UpdateScreenStatus(true);
      _currentScreen = newScreen;

      CloseOtherScreens(_currentScreen);
    }

    private void CloseOtherScreens(UIScreenType current)
    {
      foreach (var screen in _screens)
      {
        if (screen.Key != current)
        {
          screen.Value.UpdateScreenStatus(false);
        }
      }
    }

  }
}
