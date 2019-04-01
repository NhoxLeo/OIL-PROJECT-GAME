using Assets.Scripts.UIScripts.Screens.UIExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UIScripts.Screens.UITechTree.Rows
{
  public class UIWoodRowController : MonoBehaviour
  {
    [SerializeField]
    ActionButton buttonLevelOne;

    [SerializeField]
    ActionButton buttonLevelTwo;

    [SerializeField]
    ActionButton buttonLevelThree;

    [SerializeField]
    TechWood techWood;


    private void Start()
    {
      buttonLevelOne.SetButtonAction(() => 
      {
        techWood.Research(false);
        Debug.Log("Show LoadMenu");
      });

      buttonLevelOne.SetButtonAction(() =>
      {
        Debug.Log("Show LoadMenu");
      });

      buttonLevelOne.SetButtonAction(() =>
      {
        Debug.Log("Show LoadMenu");
      });


    }

  }
}
