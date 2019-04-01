using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AlterToolTipText : MonoBehaviour {
    public GameObject textObj;

    public void SetToolText(string input)
    {       
        textObj.GetComponent<Text>().text = input;
        textObj.GetComponent<RectTransform>().position = Input.mousePosition;         
    }
}
