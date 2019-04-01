using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillImage : MonoBehaviour {

    public Image image;
    public float fillSpeed;
    bool imageIsActive;
    public bool terminateFlag;
		
    public void SetImageActive(bool state)
    {
        imageIsActive = state;       
    }
    

	void Update () {
        if (imageIsActive)
        {
            if (image.fillAmount < 1)
                image.fillAmount += (fillSpeed / 60);            
        }
        else
        {
            if(image.fillAmount > 0)
            {
                image.fillAmount -= (fillSpeed / 60);
            }
            else if (image.fillAmount <= 0)
            {
                terminateFlag = true;
            }
        }
	}
}
