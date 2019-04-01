using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Misc
{

  public class CoolDown : MonoBehaviour
  {

    Button Button;
    [SerializeField]
    Sprite CoolDownSprite;

    [SerializeField]
    Sprite OriginalSprite;

    //[SerializeField]
    Text OriginalText;
    string originalText;

    private void OnEnable()
    {
      OriginalText = gameObject.GetComponentInChildren<Text>();
      if (OriginalText.text != "")
        originalText = OriginalText.text;
    }

    public void OnResearchFinished()
    {
      Button = GetComponent<Button>();

      //text = Button.GetComponentInChildren<Text>(true);
      OriginalText.text = originalText;
      gameObject.GetComponent<Image>().sprite = OriginalSprite;
      gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
      Button.enabled = true;

    }

    public void OnResearchStarted(GameObject clickedButtonObj)
    {
      Button = GetComponent<Button>();
      if (Button.interactable)
      {

        OriginalText = GetComponentInChildren<Text>(true);
        if (gameObject.Equals(clickedButtonObj))
        {
          Button.image.sprite = CoolDownSprite;
          OriginalText.text = "";
        }
        else
          gameObject.GetComponent<Image>().color = new Color32(128, 127, 127, 200);

        Button.enabled = false;
      }
    }
  }
}
