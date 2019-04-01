using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.UIElements;

public class PanelHandler : MonoBehaviour
{

    public GameObject panel, ps, button;
   

    public void EnableAndDisablePanel()
    {
        if (panel.activeSelf == false)
            panel.SetActive(true);
        else
            panel.SetActive(false);       
    }
}
