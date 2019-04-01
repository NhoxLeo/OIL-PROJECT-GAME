using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateOptionComponents : MonoBehaviour {

    public Image texImage;
    public GameObject optionComp;
    public GameObject optionsMenu;
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
        if (texImage.fillAmount == 1)
        {
            optionComp.SetActive(true);
        }
        if(GetComponent<FillImage>().terminateFlag)
        {
            optionsMenu.SetActive(false);
            GetComponent<FillImage>().terminateFlag = false;
        }

	}
}
