using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelColorToTimer : MonoBehaviour {

    Image image;
    public Slider slider;
    bool isChosenAsRandom;
    Color oldColor = new Color();
    Color c = Color.yellow;
    private void Awake()
    {
        image = GetComponent<Image>();
      //  slider = GameObject.Find("EventTimeSlider").GetComponent<Slider>();
    }
    
    // Update is called once per frame
    void Update () {
        if (isChosenAsRandom)
        {
            c.a = slider.value/ 100;
            image.color = c;
        }
	}        

    public void SetChosen()
    {
        Awake();
        isChosenAsRandom = true;
        oldColor = image.color;
        image.color = new Color(255, 255, 255, 0);
    }

    public void Reset()
    {
        image.color = oldColor;
        isChosenAsRandom = false;
    }
}
