using Assets.Scripts.UIScripts.Screens.LoadingScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.UIScripts.Screens.LoadingScreen
{
  public class LoadingScreen : UIScreen
  {

    [SerializeField]
    private Slider _loadingSlider;

    [SerializeField]
    private Text _loadingText;

    AsyncOperation _ao;
    float loadTime;
    float timer;

    int dotCount = 1;
    string originalLoadingText;

    private void Start()
    {
      loadTime = UnityEngine.Random.Range(3, 4);
      _ao = SceneManager.LoadSceneAsync("TestScene");
      _ao.allowSceneActivation = false;
      originalLoadingText = _loadingText.text;
    }

    public override void UpdateScreenStatus(bool open)
    {
      base.UpdateScreenStatus(open);
    }

    private void Update()
    {
      if (IsOpen)
      {
        timer += Time.deltaTime;
        int seconds = Convert.ToInt32(timer % 60) % 4;
        UpdateTimerDots(seconds);
        _loadingSlider.value = timer / loadTime;

        if (timer > loadTime)
        {
          _ao.allowSceneActivation = true;
        }
      }
    }


    void UpdateTimerDots(int s)
    {
      if (s == 0)
      {
        _loadingText.text = originalLoadingText + " . ";
      }
      else if (s == 1)
      {
        _loadingText.text = originalLoadingText + " . .";
      }
      else if (s == 2)
      {

        _loadingText.text = originalLoadingText + " . . .";
      }
      else if (s == 3)
      {
        _loadingText.text = originalLoadingText;
      }
    }
  }
}
