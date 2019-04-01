using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlterButtonText : MonoBehaviour {

    public Text text;   

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }
    public void SetText(string input)
    {
        text.text = input;
    }
}
