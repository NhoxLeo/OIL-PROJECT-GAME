using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnHighlightTextChange : MonoBehaviour
{
    public Button button;
    public Text buttonText;   
    public string output;
    string originalMessage;

    private void Start()
    {        
        originalMessage = buttonText.text;        
    }

    private void OnMouseOver()
    {
        buttonText.text = output;
    }

    private void OnMouseExit()
    {
        buttonText.text = originalMessage;
    }
}
