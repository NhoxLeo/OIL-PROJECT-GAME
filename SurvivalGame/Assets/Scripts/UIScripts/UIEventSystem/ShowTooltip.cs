using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTooltip : MonoBehaviour
{   
    public GameObject obj;
       
    void OnMouseEnter()
    {
        obj.SetActive(true);
    }
    void OnMouseOver()
    {
       
    }
    void OnMouseExit()
    {
        obj.SetActive(false);
    }
}
